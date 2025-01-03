resource "aws_ecr_repository" "ecr" {
  count                = var.create_resources ? 1 : 0
  name                 = var.repository_name
  image_tag_mutability = "IMMUTABLE"
  image_scanning_configuration {
    scan_on_push = true
  }
}
