#!C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe
$path=  $env:windir + '\system32\drivers\etc\hosts'
$ip=[System.Net.Dns]::GetHostAddresses("server")[0].IPAddressToString
$str = $ip + '    check.ege.test'
Add-Content $path ''
Add-Content $path $str