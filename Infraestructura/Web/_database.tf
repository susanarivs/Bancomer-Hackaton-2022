# Crear el servidor de base de datos
resource "azurerm_sql_server" "bbvatf-srv" {
  name                         = "${var.prefix}-db-sqlsvr"
  location                     = azurerm_resource_group.bbvatf-rg.location
  resource_group_name          = azurerm_resource_group.bbvatf-rg.name
  version                      = "12.0"
  administrator_login          = "4dm1n157r470r"
  administrator_login_password = "4-v3ry-53cr37-p455w0rd"
}

# Crear la base de datos
resource "azurerm_sql_database" "bbvatf-db" {
  name                             = "${var.prefix}-db"
  location                         = azurerm_resource_group.bbvatf-rg.location
  resource_group_name              = azurerm_resource_group.bbvatf-rg.name
  server_name                      = azurerm_sql_server.bbvatf-srv.name
  edition                          = "Basic"
  collation                        = "Modern_Spanish_CI_AS" # español moderno
  create_mode                      = "Default"
  requested_service_objective_name = "Basic"
}

# Reglas en el firewall
resource "azurerm_sql_firewall_rule" "bbvatf-fw" {
  name                = "${var.prefix}-fw-rules"
  resource_group_name = azurerm_resource_group.bbvatf-rg.name
  server_name         = azurerm_sql_server.bbvatf-srv.name
  start_ip_address    = "0.0.0.0"
  end_ip_address      = "255.255.255.255" # cualquier direccion solo en dev
}
