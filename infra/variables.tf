variable "region" {
  default = "us-east-1"
}

variable "vpc_id" {}

variable "subnet_ids" {
  type = list(string)
  default = [ "subnet-01ee83d367e17dbf4", "subnet-03e19f74abd507b77" ]
}

variable "ecr_image" {}
