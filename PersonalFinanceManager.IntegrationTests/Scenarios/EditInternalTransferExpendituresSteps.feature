﻿Feature: EditInternalTransferExpenditures
	Edit expenditures with payment method Internal Transfer

@editinternaltransferexpenditure
Scenario: Edit the amount of an Internal Transfer expenditures
	Given I have accessed the Expenditures List page
	And I have at least one expenditure with this payment method in the list
	When I click on edit for this expenditure
	And I edit the Cost
	And I click on the Save button
	Then the expenditure has been updated
	Then the source account is updated
	Then the target account is updated 
	Then an income has been updated
	Then a mouvement entry has been saved

@editinternaltransferexpenditure
Scenario: Edit the target account and the amount of an Internal Transfer expenditures
	Given I have accessed the Expenditures List page
	And I have at least one expenditure with this payment method in the list
	When I click on edit for this expenditure
	And I edit the Cost
	And I select another account
	And I click on the Save button
	Then the expenditure has been updated
	Then the source account is updated
	Then the old target account is updated 
	Then the new target account is updated 
	Then an income has been updated
	Then a mouvement entry has been saved

@editinternaltransferexpenditure
Scenario: Edit the amount and the payment method to Common Expenditures
	Given I have accessed the Expenditures List page
	And I have at least one expenditure with this payment method in the list
	When I click on edit for this expenditure
	And I edit the Cost
	And I change the payment method to Common Expenditures
	And I click on the Save button
	Then the expenditure has been updated
	Then the source account is updated
	Then the old target account is updated 
	Then an income has been removed
	Then a mouvement entry has been saved

@editinternaltransferexpenditure
Scenario: Edit the amount and the payment method to Cash Expenditures
	Given I have accessed the Expenditures List page
	And I have at least one expenditure with this payment method in the list
	When I click on edit for this expenditure
	And I edit the Cost
	And I change the payment method to Cash Expenditures
	And I select an ATM withdraw
	And I click on the Save button
	Then the expenditure has been updated
	Then the old source account is updated
	Then the old target account is updated 
	Then the target atm withdraw is updated 
	Then an income has been removed
	Then a mouvement entry has been saved