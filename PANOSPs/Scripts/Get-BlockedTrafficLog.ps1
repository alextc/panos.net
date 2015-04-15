#( action eq deny) and ( addr.src in 10.127.63.254 ) and ( receive_time leq '2015/03/16 12:46:23' ) and (receive_time geq '2015/03/16 00:45:00')
function Get-BlockedTraffic
{
    param ( [string]$SourceIp, [DateTime]$RangeStart = (Get-Date).AddHours(-4), [DateTime]$RangeEnd = (Get-Date) )
    $query =  [string]::Format("( action eq deny) and ( addr.src in {0} ) and ( receive_time leq '{1:yyyy/MM/dd HH:mm:ss}' ) and (receive_time geq '{2:yyyy/MM/dd HH:mm:ss}')", $SourceIp, $RangeEnd, $RangeStart)
    $connectionPropertyFW1 = New-PANOSConnectionProperties -HostName 'firewall1.it.msft.net' -Vsys 'vsys3' -AccessToken (Import-CliXml c:\PSScripts\panosAccessToken.txt)
    $connectionPropertyFW2 = New-PANOSConnectionProperties -HostName 'firewall2.it.msft.net' -Vsys 'vsys3' -AccessToken (Import-CliXml c:\PSScripts\panosAccessToken.txt)
    Get-PANOSTrafficLog -Query $query  -ConnectionProperties $connectionPropertyFW1,$connectionPropertyFW2 | Select-Object ReceiveTime, Destination, App | ft
}

Get-BlockedTraffic '10.193.160.67' (Get-Date).AddDays(-1) (Get-Date)
#Get-BlockedTraffic '10.193.160.67'