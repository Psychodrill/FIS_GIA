Param($SiteNames, $NeedDeploy)

$appcmdPath = 'C:\Windows\SysWOW64\inetsrv\appcmd.exe'

if ($SiteNames.GetType().Name -eq 'string' -and $SiteNames.Contains(','))
{
    echo 'Splitting project names'
    $SiteNames = $SiteNames.Split(',')
}

. "./Sites.ps1"

if ($NeedDeploy -eq 'true')
{	
	foreach ($siteName in $SiteNames)
	{
		if (!(Contains $StartableSites $SiteName))
		{
			Write-Host "$siteName is not stoppable"
			continue
		}
		Write-Host "stopping $siteName"
		& $appcmdPath start apppool $siteName
		& $appcmdPath start site $siteName
		& $appcmdPath stop apppool $siteName
		& $appcmdPath stop site $siteName
	}
}
