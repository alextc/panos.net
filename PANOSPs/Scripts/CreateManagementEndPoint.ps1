$getBlockedTraffic = {
    param 
        (
            [Parameter(Mandatory=$true, Position=0)]
             [string]$SourceIp, 
            [Parameter(Position=1)]
             [DateTime]$RangeStart = (Get-Date).AddHours(-4),
            [Parameter(Position=2)]
              [DateTime]$RangeEnd = (Get-Date) 
        )

    $accessToken = convertto-securestring "" -asplaintext -force
    $query =  [string]::Format("( action eq deny) and ( addr.src in {0} ) and ( receive_time leq '{1:yyyy/MM/dd HH:mm:ss}' ) and (receive_time geq '{2:yyyy/MM/dd HH:mm:ss}')", $SourceIp, $RangeEnd, $RangeStart)
    $connectionPropertyFW1 = New-PANOSConnectionProperties -HostName 'firewall1.it.msft.net' -Vsys 'vsys3' -AccessToken $accessToken
    $connectionPropertyFW2 = New-PANOSConnectionProperties -HostName 'firewall2.it.msft.net' -Vsys 'vsys3' -AccessToken $accessToken
    Get-PANOSTrafficLog -Query $query -ConnectionProperties $connectionPropertyFW1,$connectionPropertyFW2 | Format-Table
}

New-PSSessionConfigurationFile -Path c:\PSScripts\panos.pssc `
                                -ModulesToImport PANOS, Microsoft.PowerShell.Utility `
                               -Description 'PANOS Delegation EndPoint' `
                               -ExecutionPolicy Restricted `
                               -VisibleCmdlets 'Format-Table', 'Get-Help', 'Select-Object', 'Get-Date' `
                               -SessionType RestrictedRemoteServer `
                               -LanguageMode FullLanguage `
                               -FunctionDefinitions @{Name="Get-BlockedTraffic";ScriptBlock=$getBlockedTraffic; Options="AllScope"}

Unregister-pssessionconfiguration -name FirewallManagement -force
Test-PSSessionConfigurationFile -Path c:\PSScripts\panos.pssc

$secpasswd = ConvertTo-SecureString "" -AsPlainText -Force
$sessionCreds = New-Object System.Management.Automation.PSCredential ("cxeredxjea02\panosjea", $secpasswd)

Register-PSSessionConfiguration -Path 'c:\PSScripts\panos.pssc' `
                                -Name FirewallManagement `
                                -ShowSecurityDescriptorUI `
                                -RunAsCredential $sessionCreds `
                                -AccessMode Remote `
                                -Force
                                


                                
