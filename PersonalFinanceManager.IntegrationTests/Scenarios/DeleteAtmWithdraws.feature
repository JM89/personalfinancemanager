Feature: DeleteAtmWithdraws
	Delete Atm withdraws

@deleteatmwithdraw
Scenario: Delete Atm withdraws
	Given I have accessed the ATM Withdraw List page
	And I have at least one ATM Withdraw in the list
	When I click on delete for the first ATM Withdraw
	And I confirm the deletion
	Then the ATM Withdraw has been removed
	Then the source account is updated
	Then a mouvement entry has been saved