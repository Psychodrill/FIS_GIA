chcp 1251
xcopy "запрос-импорт.xsd" import.xsd /y
xcopy "запрос-импорт 1 заявления.xsd" import1app.xsd /y
xcopy "запрос-удаление.xsd" delete.xsd /y

%comspec% /k "C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\Tools\VsDevCmd.bat"
d:
cd "D:\Projects\FIS\svn\GVUZ\trunk\GVUZ.ImportService2016\Generate"
"generate.cmd"

pause