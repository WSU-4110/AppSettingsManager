provider "aws" {
  region = "us-east-2"
}

locals {
  environment      = "test"
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

data "aws_vpc" "vpc"{
  default = true
}

resource "aws_subnet" "subnet" {
  cidr_block        = "10.0.1.0/24"
  vpc_id            = data.aws_vpc.vpc.id
  availability_zone = "us-east-2a"
}

resource "aws_subnet" "subnet2" {
  cidr_block        = "10.0.2.0/24"
  vpc_id            = data.aws_vpc.vpc.id
  availability_zone = "us-east-2b" # Make sure to use a different AZ than the first subnet
}


resource "aws_security_group" "ecs_sg" {
  name        = "${local.application_name}-ecs_sg"
  description = "ECS security group"
  vpc_id      = data.aws_vpc.vpc.id

  ingress {
    from_port   = 0
    to_port     = 65535
    protocol    = "tcp"
    cidr_blocks = ["10.0.0.0/8"]
  }
  egress {
    from_port   = 0
    protocol    = "-1"
    to_port     = 0
    cidr_blocks = ["0.0.0.0/0"]
  }
}

resource "aws_security_group" "rds_sg" {
  name        = "${local.application_name}-rds_sg"
  description = "RDS security group"
  vpc_id      = data.aws_vpc.vpc.id

  ingress {
    from_port       = 3306
    to_port         = 3306
    protocol        = "tcp"
    security_groups = [aws_security_group.ecs_sg.id]
  }
  egress {
    from_port   = 0
    protocol    = "-1"
    to_port     = 0
    cidr_blocks = ["0.0.0.0/0"]
  }
}

resource "aws_route_table" "route_table" {
  vpc_id = data.aws_vpc.vpc.id
}

resource "aws_route_table_association" "table1" {
  subnet_id      = aws_subnet.subnet.id
  route_table_id = aws_route_table.route_table.id
}

resource "aws_route_table_association" "table2" {
  subnet_id      = aws_subnet.subnet2.id
  route_table_id = aws_route_table.route_table.id
}

#resource "aws_vpc_endpoint" "s3" {
#  vpc_id            = data.aws_vpc.vpc.id
#  service_name      = "com.amazonaws.us-east-2.s3"
#  vpc_endpoint_type = "Interface"
#
#  security_group_ids = [aws_security_group.ecs_sg.id]
#  subnet_ids         = [aws_subnet.subnet.id, aws_subnet.subnet2.id]
#  
#  private_dns_enabled = true
#}
#
#resource "aws_vpc_endpoint" "ecr_dkr" {
#  vpc_id            = data.aws_vpc.vpc.id
#  service_name      = "com.amazonaws.us-east-2.ecr.dkr"
#  vpc_endpoint_type = "Interface"
#
#  security_group_ids = [aws_security_group.ecs_sg.id]
#  subnet_ids         = [aws_subnet.subnet.id, aws_subnet.subnet2.id]
#
#  private_dns_enabled = true
#}
#
#resource "aws_vpc_endpoint" "ecr_api" {
#  vpc_id            = data.aws_vpc.vpc.id
#  service_name      = "com.amazonaws.us-east-2.ecr.api"
#  vpc_endpoint_type = "Interface"
#
#  security_group_ids = [aws_security_group.ecs_sg.id]
#  subnet_ids         = [aws_subnet.subnet.id, aws_subnet.subnet2.id]
#
#  private_dns_enabled = true
#}
#
#resource "aws_vpc_endpoint" "logs" {
#  vpc_id            = data.aws_vpc.vpc.id
#  service_name      = "com.amazonaws.us-east-2.logs"
#  vpc_endpoint_type = "Interface"
#
#  security_group_ids = [aws_security_group.ecs_sg.id]
#  subnet_ids         = [aws_subnet.subnet.id, aws_subnet.subnet2.id]
#
#  private_dns_enabled = true
#}
#
#resource "aws_vpc_endpoint" "ssm" {
#  vpc_id            = data.aws_vpc.vpc.id
#  service_name      = "com.amazonaws.us-east-2.ssm"
#  vpc_endpoint_type = "Interface"
#
#  security_group_ids = [aws_security_group.ecs_sg.id]
#  subnet_ids         = [aws_subnet.subnet.id, aws_subnet.subnet2.id]
#
#  private_dns_enabled = true
#}

resource "aws_security_group" "alb_api" {
  name        = "${local.api_name}-alb"
  description = "Allow inbound traffic to ALB"
  vpc_id      = data.aws_vpc.vpc.id

  ingress {
    from_port   = 80
    to_port     = 80
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }
}

resource "aws_lb" "lb_api" {
  name               = "${local.api_name}-alb"
  internal           = false
  load_balancer_type = "application"
  security_groups    = [aws_security_group.alb_api.id]
  subnets            = [aws_subnet.subnet.id, aws_subnet.subnet2.id]
}

resource "aws_lb_target_group" "tg_api" {
  name        = "${local.api_name}-tg"
  port        = 80
  protocol    = "HTTP"
  target_type = "ip"
  vpc_id      = data.aws_vpc.vpc.id

  health_check {
    enabled             = true
    healthy_threshold   = 3
    interval            = 30
    matcher             = "200"
    path                = "/"
    protocol            = "HTTP"
    timeout             = 5
    unhealthy_threshold = 3
  }
}

resource "aws_lb_listener" "listener_api" {
  load_balancer_arn = aws_lb.lb_api.arn
  port              = 80
  protocol          = "HTTP"

  default_action {
    type             = "forward"
    target_group_arn = aws_lb_target_group.tg_api.arn
  }
}

resource "aws_security_group" "alb_bff" {
  name        = "${local.bff_name}-alb"
  description = "Allow inbound traffic to ALB"
  vpc_id      = data.aws_vpc.vpc.id

  ingress {
    from_port   = 80
    to_port     = 80
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }
}

resource "aws_lb" "lb_bff" {
  name               = "${local.bff_name}-alb"
  internal           = false
  load_balancer_type = "application"
  security_groups    = [aws_security_group.alb_bff.id]
  subnets            = [aws_subnet.subnet.id, aws_subnet.subnet2.id]
}

resource "aws_lb_target_group" "tg_bff" {
  name        = "${local.bff_name}-tg"
  port        = 80
  protocol    = "HTTP"
  target_type = "ip"
  vpc_id      = data.aws_vpc.vpc.id

  health_check {
    enabled             = true
    healthy_threshold   = 3
    interval            = 30
    matcher             = "200"
    path                = "/"
    protocol            = "HTTP"
    timeout             = 5
    unhealthy_threshold = 3
  }
}

resource "aws_lb_listener" "listener_bff" {
  load_balancer_arn = aws_lb.lb_bff.arn
  port              = 80
  protocol          = "HTTP"

  default_action {
    type             = "forward"
    target_group_arn = aws_lb_target_group.tg_bff.arn
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

  load_balancer {
    target_group_arn = aws_lb_target_group.tg_api.arn
    container_name   = "${local.api_name}-container"
    container_port   = 80
  }

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

  load_balancer {
    target_group_arn = aws_lb_target_group.tg_bff.arn
    container_name   = "${local.bff_name}-container"
    container_port   = 80
  }

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

resource "aws_api_gateway_rest_api" "api" {
  name = local.api_name
}

resource "aws_api_gateway_integration" "api" {
  rest_api_id = aws_api_gateway_rest_api.api.id
  resource_id = aws_api_gateway_rest_api.api.root_resource_id
  http_method = "ANY"

  type                    = "HTTP_PROXY"
  integration_http_method = "ANY"
  uri                     = aws_lb.lb_api.dns_name
}

resource "aws_api_gateway_deployment" "api" {
  depends_on = [aws_api_gateway_integration.api]

  rest_api_id = aws_api_gateway_rest_api.api.id
  stage_name  = "v1"
}

resource "aws_api_gateway_rest_api" "bff" {
  name = local.bff_name
}

resource "aws_api_gateway_integration" "bff" {
  rest_api_id = aws_api_gateway_rest_api.bff.id
  resource_id = aws_api_gateway_rest_api.bff.root_resource_id
  http_method = "ANY"

  type                    = "HTTP_PROXY"
  integration_http_method = "ANY"
  uri                     = aws_lb.lb_bff.dns_name
}

resource "aws_api_gateway_deployment" "bff" {
  depends_on = [aws_api_gateway_integration.bff]

  rest_api_id = aws_api_gateway_rest_api.bff.id
  stage_name  = "v1"
}

output "api_gateway_url" {
  value = "https://${aws_api_gateway_rest_api.api.id}.execute-api.us-east-2.amazonaws.com/v1"
}

output "bff_gateway_url" {
  value = "https://${aws_api_gateway_rest_api.bff.id}.execute-api.us-east-2.amazonaws.com/v1"
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