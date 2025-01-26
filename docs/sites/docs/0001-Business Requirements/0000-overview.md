---
sidebar_position: 1
---

# Overview

# Requirement Definition

The use cases are described by a code according to the following descriptions:
* U_XX: Related to security including user account (authentication and authorization) and user profile. 
* MET_XX: Related to metadata of the system (accounts, currencies, countries…). 
* EI_XX: Related to entering expenses and incomes to the system. 
* D_XX: Related to visualizing dashboards. 
* BP_XX: Related to budget plan.
* AT_XX: Related to allowances and taxes. 

The use cases are more or less detailed depending of the complexity of the functionality. 

# Requirement Overview

| Use Case           | Name                                                 | Description                                                                                      | State                                                                             |
| ------------------ | ---------------------------------------------------- | ------------------------------------------------------------------------------------------------ | --------------------------------------------------------------------------------- |
| D_01               | Welcome Page                                         | Be able to show a user-specific home page.                                                       | :heavy_minus_sign:                                                                |
| [D_02](./D_02)     | Account Management Dashboard                         | Be able to visualize the expenses for an account in a dashboard.                                 | :heavy_check_mark:                                                                |
| U_01               | User Registering                                     | Register a new user so he can start to use the application                                       | :heavy_minus_sign:                                                                |
| U_02               | User Authentication                                  | Be able to log in a user so he can start to use the application                                  | :heavy_minus_sign:                                                                |
| U_03               | User Profile                                         | Be able to get some information about the user.                                                  | :heavy_minus_sign:                                                                |
| [MET_01](./MET_01) | Account Management                                   | Be able to list, create, update and delete accounts for the authenticated user                   | :heavy_check_mark:                                                                |
| [MET_02](./MET_02) | Bank Management                                      | Be able to list, create, update and delete banks for the authenticated user                      | :heavy_check_mark:                                                                |
| MET_03             | Country Management                                   | Be able to list, create, update and delete registered countries for the authenticated user       | :heavy_minus_sign:                                                                | :heavy_minus_sign: |
| MET_04             | Currency Management                                  | Be able to list, create, update and delete registered currencies for the authenticated user      | :heavy_minus_sign:                                                                | :heavy_minus_sign: |
| [MET_05](./MET_05) | Expense Type Management                              | Be able to list, create, update and delete categories of expenses for the authenticated user | :heavy_check_mark:                                                                |
| MET_06             | Payment Method                                       | Listing of static payment types                                                                  | :heavy_minus_sign:                                                                |
| MET_07             | Tax Type                                             | Listing of static tax types                                                                      | :heavy_minus_sign:                                                                |
| [EI_01](./EI_01)   | Incomes Management                                   | Be able to list, create and delete incomes for an account                                        | :heavy_check_mark:                                                                |
| [EI_02](./EI_02)   | ATM Withdraw Management                              | Be able to list ATM withdraws for an account                                                     | :heavy_check_mark:                                                                |
| [EI_03](./EI_03)   | Expense Listing (Common Features)                | Be able to list and filter expenses for an account.                                              | :heavy_check_mark:                                                                |
| [EI_04](./EI_04)   | Expense Management (Common Expense Strategy) | Be able to create / delete an expense, for CB Direct Debit and Transfer expenses.            | :heavy_check_mark:                                                                |
| [EI_05](./EI_05)   | Expense Management (ATM Withdraw Strategy)       | Be able to create / delete an expense, for Cash expenses.                                | :heavy_check_mark:                                                                |
| [EI_06](./EI_06)   | Expense Management (Internal Transfer Strategy)  | Be able to create / delete an expense, for Internal Transfer expenses.                       | :heavy_check_mark:                                                                |
| [EI_07](./EI_07)   | Savings Management                                   | Be able to list, create and delete savings for an account to a target account                    | :heavy_check_mark:                                                                |
| EI_08              | Movement Import                                      | Be able to import expenses and incomes by batch to an account                                    | :warning: [Issue #112](https://github.com/JM89/personalfinancemanager/issues/112) |
| [BP_01](./BP_01)   | Budget Management                                    | Be able to create / update / view a budget plan and start/stop it.                               |                                                                                   |
| AT_01              | Salary Management                                    | Be able to create / update / delete salary details.                                              | :heavy_minus_sign:                                                                |
| AT_02              | Pension Management                                   | Be able to create / update / delete pension details.                                             | :warning: [Issue #115](https://github.com/JM89/personalfinancemanager/issues/115) |
| AT_03              | Tax Management                                       | Be able to create / update / delete tax details.                                                 | :warning: [Issue #115](https://github.com/JM89/personalfinancemanager/issues/115) |