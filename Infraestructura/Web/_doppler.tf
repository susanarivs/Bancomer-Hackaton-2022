provider "doppler" {
  doppler_token = var.doppler_token
}

data "doppler_secrets" "this" {}
