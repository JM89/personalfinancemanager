terraform {
  backend "s3" {
    bucket                      = "pfm-keycloak-terraform-state"
    key                         = "local/main/terraform.tfstate"
    region                      = "eu-west-2"
    endpoint                    = "http://localstack:4566"
    skip_credentials_validation = true
    skip_metadata_api_check     = true
    force_path_style            = true
    skip_requesting_account_id  = true
  }
}

provider "keycloak" {
  client_id = "admin-cli"
  url       = "http://keycloak:8080"
}
