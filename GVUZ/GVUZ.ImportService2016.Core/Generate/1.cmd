chcp 1251
xcopy "������-������.xsd" import.xsd /y
xcopy "������-������ 1 ���������.xsd" import1app.xsd /y
xcopy "������-��������.xsd" delete.xsd /y

%comspec% /k "C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\Tools\VsDevCmd.bat"
d:
cd "D:\Projects\FIS\svn\GVUZ\trunk\GVUZ.ImportService2016\Generate"
"generate.cmd"

pause