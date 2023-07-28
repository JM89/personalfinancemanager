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

resource "keycloak_openid_client" "pfm_website_openid_client" {
  realm_id  = keycloak_realm.realm.id
  client_id = "pfm-website"

  name    = "PFM Website"
  enabled = true

  direct_access_grants_enabled = true

  access_type = "CONFIDENTIAL"
  valid_redirect_uris = [
    "https://localhost:7142/signin-oidc"
  ]

  standard_flow_enabled = true

  login_theme = "keycloak"

  extra_config = {
    "is_api" = "false"
    "internet_facing" = "false"
  }
}

resource "keycloak_openid_client" "pfm_api_openid_client" {
  realm_id  = keycloak_realm.realm.id
  client_id = "pfm-api"

  name    = "PFM API"
  enabled = true

  direct_access_grants_enabled = true

  access_type = "CONFIDENTIAL"
  valid_redirect_uris = [
    "https://localhost:7142/signin-oidc"
  ]

  standard_flow_enabled = true

  login_theme = "keycloak"

  extra_config = {
    "is_api" = "true"
    "internet_facing" = "false"
  }
}