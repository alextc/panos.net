Get-Content C:\temp\ADRuleDestinationHosts.txt | 
    Resolve-DnsName | 
    Select-Object -ExpandProperty NameHost | 
    Resolve-DnsName -Type A | 
    Select-Object @{Name="SQL";Expression={"update dbo.TrafficLog set SourceHostName=" + "'" + $_.Name.ToLower() + "'" + " WHERE Source=" + "'" + $_.IPAddress +  "'"}}


