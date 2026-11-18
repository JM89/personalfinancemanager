#!/usr/bin/env bash
set -euo pipefail

api_dir="$(cd "$(dirname "$0")/.." && pwd)"
repo_dir="$(cd "$api_dir/.." && pwd)"
container_id=""

cleanup() {
    if [[ -n "$container_id" ]]; then
        docker stop "$container_id" >/dev/null 2>&1 || true
    fi
}
trap cleanup EXIT

MYSQL_ADMIN_PASSWORD='test;password' GH_PKG_TOKEN='' \
    docker compose --env-file /dev/null --profile dev -f "$repo_dir/PFM.Api/docker-compose-api-init.yml" config --format json |
    python3 -c '
import json
import sys

environment = json.load(sys.stdin)["services"]["pfm-tnp-api"]["environment"]
expected = "Server=mysql-db-server;Port=3306;User ID=root;Password=\"test;password\";Database=PFM_TNP_DB"
if environment.get("APP_ApplicationSettings__DatabaseOptions__ConnectionString") != expected:
    raise SystemExit("Tax-and-pension MySQL configuration is incorrect")
if "APP_ConnectionStrings__PFMConnection" in environment:
    raise SystemExit("Tax-and-pension service still has the unrelated SQL Server configuration")
'

if env -u MYSQL_ADMIN_PASSWORD GH_PKG_TOKEN='' \
    docker compose --env-file /dev/null --profile dev -f "$repo_dir/PFM.Api/docker-compose-api-init.yml" config --quiet >/dev/null 2>&1; then
    echo "Compose accepted a missing MySQL password" >&2
    exit 1
fi

container_id="$(docker run --rm --detach --network none \
    -e MYSQL_ROOT_PASSWORD=pfm-test-only-password -e MYSQL_DATABASE=PFM_TNP_DB mysql:9.1)"

ready=false
for ((attempt=1; attempt<=90; attempt++)); do
    if docker exec "$container_id" env MYSQL_PWD=pfm-test-only-password \
        mysqladmin ping -h 127.0.0.1 -u root --silent >/dev/null 2>&1; then
        ready=true
        break
    fi
    sleep 1
done
if [[ "$ready" != true ]]; then
    echo "Test MySQL container did not become ready" >&2
    docker logs --tail 50 "$container_id" >&2
    exit 1
fi

mysql_test() {
    docker exec -i "$container_id" env MYSQL_PWD=pfm-test-only-password \
        mysql -h 127.0.0.1 -u root --batch --skip-column-names "$@"
}

assert_equal() {
    if [[ "$1" != "$2" ]]; then
        printf '%s: expected %s, got %s\n' "$3" "$1" "$2" >&2
        exit 1
    fi
}

mysql_test PFM_TNP_DB <<'SQL'
CREATE TABLE Pensions (
    Id VARCHAR(36) PRIMARY KEY,
    SchemeName NVARCHAR(500) NOT NULL,
    LoginUrl NVARCHAR(500) NOT NULL,
    LastUpdated DATETIME NOT NULL DEFAULT NOW(),
    CurrentPot DECIMAL(10,0) NOT NULL DEFAULT 0,
    CurrentContribution DECIMAL(10,0) NOT NULL DEFAULT 0,
    UserId NVARCHAR(500) NOT NULL,
    CurrencyId INT NOT NULL,
    CountryId INT NOT NULL
);
CREATE TABLE IncomeTaxReports (
    Id VARCHAR(36) PRIMARY KEY,
    PayDay DATETIME NOT NULL,
    TaxableIncome DECIMAL(10,0) NOT NULL,
    IncomeTaxPaid DECIMAL(10,0) NOT NULL,
    NationalInsurancePaid DECIMAL(10,0) NOT NULL,
    UserId NVARCHAR(500) NOT NULL,
    CurrencyId INT NOT NULL,
    CountryId INT NOT NULL
);
INSERT INTO Pensions (Id, SchemeName, LoginUrl, CurrentPot, CurrentContribution, UserId, CurrencyId, CountryId)
VALUES ('legacy-pension', 'Test', '', 17, 9, 'test-user', 1, 1);
INSERT INTO IncomeTaxReports (Id, PayDay, TaxableIncome, IncomeTaxPaid, NationalInsurancePaid, UserId, CurrencyId, CountryId)
VALUES ('legacy-tax', '2025-03-31', 100, 20, 3, 'test-user', 1, 1);
SQL

