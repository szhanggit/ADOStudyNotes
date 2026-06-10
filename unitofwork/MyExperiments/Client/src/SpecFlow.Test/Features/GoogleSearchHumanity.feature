Feature: GoogleSearchHumanity
	Search for Humanity

@searchtag
Scenario: Google Search
	Given I launch the google
	And I entery humanity
	And I click the search button
	Then the result should be 'Humanity - Google Search'