function CheckIdentityPks($path, $fix)
{
    $hasErrors = $false
    if ($fix)
    {
        $fixString = "(fixed in crebas)"
    }
    $oldCrebas = Get-Content -Path $path
    $tablesWithFkPk = @{}
    foreach ($line in $oldCrebas)
    {
        if ($line -match "alter table")
        {
            $table = $line.Trim().Split(' ')[2]
        }
        elseif ($line -match "add constraint [A-Za-z_]* foreign key \(Id\)")
        {
            $tablesWithFkPk[$table] = $true
            Write-Host $table has foreign primary key
        }
    }
    $crebas = $oldCrebas | Foreach-Object {
        $line = $_
        if ($line -match "^\s*Id\s*int\s*not\s*null" -and -not $tablesWithFkPk[$table])
        {
            $line = $line -replace "not\s*null", "identity"
            $hasErrors = $true
            Write-Host -ForegroundColor yellow "The column Id of the table" $table "is int but not identity" $fixString
        }
        elseif ($line -match "create table")
        {
            $table = $line.Trim().Split(' ')[2]
        }
        return $line
    }
    if ($fix)
    {
        Set-Content -Path $path -Value $crebas
    }
    return $hasErrors
}

function CheckFkNames($path, $fix)
{
    $hasErrors = $false
    if ($fix)
    {
        $fixString = "(fixed in crebas)"
    }
    $constraintsMap = @{}
    $crebas = Get-Content -Path $path | ForEach-Object {
        $line = $_
        $constraintName = $null
        if ($line -match "^\s*add\s*constraint")
        {
            $constraintName = $line.Trim().Split(' ')[2]
        }
        elseif ($line -match "^\s*constraint")
        {
            $constraintName = $line.Trim().Split(' ')[1]
        }
        elseif (($line -match "alter\s*table") -or ($line -match "create\s*table"))
        {
            $table = $line.Split(' ')[2]
        }
        if ($constraintName -ne $null)
        {
            if ($constraintsMap[$constraintName] -ne $null)
            {
                Write-Host -ForegroundColor yellow "Duplicate constraint name" $constraintName ": in tables" $constraintsMap[$constraintName] "and" $table $fixString
                $hasErrors = $true
                $newConstraintName = $constraintName + [guid]::NewGuid().ToString('N')
                $line = $line -replace $constraintName, $newConstraintName
            }
            $constraintsMap[$constraintName] = $table
        }
        return $line
    }
    if ($fix)
    {
        Set-Content -Path $path -Value $crebas
    }
    return $hasErrors
}

function CheckBadReferences($path, $badTables, $fix)
{
    $hasErrors = $false
    if ($fix)
    {
        $fixString = "(fixed in crebas)"
    }
    $crebas = Get-Content -Path $path 
    $fixedCrebas = New-Object System.Collections.ArrayList
    $badTables = @($badTables)
    foreach ($line in $crebas)
    {
        $bad = $false;
        foreach ($badTable in $badTables)
        {
            if ($line -match ("references\s*" + $badTable))
            {
                $bad = $true
                Write-Host -ForegroundColor yellow "There is a reference to non-existent table" $badTable "from table" $table $fixString
                $hasErrors = $true
            }
        }
        if (($line -match "create table") -or ($line -match "alter table"))
        {
            $table = $line.Trim().Split(' ')[2]
        }
        if ($bad)
        {
            $fixedCrebas.RemoveRange($cur - 1, 2)
        }
        else
        {
            $cur = $fixedCrebas.Add($line)
        }
    }
    if ($fix)
    {
        Set-Content -Path $path -Value $fixedCrebas
    }
    return $hasErrors
}

function CheckCrebas($path, $badTables, $fix)
{
    $currentDir = $PSScriptRoot
    Write-Host "Checking for non-identity int PKs"
    $hasPkErrors = (CheckIdentityPks -Path $path -Fix $fix)
    Write-Host "Checking for duplicate constraint names"
    $hasFkErrors = (CheckFkNames -Path $path -Fix $fix)
    Write-Host "Checking for references to non-generated tables"
    $hasBadRefs = (CheckBadReferences -Path $path -BadTables @("Employees", "KspmDepartments") -Fix $fix)
    
    return $hasPkErrors -or $hasFkErrors -or $hasBadRefs
}
