provider "doppler" {
  doppler_token = ${{ secrets.DOPPLER_SCR }}
}

data "doppler_secrets" "this" {}
