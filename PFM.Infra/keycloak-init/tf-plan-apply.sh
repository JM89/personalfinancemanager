#!/bin/sh

echo "Deploying $1 in $2"

cd "access-control/$1/$2"

echo "terraform init"
terraform init -lock=false

echo "terraform plan -out plan"
terraform plan -out plan -lock=false

echo "terraform apply plan"
terraform apply plan