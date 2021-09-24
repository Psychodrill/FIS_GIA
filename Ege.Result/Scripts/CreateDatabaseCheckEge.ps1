Param($noXsd=$null)

$ErrorActionPreference = "Continue"

$currentDir = Split-Path $script:MyInvocation.MyCommand.Path

$dbPath = (Resolve-Path ($currentDir + "/../Database/Check.Ege")).Path
$dbScriptPath = (Resolve-Path ($currentDir + "/../Database/Check.Ege")).Path
$seeds = $dbPath + "/Seeds"
$pdm = $dbPath + "/CheckEge.pdm"
$crebas = $dbPath + "/crebas.sql"
$crebasUt = $dbPath + "/crebas_unittest.sql"
$SqlServer = "LocalServer"
$pdm2SqlPath = "../Tools/pdm2sql"
$xsdPath = ($currentDir + '/Xsd.ps1')
. "Func/CreateDatabaseFunctions.ps1"
CreateDatabase -seeds $seeds  -pdm $pdm -crebas $crebas -crebasUt $crebasUt -SqlServer $SqlServer -pdmToSqlPath $pdm2SqlPath -fromDbName "CheckEge" -toDbName "CheckEgeUnitTest"
if ($noXsd -eq $null)
{
    Write-Host Generating XSD
    . ($xsdPath) -dbName 'CheckEgeUnitTest' -xsdPath 'C:\Projects\Result\Solution\Ege.Check.Dal.Store.Tests\TestDataSet.xsd' -csPath 'C:\Projects\Result\Solution\Ege.Check.Dal.Store.Tests\' -namespace 'Ege.Check.Dal.Store.Tests' -testDataSetName 'TestDataSet' -RunCrebas $false
}