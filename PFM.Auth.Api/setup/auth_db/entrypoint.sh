#!/bin/bash

COUNT_ATTEMPT=50

echo "Attempt to connect to the DB server: $DB_SERVER_NAME,$DB_SERVER_PORT"
for ((n=$COUNT_ATTEMPT;n>0;n--)) ; do
    /opt/mssql-tools/bin/sqlcmd -S "$DB_SERVER_NAME,$DB_SERVER_PORT" -U SA -P "$DB_SA_PASSWORD" -Q "SELECT 1" > /dev/null
    if [[ $? == 0 ]] ; then
        break
    fi
    echo "Next connection attempt in 1s"
    sleep 5
done

[[ $n == 0 ]] && echo "Failed connection to DB server: $DB_SERVER_NAME,$DB_SERVER_PORT" && exit 1

echo "Successful connection to DB server: $DB_SERVER_NAME,$DB_SERVER_PORT"

echo "Run 01_create_db_if_not_exists.sql script on PFM_AUTH_DB"
/opt/mssql-tools/bin/sqlcmd -S "$DB_SERVER_NAME,$DB_SERVER_PORT" -U SA -P "$DB_SA_PASSWORD" -i scripts/01_create_db_if_not_exists.sql

echo "Run 02_create_user_if_not_exists.sql script on PFM_AUTH_DB"
/opt/mssql-tools/bin/sqlcmd -S "$DB_SERVER_NAME,$DB_SERVER_PORT" -U SA -P "$DB_SA_PASSWORD" -i scripts/02_create_user_if_not_exists.sql

echo "Run 03_create_db_schema.sql script on PFM_AUTH_DB"
/opt/mssql-tools/bin/sqlcmd -S "$DB_SERVER_NAME,$DB_SERVER_PORT" -U SA -P "$DB_SA_PASSWORD" -i scripts/03_create_db_schema.sql

echo "Run 04_add_seed_data.sql script on PFM_AUTH_DB"
/opt/mssql-tools/bin/sqlcmd -S "$DB_SERVER_NAME,$DB_SERVER_PORT" -U SA -P "$DB_SA_PASSWORD" -i scripts/04_add_seed_data.sql

