Feature: EditIncomes
	Edit incomes

@editsaving
Scenario: Edit the cost of an income
	Given I have accessed the Income List page
	And I have at least one income in the list
	When I click on edit for the first income
	And I edit the Cost
	And I click on the Save button
	Then the Income has been updated
	Then the source account is updated
	Then a mouvement entry has been saved