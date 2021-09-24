---- Для отладки
--declare @AdmissionVolumeId int = 72846;
--declare @BudgetLevelId INT = 1;

--начало
----------------------------------
declare @EducationLevelID int = (SELECT av.AdmissionItemTypeID FROM AdmissionVolume av (NOLOCK) WHERE av.AdmissionVolumeID = @AdmissionVolumeId);
declare @DirectionID int  = (SELECT av.DirectionID FROM AdmissionVolume av (NOLOCK) WHERE av.AdmissionVolumeID = @AdmissionVolumeId);
declare @CampaignID int  = (SELECT av.CampaignID FROM AdmissionVolume av (NOLOCK) WHERE av.AdmissionVolumeID = @AdmissionVolumeId);


select * From 
(
select 
isnull(sum(isnull(cgi.[NumberBudgetO], 0)) , 0) as [NumberBudgetO]
,isnull(sum(isnull(cgi.[NumberBudgetOZ], 0)) , 0) as [NumberBudgetOZ]
,isnull(sum(isnull(cgi.[NumberBudgetZ], 0)) , 0) as [NumberBudgetZ]

,isnull(sum(isnull(cgi.[NumberPaidO], 0)) , 0) as [NumberPaidO]
,isnull(sum(isnull(cgi.[NumberPaidOZ], 0)) , 0) as [NumberPaidOZ]
,isnull(sum(isnull(cgi.[NumberPaidZ], 0)) , 0) as [NumberPaidZ]

,isnull(sum(isnull(cgi.[NumberQuotaO], 0)) , 0) as [NumberQuotaO]
,isnull(sum(isnull(cgi.[NumberQuotaOZ], 0)) , 0) as [NumberQuotaOZ]
,isnull(sum(isnull(cgi.[NumberQuotaZ], 0)) , 0) as [NumberQuotaZ]

,isnull(sum(isnull(cgi.[NumberTargetO], 0)) , 0) as [NumberTargetO]
,isnull(sum(isnull(cgi.[NumberTargetOZ], 0)) , 0) as [NumberTargetOZ]
,isnull(sum(isnull(cgi.[NumberTargetZ], 0)) , 0) as [NumberTargetZ]

From CompetitiveGroup cg  (NOLOCK)
LEFT JOIN CompetitiveGroupItem cgi (NOLOCK) on cgi.CompetitiveGroupID = cg.CompetitiveGroupID
Where cg.CampaignID = @CampaignID AND cg.EducationLevelID = @EducationLevelID AND cg.DirectionID = @DirectionID
AND cg.idLevelBudget= @BudgetLevelId
) as q
	