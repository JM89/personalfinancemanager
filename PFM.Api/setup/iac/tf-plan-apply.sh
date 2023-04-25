#!/bin/sh

echo "Deploying $1 in $2"

cd "infrastructure/$1/$2"

echo "terraform init -reconfigure"
terraform init -reconfigure

echo "terraform plan -out plan"
terraform plan -out plan

echo "terraform apply plan"
terraform apply plan