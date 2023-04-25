terraform {
  backend "s3" {
    bucket                      = "pfm-api-terraform-state"
    key                         = "local/service/terraform.tfstate"
    region                      = "eu-west-2"
    endpoint                    = "http://localhost:4566"
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

  default_tags {
    tags = {
      "env" : "local",
      "product" : "pfm-api",
      "managed-by" : "terraform"
    }
  }

  endpoints {
    ecs        = "http://localhost:4566"
    cloudwatch = "http://localhost:4566"
    iam        = "http://localhost:4566"
    ec2        = "http://localhost:4566"
    logs       = "http://localhost:4566"
    elbv2      = "http://localhost:4566"
    route53    = "http://localhost:4566"
    acm        = "http://localhost:4566"
  }
}
