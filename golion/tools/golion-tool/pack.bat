for /f "delims=" %%t in ('%SolutionDir%\tools\golion-tool\golion-tool.exe v %TargetPath%') do set AssemblyVersion=%%t
echo ���[%TargetName%]�汾��%AssemblyVersion%
rem ������
set PluginFileName=%TargetName%_%AssemblyVersion%.zip
del %PluginFileName%
md %TargetDir%\libs
copy %TargetDir%\*.dll %TargetDir%\libs
del %TargetDir%\libs\%TargetFileName%
copy %TargetPath% %TargetDir%\bundle
call %SolutionDir%\tools\7-zip\7z a %PluginFileName% %TargetDir%\bundle %TargetDir%\libs\
rem ������
del %TargetDir%\bundle
rd %TargetDir%\libs /S /Q

if "%bundleId%" == "" goto exit
echo bundleID:%bundleId%
echo TargetDir:%TargetDir%

rem ���������BundleId����װ��ָ��Ŀ¼
call %SolutionDir%\setting\setEnv
rem ��װ���(��ZIP�н�ѹ��ָ��Ŀ¼)
rd %SolutionDir%\%InstallProject%\%OutDir%\plugins\%BundleId%\ /S /Q
md %SolutionDir%\%InstallProject%\%OutDir%\plugins\%BundleId%
cd /D %SolutionDir%\%InstallProject%\%OutDir%\plugins\%BundleId%
call %SolutionDir%\tools\7-zip\7z x %TargetDir%\%PluginFileName%
echo $(TargetPath) > %SolutionDir%\%InstallProject%\%OutDir%\plugins\%BundleId%\location
:exit