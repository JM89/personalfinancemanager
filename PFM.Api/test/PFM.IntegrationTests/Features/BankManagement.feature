Feature: Manage banks

	Scenario: Banks created successfully
		When I provide valid bank details
		Then The banks are created successfully