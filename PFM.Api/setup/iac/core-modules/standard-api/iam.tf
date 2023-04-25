resource "aws_iam_role" "task_role" {
  count = var.create_resources ? 1 : 0
  name  = "${var.name}-task-role"
  assume_role_policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Action = "sts:AssumeRole"
        Effect = "Allow"
        Sid    = ""
        Principal = {
          Service = "ecs-tasks.amazonaws.com"
        }
      },
    ]
  })
}

resource "aws_iam_role_policy_attachment" "policy_attachments" {
  count      = var.create_resources ? length(var.attached_policies) : 0
  role       = aws_iam_role.task_role[0].arn
  policy_arn = var.attached_policies[count.index]
}

resource "aws_iam_role_policy" "task_role_logging_policy" {
  count  = var.create_resources ? 1 : 0
  name   = "${var.name}-logging-policy"
  role   = aws_iam_role.task_role[0].id
  policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Action = [
          "logs:CreateLogStream",
          "logs:PutLogEvents",
          "logs:DescribeLogStreams"
        ]
        Effect   = "Allow"
        Resource = "*"
      }
    ]
  })
}