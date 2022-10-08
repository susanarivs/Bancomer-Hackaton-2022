# Crear grupo de recursos
resource "azurerm_resource_group" "bbvatf-rg" {
  name     = "${var.prefix}-rg"
  location = var.location
}
