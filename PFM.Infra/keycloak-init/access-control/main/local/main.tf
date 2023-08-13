resource "keycloak_realm" "realm" {
  realm = "pfm"
}

resource "keycloak_user" "user_with_initial_password" {
  realm_id = keycloak_realm.realm.id
  username = "jess"
  enabled  = true

  email      = "jm89.ic@gmail.com"
  first_name = "J"
  last_name  = "M"

  email_verified = true

  initial_password {
    value     = "SecurityMatters!123"
    temporary = false # should be true
  }
}

resource "keycloak_group_memberships" "group_members" {
  realm_id = keycloak_realm.realm.id
  group_id = keycloak_group.group.id

  members = [
    keycloak_user.user_with_initial_password.username
  ]
}

resource "keycloak_group" "group" {
  realm_id = keycloak_realm.realm.id
  name     = "group"
}

resource "keycloak_group_roles" "group_roles" {
  realm_id = keycloak_realm.realm.id
  group_id = keycloak_group.group.id

  role_ids = [
    data.keycloak_role.offline_access.id
  ]
}

data "keycloak_role" "offline_access" {
  realm_id = keycloak_realm.realm.id
  name     = "offline_access"
}

resource "keycloak_openid_client" "openid_client" {
  realm_id  = keycloak_realm.realm.id
  client_id = "pfm"

  name    = "Client App for enabling SSO on PFM Website"
  enabled = true

  direct_access_grants_enabled = true

  access_type = "CONFIDENTIAL"
  valid_redirect_uris = [
    "https://localhost:7142/signin-oidc"
  ]

  client_secret = var.openid_client_pfm_secret

  standard_flow_enabled = true

  login_theme = "keycloak"

  extra_config = {
    "product" = "pfm"
  }
}

resource "keycloak_openid_client" "pfm_bank_account_updater_openid_client" {
  realm_id  = keycloak_realm.realm.id
  client_id = "pfm-bank-account-updater"

  name    = "Service Account for PFM Bank Account Updater"
  enabled = true

  access_type              = "CONFIDENTIAL"
  service_accounts_enabled = true

  client_secret = var.openid_client_pfm_bank_updater_secret

  extra_config = {
    "product" = "pfm"
  }
}

resource "keycloak_openid_client" "pfm_mvt_aggregator_openid_client" {
  realm_id  = keycloak_realm.realm.id
  client_id = "pfm-movement-aggregator"

  name    = "Service Account for PFM Movement Aggregator"
  enabled = true

  access_type              = "CONFIDENTIAL"
  service_accounts_enabled = true

  client_secret = var.openid_client_pfm_mvt_aggregator_secret

  extra_config = {
    "product" = "pfm"
  }
}
