function ApplyChanges($input, $change)
{
    $result = $_
    foreach ($change in $changes)
    {
        $result = $result -replace $change.regex, $change.replacement
    }
    return $result
}

function RenameDatabaseInCrebas($oldCrebasPath, $newCrebasPath, $oldDbName, $newDbName)
{
    $templates = "database name: %DATABASE%",
                 "'%DATABASE%'",
                 "'%DATABASE%_log'",
                 "database %DATABASE%",
                 "%DATABASE%.mdf",
                 "%DATABASE%_log.ldf",
                 "%DATABASE%_FileStream",
                 "filegroup %DATABASE%_FileStreamGroup",
                 "use %DATABASE%"
    $oldCrebas = Get-Content -Path $oldCrebasPath
    $changes = $templates | Select-Object `
        @{ Name = "regex"; Expression = { $_ -replace " ", "\s(\s?)" -replace "\.", "\." -replace "%DATABASE%", $oldDbName } }, 
        @{ Name = "replacement"; Expression = { $_ -replace "%DATABASE%", $newDbName }}
    $oldCrebas | Foreach-Object { ApplyChanges -Input $_ -Changes $changes } | out-file -encoding utf8 -filepath $newCrebasPath
}
