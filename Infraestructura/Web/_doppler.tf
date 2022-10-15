terraform {
  required_providers {
    doppler = {
      source  = "DopplerHQ/doppler"
      version = "1.1.2"
    }
  }
}

provider "doppler" {
  doppler_token = var.doppler_token
}

data "doppler_secrets" "this" {}
