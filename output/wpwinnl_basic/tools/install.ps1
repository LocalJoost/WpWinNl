param($installPath, $toolsPath, $package, $project)

$moniker = $project.Properties.Item("TargetFrameworkMoniker").Value
$frameworkName = New-Object System.Runtime.Versioning.FrameworkName($moniker)

if($frameworkName.FullName -eq ".NETCore,Version=v4.5.1" -or $frameworkName.FullName -eq "WindowsPhoneApp,Version=v8.1")
{
 Write-Host ">> Adding Behaviors SDK (XAML)"
    $project.Object.References.AddSDK("Behaviors SDK (XAML)", "BehaviorsXamlSDKManaged, version=12.0")
}

if($frameworkNameFullName -eq "WindowsPhone,Version=v8.0")
{
  
}


