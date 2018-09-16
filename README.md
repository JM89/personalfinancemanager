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
