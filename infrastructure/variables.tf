variable "location" {
  type        = string
  default     = "westeurope"
  description = "Deployment region"
}

variable "resource_group_name" {
  type        = string
  default     = "order-notification-rg"
  description = "Resource group name"
}

variable "servicebus_namespace_name" {
  type        = string
  default     = "orderbusnamespace"
}

variable "servicebus_topic_name" {
  type        = string
  default     = "ordercreatedtopic"
}

variable "aks_cluster_name" {
  type        = string
  default     = "order-cluster"
}

variable "sql_admin_login" {
  description = "Admin login for SQL Server"
  default     = "sqladmin"
}

variable "sql_admin_password" {
  description = "Admin password (should be overridden via tfvars or env)"
  sensitive   = true
}