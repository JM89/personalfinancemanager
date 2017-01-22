Feature: CreateCashExpenditures
	Create expenditures with payment method Cash

@createcashexpenditure
Scenario: Create Cash expenditures
	Given I have accessed the Expenditures List page
	And I have clicked on the Create button
	When I enter Description
	And I enter a Cost
	And I select the first expenditure type
	And I select the Cash payment type 
	And I select an ATM withdraw
	And I click on the Save button
	Then the Expenditure Has Been Created
	Then the source account is unchanged
	Then the target atm withdraw is updated
	Then a mouvement entry has been saved