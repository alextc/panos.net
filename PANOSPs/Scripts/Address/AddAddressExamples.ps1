$connection = New-PANOSConnection -HostName 'palab.redmond.corp.microsoft.com' -Vsys 'vsys1' -AccessToken (Import-CliXml c:\PSScripts\panosAccessToken)
Add-PANOSAddress -Connection $connection -Name "web01" -IpAddress '10.10.10.12' -Description "Web Server 01"
New-Object -TypeName PANOS.AddressObject -ArgumentList "web02", ([System.Net.IPAddress]::Parse("25.1.1.14")) | Add-PANOSAddress -Connection $connection 
Add-PANOSAddress -Connection $connection -Name "web03" -IpAddress '10.10.10.14' -Description "Web Server 03" -PassThru | Get-PANOSAddress -Connection $connection -FromCandidateConfig

