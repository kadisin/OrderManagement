# Resource Group
resource "azurerm_resource_group" "rg" {
  name     = var.resource_group_name
  location = var.location
}

# Azure Service Bus Namespace
resource "azurerm_servicebus_namespace" "sb_namespace" {
  name                = var.servicebus_namespace_name
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  sku                 = "Standard"
}

# Azure Service Bus Topic
resource "azurerm_servicebus_topic" "sb_topic" {
  name                = var.servicebus_topic_name
  resource_group_name = azurerm_resource_group.rg.name
  namespace_name      = azurerm_servicebus_namespace.sb_namespace.name
}

# Azure Kubernetes Service (AKS)
resource "azurerm_kubernetes_cluster" "aks" {
  name                = var.aks_cluster_name
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  dns_prefix          = "order-notif"

  default_node_pool {
    name       = "default"
    node_count = 2
    vm_size    = "Standard_DS2_v2"
  }

  identity {
    type = "SystemAssigned"
  }

  tags = {
    Environment = "Dev"
  }
}


# Azure SQL Server
resource "azurerm_mssql_server" "sql_server" {
  name                         = "notifsqlserver01"
  resource_group_name          = azurerm_resource_group.main.name
  location                     = azurerm_resource_group.main.location
  version                      = "12.0"
  administrator_login          = var.sql_admin_login
  administrator_login_password = var.sql_admin_password

  tags = {
    environment = "dev"
  }
}

# Azure Key Vault
resource "azurerm_key_vault" "kv" {
  name                        = "notifkvvault01"
  location                    = azurerm_resource_group.main.location
  resource_group_name         = azurerm_resource_group.main.name
  tenant_id                   = data.azurerm_client_config.current.tenant_id
  sku_name                    = "standard"
  purge_protection_enabled    = true
  soft_delete_retention_days  = 7

  access_policy {
    tenant_id = data.azurerm_client_config.current.tenant_id
    object_id = data.azurerm_client_config.current.object_id

    secret_permissions = [
      "Get", "List", "Set", "Delete"
    ]
  }
}

# access_policy
data "azurerm_client_config" "current" {}