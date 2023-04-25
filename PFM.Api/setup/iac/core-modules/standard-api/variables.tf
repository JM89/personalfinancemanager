variable "create_resources" {
  description = "Enable the creation of the resources."
  type        = bool
  default     = true
}

variable "create_alb_resources" {
  description = "Enable the creation of the load balancer resources."
  type        = bool
  default     = true
}

variable "aws_region" {
  description = "AWS Region"
  type        = string
  default     = "eu-west-2"
}

variable "name" {
  description = "Descriptive kebab-case name for the service. Will be used to set cluster name, service name, etc."
  type        = string
}

variable "app_image" {
  description = "Location of the docker image of the service"
  type        = string
}

variable "app_port" {
  description = "Port exposed by the service"
  type        = number
}

variable "app_cpu" {
  description = "Max CPU allocated for the service"
  type        = number
  default     = 10
}

variable "app_memory" {
  description = "Max memory allocated for the service"
  type        = number
  default     = 512
}

variable "desired_count" {
  description = "Number of desired instances"
  type        = number
  default     = 1
}

variable "vpc_id" {
  description = "VPC ID"
  type        = string
}

variable "log_retention_in_days" {
  description = "CloudWatch Log retention in days"
  type        = number
  default     = 7
}

variable "attached_policies" {
  description = "List of IAM policies to attached to the task role"
  type        = list(string)
  default     = []
}

variable "app_health_checks" {
  description = "Service Health check path"
  type        = string
}
