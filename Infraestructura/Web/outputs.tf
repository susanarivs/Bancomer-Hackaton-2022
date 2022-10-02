output "app_service_name" {
  value = azurerm_windows_web_app.bbvatf-back.name
}

output "app_service_default_hostname" {
  value = "https://${azurerm_windows_web_app.bbvatf-back.default_hostname}"
}
