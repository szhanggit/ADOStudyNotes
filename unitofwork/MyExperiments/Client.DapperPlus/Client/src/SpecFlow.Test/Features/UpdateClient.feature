Feature: UpdateClient

Scenario: Update client information By Client Id
	Given Environment <Environment>
	And The parameter TenantBasicInfoId '<TenantBasicInfoId>'
	And The parameter TenantName '<TenantName>'
	And The parameter ClientId '<ClientId>'
	When I update client by client Id
	Then The response message is <ResponseMessage>

	Examples:
		| Environment | TenantBasicInfoId | TenantName | ClientId | ResponseMessage |
		| dev         | 9                 | GL         | 1        | success         |