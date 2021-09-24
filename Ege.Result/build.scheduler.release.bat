@ ECHO off
cls
REM "c:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" Build.BlanksApp.proj %*
"c:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe" Build.Scheduler.proj /m /p:ConfigurationName=Release /p:Deploy=false /p:Archive=true %*
pause