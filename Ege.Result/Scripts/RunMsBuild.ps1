Param($SiteNames, $ConfigurationName, $NeedDeploy, $NeedArchive)

$msBuildPath = 'C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe'

$ProjectFiles = @{}
$ProjectFiles['check.ege.test']='Build.proj'
$ProjectFiles['check.ege.services.test']='Build.LoadService.proj'
$ProjectFiles['check.ege.blanks.test']='Build.BlanksApp.proj'
$ProjectFiles['hsc.scheduler']='Build.Scheduler.proj'

if ($SiteNames.GetType().Name -eq 'string' -and $SiteNames.Contains(','))
{
    echo 'Splitting project names'
    $SiteNames = $SiteNames.Split(',')
}

foreach ($siteName in $SiteNames)
{
	$ProjectFile = $ProjectFiles[$siteName]
	if (!$projectFile)
	{
		throw "No project file defined for $siteName"
	}
	Write-Host "Running $ProjectFile to build $SiteName"
	& $msBuildPath $projectFile /m /p:ConfigurationName=$ConfigurationName /p:Deploy=$NeedDeploy /p:Archive=$NeedArchive /p:DeployDirectory="C:\inetpub\$SiteName"
}
