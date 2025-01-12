resource "aws_ecs_cluster" "ecs_cluster" {
  name = "ecs-fargate-cluster"
}

resource "aws_ecs_task_definition" "task_definition" {
  family                   = "ecs-fargate-task"
  network_mode             = "awsvpc"
  requires_compatibilities = ["FARGATE"]
  cpu                      = "256" # Ajuste conforme necessário
  memory                   = "512" # Ajuste conforme necessário
  execution_role_arn       = aws_iam_role.ecs_task_execution_role.arn

  container_definitions = jsonencode([{
    name      = "stocks-aws"
    image     = var.ecr_image
    essential = true
    logConfiguration = {
      logDriver = "awslogs"
      options = {
        "awslogs-group"         = "/ecs/stocks-aws"
        "awslogs-region"        = var.region
        "awslogs-stream-prefix" = "ecs"
      }
    }
  }])

  depends_on = [ 
    aws_iam_role.ecs_task_execution_role
  ]
}

resource "aws_ecs_service" "ecs_service" {
  name            = "ecs-fargate-service"
  cluster         = aws_ecs_cluster.ecs_cluster.id
  task_definition = aws_ecs_task_definition.task_definition.arn
  desired_count   = 1
  launch_type     = "FARGATE"

  network_configuration {
    subnets         = var.subnet_ids
    security_groups = [aws_security_group.ecs_service_sg.id]
  }

  depends_on = [ 
    aws_ecs_cluster.ecs_cluster,
    aws_ecs_task_definition.task_definition,
    aws_security_group.ecs_service_sg
  ]
}

resource "aws_security_group" "ecs_service_sg" {
  name        = "ecs-fargate-sg"
  description = "Allow traffic for ECS Fargate service"
  vpc_id      = var.vpc_id

  ingress {
    from_port   = 8080
    to_port     = 8081
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }
}

resource "aws_iam_role" "ecs_task_execution_role" {
  name = "ecsTaskExecutionRoleStocks"

  assume_role_policy = jsonencode({
    Version = "2012-10-17"
    Statement = [{
      Action    = "sts:AssumeRole"
      Effect    = "Allow"
      Principal = {
        Service = "ecs-tasks.amazonaws.com"
      }
    }]
  })
}

resource "aws_iam_policy_attachment" "ecs_task_execution_policy" {
  name       = "ecs-task-execution-policy"
  roles      = [aws_iam_role.ecs_task_execution_role.name]
  policy_arn = "arn:aws:iam::aws:policy/service-role/AmazonECSTaskExecutionRolePolicy"

  depends_on = [ 
    aws_iam_role.ecs_task_execution_role
  ]
}
