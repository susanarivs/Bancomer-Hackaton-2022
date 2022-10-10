resource "azurerm_cognitive_account" "bbvatf-form" {
  name                = "${var.prefix}-frm-ine"
  location            = azurerm_resource_group.bbvatf-rg.location
  resource_group_name = azurerm_resource_group.bbvatf-rg.name
  kind                = "FormRecognizer"

  sku_name = "S0"
}

resource "azurerm_storage_account" "bbvatf-form-store" {
  name                     = "almacendeines"
  location                 = azurerm_resource_group.bbvatf-rg.location
  resource_group_name      = azurerm_resource_group.bbvatf-rg.name
  account_tier             = "Standard"
  account_replication_type = "LRS"
  min_tls_version = "TLS1_2"
}

resource "azurerm_storage_container" "bbvatf-form-store-co" {
  name                  = "ines"
  storage_account_name  = azurerm_storage_account.bbvatf-form-store.name
  container_access_type = "private"
}