variable "resource_group_name" {
  type = string
}

variable "location" {
  type = string
}

variable "sku" {
  type    = string
  default = "Standard"
}

variable "topic_name" {
  type = string
}

variable "subscription_name" {
  type = string
}
