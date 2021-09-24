set PublishPath="C:\inetpub\wwwroot\GVUZPreRelease"
set ServicePath="C:\inetpub\GVUZ.ImportService"
set ProjectPath="C:\develop\ccnet\GVUZ_PreRelease"

C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe /t:Build;PipelinePreDeployCopyAllFilesToOneFolder /p:Configuration=Release;_PackageTempDir=%PublishPath% %ProjectPath%\GVUZ.Web\GVUZ.Web.csproj /p:AutoParameterizationWebConfigConnectionStrings=False

taskkill /f /im GVUZ.ImportServiceTest.exe /fi "memusage gt 40" 2>NUL | findstr SUCCESS >NUL && if errorlevel 1 ( echo ImportServiceTest was not killed ) else ( echo ImportServiceTest was killed )
taskkill /f /im GVUZ.ImportService.exe /fi "memusage gt 40" 2>NUL | findstr SUCCESS >NUL && if errorlevel 1 ( echo ImportService was not killed ) else ( echo ImportService was killed )

copy "%ProjectPath%\GVUZ.Web\Web.Debug.config" "%PublishPath%\Web.config"
copy "%ProjectPath%\GVUZ.ImportServiceTest\bin\Release\GVUZ.ImportServiceTest.exe" "C:\inetpub\wwwroot\GVUZPROD\GVUZ.Import.TestConsole\GVUZ.ImportServiceTest.exe"

xcopy "%ServicePath%\GVUZ.ImportService.exe.config" "%ProjectPath%\GVUZ.ImportService\bin\Release\GVUZ.ImportService.exe.config" /Y
xcopy "%ProjectPath%\GVUZ.ImportService\bin\Release" "%ServicePath%" /Y /e

net start GvuzProdImportService