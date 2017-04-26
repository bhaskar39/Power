$LogonUser =$args[0];
$Password=$args[1];
$server=$args[2];
$LogonPass = ConvertTo-SecureString $Password -AsPlainText -Force;
$Cred = New-Object System.Management.Automation.PSCredential($LogonUser,$LogonPass);
Invoke-Command -Computername $server -Cred $Cred -ScriptBlock {"dir \\$server\c$";} -ArgumentList $server -ErrorVariable errortext ;

if($errortext -ne '')
{
 "No Access"
}
else
{
 "Access"
}