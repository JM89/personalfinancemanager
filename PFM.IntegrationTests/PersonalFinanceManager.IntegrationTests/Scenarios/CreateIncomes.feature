Feature: CreateIncomes
	Create incomes

@createincome
Scenario: Create Incomes
	Given I have accessed the Income List page
	And I have clicked on the Create button
	When I enter a description
	And I enter Cost 
	And I click on the Save button
	Then the source account is updated
	Then an income has been created
	Then a mouvement entry has been saved