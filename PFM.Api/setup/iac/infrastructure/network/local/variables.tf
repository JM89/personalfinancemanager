variable "create_resources" {
  description = "Enable the creation of the resources."
  type        = bool
  default     = true
}

variable "vpc_name" {
  description = "Name of the VPC"
  type        = string
  default     = "pfm-api"
}
