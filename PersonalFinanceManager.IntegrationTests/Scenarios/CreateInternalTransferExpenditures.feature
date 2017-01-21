Feature: CreateInternalTransferExpenditures
	Create expenditures with payment method Internal Transfer

@createinternaltransferexpenditure
Scenario: Create Internal Transfer expenditures
	Given I have accessed the Expenditures List page
	And I have clicked on the Create button
	When I enter Description
	And I enter a Cost
	And I select the first expenditure type
	And I select the Internal Transfer payment type 
	And I select the first target account
	And I click on the Save button
	Then the Expenditure Has Been Created
	Then the source account is updated
	Then the target account is updated
	Then an income has been created
	Then a mouvement entry has been saved