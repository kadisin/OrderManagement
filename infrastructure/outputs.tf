output "servicebus_topic_id" {
  value = azurerm_servicebus_topic.sb_topic.id
}

output "aks_cluster_name" {
  value = azurerm_kubernetes_cluster.aks.name
}
