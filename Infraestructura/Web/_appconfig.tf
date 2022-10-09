data "azurerm_client_config" "for_config" {}

resource "azurerm_role_assignment" "for_config" {
  scope                = azurerm_resource_group.bbvatf-rg.id
  role_definition_name = "App Configuration Data Owner"
  principal_id         = data.azurerm_client_config.for_config.object_id
}

resource "azurerm_app_configuration" "backend-config" {
  name                = "${var.prefix}-backend-config"
  location            = azurerm_resource_group.bbvatf-rg.location
  resource_group_name = azurerm_resource_group.bbvatf-rg.name
  sku                 = "free"

  identity {
    type = "SystemAssigned"
  }

  depends_on = [
    azurerm_role_assignment.for_config
  ]
}

# Valores de configuración
resource "azurerm_app_configuration_key" "ine-recaptchatoken" {
  configuration_store_id = azurerm_app_configuration.backend-config.id
  key                    = "recaptchatoken"
  label                  = "Secrets"
  value                  = "tmp"
}

resource "azurerm_app_configuration_key" "ine-sitetoken" {
  configuration_store_id = azurerm_app_configuration.backend-config.id
  key                    = "sitetoken"
  label                  = "Secrets"
  value                  = "tmp"
}

resource "azurerm_app_configuration_key" "ine-url" {
  configuration_store_id = azurerm_app_configuration.backend-config.id
  key                    = "ineurl"
  label                  = "Secrets"
  value                  = "tmp"
}

resource "azurerm_app_configuration_key" "ine-url-resultado" {
  configuration_store_id = azurerm_app_configuration.backend-config.id
  key                    = "ineurlresultado"
  label                  = "Secrets"
  value                  = "tmp"
}

resource "azurerm_app_configuration_key" "renapo-url" {
  configuration_store_id = azurerm_app_configuration.backend-config.id
  key                    = "renapourl"
  label                  = "Secrets"
  value                  = "tmp"
}

resource "azurerm_app_configuration_key" "imss-apiurl" {
  configuration_store_id = azurerm_app_configuration.backend-config.id
  key                    = "imssapiurl"
  label                  = "Secrets"
  value                  = "tmp"
}

resource "azurerm_app_configuration_key" "imss-apiurlum" {
  configuration_store_id = azurerm_app_configuration.backend-config.id
  key                    = "imssapiurlum"
  label                  = "Secrets"
  value                  = "tmp"
}

resource "azurerm_app_configuration_key" "imss-apiurlvg" {
  configuration_store_id = azurerm_app_configuration.backend-config.id
  key                    = "imssapiurlvg"
  label                  = "Secrets"
  value                  = "tmp"
}
