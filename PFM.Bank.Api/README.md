# PFM Bank API

PFM Bank Api handles the management of banks and bank accounts, and list registered countries and currencies.

## Getting Started

First of all, the infrastructure must have been started. 

### Debug the API

Open the solution and make sure you set Api as the start up project. 

The API should open in the browser directly: http://localhost:52688/swagger/index.html.

The application can be found [here](http://localhost/#/events?filter=Application%20%3D%20'PFM.Api').

### Test the API

All actions require to be authenticated.

To generate a token, you can use the pre-existing user 'jess' and 'pfm-bank-api' client ID. Please note that the client secrets will be regenerated everytime the application starts. This info can be retrieved using Keycloak administration console.

```bash
curl -v -L -X POST "http://localhost:8080/realms/pfm/protocol/openid-connect/token" \
-H 'Content-Type: application/x-www-form-urlencoded' \
--data-urlencode 'client_id=pfm-bank-api' \
--data-urlencode 'grant_type=password' \
--data-urlencode 'client_secret=<secret>' \
--data-urlencode 'scope=openid' \
--data-urlencode 'username=jess' \
--data-urlencode 'password=<user-password>' | jq .access_token
```

The bearer token generated can then be used in Swagger.

Click on the "Authorize" button at the top of the page, enter "Bearer ", copy the token and Authorize:

![](../Documentation/Pictures/API-BearerToken.PNG)

You can now access the actions.