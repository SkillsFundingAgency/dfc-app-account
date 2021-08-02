Feature: Login to Citizen account update contact details

@Accounts @Smoke
Scenario: Login to citizen account and edit phone number
	Given I am on the action plans landing page
	When I click the Go to your action plan button
	And I enter the email address in the Email address field
	And I enter the password in the Password field
	And I click the Sign in button
	Then I am taken to the Your account page
	When I click the Your details link
	Then I am taken to the Your details page
	When I click the Edit your details button
	Then I am taken to the Edit your details page
	When I enter a valid phone number in the Home number field
	When I click the Save my changes button
	Then I am taken to the Your details page
	And the Home number field contains the updated details