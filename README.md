# panos.net
PowerShell Module for PAN-OS

##Installing 
1. Download the latest release
2. Unzip it to a path on $PSModulePath, for more details on insalling PS modules follow this link
https://msdn.microsoft.com/en-us/library/dd878350(v=vs.85).aspx

##Some Examples

###Saving and retriving PAN-OS Access Token via CliXML
Fore more details on PAN-OS XML API Access Token see PAN-OS 6.1 XML API Reference (section 2.1 – Key Generation).
ConvertTo-SecureString "LUF.....=" -AsPlainText -Force |
  Export-Clixml C:\PSScripts\panosAccessToken
  
###Creating a new Connection object
$connection = New-PANOSConnection `
  -HostName 'firewall1.corp.net' `
  -Vsys 'vsys1' `
  -AccessToken (Import-CliXml c:\PSScripts\panosAccessToken.txt)

###Getting Traffic Log
$query = @"
  ( receive_time leq '2015/04/20 10:16:44' ) and 
  ( receive_time geq '2015/04/20 09:16:44' ) and 
  ( addr.dst in 23.59.190.121 ) and ( action eq deny )"@
Get-PANOSTrafficLog  -Query $query -Connection $connection -Delay 4 | ft 

####Searching for Address Objects with specific address
Get-PANOSAddress -Connection $connection | Where { $_.Address -eq '25.1.1.4' }

####Explore other CmdLets in the PANOS Module
Get-Command -Module PANOS
Get-Help -full Add-PANOSAddress





