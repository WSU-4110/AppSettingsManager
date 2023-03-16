provider "aws" {
  region = "us-east-2"
}

locals {
  environment      = "prod"
  application_name = "appsettingsmanager-${local.environment}"
  api_name         = "${local.application_name}-api"
  bff_name         = "${local.application_name}-bff"
  db_name          = "appsettingsmanager${local.environment}"
  ecr_secret_arn   = "arn:aws:secretsmanager:us-east-2:860701506846:secret:test/appsettingsmanager-test/ecr-SY1nQh"
}

variable "max_tasks" {
  default = 2
}

variable "min_tasks" {
  default = 1
}

variable "desired_tasks" {
  default = 1
}

resource "aws_vpc" "vpc" {
  cidr_block = "10.0.0.0/16"
}

resource "aws_subnet" "subnet" {
  cidr_block        = "10.0.1.0/24"
  vpc_id            = aws_vpc.vpc.id
  availability_zone = "us-east-2a"
}

resource "aws_subnet" "subnet2" {
  cidr_block        = "10.0.2.0/24"
  vpc_id            = aws_vpc.vpc.id
  availability_zone = "us-east-2b" # Make sure to use a different AZ than the first subnet
}


resource "aws_security_group" "ecs_sg" {
  name        = "${local.application_name}-ecs_sg"
  description = "ECS security group"
  vpc_id      = aws_vpc.vpc.id

  ingress {
    from_port = 0
    to_port   = 65535
    protocol  = "tcp"
  }
}

resource "aws_security_group" "rds_sg" {
  name        = "${local.application_name}-rds_sg"
  description = "RDS security group"
  vpc_id      = aws_vpc.vpc.id

  ingress {
    from_port       = 3306
    to_port         = 3306
    protocol        = "tcp"
    security_groups = [aws_security_group.ecs_sg.id]
  }
}

resource "aws_db_subnet_group" "rds_subnet_group" {
  name       = "${local.application_name}_rds_subnet_group"
  subnet_ids = [aws_subnet.subnet.id, aws_subnet.subnet2.id]

  tags = {
    Name = "RDS Subnet Group"
  }
}

resource "aws_db_instance" "rds" {
  allocated_storage      = 20
  storage_type           = "gp2"
  engine                 = "mysql"
  engine_version         = "8.0"
  instance_class         = "db.t2.micro"
  db_name                = local.db_name
  username               = "admin"
  password               = "password"
  vpc_security_group_ids = [aws_security_group.rds_sg.id]
  db_subnet_group_name   = aws_db_subnet_group.rds_subnet_group.name
  skip_final_snapshot    = true
}

resource "aws_s3_bucket" "bucket" {
  bucket = local.application_name
}

resource "aws_cloudfront_origin_access_identity" "oai" {
  comment = "Origin access identity for CloudFront distribution"
}

resource "aws_ecs_cluster" "cluster" {
  name = "${local.application_name}-ecs_cluster"
}

resource "aws_ecr_repository" "api" {
  name = local.api_name
}

resource "aws_ecr_repository" "bff" {
  name = local.bff_name
}

data "aws_iam_policy_document" "assume_role_policy" {
  statement {
    actions = ["sts:AssumeRole"]

    principals {
      identifiers = ["ecs-tasks.amazonaws.com"]
      type        = "Service"
    }
  }
}

resource "aws_iam_role" "ecs_task_role" {
  name               = "${local.application_name}-ecs-task-role"
  assume_role_policy = data.aws_iam_policy_document.assume_role_policy.json

  force_detach_policies = true
}

resource "aws_iam_role" "ecs_execution_role" {
  name               = "${local.application_name}-ecs-execution-role"
  assume_role_policy = data.aws_iam_policy_document.assume_role_policy.json

  force_detach_policies = true
}

resource "aws_iam_role_policy" "ecs_execution_role_inline_policy" {
  name = "${local.application_name}-ecs-execution-role"
  role = aws_iam_role.ecs_execution_role.name
  policy = jsonencode({
    "Version" : "2012-10-17",
    "Statement" : [{
      "Effect" : "Allow",
      "Action" : [
        "ecr:GetDownloadUrlForLayer",
        "ecr:BatchGetImage",
        "ecr:BatchCheckLayerAvailability",
        "ecr:GetAuthorizationToken",
        "logs:CreateLogStream",
        "logs:PutLogEvents",
        "ssm:GetParameters",
        "secretsmanager:GetSecretValue",
        "kms:Decrypt"
      ],
      "Resource" : "*"
    }
    ]
  })
}

resource "aws_cloudwatch_log_group" "api" {
  name = "/ecs/${local.api_name}"
}

resource "aws_ecs_task_definition" "api_task" {
  family                   = "${local.api_name}_task_family"
  network_mode             = "awsvpc"
  requires_compatibilities = ["FARGATE"]
  cpu                      = "256"
  memory                   = "512"
  execution_role_arn       = aws_iam_role.ecs_execution_role.arn
  task_role_arn            = aws_iam_role.ecs_task_role.arn

  container_definitions = jsonencode([
    {
      name  = "${local.api_name}-container"
      image = "${aws_ecr_repository.api.repository_url}:latest"
      portMappings = [
        {
          containerPort = 80
          hostPort      = 80
        }
      ]

      logConfiguration = {
        logDriver = "awslogs"
        options = {
          "awslogs-group"         = aws_cloudwatch_log_group.api.name
          "awslogs-region"        = "us-east-2"
          "awslogs-stream-prefix" = "ecs"
        }
      }
    }
  ])
}

resource "aws_cloudwatch_log_group" "bff" {
  name = "/ecs/${local.bff_name}"
}

