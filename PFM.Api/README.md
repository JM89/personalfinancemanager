# PFM API

This API has currently a monolithic architecture.  

## Pre-requisites

- [x] .NET Core 2.1 SDK
- [x] Docker

## Getting Started

### Run the dependencies

To start the dependencies of the api, just run the following command from the root folder:

```shell
sh ./run-locally.sh
```

This will start:
- [x] A SQL Server instance
- [x] A SQL script to build the main PFM DB with seed data (run once)

### Debug the API

Open the solution and make sure you set PFM.Api as the start up project. 

The API should open in the browser directly: http://localhost:52688/swagger/index.html.

### Test the API

All actions require to be authenticated. 

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

You can now access the actions, for instance, Payment methods are part of the seed data:

![](../Documentation/Pictures/API-GetPaymentMethods.png)

Some actions require a user-id that can be found when Login. 

![](../Documentation/Pictures/API-GetBankByUserId.PNG)