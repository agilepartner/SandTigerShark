Feature: Feature in project root

Scenario: SpecFlow glue files are generated
	Given I am curious
	When I request the version
	Then the result is content

Scenario Outline: Echo
	Given I am curious
	When I yell '<exclamation>'
	Then I hear '<echo>' echoed back

	Examples: 
		| exclamation    | echo			 |
		| Yodelay-yi-hoo |Yodelay-yi-hoo |
		| Helloooo       |Helloooo       |