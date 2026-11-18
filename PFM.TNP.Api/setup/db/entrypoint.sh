#!/bin/bash

COUNT_ATTEMPT=50

echo "Attempt to connect to the MySQL DB server: $DB_SERVER_NAME,$DB_SERVER_PORT"
for ((n=$COUNT_ATTEMPT;n>0;n--)) ; do
    mysql --host=$DB_SERVER_NAME --port=$DB_SERVER_PORT -uroot --password="$DB_ROOT_PASSWORD" > /dev/null
    if [[ $? == 0 ]] ; then
        break
    fi
    echo "Next connection attempt in 1s"
    sleep 2
done

[[ $n == 0 ]] && echo "Failed connection to DB server: $DB_SERVER_NAME,$DB_SERVER_PORT" && exit 1

echo "Successful connection to DB server: $DB_SERVER_NAME,$DB_SERVER_PORT"

set -e

echo "Run 01_create_main_db_if_not_exists.sql script"
mysql --host=$DB_SERVER_NAME --port=$DB_SERVER_PORT -uroot --password="$DB_ROOT_PASSWORD" < scripts/01_create_main_db_if_not_exists.sql

echo "Run 03_create_main_db_schema.sql script"
mysql --host=$DB_SERVER_NAME --port=$DB_SERVER_PORT -uroot --password="$DB_ROOT_PASSWORD" < scripts/03_create_main_db_schema.sql

echo "Run 04_widen_monetary_columns.sql script"
MYSQL_PWD="$DB_ROOT_PASSWORD" mysql --host="$DB_SERVER_NAME" --port="$DB_SERVER_PORT" -uroot < scripts/04_widen_monetary_columns.sql

echo "Run 05_add_test_data.sql script"
mysql --host=$DB_SERVER_NAME --port=$DB_SERVER_PORT -uroot --password="$DB_ROOT_PASSWORD" < scripts/05_add_test_data.sql
