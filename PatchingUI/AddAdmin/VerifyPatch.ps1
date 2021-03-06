
$text=$args[0];
$filepath=$args[1];
$resultspath=$args[2];
$server_list = get-content $filepath;
$kbValue=$text;
"" | Out-File -FilePath $resultspath

if($KbValue | Select-string ',' -quiet)
{
    $values=$KbValue.split(',');
    foreach ($server in $server_list) 
    {

        $text='';
         foreach($val in $values)
         {

             #if (Get-WmiObject -Computer $server -Class Win32_QuickFixEngineering -Filter "ServicePackInEffect='KB981391'" -erroraction silentlycontinue) {
              if (Get-WmiObject -Computer $server -Class Win32_QuickFixEngineering -Filter "HotFixID='$val'" -erroraction silentlycontinue) 
              {     
              
                   if($text -ne '')
                   {
                      $text=$text + "@" + $val + "&Y"
                      }
                      else
                      {
                      $text=$text + "@" + $val + "&Y"
                      }
                      #"$server $val Y $LastBootUpTime" | Out-File -FilePath $resultspath -Append
                
                }
             else
             {
                     if($text -ne '')
                       {
                     $text=$text +"@" + $val + "&N"
                     }
                     else
                     {
                     $text=$text + "@" + $val + "&N"
                     }
                   
              #"$server $val N $LastBootUpTime" | Out-File -FilePath $resultspath -Append
             }
             
          }
           $Uptime = Get-WmiObject -Class Win32_OperatingSystem -ComputerName $server
           $LastBootUpTime = $Uptime.ConvertToDateTime($Uptime.LastBootUpTime)
          "$server $text@$LastBootUpTime" | Out-File -FilePath $resultspath -Append
    } 

}
else
{

        foreach ($server in $server_list) 
        {

			$text='';
         
				 #if (Get-WmiObject -Computer $server -Class Win32_QuickFixEngineering -Filter "ServicePackInEffect='KB981391'" -erroraction silentlycontinue) {
					  if (Get-WmiObject -Computer $server -Class Win32_QuickFixEngineering -Filter "HotFixID='$kbValue'" -erroraction silentlycontinue) 
					  {     
                  
						  if($text -ne '')
						   {
						  $text=$text + "@" + $kbValue + "&Y"
						  }
						  else
						  {
						  $text=$text + "@" + $kbValue + "&Y"
						  }
						  #"$server $val Y $LastBootUpTime" | Out-File -FilePath $resultspath -Append
                
					 }
					 else
					 {
							 if($text -ne '')
							  {
							 $text=$text +"@" + $kbValue + "&N"
							 }
							 else
							 {
							 $text=$text + "@" + $kbValue + "&N"
							 }
                       
					   #"$server $val N $LastBootUpTime" | Out-File -FilePath $resultspath -Append
					 }
		   $Uptime = Get-WmiObject -Class Win32_OperatingSystem -ComputerName $server
           $LastBootUpTime = $Uptime.ConvertToDateTime($Uptime.LastBootUpTime)
          "$server $text@$LastBootUpTime" | Out-File -FilePath $resultspath -Append
             
          }
          
  }






 