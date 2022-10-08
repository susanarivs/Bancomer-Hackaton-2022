# Crear app service plan windows
resource "azurerm_service_plan" "bbvatf-plan" {
  name                = "${var.prefix}-asp"
  location            = azurerm_resource_group.bbvatf-rg.location
  resource_group_name = azurerm_resource_group.bbvatf-rg.name
  os_type             = "Windows"
  sku_name            = "B1"
}

# Crear app service para el back
resource "azurerm_windows_web_app" "bbvatf-back" {
  name                = "${var.prefix}-back"
  location            = azurerm_resource_group.bbvatf-rg.location
  resource_group_name = azurerm_resource_group.bbvatf-rg.name
  service_plan_id     = azurerm_service_plan.bbvatf-plan.id

  site_config {
    always_on = true
    application_stack {
      current_stack  = "dotnetcore"
      dotnet_version = "v6.0"
    }
  }

  app_settings = {
    "ASPNETCORE_ENVIRONMENT" = "Development"
  }

  https_only = true
}