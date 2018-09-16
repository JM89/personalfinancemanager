# Personal Finance Manager

PFM is a web application to manage your personal finance and visualise your expenses in a dashboard.

![PFM.png](https://github.com/JM89/personalfinancemanager/blob/master/Documentation/Pictures/PFM.png)

The main features are: 
* Management of bank accounts
* Dashboard of expenses, incomes & savings
* Import of historic movements using CSV files

The detailed specifications are detailed [here](https://github.com/JM89/personalfinancemanager/wiki/Functional-Requirements)

## General Architecture & Technologies

This project is made of 4 solutions:

* **PFM.Api**: ASP.NET Core 2.0 API, intented to run as a self-hosted API (Windows Service).
* **PFM.Website**: ASP.NET MVC Website, which would run on IIS
* **PFM.CommonLibraries**: NET Standard 2.0 Library for sharing common code between the two main solutions.
* **PFM.IntegrationTests**: Automation test solution, runnable by a CI tool

![General Architecture.png](https://github.com/JM89/personalfinancemanager/blob/master/Documentation/Pictures/GeneralArchitecture.png)

## Getting Started

### Debug the application

1. Open the solution PFM.API and restore Nuget packages. 
2. Create a Nuget Local package source and refence the shared libraries (rootproject/SharedLibs). Restore the missing packages.
3. Change the appsettings.json & appsettings.Development.json to reference a valid DB server and JWT configurations. 
4. In Package Manager Console, select DAL project and run the following commands to create the database, schema and insert metadata.
	* enable-migration
	* add-migration dbcreation
	* update-database
5. Set PFM.API project as startup project.
6. Run the API
7. Open the solution PFM.Website and restore Nuget packages. 
8. Change the web.config to reference the API.
9. Run the Website. 

### Start using the website

1. First "Register", then login with this new user account. 
2. Set User Profile (top menu) 
3. Configure your data: Country, Currency, Bank and Accounts (Configuration menu). 
4. Start creating movements from the Account Management dashboard screen