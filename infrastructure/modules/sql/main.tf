resource "azurerm_sql_server" "sql_server" {
  name                         = "${var.resource_group_name}-sqlsrv"
  resource_group_name          = var.resource_group_name
  location                     = var.location
  version                      = "12.0"
  administrator_login          = var.admin_username
  administrator_login_password = var.admin_password

  minimum_tls_version = "1.2"
}

resource "azurerm_sql_database" "sql_database" {
  name                = var.database_name
  resource_group_name = var.resource_group_name
  location            = var.location
  server_name         = azurerm_sql_server.sql_server.name
  sku_name            = "Basic"
  max_size_gb         = 5
}
