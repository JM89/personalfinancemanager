resource "aws_ecs_cluster" "cluster" {
  count = var.create_resources ? 1 : 0
  name  = var.name
}

resource "aws_ecs_service" "service" {
  count           = var.create_resources ? 1 : 0
  name            = var.name
  cluster         = aws_ecs_cluster.cluster[0].id
  task_definition = aws_ecs_task_definition.task_definition[0].arn
  desired_count   = var.desired_count

  launch_type = "FARGATE"
  network_configuration {
    subnets          = toset(data.aws_subnets.private_subnets.ids)
    security_groups  = var.create_alb_resources ? [aws_security_group.default_sg[0].id] : []
    assign_public_ip = false
  }

  dynamic "load_balancer" {
    for_each = var.create_alb_resources ? [1] : []
    content {
      container_name   = var.name
      container_port   = var.app_port
      target_group_arn = aws_lb_target_group.service_target_group[0].arn
    }
  }
}

resource "aws_ecs_task_definition" "task_definition" {
  count                    = var.create_resources ? 1 : 0
  family                   = var.name
  requires_compatibilities = ["FARGATE"]
  network_mode             = "awsvpc"
  task_role_arn            = aws_iam_role.task_role[0].arn

  container_definitions = jsonencode([
    {
      name      = var.name
      image     = var.app_image
      cpu       = var.app_cpu
      memory    = var.app_memory
      essential = true
      portMappings = [
        {
          containerPort = var.app_port
          hostPort      = local.default_https_port
        }
      ]
      loggerConfiguration = {
        logDriver = "awslogs"
        options = {
          awslogs-group         = aws_cloudwatch_log_group.log_group[0].name
          awslog-region         = var.aws_region
          awslogs-stream-prefix = "ecs"
        }
      }
    }
  ])
}

resource "aws_security_group" "default_sg" {
  count       = var.create_resources && var.create_alb_resources ? 1 : 0
  name        = "${var.name}-svc"
  description = "Allow traffic from ALB"
  vpc_id      = var.vpc_id

  ingress {
    description     = "Allow HTTPS"
    from_port       = var.app_port
    to_port         = var.app_port
    protocol        = "tcp"
    security_groups = [aws_security_group.alb_sg[0].id]
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }

  tags = {
    Name = "${var.name}-svc"
  }
}

resource "aws_cloudwatch_log_group" "log_group" {
  count             = var.create_resources ? 1 : 0
  name              = "/aws/ecs/${var.name}"
  retention_in_days = var.log_retention_in_days
}

resource "aws_lb_target_group" "service_target_group" {
  count       = var.create_resources && var.create_alb_resources ? 1 : 0
  name        = var.name
  target_type = "ip"
  port        = var.app_port
  vpc_id      = var.vpc_id
  protocol    = "HTTPS"

  health_check {
    path = var.app_health_checks
  }
}

resource "aws_lb_listener_rule" "redirect_rule" {
  count        = var.create_resources && var.create_alb_resources ? 1 : 0
  listener_arn = aws_lb_listener.alb_default_https_listener[0].arn
  action {
    type             = "forward"
    target_group_arn = aws_lb_target_group.service_target_group[0].arn
  }
  condition {
    path_pattern {
      values = ["/${var.name}/*"]
    }
  }
}