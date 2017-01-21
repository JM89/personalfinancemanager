Feature: EditAtmWithdraws
	Edit Atm Withdraws

@editatmwithdraw
Scenario: Edit the cost of an ATM withdraws
	Given I have accessed the ATM Withdraw List page
	And I have at least one ATM withdraw in the list
	When I click on edit for the first ATM withdraw
	And I edit the Cost
	And I click on the Save button
	Then the ATM withdraw has been updated
	Then the source account is updated
	Then a mouvement entry has been saved
