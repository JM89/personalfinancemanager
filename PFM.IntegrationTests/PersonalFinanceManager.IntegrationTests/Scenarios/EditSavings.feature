Feature: EditSavings
	Edit savings

@editsaving
Scenario: Edit the amount of a saving
	Given I have accessed the Saving List page
	And I have at least one saving in the list
	When I click on edit for the first saving
	And I edit the Amount
	And I click on the Save button
	Then the Saving has been updated
	Then the source account is updated
	Then the target account is updated 
	Then an income has been updated
	Then a mouvement entry has been saved

@editsaving
Scenario: Edit the target account and the amount of a saving
	Given I have accessed the Saving List page
	And I have at least one saving in the list
	When I click on edit for the first saving
	And I edit the Amount
	And I select another account
	And I click on the Save button
	Then the Saving has been updated
	Then the source account is updated
	Then the old target account is updated 
	Then the new target account is updated 
	Then an income has been updated
	Then a mouvement entry has been saved