migration="$api_dir/setup/db/scripts/04_widen_monetary_columns.sql"
mysql_test < "$migration"
mysql_test < "$migration"
assert_equal '17.00|9.00' "$(mysql_test PFM_TNP_DB -e "SELECT CONCAT(CurrentPot, '|', CurrentContribution) FROM Pensions WHERE Id = 'legacy-pension'")" "Existing pension amounts"
assert_equal '100.00|20.00|3.00' "$(mysql_test PFM_TNP_DB -e "SELECT CONCAT(TaxableIncome, '|', IncomeTaxPaid, '|', NationalInsurancePaid) FROM IncomeTaxReports WHERE Id = 'legacy-tax'")" "Existing tax amounts"

mysql_test PFM_TNP_DB <<'SQL'
UPDATE Pensions SET CurrentPot = 123.45, CurrentContribution = 67.89 WHERE Id = 'legacy-pension';
UPDATE IncomeTaxReports SET TaxableIncome = 123.45, IncomeTaxPaid = 12.34, NationalInsurancePaid = 5.67 WHERE Id = 'legacy-tax';
SQL
assert_equal '123.45|67.89' "$(mysql_test PFM_TNP_DB -e "SELECT CONCAT(CurrentPot, '|', CurrentContribution) FROM Pensions WHERE Id = 'legacy-pension'")" "Pension decimal amounts"
assert_equal '123.45|12.34|5.67' "$(mysql_test PFM_TNP_DB -e "SELECT CONCAT(TaxableIncome, '|', IncomeTaxPaid, '|', NationalInsurancePaid) FROM IncomeTaxReports WHERE Id = 'legacy-tax'")" "Tax decimal amounts"

mysql_test PFM_TNP_DB <<'SQL'
ALTER TABLE Pensions MODIFY CurrentPot DECIMAL(22,4) NOT NULL DEFAULT 0;
UPDATE Pensions SET CurrentPot = 123.4567 WHERE Id = 'legacy-pension';
SQL
mysql_test < "$migration"
assert_equal '123.4567' "$(mysql_test PFM_TNP_DB -e "SELECT CurrentPot FROM Pensions WHERE Id = 'legacy-pension'")" "Existing higher precision"
assert_equal 'decimal(22,4)' "$(mysql_test PFM_TNP_DB -e "SELECT COLUMN_TYPE FROM information_schema.COLUMNS WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'Pensions' AND COLUMN_NAME = 'CurrentPot'")" "Existing column type"

mysql_test -e 'CREATE DATABASE PFM_TNP_FRESH; CREATE DATABASE PFM_TNP_BROKEN;'
sed 's/USE PFM_TNP_DB;/USE PFM_TNP_FRESH;/' "$api_dir/setup/db/scripts/03_create_main_db_schema.sql" | mysql_test
sed 's/USE PFM_TNP_DB;/USE PFM_TNP_FRESH;/' "$migration" | mysql_test
assert_equal '5' "$(mysql_test -e "SELECT COUNT(*) FROM information_schema.COLUMNS WHERE TABLE_SCHEMA = 'PFM_TNP_FRESH' AND DATA_TYPE = 'decimal' AND NUMERIC_SCALE = 2 AND ((TABLE_NAME = 'Pensions' AND COLUMN_NAME IN ('CurrentPot', 'CurrentContribution')) OR (TABLE_NAME = 'IncomeTaxReports' AND COLUMN_NAME IN ('TaxableIncome', 'IncomeTaxPaid', 'NationalInsurancePaid')))")" "Fresh monetary columns"
if sed 's/USE PFM_TNP_DB;/USE PFM_TNP_BROKEN;/' "$migration" | mysql_test >/dev/null 2>&1; then
    echo "Migration accepted an incomplete schema" >&2
    exit 1
fi

docker exec "$container_id" mkdir -p /tmp/pfm-db-setup
docker cp "$api_dir/setup/db/entrypoint.sh" "$container_id:/tmp/pfm-db-setup/"
docker cp "$api_dir/setup/db/scripts" "$container_id:/tmp/pfm-db-setup/"
docker exec -w /tmp/pfm-db-setup -e DB_SERVER_NAME=127.0.0.1 -e DB_SERVER_PORT=3306 \
    -e DB_ROOT_PASSWORD=pfm-test-only-password "$container_id" bash ./entrypoint.sh

echo "Tax-and-pension database setup tests passed."
