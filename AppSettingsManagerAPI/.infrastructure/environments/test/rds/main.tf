provider "aws" {
  region = "us-east-2"
}

resource "aws_db_instance" "test_rds" {
  allocated_storage   = 20
  engine              = "mysql"
  engine_version      = "5.7"
  instance_class      = "db.t2.micro"
  db_name             = "appsettingsmanager"
  username            = "dbuser"
  password            = "dbpassword"
  identifier          = "appsettingsmanager-test"
  skip_final_snapshot = true
}

output "rds_connection_string" {
  value     = "mysql://${aws_db_instance.test_rds.username}:${aws_db_instance.test_rds.password}@${aws_db_instance.test_rds.endpoint}:${aws_db_instance.test_rds.port}/${aws_db_instance.test_rds.db_name}"
  sensitive = true
}

