Feature: TenantCreation
	Creating tenant

@mytag
Scenario: Creat new tenant
	Given I launch the tenant application
	And I navigate to tenant
	And I click the button create
	And I enter the tenant information
		| TenantName | Country     | TimeZone              | TimeFormat | CompanyTaxRate | EffectivityDate | Language   |
		| Sample 1   | Philippines | Philippines UTC+08:00 | YYYY/MM/DD | 12             | 2021-09-24      | Tagalog |
	And I click the upload
	And I click the tenant save button
	Then the result should be true