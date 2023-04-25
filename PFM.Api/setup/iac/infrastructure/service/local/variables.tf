variable "create_resources" {
  description = "Enable the creation of the resources."
  type        = bool
  default     = true
}

variable "aws_region" {
  description = "AWS Region"
  type        = string
  default     = "eu-west-2"
}

variable "app_name" {
  description = "Name of the ECS Service"
  type        = string
  default     = "pfm-api"
}