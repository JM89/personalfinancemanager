variable "create_resources" {
  description = "Enable the creation of the resources."
  type        = bool
  default     = true
}

variable "repository_name" {
  description = "Name of the ECR repository"
  type        = string
  default     = "pfm-api"
}
