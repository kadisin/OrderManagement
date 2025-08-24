resource "azurerm_servicebus_namespace" "sb" {
  name                = "${var.resource_group_name}-sbns"
  location            = var.location
  resource_group_name = var.resource_group_name
  sku                 = var.sku
}

resource "azurerm_servicebus_topic" "topic" {
  name                = var.topic_name
  namespace_name      = azurerm_servicebus_namespace.sb.name
  resource_group_name = var.resource_group_name
  enable_partitioning = true
}

resource "azurerm_servicebus_subscription" "subscription" {
  name                = var.subscription_name
  namespace_name      = azurerm_servicebus_namespace.sb.name
  topic_name          = azurerm_servicebus_topic.topic.name
  resource_group_name = var.resource_group_name
}
