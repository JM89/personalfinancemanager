#!/bin/bash

COUNT_ATTEMPT=20

echo "Attempt to connect to the DB server: $DB_SERVER_NAME,$DB_SERVER_PORT"
for ((n=$COUNT_ATTEMPT;n>0;n--)) ; do
    /opt/mssql-tools/bin/sqlcmd -S "$DB_SERVER_NAME,$DB_SERVER_PORT" -U SA -P "$DB_SA_PASSWORD" -Q "SELECT 1" > /dev/null
    if [[ $? == 0 ]] ; then
        break
    fi
    echo "Next connection attempt in 1s"
    sleep 1
done

[[ $n == 0 ]] && echo "Failed connection to DB server: $DB_SERVER_NAME,$DB_SERVER_PORT" && exit 1

echo "Successful connection to DB server: $DB_SERVER_NAME,$DB_SERVER_PORT"

echo "Run 01_create_main_db_if_not_exists.sql script"
/opt/mssql-tools/bin/sqlcmd -S "$DB_SERVER_NAME,$DB_SERVER_PORT" -U SA -P "$DB_SA_PASSWORD" -i scripts/01_create_main_db_if_not_exists.sql

echo "Run 02_create_pfmapi_user_if_not_exists.sql script"
/opt/mssql-tools/bin/sqlcmd -S "$DB_SERVER_NAME,$DB_SERVER_PORT" -U SA -P "$DB_SA_PASSWORD" -i scripts/02_create_pfmapi_user_if_not_exists.sql

echo "Run 03_create_main_db_schema.sql script"
/opt/mssql-tools/bin/sqlcmd -S "$DB_SERVER_NAME,$DB_SERVER_PORT" -U SA -P "$DB_SA_PASSWORD" -i scripts/03_create_main_db_schema.sql

echo "Run 04_add_seed_data.sql script"
/opt/mssql-tools/bin/sqlcmd -S "$DB_SERVER_NAME,$DB_SERVER_PORT" -U SA -P "$DB_SA_PASSWORD" -i scripts/04_add_seed_data.sql

echo "Run 05_add_test_data.sql script"
/opt/mssql-tools/bin/sqlcmd -S "$DB_SERVER_NAME,$DB_SERVER_PORT" -U SA -P "$DB_SA_PASSWORD" -i scripts/05_add_test_data.sql

