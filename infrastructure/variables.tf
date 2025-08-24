variable "location" {
  description = "Azure region for all resources"
  type        = string
  default     = "westeurope"
}

variable "resource_group_name" {
  description = "Name of the Azure Resource Group"
  type        = string
  default     = "rg-order-management"
}

variable "aks_node_count" {
  description = "Number of AKS nodes"
  type        = number
  default     = 2
}

variable "aks_node_size" {
  description = "Size of AKS nodes"
  type        = string
  default     = "Standard_DS2_v2"
}

variable "sql_admin_username" {
  description = "SQL server admin username"
  type        = string
  default     = "sqladminuser"
}

variable "sql_admin_password" {
  description = "SQL server admin password"
  type        = string
  sensitive   = true
}

variable "servicebus_sku" {
  description = "Service Bus SKU"
  type        = string
  default     = "Standard"
}
