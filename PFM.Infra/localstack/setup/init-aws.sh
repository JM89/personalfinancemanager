#!/bin/sh

aws s3api create-bucket --bucket terraform-state --endpoint-url http://localstack:4566 --region eu-west-2 --create-bucket-configuration LocationConstraint=eu-west-2

/etc/localstack/init/ready.d/terraform/repository/tf-plan-apply.sh

