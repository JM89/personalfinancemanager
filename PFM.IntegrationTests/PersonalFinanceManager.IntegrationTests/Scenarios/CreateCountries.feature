Feature: CreateCountries
	Create Countries

@createcountries
Scenario: Create Countries
	Given I have accessed the Country List page
	And I have clicked on the Create button
	When I enter a name
	And I click on the Save button
	Then an country has been created
