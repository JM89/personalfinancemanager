Feature: CreateSavings
	Create savings

@createsaving
Scenario: Create Successfully Savings
	Given I have accessed the Saving List page
	And I have clicked on the Create button
	When I enter Amount 
	And I select the first Saving Account
	And I click on the Save button
	Then the Saving Has Been Created
	Then the source account is updated
	Then the target account is updated
	Then an income has been created
	Then a mouvement entry has been saved