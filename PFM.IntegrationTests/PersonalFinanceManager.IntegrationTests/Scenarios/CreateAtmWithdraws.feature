Feature: CreateAtmWithdraws
	Create ATM Withdraws

@createatmwithdraw
Scenario: Create ATM Withdraws
	Given I have accessed the ATM Withdraw List page
	And I have clicked on the Create button
	When I enter Cost 
	And I click on the Save button
	Then the source account is updated
	Then an ATM withdraw has been created
	Then a mouvement entry has been saved