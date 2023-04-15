#!/bin/sh

aws s3api create-bucket --bucket pfm-terraform-state --endpoint-url http://localstack:4566 --region eu-west-2 --create-bucket-configuration LocationConstraint=eu-west-2