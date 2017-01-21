Feature: DeleteIncomes
	Delete incomes

@deleteincome
Scenario: Delete Incomes
	Given I have accessed the Income List page
	And I have at least one income in the list
	When I click on delete for the first income
	And I confirm the deletion
	Then the income has been removed
	Then the source account is updated
	Then a mouvement entry has been saved