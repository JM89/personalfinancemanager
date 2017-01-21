Feature: DeleteSavings
	Delete savings

@deletesaving
Scenario: Delete Savings
	Given I have accessed the Saving List page
	And I have at least one saving in the list
	When I click on delete for the first saving
	And I confirm the deletion
	Then the Saving has been removed
	Then the source account is updated
	Then the target account is updated 
	Then an income has been removed
	Then a mouvement entry has been saved