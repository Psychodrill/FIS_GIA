#!C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe
Param([string]$dbName, [string]$sqlServerInstance, [string[]]$fileTables, [string]$xsdPath, [string]$csPath, [string]$namespace, [string]$testDataSetName, [string[]]$excludeTables, $runCrebas)

if (!$dbName)
{
    $dbName = 'CheckEgeUnitTest'
}
if (!$sqlServerInstance)
{
    $sqlServerInstance = 'LocalServer'
}
if (!$sqlServerLogin) 
{
	$sqlServerLogin = 'sa'
}
if (!$sqlServerPas) 
{
	$sqlServerPas = 'flvbycrbq'
}
if (!$fileTables)
{
    $fileTables = @()
}
if (!$xsdPath)
{
    $xsdPath = 'C:\Projects\Result\Solution\Ege.Check.Dal.Store.Tests\TestDataSet.xsd'
}
if (!$csPath)
{
    $csPath = 'C:\Projects\Result\Solution\Ege.Check.Dal.Store.Tests\'
}
if (!$namespace)
{
    $namespace = 'Ege.Check.Dal.Store.Tests'
}
if (!$testDataSetName)
{
    $testDataSetName = 'TestDataSet'
}
if (!$excludeTables)
{
    $excludeTables = @(,'Migrations')
}

$xsdExePath = 'C:\Program Files (x86)\Microsoft SDKs\Windows\v8.0A\bin\NETFX 4.0 Tools\xsd.exe'

$assembly = [System.Reflection.Assembly]::LoadWithPartialName('Microsoft.SQLServer.SMO')
$serverConnection = New-Object('Microsoft.SqlServer.Management.Common.ServerConnection') -ArgumentList @($sqlServerInstance, $sqlServerLogin, $sqlServerPas)
$server = New-Object('Microsoft.SqlServer.Management.SMO.Server') $serverConnection

function Contains($collection, $element)
{
    return ($collection | Where-Object {$_ -eq $element } | measure).Count -gt 0;
}

$db = $server.Databases[$dbName]
$tables = $db.Tables
$tableNames = $tables | Foreach-Object { $_.Name } | Where-Object { (-not (Contains $excludeTables $_) -and -not $_.StartsWith("tmp_")) }
$query = "use " + $dbName + ";"
foreach ($tableName in $tableNames)
{
    if (!(Contains $fileTables $tableName))
    {
        $query += 'select * from [' + $tableName + '] where 0 = 1;'
    }
    else
    {
        $query += 'select stream_id, file_stream, name from [' + $tableName + '] where 0 = 1;'
    }
}
$res = $db.ExecuteWithResults($query)
$res.DataSetName = $testDataSetName
$res.Namespace = 'http://tempuri.org/' + $testDataSetName + '.xsd'
$i = 0
foreach ($tableName in $tableNames)
{
    $res.Tables[$i].TableName = $tableName
    $i++
}
$res.WriteXmlSchema($xsdPath)

[xml]$xsd = Get-Content $xsdPath
# $comment = $xsd.CreateComment('XSD generated with Xsd.ps1, ' + (Get-Date -Format "yyyy-MM-dd HH:mm:ss.fff"))
# $xsd.schema.AppendChild($comment)
$nsManager = New-Object System.Xml.XmlNamespaceManager($xsd.NameTable)
$nsManager.AddNamespace('x', $xsd.DocumentElement.NamespaceURI)

$identityQuery = @"
    select COLUMN_NAME, TABLE_NAME
    from INFORMATION_SCHEMA.COLUMNS
    where COLUMNPROPERTY(object_id(TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 1 AND NOT TABLE_NAME LIKE 'tmp_%'
    order by TABLE_NAME
"@
$identityRows = $db.ExecuteWithResults($identityQuery).Tables[0]
foreach ($row in $identityRows)
{
    $node = $xsd.SelectSingleNode("//x:element/x:complexType/x:choice/x:element[@name='" + $row.TABLE_NAME + "']/x:complexType/x:sequence/x:element[@name='" + $row.COLUMN_NAME + "']", $nsManager)
	#echo "table" $row.TABLE_NAME "column" $row.COLUMN_NAME "- identity"
    $notInterested = $node.SetAttribute('AutoIncrement', 'urn:schemas-microsoft-com:xml-msdata', 'true')
}

$xsd.Save($xsdPath)

& $xsdExePath $xsdPath /d /n:$namespace /out:$csPath
Remove-Item ($csPath + $testDataSetName + '.Designer.cs')
Rename-Item ($csPath + $testDataSetName + '.cs') ($csPath + $testDataSetName + '.Designer.cs') -Force
#echo '' >>($csPath + $testDataSetName + '.Designer.cs')

# without this shit m$ tools generate some chinese and thai symbols
$designer = 'namespace ' + $namespace + '{ public partial class ' + $testDataSetName + '{}}'
$designer += $designer
echo $designer >($csPath + $testDataSetName + '.cs')
echo '' >($csPath + $testDataSetName + '.xsc')
echo '' >($csPath + $testDataSetName + '.xss')

c:
