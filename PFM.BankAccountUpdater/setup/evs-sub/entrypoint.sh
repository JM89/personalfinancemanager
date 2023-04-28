#!/bin/bash

COUNT_ATTEMPT=20

echo "Attempt to connect to the EVS server: $EVS_API_URL"
for ((n=$COUNT_ATTEMPT;n>0;n--)) ; do
    curl -kv "$EVS_API_URL/subscriptions" -o /dev/null
    if [[ $? == 0 ]] ; then
        break
    fi
    echo "Next connection attempt in 1s"
    sleep 1
done

[[ $n == 0 ]] && echo "Failed connection to EVS server: $EVS_API_URL" && exit 1

echo "Successful connection to EVS server: $EVS_API_URL"

echo "Creating subscription"
curl -X PUT "$EVS_API_URL/subscriptions/BankAccount/Updater" -d @persistent-subscription.json -H "Content-Type: application/json"