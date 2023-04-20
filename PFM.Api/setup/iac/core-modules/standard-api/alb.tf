resource "aws_security_group" "alb_sg" {
  count       = var.create_resources && var.create_alb_resources ? 1 : 0
  name        = "${var.name}-alb"
  description = "Allow HTTPS traffic"
  vpc_id      = var.vpc_id

  ingress {
    description = "Allow HTTPS"
    from_port   = 443
    to_port     = 443
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }

  tags = {
    Name = "${var.name}-alb"
  }
}

resource "aws_lb" "alb" {
  count              = var.create_resources && var.create_alb_resources ? 1 : 0
  name               = "${var.name}-alb"
  internal           = false
  load_balancer_type = "application"
  security_groups    = [aws_security_group.alb_sg[0].id]
  subnets            = toset(data.aws_subnets.private_subnets.ids)
}

resource "aws_lb_listener" "alb_default_http_listener" {
  count             = var.create_resources && var.create_alb_resources ? 1 : 0
  load_balancer_arn = aws_lb.alb[0].arn
  port              = local.default_http_port
  protocol          = "HTTP"
  default_action {
    type = "redirect"
    redirect {
      port        = local.default_https_port
      protocol    = "HTTPS"
      status_code = "HTTP_301"
    }
  }
}

resource "aws_lb_listener" "alb_default_https_listener" {
  count             = var.create_resources && var.create_alb_resources ? 1 : 0
  load_balancer_arn = aws_lb.alb[0].arn
  port              = local.default_https_port
  protocol          = "HTTPS"
  ssl_policy        = "ELBSecurityPolicy-2016-08"
  certificate_arn   = var.ssl_server_certificate

  default_action {
    type             = "forward"
    target_group_arn = aws_lb_target_group.service_target_group[0].arn
  }
}

