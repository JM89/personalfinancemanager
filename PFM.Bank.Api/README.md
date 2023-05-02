# PFM Bank API

PFM Bank Api handles the management of banks and bank accounts, and list registered countries and currencies.

## Getting Started

### Debug the API

Open the solution and make sure you set Api as the start up project. 

The API should open in the browser directly: http://localhost:52688/swagger/index.html.

The application can be found [here](http://localhost/#/events?filter=Application%20%3D%20'PFM.Api').

### Test the API

All actions require to be authenticated. Registering a new user can be done in the PFM Auth API (http://localhost:5000).

Start by registering a new user:

```json
{
  "email": "test@test.com",
  "password": "Helloworld123!"
}
```

![](../Documentation/Pictures/API-Register.png)

The response returned by this call is a token. 

Click on the "Authorize" button at the top of the page, enter "Bearer ", copy the token and Authorize:

![](../Documentation/Pictures/API-BearerToken.PNG)

You can now access the actions.
