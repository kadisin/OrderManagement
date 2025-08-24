output "namespace_name" {
  value = azurerm_servicebus_namespace.sb.name
}

output "topic_name" {
  value = azurerm_servicebus_topic.topic.name
}

output "subscription_name" {
  value = azurerm_servicebus_subscription.subscription.name
}
