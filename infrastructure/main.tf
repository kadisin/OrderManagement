resource "azurerm_resource_group" "main" {
  name     = var.resource_group_name
  location = var.location
}

module "aks" {
  source              = "./modules/aks"
  resource_group_name = azurerm_resource_group.main.name
  location            = var.location
  node_count          = var.aks_node_count
  node_size           = var.aks_node_size
}

module "servicebus" {
  source              = "./modules/servicebus"
  resource_group_name = azurerm_resource_group.main.name
  location            = var.location
  sku                 = var.servicebus_sku
  topic_name          = "orders-topic"
  subscription_name   = "notifications-subscription"
}

module "sql" {
  source              = "./modules/sql"
  resource_group_name = azurerm_resource_group.main.name
  location            = var.location
  admin_username      = var.sql_admin_username
  admin_password      = var.sql_admin_password
  database_name       = "orderdb"
}
