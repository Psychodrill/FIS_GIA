@ ECHO off
cls
REM "c:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" Build.proj %*
"c:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe" Build.proj /m /p:ConfigurationName=Test /p:Deploy=false /p:Archive=true %*
pause