resource "aws_ecs_task_definition" "bff_task" {
  family                   = "${local.bff_name}_task_family"
  network_mode             = "awsvpc"
  requires_compatibilities = ["FARGATE"]
  cpu                      = "256"
  memory                   = "512"
  execution_role_arn       = aws_iam_role.ecs_execution_role.arn
  task_role_arn            = aws_iam_role.ecs_task_role.arn

  container_definitions = jsonencode([
    {
      name  = "${local.bff_name}-container"
      image = "${aws_ecr_repository.bff.repository_url}:latest"
      portMappings = [
        {
          containerPort = 80
          hostPort      = 80
        }
      ]

      logConfiguration = {
        logDriver = "awslogs"
        options = {
          "awslogs-group"         = aws_cloudwatch_log_group.bff.name
          "awslogs-region"        = "us-east-2"
          "awslogs-stream-prefix" = "ecs"
        }
      }
    }
  ])
}

resource "aws_ecs_service" "api" {
  name            = local.api_name
  cluster         = aws_ecs_cluster.cluster.id
  task_definition = aws_ecs_task_definition.api_task.arn
  desired_count   = var.desired_tasks
  launch_type     = "FARGATE"

  network_configuration {
    subnets          = [aws_subnet.subnet.id]
    security_groups  = [aws_security_group.ecs_sg.id]
    assign_public_ip = true
  }
}

resource "aws_appautoscaling_target" "api" {
  max_capacity       = var.max_tasks
  min_capacity       = var.min_tasks
  resource_id        = "service/${aws_ecs_cluster.cluster.name}/${aws_ecs_service.api.name}"
  scalable_dimension = "ecs:service:DesiredCount"
  service_namespace  = "ecs"
}

resource "aws_appautoscaling_policy" "api" {
  name               = local.api_name
  policy_type        = "TargetTrackingScaling"
  resource_id        = aws_appautoscaling_target.api.resource_id
  scalable_dimension = aws_appautoscaling_target.api.scalable_dimension
  service_namespace  = aws_appautoscaling_target.api.service_namespace

  target_tracking_scaling_policy_configuration {
    target_value = 50.0
    predefined_metric_specification {
      predefined_metric_type = "ECSServiceAverageCPUUtilization"
    }
  }
}

resource "aws_ecs_service" "bff" {
  name            = local.bff_name
  cluster         = aws_ecs_cluster.cluster.id
  task_definition = aws_ecs_task_definition.bff_task.arn
  desired_count   = var.desired_tasks
  launch_type     = "FARGATE"

  network_configuration {
    subnets          = [aws_subnet.subnet.id]
    security_groups  = [aws_security_group.ecs_sg.id]
    assign_public_ip = true
  }
}

resource "aws_appautoscaling_target" "bff" {
  max_capacity       = var.max_tasks
  min_capacity       = var.min_tasks
  resource_id        = "service/${aws_ecs_cluster.cluster.name}/${aws_ecs_service.bff.name}"
  scalable_dimension = "ecs:service:DesiredCount"
  service_namespace  = "ecs"
}

resource "aws_appautoscaling_policy" "bff" {
  name               = local.bff_name
  policy_type        = "TargetTrackingScaling"
  resource_id        = aws_appautoscaling_target.bff.resource_id
  scalable_dimension = aws_appautoscaling_target.bff.scalable_dimension
  service_namespace  = aws_appautoscaling_target.bff.service_namespace

  target_tracking_scaling_policy_configuration {
    target_value = 50.0
    predefined_metric_specification {
      predefined_metric_type = "ECSServiceAverageCPUUtilization"
    }
  }
}

resource "aws_cloudfront_distribution" "s3_distribution" {
  origin {
    domain_name = aws_s3_bucket.bucket.bucket_regional_domain_name
    origin_id   = "S3Origin"
    s3_origin_config {
      origin_access_identity = aws_cloudfront_origin_access_identity.oai.cloudfront_access_identity_path
    }
  }

  enabled         = true
  is_ipv6_enabled = true
  comment         = "CloudFront Distribution for ECS2"

  default_cache_behavior {
    allowed_methods  = ["DELETE", "GET", "HEAD", "OPTIONS", "PATCH", "POST", "PUT"]
    cached_methods   = ["GET", "HEAD"]
    target_origin_id = "S3Origin"
    forwarded_values {
      query_string = true
      headers      = []

      cookies {
        forward = "none"
      }
    }

    viewer_protocol_policy = "redirect-to-https"
    min_ttl                = 0
    default_ttl            = 3600
    max_ttl                = 86400
  }

  restrictions {
    geo_restriction {
      restriction_type = "none"
    }
  }

  viewer_certificate {
    cloudfront_default_certificate = true
  }
}

resource "aws_security_group_rule" "cloudfront_to_bff_ipv4" {
  security_group_id = aws_security_group.ecs_sg.id

  type        = "ingress"
  from_port   = 80
  to_port     = 80
  protocol    = "tcp"
  cidr_blocks = ["0.0.0.0/0"]
}

resource "aws_security_group_rule" "cloudfront_to_bff_ipv6" {
  security_group_id = aws_security_group.ecs_sg.id

  type             = "ingress"
  from_port        = 80
  to_port          = 80
  protocol         = "tcp"
  ipv6_cidr_blocks = ["::/0"]
}


output "rds_endpoint" {
  value = aws_db_instance.rds.endpoint
}

output "api_service_url" {
  value = aws_ecs_service.api.id
}

output "bff_service_url" {
  value = aws_ecs_service.bff.id
}

output "cloudfront_distribution_url" {
  value = aws_cloudfront_distribution.s3_distribution.domain_name
}

output "ecr_api_url" {
  value = aws_ecr_repository.api.repository_url
}