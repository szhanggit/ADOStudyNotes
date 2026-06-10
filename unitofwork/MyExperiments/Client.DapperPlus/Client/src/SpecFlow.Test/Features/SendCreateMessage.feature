Feature: SendCreateMessage

Scenario: Send Create Message
	Given Environment <Environment>
	And The parameter TenantBasicInfoId '<TenantBasicInfoId>'
	And The parameter TenantName '<TenantName>'
	And The parameter ClientId '<ClientId>'
	When I send create message to service bus
	Then The response message is <ResponseMessage>

	Examples:
		| Environment | TenantBasicInfoId | TenantName | ClientId | ResponseMessage |
		| dev         | 9                 | GL         | 1        | success         |