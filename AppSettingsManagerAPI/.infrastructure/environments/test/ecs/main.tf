provider "aws" {
  region = "us-east-2"
}

locals {
  application_name             = "appsettingsmanager"
  environment                  = "test"
  instance_name                = "${local.application_name}-${local.environment}"
  abbreviated_application_name = "asm"
}

resource "aws_ecs_cluster" "my_cluster" {
  name = "${local.instance_name}-cluster"
}

resource "aws_ecr_repository" "my_repository" {
  name = local.instance_name
}

resource "aws_iam_role" "ecs_execution_role" {
  name = "${local.instance_name}-ecs-execution-role"

  assume_role_policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Action = "sts:AssumeRole"
        Effect = "Allow"
        Principal = {
          Service = "ecs-tasks.amazonaws.com"
        }
      }
    ]
  })

  tags = {
    Name = "ecs-execution-role"
  }
}

resource "aws_ecs_task_definition" "my_task_definition" {
  family                   = "my-task-family"
  network_mode             = "awsvpc"
  requires_compatibilities = ["FARGATE"]
  cpu                      = "256"
  memory                   = "512"
  execution_role_arn       = aws_iam_role.ecs_execution_role.arn

  container_definitions = jsonencode([
    {
      name  = "my-container"
      image = "${aws_ecr_repository.my_repository.repository_url}:latest"
      portMappings = [
        {
          containerPort = 80
        }
      ],
      essential = true
    }
  ])
}

resource "aws_ecs_service" "my_service" {
  name            = "${local.instance_name}-service"
  cluster         = aws_ecs_cluster.my_cluster.id
  task_definition = aws_ecs_task_definition.my_task_definition.arn
  desired_count   = 1
  launch_type     = "FARGATE"
  network_configuration {
    subnets          = ["${aws_subnet.east-2c.id}", "${aws_subnet.east-2b.id}"]
    assign_public_ip = true
    security_groups  = ["${aws_security_group.my_security_group.id}"]
  }
  load_balancer {
    target_group_arn = aws_lb_target_group.my_target_group.arn
    container_name   = "my-container"
    container_port   = 80
  }
}

resource "aws_security_group" "my_security_group" {
  name_prefix = "${local.instance_name}-sg"
  vpc_id      = aws_vpc.my_vpc.id

  ingress {
    from_port   = 80
    to_port     = 80
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }
}

resource "aws_subnet" "east-2c" {
  vpc_id            = aws_vpc.my_vpc.id
  cidr_block        = "10.0.3.0/24"
  availability_zone = "us-east-2c"
}

resource "aws_subnet" "east-2b" {
  vpc_id            = aws_vpc.my_vpc.id
  cidr_block        = "10.0.2.0/24"
  availability_zone = "us-east-2b"
}

resource "aws_vpc" "my_vpc" {
  cidr_block = "10.0.0.0/16"
}

resource "aws_lb_target_group" "my_target_group" {
  name_prefix = "${local.abbreviated_application_name}-tg"
  port        = 80
  protocol    = "HTTP"
  vpc_id      = aws_vpc.my_vpc.id
  target_type = "ip"
}

resource "aws_lb" "my_lb" {
  name               = "${local.environment}lb"
  internal           = false
  load_balancer_type = "application"

  subnet_mapping {
    subnet_id = aws_subnet.east-2c.id
  }

  subnet_mapping {
    subnet_id = aws_subnet.east-2b.id
  }

  access_logs {
    bucket = "my-bucket"
  }
}

resource "aws_lb_listener" "my_listener" {
  load_balancer_arn = aws_lb.my_lb.arn
  port              = "80"
  protocol          = "HTTP"

  default_action {
    type             = "forward"
    target_group_arn = aws_lb_target_group.my_target_group.arn
  }
}

resource "aws_internet_gateway" "my_internet_gateway" {
  vpc_id = aws_vpc.my_vpc.id

  tags = {
    Name = "${local.instance_name}-internet-gateway"
  }
}

resource "aws_route_table" "my_route_table" {
  vpc_id = aws_vpc.my_vpc.id

  route {
    cidr_block = "0.0.0.0/0"
    gateway_id = aws_internet_gateway.my_internet_gateway.id
  }

  tags = {
    Name = "${local.instance_name}-route-table"
  }
}

resource "aws_route_table_association" "my_route_table_association_1" {
  subnet_id      = aws_subnet.east-2c.id
  route_table_id = aws_route_table.my_route_table.id
}

resource "aws_route_table_association" "my_route_table_association_2" {
  subnet_id      = aws_subnet.east-2b.id
  route_table_id = aws_route_table.my_route_table.id
}


output "application_url" {
  value = "http://${aws_lb.my_lb.dns_name}"
}