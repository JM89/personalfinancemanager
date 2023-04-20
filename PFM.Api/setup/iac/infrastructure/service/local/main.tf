module "standard_api" {
  count  = var.create_resources ? 1 : 0
  source = "../../../core-modules/standard-api"

  name       = var.app_name
  vpc_id     = data.aws_vpc.vpc.id
  aws_region = var.aws_region

  app_image         = "localhost.localstack.cloud:4510/pfm-api:latest"
  app_port          = 4431
  app_health_checks = "/api/health-check"
}
