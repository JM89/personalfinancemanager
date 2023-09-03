terraform {
  backend "s3" {
    bucket                      = "terraform-state"
    key                         = "local/iac-batch-movemnents/terraform.tfstate"
    region                      = "eu-west-2"
    endpoint                    = "http://localstack:4566"
    skip_credentials_validation = true
    skip_metadata_api_check     = true
    force_path_style            = true
  }
}

provider "aws" {
  access_key = "XXX"
  secret_key = "XXX"
  region     = "eu-west-2"

  skip_credentials_validation = true
  skip_metadata_api_check     = true
  skip_requesting_account_id  = true

  endpoints {
    sqs = "http://localstack:4566"
    kms = "http://localstack:4566"
  }
}