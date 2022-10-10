# Declarar tipo y versión de proveedor en cloud
terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = ">=3.0.0"
    }
    random = {
      version = "3.1.0"
    }
    doppler = {
      source  = "DopplerHQ/doppler"
      version = "1.1.2"
    }
  }

  # store para el tfstate en la nube
  backend "azurerm" {
    resource_group_name  = "ccps-rg"
    storage_account_name = "ccpsazurest01"
    container_name       = "ccpsterraformstatefile"
    key                  = "ccpsterraform.tfstate"
  }
}

# Configurar proveedor de Azure
provider "azurerm" {
  features {
    cognitive_account {
      purge_soft_delete_on_destroy = true
    }
  }
}
