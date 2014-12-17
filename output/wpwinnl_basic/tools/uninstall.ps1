param($installPath, $toolsPath, $package, $project)

$moniker = $project.Properties.Item("TargetFrameworkMoniker").Value
$frameworkName = New-Object System.Runtime.Versioning.FrameworkName($moniker)

if($frameworkName.FullName -eq ".NETCore,Version=v4.5.1")
{
Write-Host ">> WpWinNl and all dependencies removed.
>> The Blend SDK (XAML) was not as it may be neccesary in your project
>> and there is no way to determine if it could be safely removed.
>> If you do not need it, manually remove it."

}

if($frameworkNameFullName -eq "WindowsPhone,Version=v8.0")
{
  
}


