# Personal Finance Manager

PFM is a web application to manage your personal finance and visualise your expenses in a dashboard.

![PFM.png](./Documentation/Pictures/PFM.png)

The main features are: 
* Management of bank accounts
* Dashboard of expenses, incomes & savings
* Import of historic movements using CSV files

The detailed specifications are detailed [here](https://github.com/JM89/personalfinancemanager/wiki/Functional-Requirements)

## General Architecture & Technologies

### Overview

![Architecture-C4-Container.png](/Documentation/Pictures/Architecture/Architecture-C4-Container.png)

The evolution of the architecture is documented in this wiki page: [System Architecture Evolution](https://github.com/JM89/personalfinancemanager/wiki/System-Architecture-Evolution). 

## Getting Started

### Secrets

To run from local machine, you will need a GitHub token to access the private GitHub packages. 

In the `./PFM.Infra/configs`, you will find an .env-example file. If you copy this file and name it `.env` in the same location, the `run-locally.sh` script will pick it up automatically. 

:warning: This file is part of gitignore. It will contains sensitive data, do not commit the .env file. 

### Shared and specific infrastructure

Some infrastructure resources are shared accross different projects (e.g. SQL server, SEQ, AWS), some are specifics to each apps (e.g. creation DB, a SQS queue). The resources available in the PFM.Infra folder, contains the shared resources. 

It includes:
- [x] SQL Server instance: a single container is used for several isolated DB, to reduce the setup time, space and memory in local machine. 
- [x] SEQ for logging purpose
- [x] Localstack for AWS resources.

To get started, run the following command:

```shell
sh ./run-locally.sh
```

The command will also start all the application-specific docker-compose files.

### Debug the main API

Check the documentation [here](./PFM.Api/README.md).

### Debug the Auth API

Check the documentation [here](./PFM.Auth.Api/README.md).

### Start using the website

1. First "Register", then login with this new user account. 
2. Set User Profile (top menu) 
3. Configure your data: Country, Currency, Expense Types, Bank and Accounts (Configuration menu). 
4. Start creating movements from the Account Management dashboard screen

## Useful Links

|Resources|Docker|Debug Mode|Internal Docker|
|---|---|---|---|
|App - PFM.Website|N/A|http://localhost:54401/|N/A|
|App - PFM.Api - Endpoints|http://localhost:5001/api|https://localhost:7098/api|https://pfm-api:5001/api|
|App - PFM.Api - Swagger|http://localhost:5001/swagger/index.html|https://localhost:7098/swagger/index.html|N/A|
|App - PFM.Auth.Api|http://localhost:5000|https://localhost:4000|https://pfm-auth-api:5000|
|SEQ - Log Ingest|http://localhost:5341|http://localhost:5341|http://seq:5341|
|SEQ - UI|http://localhost:80|http://localhost:80|http://seq:80|
|SQL Server|localhost,1433|localhost,1433|db-server,1433|
|Localstack|http://localhost:4566|http://localhost:4566|http://localstack:4566|