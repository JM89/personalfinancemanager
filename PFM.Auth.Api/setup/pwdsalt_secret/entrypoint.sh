#!/bin/bash

COUNT_ATTEMPT=10

echo "Attempt to connect to the endpoint: $AWS_ENDPOINT in region $AWS_REGION"
for ((n=$COUNT_ATTEMPT;n>0;n--)) ; do
    aws secretsmanager list-secrets --endpoint $AWS_ENDPOINT --region $AWS_REGION > /dev/null
    if [[ $? == 0 ]] ; then
        break
    fi
    echo "Next connection attempt in 1s"
    sleep 1
done

[[ $n == 0 ]] && echo "Failed connection to endpoint: $AWS_ENDPOINT in region $AWS_REGION" && exit 1

echo "Successful connection to endpoint: $AWS_ENDPOINT in region $AWS_REGION"

aws secretsmanager create-secret --name "pfm/pwdsalt" --description "Password Salt" --secret-string "{PasswordSalt: \"SOME SALT\"}" --endpoint $AWS_ENDPOINT --region $AWS_REGION
