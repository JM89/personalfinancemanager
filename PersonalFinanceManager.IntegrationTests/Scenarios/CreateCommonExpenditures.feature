Feature: CreateCommonExpenditures
	Create expenditures with payment method Common expenditures

@createcommonexpenditure
Scenario: Create Common expenditures
	Given I have accessed the Expenditures List page
	And I have clicked on the Create button
	When I enter Description
	And I enter a Cost
	And I select the first expenditure type
	And I select the CB payment type 
	And I click on the Save button
	Then the Expenditure Has Been Created
	Then the source account is updated
	Then a mouvement entry has been saved