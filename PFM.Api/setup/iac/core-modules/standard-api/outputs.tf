output "cluster_arn" {
  value = var.create_resources ? aws_ecs_cluster.cluster[0].id : ""
}

output "service_arn" {
  value = var.create_resources ? aws_ecs_service.service[0].id : ""
}

output "private_subnets" {
  value = "[${join(",", data.aws_subnets.private_subnets.ids)}]"
}

output "public_subnets" {
  value = "[${join(",", data.aws_subnets.public_subnets.ids)}]"
}