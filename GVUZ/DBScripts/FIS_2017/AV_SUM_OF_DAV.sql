-- Если update по всем данным вуза - задать только @institutionId,
-- Если только по ПК - задать @campaignId (тогда можно @institutionId = null)
-- Если по ПК и одному направлению - задать @campaignId и @directionId

-- Если выдало ошибку, что не определена функция dbo.InlineMAX, 
-- то 1 раз надо запустить ее создание внизу скрипта 

USE [GVUZ_PROM_COPY] -- укажите явно базу, чтобы не запустить случайно на какой-то другой БД!

declare @institutionId int = null; -- не обязательно задавать, если задан @campaignId; 
declare @campaignId int = 9985;
declare @directionId int = null; 

if (@institutionId is null and @campaignId is null) 
begin
	select 'Вы точно уверены, что хотите сделать update по всей-всей базе? Тогда временно закоментарьте команду return строчкой ниже';
	return;
end;

update AV
SET 
	NumberBudgetO = dbo.InlineMAX(av.NumberBudgetO, davSum.BudgetO)
	,NumberBudgetOZ = dbo.InlineMAX(av.NumberBudgetOZ, davSum.BudgetOZ)
	,NumberBudgetZ = dbo.InlineMAX(av.NumberBudgetZ, davSum.BudgetZ)

	,NumberQuotaO = dbo.InlineMAX(av.NumberQuotaO, davSum.QuotaO)
	,NumberQuotaOZ = dbo.InlineMAX(av.NumberQuotaOZ, davSum.QuotaOZ)
	,NumberQuotaZ = dbo.InlineMAX(av.NumberQuotaZ, davSum.QuotaZ)

	,NumberTargetO = dbo.InlineMAX(av.NumberTargetO, davSum.TargetO)
	,NumberTargetOZ = dbo.InlineMAX(av.NumberTargetOZ, davSum.TargetOZ)
	,NumberTargetZ = dbo.InlineMAX(av.NumberTargetZ, davSum.TargetZ)
From AdmissionVolume av 
outer apply (
	select 
		sum(NumberBudgetO) as BudgetO
		,sum(NumberBudgetOZ) as BudgetOZ
		,sum(NumberBudgetZ) as BudgetZ
		,sum(NumberQuotaO) as QuotaO
		,sum(NumberQuotaOZ) as QuotaOZ
		,sum(NumberQuotaZ) as QuotaZ
		,sum(NumberTargetO) as TargetO
		,sum(NumberTargetOZ) as TargetOZ
		,sum(NumberTargetZ) as TargetZ
	from DistributedAdmissionVolume dav
	where dav.AdmissionVolumeID = av.AdmissionVolumeID
) as davSum
where av.InstitutionID = isnull(@institutionId, av.InstitutionID)
and av.CampaignID = isnull(@campaignId, av.CampaignID)
and av.DirectionID = isnull(@directionId, av.DirectionID);





/* 
-- ЗАПУСТИТЬ 1 РАЗ! --
create function dbo.InlineMax(@val1 int, @val2 int)
returns int
as
begin
  if @val1 > @val2
    return @val1
  return isnull(@val2,@val1)
end
*/