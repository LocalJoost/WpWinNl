echo on
cd /d "%~dp0"
del /S/Q ..\output\*.pdb
del /S/Q ..\output\Galasoft.*
del /S/Q ..\output\Microsoft.Phone.*
del /S/Q ..\output\Microsoft.Practices.ServiceLocation.*
del /S/Q ..\output\Microsoft.Xaml.*
del /S/Q ..\output\System.Reactive.*
del /S/Q ..\output\Microsoft.Xaml.*
del /S/Q ..\output\System.Windows.Interactivity.*
del /S/Q ..\output\IBehaviorP.*

del /S/Q "..\output\wpwinnl\lib\uap10.0\WpWinNl.dll"
del /S/Q "..\output\wpwinnl\lib\uap10.0\WpWinNl.pri"
del /S/Q "..\output\wpwinnl\lib\uap10.0\WpWinNl.External.dll"
del /S/Q "..\output\wpwinnl\lib\uap10.0\WpWinNl.External.pri"

del /S/Q "..\output\wpwinnl\lib\portable-win81+wpa81\WpWinNl.dll"
del /S/Q "..\output\wpwinnl\lib\portable-win81+wpa81\WpWinNl.pri"
del /S/Q "..\output\wpwinnl\lib\portable-win81+wpa81\WpWinNl.External.dll"
del /S/Q "..\output\wpwinnl\lib\portable-win81+wpa81\WpWinNl.External.pri"

del /S/Q "..\output\wpwinnl\lib\wpa81\WpWinNl.dll"
del /S/Q "..\output\wpwinnl\lib\wpa81\WpWinNl.pri"
del /S/Q "..\output\wpwinnl\lib\wpa81\WpWinNl.External.dll"
del /S/Q "..\output\wpwinnl\lib\wpa81\WpWinNl.External.pri"

del /S/Q "..\output\wpwinnl\lib\WindowsPhone8\WpWinNl.dll"
del /S/Q "..\output\wpwinnl\lib\WindowsPhone8\WpWinNl.pri"
del /S/Q "..\output\wpwinnl\lib\WindowsPhone8\WpWinNl.External.dll"
del /S/Q "..\output\wpwinnl\lib\WindowsPhone8\WpWinNl.External.pri"


del /S/Q "..\output\wpwinnl_maps\lib\uap10.0\WpWinNl.dll"
del /S/Q "..\output\wpwinnl_maps\lib\uap10.0\WpWinNl.pri"
del /S/Q "..\output\wpwinnl_maps\lib\uap10.0\WpWinNl.External.dll"
del /S/Q "..\output\wpwinnl_maps\lib\uap10.0\WpWinNl.External.pri"

del /S/Q "..\output\wpwinnl_maps\lib\wpa81\WpWinNl.dll"
del /S/Q "..\output\wpwinnl_maps\lib\wpa81\WpWinNl.pri"
del /S/Q "..\output\wpwinnl_maps\lib\wpa81\WpWinNl.External.dll"
del /S/Q "..\output\wpwinnl_maps\lib\wpa81\WpWinNl.External.pri"

mkdir ..\output\wpwinnl_basic\tools
copy *.ps1 ..\output\wpwinnl_basic\tools

Nuget pack ..\SolutionInfo\WpWinNl.nuspec -BasePath ..\output\wpwinnl -OutputDirectory ..\output 
Nuget pack ..\SolutionInfo\WpWinNlBasic.nuspec -BasePath ..\output\wpwinnl_basic -OutputDirectory ..\output 
Nuget pack ..\SolutionInfo\WpWinNlMaps.nuspec -BasePath ..\output\wpwinnl_maps -OutputDirectory ..\output 

pause
