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
    $accessToken = Import-Clixml c:\PSScripts\panosAccessTokenForFwDelegation
    $query =  [string]::Format("( action eq deny) and ( addr.src in {0} ) and ( receive_time leq '{1:yyyy/MM/dd HH:mm:ss}' ) and (receive_time geq '{2:yyyy/MM/dd HH:mm:ss}')", $SourceIp, $RangeEnd, $RangeStart)
    $connectionFW1 = New-PANOSConnection -HostName 'firewall1.it.msft.net' -Vsys 'vsys3' -AccessToken $accessToken
    $connectionFW2 = New-PANOSConnection -HostName 'firewall2.it.msft.net' -Vsys 'vsys3' -AccessToken $accessToken
    Get-PANOSTrafficLog -Query $query -Connection $connectionFW1,$connectionFW2 | Format-Table
}


function GetSidForUserName ($userName)
{
    $objUser = New-Object System.Security.Principal.NTAccount($userName)
    $strSID = $objUser.Translate([System.Security.Principal.SecurityIdentifier])
    $strSID.Value
}

function Register-ConstrainedFwManagmentSession
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true)]
        $SessionName,
        [Parameter(Mandatory=$true)]
        $RunAsAccountName,
        [Parameter(Mandatory=$true)]
        $RunAsAccountPwd,
        [Parameter(Mandatory=$true)]
        $GrantExecuteTo
    )

    Process
    {
        New-PSSessionConfigurationFile -Path c:\PSScripts\panos.pssc `
                               -Description 'PANOS Delegation EndPoint' `
                               -ExecutionPolicy Restricted `
                               -SessionType RestrictedRemoteServer `
                               -LanguageMode FullLanguage `
                               -FunctionDefinitions @{Name="Get-PANOSBlockedTraffic";ScriptBlock=$getBlockedTraffic; Options="AllScope"} `
                               -VisibleProviders FileSystem `
                               -ModulesToImport PANOS `
                               -VisibleCmdlets Select-Object

        Unregister-pssessionconfiguration -name $SessionName -force
        Test-PSSessionConfigurationFile -Path c:\PSScripts\panos.pssc

        $secpasswd = ConvertTo-SecureString $RunAsAccountPwd -AsPlainText -Force
        $sessionCreds = New-Object System.Management.Automation.PSCredential ($RunAsAccountName, $secpasswd)
        $grantToSid = GetSidForUserName($GrantExecuteTo)
        $securityDescriptor =  [string]::Format("O:NSG:BAD:P(A;;GA;;;BA)(A;;GR;;;IU)(A;;GXGR;;;{0})S:P(AU;FA;GA;;;WD)(AU;SA;GXGW;;;WD)", $grantToSid)
        
        Register-PSSessionConfiguration -Path 'c:\PSScripts\panos.pssc' `
                                    -Name FirewallManagement `
                                    -SecurityDescriptorSddl $securityDescriptor `
                                    -RunAsCredential $sessionCreds `
                                    -AccessMode Remote `
                                    -Force
    }
}

                                

