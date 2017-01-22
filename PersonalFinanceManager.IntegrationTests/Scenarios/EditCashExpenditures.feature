Feature: EditCashExpenditures
	Edit expenditures with payment method Cash

@editcashexpenditure
Scenario: Edit the amount of Cash expenditures
	Given I have accessed the Expenditures List page
	And I have at least one expenditure with this payment method in the list
	When I click on edit for this expenditure
	And I edit the Cost
	And I click on the Save button
	Then the expenditure has been updated
	Then the source account is unchanged
	Then the target atm withdraw is updated
	Then a mouvement entry has been saved

@editcashexpenditure
Scenario: Edit the amount and the ATM withraw of Cash expenditures
	Given I have accessed the Expenditures List page
	And I have at least one expenditure with this payment method in the list
	When I click on edit for this expenditure
	And I edit the Cost
	And I select another ATM withdraw
	And I click on the Save button
	Then the expenditure has been updated
	Then the source account is unchanged
	Then the old atm withdraw is updated
	Then the new atm withdraw is updated
	Then a mouvement entry has been saved

@editcashexpenditure
Scenario: Edit the amount and the payment method to Common Expenditures
	Given I have accessed the Expenditures List page
	And I have at least one expenditure with this payment method in the list
	When I click on edit for this expenditure
	And I edit the Cost
	And I change the payment method to Common Expenditures
	And I click on the Save button
	Then the expenditure has been updated
	Then the source account is updated
	Then the old atm withdraw is updated
	Then a mouvement entry has been saved

@editcashexpenditure
Scenario: Edit the amount of Internal Transfer expenditures
	Given I have accessed the Expenditures List page
	And I have at least one expenditure with this payment method in the list
	When I click on edit for this expenditure
	And I edit the Cost
	And I change the payment method to Internal Transfer Expenditures
	And I select a target account
	And I click on the Save button
	Then the expenditure has been updated
	Then the source account is updated
	Then the target account is updated 
	Then the old atm withdraw is updated
	Then an income has been updated
	Then a mouvement entry has been saved