for /f "delims=" %%t in ('%SolutionDir%\tools\golion-tool\golion-tool.exe v %TargetPath%') do set AssemblyVersion=%%t
echo 插件[%TargetName%]版本：%AssemblyVersion%
rem 打包插件
set PluginFileName=%TargetName%_%AssemblyVersion%.zip
del %PluginFileName%
md %TargetDir%\libs
copy %TargetDir%\*.dll %TargetDir%\libs
del %TargetDir%\libs\%TargetFileName%
copy %TargetPath% %TargetDir%\bundle
call %SolutionDir%\tools\7-zip\7z a %PluginFileName% %TargetDir%\bundle %TargetDir%\libs\
rem 清理工作
del %TargetDir%\bundle
rd %TargetDir%\libs /S /Q

if "%bundleId%" == "" goto exit
echo bundleID:%bundleId%
echo TargetDir:%TargetDir%

rem 如果设置了BundleId，则安装到指定目录
call %SolutionDir%\setting\setEnv
rem 安装插件(从ZIP中解压到指定目录)
rd %SolutionDir%\%InstallProject%\%OutDir%\plugins\%BundleId%\ /S /Q
md %SolutionDir%\%InstallProject%\%OutDir%\plugins\%BundleId%
cd /D %SolutionDir%\%InstallProject%\%OutDir%\plugins\%BundleId%
call %SolutionDir%\tools\7-zip\7z x %TargetDir%\%PluginFileName%
echo $(TargetPath) > %SolutionDir%\%InstallProject%\%OutDir%\plugins\%BundleId%\location
:exit