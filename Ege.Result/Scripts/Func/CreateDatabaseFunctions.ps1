function CreateDatabase($seeds, $pdm, $crebas, $crebasUt, $SqlServer, $pdmToSqlPath, $fromDbName, $toDbName)
{
    $ErrorActionPreference = "Continue"

    # Generating crebas
    Write-Host Generating crebas into $crebas
    & $pdmToSqlPath $pdm $crebas
    Write-Host Generating crebas for unit tests into $crebasUt
    . "Func/RenameDatabaseInCrebasFunc.ps1"
    Write-Host Renaming db from $fromDbName to $toDbName
    RenameDatabaseInCrebas -OldCrebasPath $crebas -NewCrebasPath $crebasUt -OldDbName $fromDbName -NewDbName $toDbName

    # Running crebas
    Write-Host Running crebas
    Invoke-Sqlcmd -InputFile $crebas -ServerInstance $SqlServer
    Write-Host 'Running crebas for unit tests'
    Invoke-Sqlcmd -InputFile $crebasUt -ServerInstance $SqlServer

    # Running static seeds
    Write-Host Applying static seeds
    $sqlSeeds = Get-ChildItem $seeds -Filter *_seed.sql | Where-Object {-not $_.Name.Contains("test_seed.sql")}
    if ($sqlSeeds -ne $null)
    {
        foreach ($sqlSeed in $sqlSeeds)
        {
            Write-Host 'Invoking' $sqlSeed.FullName 
            Invoke-Sqlcmd -InputFile $sqlSeed.FullName -ServerInstance $SqlServer -Database $fromDbName -QueryTimeout 65535
        }
    }
}