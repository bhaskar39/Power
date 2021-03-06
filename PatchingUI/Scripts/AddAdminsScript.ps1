$servers = Get-Content D:\Sudha\sep27\PatchingUI\PatchingUI\Servers.txt

"Name`tStatus" | Out-File -FilePath D:\Sudha\sep27\PatchingUI\PatchingUI\results.txt
foreach ($server in $servers){
 try{
 $adsi = [ADSI]"WinNT://$server/administrators,group" 
 $adsi.add("WinNT://fareast/stautoteam,group")
  "$server`tSuccess"
  "$server`tSuccess" | Out-File -FilePath .\results.txt -Append
 }
 catch{
  "$server`t" + $_.Exception.Message.ToString().Split(":")[1].Replace("`n","")
  "$server`t" + $_.Exception.Message.ToString().Split(":")[1].Replace("`n","") | Out-File -FilePath .\results.txt -Append
 }
}
