output "cluster_arn" {
  value = var.create_resources ? module.standard_api[0].cluster_arn : ""
}

output "service_arn" {
  value = var.create_resources ? module.standard_api[0].service_arn : ""
}

output "private_subnets" {
  value = var.create_resources ? module.standard_api[0].private_subnets : ""
}

output "public_subnets" {
  value = var.create_resources ? module.standard_api[0].public_subnets : ""
}