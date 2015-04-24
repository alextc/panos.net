$connection = New-PANOSConnection -HostName 'palab.redmond.corp.microsoft.com' -Vsys 'vsys1' -AccessToken (Import-CliXml c:\PSScripts\panosAccessToken)
Remove-PANOSAddress -Connection $connection -Name API-TEST-10897
Get-PANOSAddress -Connection $connection -Name API-TEST-11745 | Remove-PANOSAddress -Connection $connection
Get-PANOSAddress -Connection $connection -FromCandidateConfig | where { $_.Address -eq '10.160.49.218' } | Remove-PANOSAddress -Connection $connection
