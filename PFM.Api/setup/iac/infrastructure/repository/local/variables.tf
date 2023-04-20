variable "create_ecr" {
  description = "Enable the creation of the ECR repository."
  type        = bool
  default     = true
}

variable "repository_name" {
  description = "PFM API"
  type        = string
  default     = "pfm-api"
}