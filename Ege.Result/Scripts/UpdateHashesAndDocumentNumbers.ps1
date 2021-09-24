$sql = @"
select 
	p.ParticipantRbdId RbdId
	,p.ParticipantHash NewHash
	,pp.ParticipantHash OldHash
	,p.DocumentNumber NewNumber
	,pp.DocumentNumber OldNumber
from 
	checkege.dbo.ap_Participants p
	join hsc.dbo.Participants pp on 
		p.ParticipantCode = pp.ParticipantCode and p.RegionId = pp.RegionId
		and (p.ParticipantHash != pp.ParticipantHash or p.DocumentNumber != pp.DocumentNumber)
"@

$assembly = [System.Reflection.Assembly]::LoadWithPartialName('Microsoft.SQLServer.SMO')
$assembly = [System.Reflection.Assembly]::LoadWithPartialName('Microsoft.SqlServer.ConnectionInfo')
$serverConnection = New-Object('Microsoft.SqlServer.Management.Common.ServerConnection') -ArgumentList @('85.143.100.34')
$server = New-Object('Microsoft.SqlServer.Management.SMO.Server') $serverConnection

$result = $server.Databases['CheckEge'].ExecuteWithResults($sql)


function GetStorage($hash, $number, $rbdId)
{
	$storage = 'c:/tmp/ege/blankstorage/'
	return $storage + $hash.Substring(0, 2) + '/' + $hash.Substring(0, 4) + '/' + $hash + '/' + $number + '/' +  $rbdId
}

foreach ($changed in $result.Tables[0].Rows)
{
	$oldStorage = GetStorage $changed.OldHash $changed.OldNumber $changed.RbdId
	$newStorage = GetStorage $changed.NewHash $changed.NewNumber $changed.RbdId
	
	echo "cp -r $oldStorage/* $newStorage"
	cp -r $oldStorage $newStorage
}

$updateSql = @"
merge
	hsc.dbo.Participants target
using
(
	select 
		p.ParticipantRbdId
		,p.ParticipantHash NewHash
		,p.DocumentNumber NewNumber
		,p.ParticipantCode
		,p.RegionId
	from 
		checkege.dbo.ap_Participants p
		join hsc.dbo.Participants pp on 
			p.ParticipantCode = pp.ParticipantCode and p.RegionId = pp.RegionId
			and (p.ParticipantHash != pp.ParticipantHash or p.DocumentNumber != pp.DocumentNumber)
) source
on target.ParticipantCode = source.ParticipantCode and target.RegionId = source.RegionId
when matched then
	update set 
		target.ParticipantHash = source.NewHash
		,target.DocumentNumber = source.NewNumber;
"@

$server.Databases['CheckEge'].ExecuteNonQuery($updateSql)
