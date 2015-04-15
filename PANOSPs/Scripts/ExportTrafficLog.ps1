$query = "( receive_time leq '2015/03/17 16:30:03' ) and ( receive_time geq '2015/03/17 15:30:03' )"
Get-PANOSTrafficLog  -Query $query -Page -Delay 5 -Verbose |    
    convertto-csv -NoTypeInformation -Delimiter "," | % {$_ -replace '"',''} | 
    Out-File -FilePath D:\PANOSData\test1.csv -Force -Encoding ascii
    