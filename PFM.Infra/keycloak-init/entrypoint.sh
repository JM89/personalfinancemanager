#!/bin/bash

COUNT_ATTEMPT=10

echo "Attempt to connect to the endpoint: $AWS_ENDPOINT in region $AWS_REGION"
for ((n=$COUNT_ATTEMPT;n>0;n--)) ; do
    aws s3 ls --endpoint $AWS_ENDPOINT --region $AWS_REGION > /dev/null
    if [[ $? == 0 ]] ; then
        break
    fi
    echo "Next connection attempt in 1s"
    sleep 1
done

[[ $n == 0 ]] && echo "Failed connection to endpoint: $AWS_ENDPOINT in region $AWS_REGION" && exit 1

echo "Successful connection to endpoint: $AWS_ENDPOINT in region $AWS_REGION"

echo "Add bucket for terraform code"
aws s3api create-bucket --bucket pfm-keycloak-terraform-state --endpoint-url $AWS_ENDPOINT --region $AWS_REGION --create-bucket-configuration LocationConstraint=$AWS_REGION > /dev/null

COUNT_ATTEMPT=250
echo "Attempt to connect to the endpoint: $KC_ENDPOINT"
for ((n=$COUNT_ATTEMPT;n>0;n--)) ; do
    curl -kv $KC_ENDPOINT > /dev/null
    if [[ $? == 0 ]] ; then
        break
    fi
    echo "Next connection attempt in 1s"
    sleep 1
done

[[ $n == 0 ]] && echo "Failed connection to endpoint: $KC_ENDPOINT" && exit 1

echo "Successful connection to endpoint: $KC_ENDPOINT"

echo "Run Main - IaC"
sh tf-plan-apply.sh "main" "local"