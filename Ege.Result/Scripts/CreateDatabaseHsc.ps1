Param($noXsd=$null)
."Func/CreateDatabaseFunctions.ps1"
$ErrorActionPreference = "Continue"


$currentDir = Split-Path $script:MyInvocation.MyCommand.Path

$dbPath = (Resolve-Path ($currentDir + "/../Database/Hsc")).Path
$dbScriptPath = (Resolve-Path ($currentDir + "/../Database/Hsc")).Path
$seeds = $dbPath + "\Seeds"
$pdm = $dbPath + "\Hsc.pdm"
$crebas = $dbPath + "\crebas.sql"
$crebasUt = $dbPath + "\crebas_unittest.sql"
$SqlServer = "LocalServer"
$pdm2SqlPath = "..\Tools\pdm2sql"
$xsdPath = ($currentDir + '\Xsd.ps1')

CreateDatabase -seeds $seeds  -pdm $pdm -crebas $crebas -crebasUt $crebasUt -SqlServer $SqlServer -pdmToSqlPath $pdm2SqlPath -fromDbName "Hsc" -toDbName "HscUnitTest"
if ($noXsd -eq $null)
{
    Write-Host Generating XSD
    . ($xsdPath) -dbName 'HscUnitTest' -xsdPath 'C:\Projects\Result\Solution\Ege.Hsc.Dal.Store.Tests\TestDataSet.xsd' -csPath 'C:\Projects\Result\Solution\Ege.Hsc.Dal.Store.Tests\' -namespace 'Ege.Hsc.Dal.Store.Tests' -testDataSetName 'TestDataSet' -RunCrebas $false
}