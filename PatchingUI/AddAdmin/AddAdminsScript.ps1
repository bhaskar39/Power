$serverspath=$args[0];
$resultspath=$args[1];
$AdminAcctName=$args[2];
$servers = Get-Content $serverspath
"" | Out-File -FilePath $resultspath
foreach ($server in $servers){
 try{
 $adsi = [ADSI]"WinNT://$server/administrators,group" 
 $adsi.add("WinNT://$AdminAcctName,group")
  "$server@Success"
  "$server@Success" | Out-File -FilePath $resultspath -Append
 }
 catch{
  "$server@" + $_.Exception.Message.ToString().Split(":")[1].Replace("`n","")
  "$server@" + $_.Exception.Message.ToString().Split(":")[1].Replace("`n","") | Out-File -FilePath $resultspath -Append
 }
}
