$filepath='D:\Sudha\sep27\PatchingUI\PatchingUI\AddAdmin\ValidateServers.txt';
$resultspath='D:\Sudha\sep27\Suhasini\Results.txt';
$StartDate='10/01/2012';
$EndDate='10/23/2012';
$Result='';
$server_list = get-content $filepath;

"" | Out-File -FilePath $resultspath;

        foreach ($server in $server_list) 
        {

		                 
            $Result=Get-WMIObject -Computer $server -Class Win32_QuickFixEngineering | where {$_.InstalledOn -ge $StartDate -and $_.InstalledOn -le $EndDate}  -erroraction silentlycontinue  
           

            $Result | Out-File -FilePath $resultspath -Append  
         
					
             
          }
        






 