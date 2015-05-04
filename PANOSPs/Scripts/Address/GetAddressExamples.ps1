#Get All Addresses from Candidate Config Example
$connection = New-PANOSConnection -HostName 'palab.redmond.corp.microsoft.com' -Vsys 'vsys1' -AccessToken (Import-CliXml c:\PSScripts\panosAccessToken)
Get-PANOSAddress -Connection $connection -FromCandidateConfig

#Use session var
New-PANOSConnection -HostName 'palab.redmond.corp.microsoft.com' -Vsys 'vsys1' -AccessToken (Import-CliXml c:\PSScripts\panosAccessToken) -StoreInSession | Out-Null
Get-PANOSAddress -FromCandidateConfig

#Get specific address by name
Get-PANOSAddress -Connection $connection -Name CP1PD1SLKAPP01

#Filter object with Select
Get-PANOSAddress -Connection $connection | Where { $_.Address -eq '2.1.1.1' }

#Getting a single object tha matches both names and ip
$addressToSearch = New-Object -TypeName PANOS.AddressObject -ArgumentList "CP1PD1SLKAPP01", ([System.Net.IPAddress]::Parse("2.1.1.1")) 
Get-PANOSAddress -Connection $connection -FirewallObject $addressToSearch

#Supplying the FirewallObject via Pipeline
New-Object -TypeName PANOS.AddressObject -ArgumentList "CP1PD1SLKAPP01", ([System.Net.IPAddress]::Parse("2.1.1.1")) |
    Get-PANOSAddress -Connection $connection 

New-Object -TypeName PANOS.AddressObject -ArgumentList "CP1PD1SLKAPP01", ([System.Net.IPAddress]::Parse("2.5.1.1")) |
    Get-PANOSAddress -Connection $connection 

#Check if an object exists
(New-Object -TypeName PANOS.AddressObject -ArgumentList "CP1PD1SLKAPP01", ([System.Net.IPAddress]::Parse("2.5.1.1")) |
    Get-PANOSAddress -Connection $connection) -eq $null 