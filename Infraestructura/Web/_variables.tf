variable "prefix" {
  description = "Prefijo para los recursos creados en Azure"
  default     = "bbva"
}

variable "location" {
  description = "Ubicación preferente para crear los recursos"
  default     = "Central US"
}

variable "doppler_token" {
  type        = string
  description = "Token de auth con Doppler"
}
