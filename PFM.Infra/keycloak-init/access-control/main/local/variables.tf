variable "openid_client_pfm_secret" {
    description = "Client secret for PFM so we do not have to regenerate the key all the time. This would need to be moved in some secret storage."
    type        = string
    default     = "RTkyA3RNh4cHHhS8ftXe17WOQu9a0Jjd"
}

variable "openid_client_pfm_bank_updater_secret" {
    description = "Client secret for PFM Bank Updater service account so we do not have to regenerate the key all the time. This would need to be moved in some secret storage."
    type        = string
    default     = "RTkyA3RNh4cHHhS8ftXe17WOQu9a0Jje"
}

variable "openid_client_pfm_mvt_aggregator_secret" {
    description = "Client secret for PFM Movement Aggregator service account so we do not have to regenerate the key all the time. This would need to be moved in some secret storage."
    type        = string
    default     = "RTkyA3RNh4cHHhS8ftXe17WOQu9a0Jjf"
}