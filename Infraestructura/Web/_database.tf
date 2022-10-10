# Crear el servidor de base de datos
resource "azurerm_mssql_server" "bbvatf-srv" {
  name                         = "${var.prefix}-db-sqlsvr"
  location                     = azurerm_resource_group.bbvatf-rg.location
  resource_group_name          = azurerm_resource_group.bbvatf-rg.name
  version                      = "12.0"
  administrator_login          = "4dm1n157r470r"
  administrator_login_password = "4-v3ry-53cr37-p455w0rd"
  minimum_tls_version          = "1.2"

  azuread_administrator {
    login_username = "AzureAD Admin"
    object_id      = "00000000-0000-0000-0000-000000000000"
  }

  tags = {
    environment = "production"
  }
}

# Crear la base de datos
resource "azurerm_mssql_database" "bbvatf-db" {
  name           = "${var.prefix}-db"
  server_id      = azurerm_mssql_server.bbvatf-srv.id
  collation      = "SQL_Latin1_General_CP1_CI_AS"
  license_type   = "LicenseIncluded"
  max_size_gb    = 4
  read_scale     = false
  zone_redundant = false
  sku_name       = "Basic"
  create_mode    = "Default"
  geo_backup_enabled = false
}

# Reglas en el firewall
resource "azurerm_mssql_firewall_rule" "bbvatf-fw" {
  name             = "${var.prefix}-fw-rules"
  server_id        = azurerm_mssql_server.bbvatf-srv.id
  start_ip_address = "0.0.0.0"
  end_ip_address   = "255.255.255.255" # cualquier direccion solo en dev
}
