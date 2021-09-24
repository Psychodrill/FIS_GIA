CREATE function [dbo].[ReportRegistrationShortTVF](
	@periodBegin DATETIME = NULL
	, @periodEnd DATETIME = NULL)
RETURNS @report TABLE 
(	
[Правовая форма] NVARCHAR(255) NULL
,[Зарегистрировано] INT null
,[Не зарегистрировано] INT null
,[Всего] INT null
)
AS 
BEGIN

 
DECLARE @RegistredOrgsPrivCount INT
SELECT @RegistredOrgsPrivCount=COUNT(*) FROM dbo.ReportRegistredOrgsTVF(null,null)
WHERE [ОПФ]='Частный' AND [Тип]='ВУЗ'
DECLARE @NotRegistredOrgsPrivCount INT
SELECT @NotRegistredOrgsPrivCount=COUNT(*) FROM dbo.ReportNotRegistredOrgsTVF(null,null)
WHERE [ОПФ]='Частный' AND [Тип]='ВУЗ'
DECLARE @RegistredOrgsStateCount INT
SELECT @RegistredOrgsStateCount=COUNT(*) FROM dbo.ReportRegistredOrgsTVF(null,null)
WHERE [ОПФ]='Гос-ный' AND [Тип]='ВУЗ'
DECLARE @NotRegistredOrgsStateCount INT
SELECT @NotRegistredOrgsStateCount=COUNT(*) FROM dbo.ReportNotRegistredOrgsTVF(null,null)
WHERE [ОПФ]='Гос-ный' AND [Тип]='ВУЗ'


DECLARE @RegistredOrgsPrivAccredCount INT
SELECT @RegistredOrgsPrivAccredCount=COUNT(*) FROM dbo.ReportRegistredOrgsTVF(null,null)
WHERE [ОПФ]='Частный' AND [Тип]='ВУЗ' AND [Аккредитация по справочнику]='Есть'
DECLARE @NotRegistredOrgsPrivAccredCount INT
SELECT @NotRegistredOrgsPrivAccredCount=COUNT(*) FROM dbo.ReportNotRegistredOrgsTVF(null,null)
WHERE [ОПФ]='Частный' AND [Тип]='ВУЗ' AND [Аккредитация по справочнику]='Есть'
DECLARE @RegistredOrgsStateAccredCount INT
SELECT @RegistredOrgsStateAccredCount=COUNT(*) FROM dbo.ReportRegistredOrgsTVF(null,null)
WHERE [ОПФ]='Гос-ный' AND [Тип]='ВУЗ' AND [Аккредитация по справочнику]='Есть'
DECLARE @NotRegistredOrgsStateAccredCount INT
SELECT @NotRegistredOrgsStateAccredCount=COUNT(*) FROM dbo.ReportNotRegistredOrgsTVF(null,null)
WHERE [ОПФ]='Гос-ный' AND [Тип]='ВУЗ' AND [Аккредитация по справочнику]='Есть'

INSERT INTO @Report
SELECT 'Государственный',@RegistredOrgsStateCount,@NotRegistredOrgsStateCount,@RegistredOrgsStateCount+@NotRegistredOrgsStateCount
INSERT INTO @Report
SELECT 'Негосударственный',@RegistredOrgsPrivCount,@NotRegistredOrgsPrivCount,@RegistredOrgsPrivCount+@NotRegistredOrgsPrivCount
INSERT INTO @Report
SELECT 'Итого'
,@RegistredOrgsStateCount+@RegistredOrgsPrivCount
,@NotRegistredOrgsStateCount+@NotRegistredOrgsPrivCount
,@RegistredOrgsStateCount+@NotRegistredOrgsStateCount+@RegistredOrgsPrivCount+@NotRegistredOrgsPrivCount


INSERT INTO @Report
SELECT '',null,null,null
INSERT INTO @Report
SELECT 'Аккредитованных',null,null,null


INSERT INTO @Report
SELECT 'Государственный',@RegistredOrgsStateAccredCount,@NotRegistredOrgsStateAccredCount,@RegistredOrgsStateAccredCount+@NotRegistredOrgsStateAccredCount
INSERT INTO @Report
SELECT 'Негосударственный',@RegistredOrgsPrivAccredCount,@NotRegistredOrgsPrivAccredCount,@RegistredOrgsPrivAccredCount+@NotRegistredOrgsPrivAccredCount
INSERT INTO @Report
SELECT 'Итого'
,@RegistredOrgsStateAccredCount+@RegistredOrgsPrivAccredCount
,@NotRegistredOrgsStateAccredCount+@NotRegistredOrgsPrivAccredCount
,@RegistredOrgsStateAccredCount+@NotRegistredOrgsStateAccredCount+@RegistredOrgsPrivAccredCount+@NotRegistredOrgsPrivAccredCount

RETURN
END
