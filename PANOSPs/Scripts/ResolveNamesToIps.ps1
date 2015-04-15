Get-Content C:\temp\ExtranetHosts.txt | 
    Resolve-DnsName -Type A -QuickTimeout 2>null | 
    where { $_.GetType().FullName.Equals("Microsoft.DnsClient.Commands.DnsRecord_A") } | 
    select -Property Name, IPAddress |
    Export-Csv C:\temp\ExtranetIps.txt -NoTypeInformation