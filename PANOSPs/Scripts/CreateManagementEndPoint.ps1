Unregister-pssessionconfiguration -name FirewallManagement -force

New-PSSessionConfigurationFile -Path c:\PSScripts\panos.pssc `
                               -Description 'PANOS Delegation EndPoint' `
                               -ExecutionPolicy Restricted `
                               -ModulesToImport PANOS, Microsoft.PowerShell.Utility `
                               -VisibleCmdlets 'Get-PANOSBlockedTraffic', 'Format-Table' `
                               -SessionType RestrictedRemoteServer

Test-PSSessionConfigurationFile -Path c:\PSScripts\panos.pssc

$secpasswd = ConvertTo-SecureString "PASSWORDHERE" -AsPlainText -Force
$sessionCreds = New-Object System.Management.Automation.PSCredential ("cxeredxjea02\panosjea", $secpasswd)
Register-PSSessionConfiguration -Path c:\PSScripts\panos.pssc `
                                -Name FirewallManagement `
                                -ShowSecurityDescriptorUI `
                                -AccessMode Remote `
                                -RunAsCredential $sessionCreds
