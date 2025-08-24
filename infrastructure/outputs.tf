output "aks_cluster_name" {
  description = "Name of the AKS cluster"
  value       = module.aks.cluster_name
}

output "aks_cluster_resource_group" {
  description = "Resource group of the AKS cluster"
  value       = module.aks.resource_group_name
}

output "servicebus_namespace_name" {
  description = "Azure Service Bus Namespace"
  value       = module.servicebus.namespace_name
}

output "servicebus_topic_name" {
  description = "Azure Service Bus Topic"
  value       = module.servicebus.topic_name
}

output "sql_server_name" {
  description = "Azure SQL Server name"
  value       = module.sql.sql_server_name
}

output "sql_database_name" {
  description = "Azure SQL Database name"
  value       = module.sql.sql_database_name
}
