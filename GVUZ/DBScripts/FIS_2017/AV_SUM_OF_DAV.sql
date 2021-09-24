-- ���� update �� ���� ������ ���� - ������ ������ @institutionId,
-- ���� ������ �� �� - ������ @campaignId (����� ����� @institutionId = null)
-- ���� �� �� � ������ ����������� - ������ @campaignId � @directionId

-- ���� ������ ������, ��� �� ���������� ������� dbo.InlineMAX, 
-- �� 1 ��� ���� ��������� �� �������� ����� ������� 

USE [GVUZ_PROM_COPY] -- ������� ���� ����, ����� �� ��������� �������� �� �����-�� ������ ��!

declare @institutionId int = null; -- �� ����������� ��������, ���� ����� @campaignId; 
declare @campaignId int = 9985;
declare @directionId int = null; 

if (@institutionId is null and @campaignId is null) 
begin
	select '�� ����� �������, ��� ������ ������� update �� ����-���� ����? ����� �������� ������������� ������� return �������� ����';
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
-- ��������� 1 ���! --
create function dbo.InlineMax(@val1 int, @val2 int)
returns int
as
begin
  if @val1 > @val2
    return @val1
  return isnull(@val2,@val1)
end
*/