# Personal Finance Manager

PFM is a web application to manage your personal finance and visualise your expenses in a dashboard.

![PFM.png](./Documentation/Pictures/PFM.png)

The main features are: 
* Management of bank accounts
* Dashboard of expenses, incomes & savings
* Import of historic movements using CSV files

The detailed specifications are detailed [here](https://github.com/JM89/personalfinancemanager/wiki/Functional-Requirements)

## General Architecture & Technologies

:warning: The system is greatly outdated. As the previous attempt to rework the whole system was not successful, an iterative approach is adopted for this new phase of development. The evolution of the architecture is detailed in this wiki page: [System Architecture Evolution](https://github.com/JM89/personalfinancemanager/wiki/System-Architecture-Evolution). 

### Overview

![Architecture-C4-Container.png](/Documentation/Pictures/Architecture/Architecture-C4-Container.png)

## Getting Started

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
