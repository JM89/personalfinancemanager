# PFM Bank Account Updater

PFM Bank Account Updater credites and debits when movements have been done on a bank account.

## Getting Started

The service needs to have valid credentials. Registering a new service account can be done in the PFM Auth API (http://localhost:5000).

Run the following command:

```shell
curl -X POST http://localhost:5000/users/register -d "{\"username\":\"pfm-bank-account-updater@gmail.com\", \"password\":\"String123456!\", \"firstname\": \"\", \"lastname\":\"\" }" -H "Content-Type: application/json"
```

The client Id and secret are set in the appSettings.json.