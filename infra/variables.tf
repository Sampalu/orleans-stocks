variable "region" {
  default = "us-east-1"
}

variable "vpc_id" {}

variable "subnet_ids" {
  type = list(string)
  default = [ "subnet-016a0b9b578d41c29", "subnet-0f6183ddd4ce96326" ]
}

variable "ecr_image" {}
