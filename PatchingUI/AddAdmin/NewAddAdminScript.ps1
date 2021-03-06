$serverspath=$args[0];
$resultspath=$args[1];
$AdminName=$args[2];
$AdminPassword=$args[3];


$servers=get-content $serverspath;
$AdminAcctName='redmond/stpatcha'
"Name`tStatus" | Out-File -FilePath $resultspath
foreach ($server in $servers){
 try{ 
$LogonUser = $AdminName;
$Password=$AdminPassword;
$LogonPass = ConvertTo-SecureString $Password -AsPlainText -Force
$Cred = New-Object System.Management.Automation.PSCredential($LogonUser,$LogonPass)

   Invoke-Command -Computername $server -Cred $Cred -ScriptBlock {
        param ($AdminAcctName, $server)
        $Group = [ADSI]("WinNT://$server/Administrators,Group")
        $Group.add("WinNT://$AdminAcctName,user")
       
       
    }  -ArgumentList $AdminAcctName, $server -ErrorVariable errortext 

 "$server`tSuccess"
 if($errortext -ne ''){
  "$server`tInvalid Credentials" | Out-File -FilePath $resultspath -Append
  }
  else
  {
   "$server`tSuccess"| Out-File -FilePath $resultspath -Append
  }
  
 }
 catch{
  "$server`t" + $_.Exception.Message.ToString().Split(":")[1].Replace("`n","")
  "$server`t" + $_.Exception.Message.ToString().Split(":")[1].Replace("`n","") | Out-File -FilePath $resultspath -Append
 }
}


