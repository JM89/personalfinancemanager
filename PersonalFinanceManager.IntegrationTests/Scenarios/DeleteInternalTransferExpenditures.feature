Feature: DeleteInternalTransferExpenditures
	Delete expenditures with payment method Internal Transfer

@deleteinternaltransferexpenditure
Scenario: Delete Internal Transfer expenditures
	Given I have accessed the Expenditures List page
	And I have at least one expenditure with this payment method in the list
	When I click on delete for this expenditure
	And I confirm the deletion
	Then the expenditure has been removed
	Then the source account is updated
	Then the target account is updated 
	Then an income has been removed
	Then a mouvement entry has been saved