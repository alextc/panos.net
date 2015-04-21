#Get All Addresses
$connection = New-PANOSConnection -HostName 'palab.redmond.corp.microsoft.com' -Vsys 'vsys1' -AccessToken (Import-CliXml c:\PSScripts\panosAccessToken)
Get-PANOSAddress -Connection $connection | ft