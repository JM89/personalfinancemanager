resource "aws_route53_zone" "zone" {
  count = var.create_resources ? 1 : 0
  name  = "jm89.pfm.com"
}