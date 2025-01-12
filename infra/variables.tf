variable "region" {
  default = "us-east-1"
}

variable "vpc_id" {}

variable "subnet_ids" {
  type = list(string)
  default = [ "subnet-1bfe087d", "subnet-1bfe087d", "subnet-5062e41d" ]
}

variable "ecr_image" {}
