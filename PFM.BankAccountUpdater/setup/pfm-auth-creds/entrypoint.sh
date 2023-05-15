#!/bin/bash

COUNT_ATTEMPT=20

echo "Attempt to connect to the server: $API_URL"
for ((n=$COUNT_ATTEMPT;n>0;n--)) ; do
    curl -kv "$API_URL" -o /dev/null
    if [[ $? == 0 ]] ; then
        break
    fi
    echo "Next connection attempt in 10s"
    sleep 10
done

[[ $n == 0 ]] && echo "Failed connection to server: $API_URL" && exit 1

echo "Successful connection to server: $API_URL"

curl -X POST "$API_URL/users/register" -d "{\"username\":\"pfm-bank-account-updater@gmail.com\", \"password\":\"String123456!\", \"firstname\": \"\", \"lastname\":\"\" }" -H "Content-Type: application/json"