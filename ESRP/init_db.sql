USE [esrp_prod_2016]
GO
/****** Object:  FullTextCatalog [Organization2010]    Script Date: 27.11.2018 15:32:58 ******/
CREATE FULLTEXT CATALOG [Organization2010] WITH ACCENT_SENSITIVITY = ON
GO
/****** Object:  UserDefinedFunction [dbo].[CanEditUserAccount]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--------------------------------------------------
-- Можно ли редактировать пользователю свою
-- учетную запись.
-- v.1.0: Created by Fomin Dmitriy 04.04.2008
-- v.1.0: Modified by Fomin Dmitriy 19.05.2008
-- Модифицировать анкету можно до утверждения ее
-- документом.
--------------------------------------------------
CREATE function [dbo].[CanEditUserAccount]
	(
	@status nvarchar(255)
	, @confirmYear int
	, @currentYear int
	)
returns bit
as
begin
	return case
			when not @status in ('activated', 'deactivated') then 1
			else 0
		end
end
GO
/****** Object:  UserDefinedFunction [dbo].[CanEditUserAccountRegistrationDocument]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--------------------------------------------------
-- Можно ли редактировать пользователю свой
-- документ регистрации.
-- v1.0: Created by Fomin Dmitriy 04.04.2008
--------------------------------------------------
CREATE function [dbo].[CanEditUserAccountRegistrationDocument]
	(
	@status nvarchar(255)
	)
returns bit
as
begin
	return case
			when not @status in ('activated', 'deactivated') then 1
			else 0
		end
end
GO
/****** Object:  UserDefinedFunction [dbo].[CanViewUserAccountRegistrationDocument]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--------------------------------------------------
-- Можно ли просматривать документ регистрации.
-- v1.0: Created by Fomin Dmitriy 07.04.2008
--------------------------------------------------
CREATE function [dbo].[CanViewUserAccountRegistrationDocument]
	(
	@confirmYear int
	)
returns bit
as
begin
	return case
			when @confirmYear = year(getdate()) then 1
			else 0
		end
end
GO
/****** Object:  UserDefinedFunction [dbo].[CompareStrings]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Получить внешний ИД.
-- v.1.0: Created by Fomin Dmitriy 17.04.2008
-- v.1.1: Modified by Fomin Dmitriy 06.05.2008
-- Длина сравниваемых строк увеличена.
-- =============================================
CREATE function [dbo].[CompareStrings]
	(
	@string1 nvarchar(4000)
	, @string2 nvarchar(4000)
	-- Чувствительность: кол-во совпадающих символов подстроки.
	, @matchCount int 
	)
returns decimal(18, 4)
as
begin
	declare
		@compareStr1 nvarchar(4000)
		, @compareStr2 nvarchar(4000)
		, @i int
		, @j int
		, @count1 int
		, @count int

	set @matchCount = isnull(@matchCount, 3)
	set @compareStr1 = replace(isnull(@string1, ''), ' ', '')
	set @compareStr2 = replace(isnull(@string2, ''), ' ', '')
	set @count = 0

	if @compareStr1 = @compareStr2
		return 1

	if len(@compareStr1) = 0 or len(@compareStr2) = 0
		return 0

	set @i = 1
	while @i < len(@compareStr1)
	begin
		set @j = 1
		while @j < len(@compareStr2)	
		begin
			if substring(@compareStr1, @i, 1) = substring(@compareStr2, @j, 1)
			begin
				set @count1 = 1
				while (@i + @count1 <= len(@compareStr1)) and (@j + @count1 <= len(@compareStr2))
						and (substring(@compareStr1, @i + @count1, 1) = substring(@compareStr2, @j + @count1, 1))
					set @count1 = @count1 + 1
				set @i = @i + @count1 - 1
				set @j = @j + @count1 - 1
				if @count1 >= @matchCount
					set @count = @count + @count1
			end
			set @j = @j + 1
		end
		set @i = @i + 1
	end
	
	if len(@compareStr1) > len(@compareStr2)
		return cast(@count as decimal(18, 4)) / cast(len(@compareStr1) as decimal(18, 4))
	else
		return cast(@count as decimal(18, 4)) / cast(len(@compareStr2) as decimal(18, 4))

	return 0
end
GO
/****** Object:  UserDefinedFunction [dbo].[GetDelimitedValues]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--------------------------------------------------
-- Разбивает исходную строку на части, разделенные запятыми.
-- v.1.0: Created by Makarev Andrey 04.04.2008
-- v.1.1: Modified by Makarev Andrey 16.04.2008
-- Измение размера выходного массива.
-- v.1.2: Rewritten by Valeev Denis 20.05.2009
-- Переписал в рамках оптимизации через xml
--------------------------------------------------
CREATE function [dbo].[GetDelimitedValues]
	(
	@ids nvarchar(4000)
	)
returns @Values table ([value] nvarchar(4000))
as
begin
	if len(ltrim(rtrim(@ids))) > 0
	begin
		DECLARE @x xml
		set @x = '<root><v>' + replace(@ids, ',', '</v><v>') + '</v></root>'
		insert into @Values
		SELECT  T.c.value('.','nvarchar(4000)')
		FROM    @x.nodes('/root/v') T ( c )
	end
	return	
end


GO
/****** Object:  UserDefinedFunction [dbo].[GetExternalId]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Получить внешний ИД.
-- v.1.0: Created by Makarev Andrey 16.04.2008
-- =============================================
CREATE function [dbo].[GetExternalId]
	(
	@internalId bigint
	)
returns bigint
as
begin
	if isnull(@internalId, -1) < 0
		return null
	if @internalId = 0
		return 0

	declare
		@base bigint
		, @shift bigint
		, @shiftedId bigint

	set @base = power(2, 20)
	set @shift = 11541954384
	
	set @shiftedId = @internalId + @shift
	return (@shiftedId / @base) + (@shiftedId % @base) * @base
end
GO
/****** Object:  UserDefinedFunction [dbo].[GetInternalId]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Получить внутренний ИД.
-- v.1.0: Created by Makarev Andrey 16.04.2008
-- =============================================
CREATE function [dbo].[GetInternalId]
	(
	@externalId bigint
	)
returns bigint
as
begin
	declare
		@result bigint

	if isnull(@externalId, -1) < 0
		return null

	if @externalId = 0
		return 0

	declare
		@base bigint
		, @shift bigint

	set @base = power(2, 20)
	set @shift = 11541954384
	
	set @result = (@externalId / @base) + (@externalId % @base) * @base - @shift
	if dbo.GetExternalId(@result) <> @externalId
		return -1
	return @result
end
GO
/****** Object:  UserDefinedFunction [dbo].[GetShortOrganizationName]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Получить короткое наименование организации.
-- v.1.0: Created by Fomin Dmitriy 06.05.2008
-- v.1.1: Modified by Fomin Dmitriy 08.05.2008
-- Поле наименования организации увеличено.
-- =============================================
CREATE function [dbo].[GetShortOrganizationName]
	(
	@organizationName nvarchar(2000)
	)
returns nvarchar(2000)
as
begin
	-- Список сокращений.
	declare @word_abbreviation table
		(
		Word nvarchar(255)
		, Abbreviation nvarchar(255)
		)

	insert into @word_abbreviation values ('федеральный', 'Ф')
	insert into @word_abbreviation values ('республиканский', 'Р')
	insert into @word_abbreviation values ('областной', 'О')

	insert into @word_abbreviation values ('государственный', 'Г')
	insert into @word_abbreviation values ('негосударственный', 'НГ')
	insert into @word_abbreviation values ('образовательное', 'О')
	insert into @word_abbreviation values ('учреждение', 'У')
	insert into @word_abbreviation values ('высшего', 'В')
	insert into @word_abbreviation values ('среднего', 'С')
	insert into @word_abbreviation values ('профессионального', 'П')
	insert into @word_abbreviation values ('образования', 'О')

	insert into @word_abbreviation values ('университет', 'УНВ')
	insert into @word_abbreviation values ('академия', 'АКД')
	insert into @word_abbreviation values ('институт', 'ИНС')
	insert into @word_abbreviation values ('училище', 'УЧЛ')
	insert into @word_abbreviation values ('техникум', 'ТХК')
	insert into @word_abbreviation values ('колледж', 'КЛЖ')

	insert into @word_abbreviation values ('медицинский', 'МЕДИЦ')
	insert into @word_abbreviation values ('педагогический', 'ПЕДАГ')
	insert into @word_abbreviation values ('правосудия', 'ПРАВО')
	insert into @word_abbreviation values ('технический', 'ТЕХНЧ')
	insert into @word_abbreviation values ('технологический', 'ТЕХЛГ')
	insert into @word_abbreviation values ('политехнический', 'ПОЛТХ')
	insert into @word_abbreviation values ('юридический', 'ЮРИДЧ')
	insert into @word_abbreviation values ('текстильный', 'ТЕКСТ')
	insert into @word_abbreviation values ('сельскохозяйственная', 'СЕЛХЗ')
	insert into @word_abbreviation values ('машиностроительный', 'МАШСТ')
	insert into @word_abbreviation values ('приборостроительный', 'ПРБСТ')
	insert into @word_abbreviation values ('строительный', 'СТРОЙ')

	insert into @word_abbreviation values ('министерства', 'М')
	insert into @word_abbreviation values ('внутренних дел', 'ВД')
	insert into @word_abbreviation values ('юстиции', 'Ю')
	insert into @word_abbreviation values ('Российской Федерации', 'РФ')
	insert into @word_abbreviation values ('имени', 'им')

	-- Разбиение названия организации на слова.
	declare @organization_word table
		(
		Word nvarchar(255)
		)

	declare
		@startIndex int
		, @delimiterIndex int
		, @value nvarchar(4000)

	set @startIndex = 1
	set @delimiterIndex = charindex(' ', isnull(@organizationName, ''))
	while @delimiterIndex > 0
	begin
		set @value = ltrim(rtrim(substring(@organizationName, @startIndex, @delimiterIndex  - @startIndex)))
		if @value <> ''
			insert into @organization_word
			select @value
	
		set @startIndex = @delimiterIndex + 1
		set @delimiterIndex = charindex(' ', @organizationName, @startIndex)
	end

	if len(@organizationName) >= @startIndex 
	begin
		set @value = ltrim(rtrim(substring(@organizationName, @startIndex, len(@organizationName) - @startIndex + 1)))
		if @value <> ''
				insert into @organization_word
				select @value
	end

	-- Вывод результата.
	declare
		@word nvarchar(255)
		, @result nvarchar(2000)
		, @sameLevel decimal(18, 4)
		, @matchCount int

	set @sameLevel = 0.7
	set @matchCount = 3

	set @result = ''

	declare abbreviation_cursor cursor for
	select
		-- Вывести наилучшее сокращение, если такое есть.
		isnull((select top 1 word_abbreviation.Abbreviation 
				from (select word_abbreviation.Abbreviation 
							, dbo.CompareStrings(organization_word.Word, word_abbreviation.Word, @matchCount) SameLevel
						from @word_abbreviation word_abbreviation) word_abbreviation
				where word_abbreviation.SameLevel >= @sameLevel
				order by word_abbreviation.SameLevel desc), organization_word.Word)
	from @organization_word organization_word

	open abbreviation_cursor 
	fetch next from abbreviation_cursor into @word
	while @@fetch_status = 0
	begin
		if len(@result) > 0
			set @result = @result + ' '
		set @result = @result + @word

		fetch next from abbreviation_cursor into @word
	end
	close abbreviation_cursor
	deallocate abbreviation_cursor

	return @result
end
GO
/****** Object:  UserDefinedFunction [dbo].[GetSubjectMarks]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--------------------------------------------------
-- Разбивает исходную строку на части, разделенные 
-- запятыми и знаками =.
-- v.1.0: Created by Makarev Andrey 23.05.2008
-- v.1.1: Modified by Fomin Dmitriy 27.05.2008
-- Приведение к стандарту.
-- v.1.2: Rewritten by Valeev Denis 20.05.2009
-- Переписал через xml для оптимизации
-- v.1.3: Rewritten by Yusupov Kirill 1.07.2010
-- Переписал через цикл для оптимизации
--------------------------------------------------
CREATE function [dbo].[GetSubjectMarks]
	(
	@subjectMarks nvarchar(4000)
	)
returns @SubjectMark table (SubjectId int, Mark numeric(5,1))
--returns @SubjectMark table (SubjectId NVARCHAR(20), Mark NVARCHAR(20))
as
begin
	DECLARE @RawMark NVARCHAR(20)
	DECLARE @EQIndex INT
	WHILE (CHARINDEX(',',@subjectMarks)>0)
	BEGIN
		SET @RawMark= SUBSTRING(@subjectMarks,1,CHARINDEX(',',@subjectMarks)-1)

		SET @EQIndex=CHARINDEX('=',@RawMark)

		INSERT INTO @SubjectMark (SubjectId,Mark)
		SELECT 
			SUBSTRING(@RawMark,1,@EQIndex-1),SUBSTRING(@RawMark,@EQIndex+1,LEN(@RawMark)-@EQIndex+1)

		SET @subjectMarks = SUBSTRING(@subjectMarks,CHARINDEX(',',@subjectMarks)+1,LEN(@subjectMarks))
	END
	IF (LEN(@subjectMarks)>0)
	BEGIN
		SET @RawMark= @subjectMarks

		SET @EQIndex=CHARINDEX('=',@RawMark)

		INSERT INTO @SubjectMark (SubjectId,Mark)
		SELECT 
			SUBSTRING(@RawMark,1,@EQIndex-1),SUBSTRING(@RawMark,@EQIndex+1,LEN(@RawMark)-@EQIndex+1)
	END
	RETURN
end
GO
/****** Object:  UserDefinedFunction [dbo].[GetUserIsActive]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--------------------------------------------------
-- Возвращает состояние действующей записи пользователя.
-- v1.0: Created by Fomin Dmitriy 10.04.2008
--------------------------------------------------
CREATE function [dbo].[GetUserIsActive]
	(
	@status nvarchar(255)
	)
returns nvarchar(255) 
as  
begin
	return case
			when @status = N'deactivated' then 0
			else 1
		end
end

	
	
GO
/****** Object:  UserDefinedFunction [dbo].[GetUserStatus]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--------------------------------------------------
-- Возвращает статус пользователя.
-- v.1.0: Created by Makarev Andrey 04.04.2008
-- v.1.1: Modified by Fomin Dmitriy 10.04.2008
-- Убран параметр isActive, теперь он вычисляется 
-- на основании статуса, а не наоборот.
-- v.1.2: Modified by Fomin Dmitriy 19.04.2008
-- Статус корректируется автоматически.
--------------------------------------------------
CREATE function [dbo].[GetUserStatus]
  (
  @confirmYear int
  , @status nvarchar(255)
  , @currentYear int
  , @registrationDocument image
  )
returns nvarchar(255) 
as  
begin
  set @status = isnull(@status, N'registration')
  --if @confirmYear < Year(GetDate()) 
  --  set @status = N'registration'

  return case
      when not @registrationDocument is null and @status = N'registration'
        then N'consideration'
      when @registrationDocument is null and not @status in (N'activated', N'deactivated')
        then N'registration'
      else @status
    end
end

  
  
GO
/****** Object:  UserDefinedFunction [dbo].[GetUserStatusOrder]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--------------------------------------------------
-- Порядковый номер статуса.
-- v.1.0: Created by Fomin Dmitriy 11.04.2008
--------------------------------------------------
CREATE function [dbo].[GetUserStatusOrder]
	(
	@status nvarchar(255)
	)
returns int
as
begin
	return case
			when @status = 'consideration' then 1
			when @status = 'revision' then 2
			when @status = 'activated' then 3
			when @status = 'registration' then 4
			when @status = 'deactivated' then 5
			else 5
		end
end
GO
/****** Object:  UserDefinedFunction [dbo].[IsUserFromMainOrg]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE function [dbo].[IsUserFromMainOrg](@login nvarchar(255))
returns bit
as
BEGIN
DECLARE @ret bit
SELECT @ret = CASE WHEN EXISTS ( SELECT   *
                       FROM     dbo.Organization2010
                       WHERE    MainId IN (
                                SELECT  org.Id
                                FROM    dbo.Account acc
                                        INNER JOIN dbo.OrganizationRequest2010 req ON acc.OrganizationId = req.Id
                                        INNER JOIN dbo.Organization2010 org ON req.OrganizationId = org.Id
                                WHERE   acc.[Login] = LTRIM(RTRIM(@login)) ) )
         THEN 1
         ELSE 0
    END 
	return @ret
END

GO
/****** Object:  UserDefinedFunction [dbo].[ReportEditedOrgsTVF]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[ReportEditedOrgsTVF](
	@periodBegin DATETIME = NULL
	, @periodEnd DATETIME = NULL)
RETURNS @report TABLE 
(
[Полное наименование] NVARCHAR(4000) NULL
,[Краткое наименование] NVARCHAR(2000) null
--,[Дата создания] datetime null
--,[Создана из справочника] bit null
,[Имя региона] nvarchar(255) null
,[Код региона] nvarchar(255) null
,[Тип] nvarchar(255) null
,[Вид] nvarchar(255) null
,[ОПФ] nvarchar(50) null
,[Филиал] nvarchar(50) null
,[Аккредитация по справочнику] nvarchar(20) null
,[Свидетельство об аккредитации] nvarchar(255) null
,[Аккредитация по факту] nvarchar(255) null
,[ФИО руководителя] nvarchar(255) null
,[Должность руководителя] nvarchar(255) null
,[Ведомственная принадлежность] nvarchar(500) null
,[Фактический адрес] nvarchar(255) null
,[Юридический адрес] nvarchar(255) null
,[Код города] nvarchar(255) null
,[Телефон] nvarchar(255) null
,[EMail] nvarchar(255) null
,[ИНН] nvarchar(10) null
,[ОГРН] nvarchar(13) null
,[Импортирована из справочника] nvarchar(13) null
)
AS 
BEGIN


INSERT INTO @Report
SELECT 
Org.FullName AS [Полное наименование]
,ISNULL(Org.ShortName,'') AS [Краткое наименование]
--,Org.CreateDate AS [Дата создания]
--,Org.WasImportedAtStart AS [Создана из справочника]
,Reg.[Name] AS [Имя региона]
,Reg.Code AS [Код региона]
,OrgType.[Name] AS [Тип]
,OrgKind.[Name] AS [Вид]
,REPLACE(REPLACE(Org.IsPrivate,1,'Частный'),0,'Гос-ный') AS [ОПФ]
,REPLACE(REPLACE(Org.IsFilial,1,'Да'),0,'Нет') AS [Филиал]
,CASE 
	WHEN (Org.IsAccredited IS NULL OR Org.IsAccredited=0)
	THEN 'Нет'
	ELSE 'Есть'
	END AS [Аккредитация по справочнику]
,ISNULL(Org.AccreditationSertificate,'') AS [Свидетельство об аккредитации]
,CASE 
	WHEN (Org.IsAccredited=1 OR (Org.AccreditationSertificate IS NOT NULL AND Org.AccreditationSertificate!= ''))
	THEN 'Есть'
	ELSE 'Нет'
	END AS [Аккредитация по факту] 	
,Org.DirectorFullName AS [ФИО руководителя]
,Org.DirectorPosition AS [Должность руководителя]
,Org.OwnerDepartment AS [Ведомственная принадлежность]
,Org.FactAddress AS [Фактический адрес]
,Org.LawAddress AS [Юридический адрес]
,Org.PhoneCityCode AS[Код города]
,Org.Phone AS [Телефон]
,Org.EMail AS [EMail]
,Org.INN AS [ИНН]
,Org.OGRN AS [ОГРН]
,CASE WHEN (Org.WasImportedAtStart=1)
	THEN 'Да'
	ELSE 'Нет'
	END AS [Импортирована из справочника]

FROM 
Organization2010 Org 
INNER JOIN Region Reg 
ON Reg.Id=Org.RegionId
INNER JOIN OrganizationType2010 OrgType
ON OrgType.Id=Org.TypeId
INNER JOIN OrganizationKind OrgKind
ON OrgKind.Id=Org.KindId
WHERE (Org.CreateDate != Org.UpdateDate AND Org.WasImportedAtStart =1) OR (Org.WasImportedAtStart=0)
ORDER BY Org.WasImportedAtStart


RETURN
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportNotRegistredOrgsTVF]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[ReportNotRegistredOrgsTVF](
	@periodBegin DATETIME = NULL
	, @periodEnd DATETIME = NULL)
RETURNS @report TABLE 
(
[Полное наименование] NVARCHAR(4000) NULL
,[Краткое наименование] NVARCHAR(2000) null
--,[Дата создания] datetime null
--,[Создана из справочника] bit null
,[Имя региона] nvarchar(255) null
,[Код региона] nvarchar(255) null
,[Тип] nvarchar(255) null
,[Вид] nvarchar(255) null
,[ОПФ] nvarchar(50) null
,[Филиал] nvarchar(50) null
,[Аккредитация по справочнику] nvarchar(20) null
,[Свидетельство об аккредитации] nvarchar(255) null
,[Аккредитация по факту] nvarchar(255) null
,[ФИО руководителя] nvarchar(255) null
,[Должность руководителя] nvarchar(255) null
,[Ведомственная принадлежность] nvarchar(500) null
,[Фактический адрес] nvarchar(255) null
,[Юридический адрес] nvarchar(255) null
,[Код города] nvarchar(255) null
,[Телефон] nvarchar(255) null
,[EMail] nvarchar(255) null
,[ИНН] nvarchar(10) null
,[ОГРН] nvarchar(13) null
)
AS 
BEGIN

 
DECLARE @UsersByOrgs TABLE (
OrganizationId INT
,UsersCount INT
)
INSERT INTO @UsersByOrgs
SELECT 
IOrgReq.OrganizationId AS OrganizationId
,COUNT(*) AS UsersCount
FROM 
OrganizationRequest2010 IOrgReq		
WHERE IOrgReq.OrganizationId IS NOT NULL 
GROUP BY IOrgReq.OrganizationId


INSERT INTO @Report
SELECT 
Org.[Полное наименование]  
,Org.[Краткое наименование]  
,Org.[Имя региона]  
,Org.[Код региона]  
,Org.[Тип]  
,Org.[Вид]  
,Org.[ОПФ]  
,Org.[Филиал]  
,Org.[Аккредитация по справочнику]  
,Org.[Свидетельство об аккредитации]  
,Org.[Аккредитация по факту]  
,Org.[ФИО руководителя]  
,Org.[Должность руководителя]  
,Org.[Ведомственная принадлежность]  
,Org.[Фактический адрес]  
,Org.[Юридический адрес]  
,Org.[Код города]  
,Org.[Телефон]  
,Org.[EMail]  
,Org.[ИНН] 
,Org.[ОГРН] 

FROM dbo.ReportOrgsBASE() Org
WHERE Id NOT IN 
	(SELECT OReq.OrganizationId 
	FROM OrganizationRequest2010 OReq
	WHERE OReq.OrganizationId  IS NOT NULL)


RETURN
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportOrgActivation_OTHER]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[ReportOrgActivation_OTHER]()
RETURNS @OTHER TABLE 
(
	[Вид] nvarchar(500) null, 
	[Правовая форма] nvarchar(50) null, 
	[В БД] int null,
	[Всего] int null,
	[из них на регистрации] int null, 
	[из них на согласовании] int null,
	[из них на доработке] int null, 
	[из них действующие] int null,
	[из них отключенные] int null
)
AS
BEGIN
	
DECLARE @RCOI INT
DECLARE @OUO INT
DECLARE @OtherOrg INT
SELECT @RCOI = COUNT(*) FROM Organization2010 WHERE TypeId=3 
SELECT @OUO = COUNT(*) FROM Organization2010 WHERE TypeId=4 
SELECT @OtherOrg = COUNT(*) FROM Organization2010 WHERE TypeId=5

INSERT INTO @OTHER
(
[Вид],
[Правовая форма],
[В БД],
[Всего],
[из них на регистрации],
[из них на согласовании],
[из них на доработке],
[из них действующие],
[из них отключенные]
)
SELECT 'РЦОИ','',@RCOI,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM OrganizationRequest2010 OrgReq
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND OrgReq.TypeId=3

UNION ALL
SELECT
'Орган управления образованием','',@OUO,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM OrganizationRequest2010 OrgReq
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND OrgReq.TypeId=4 
UNION ALL
SELECT
'Другое','',@OtherOrg,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM OrganizationRequest2010 OrgReq
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND OrgReq.TypeId<>1 AND OrgReq.TypeId<>2 AND OrgReq.TypeId<>3 AND OrgReq.TypeId<>4 AND OrgReq.TypeId<>5

return
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportOrgActivation_SSUZ]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[ReportOrgActivation_SSUZ]()
RETURNS @VUZ TABLE 
(
	[Вид] nvarchar(500) null, 
	[Правовая форма] nvarchar(50) null, 
	[В БД] int null,
	[Всего] int null,
	[из них на регистрации] int null, 
	[из них на согласовании] int null,
	[из них на доработке] int null, 
	[из них действующие] int null,
	[из них отключенные] int null
)
AS
BEGIN
	
DECLARE @SSUZState INT
SELECT @SSUZState = COUNT(*) FROM Organization2010 WHERE TypeId=2 AND  IsPrivate=0
DECLARE @SSUZPriv INT
SELECT @SSUZPriv = COUNT(*) FROM Organization2010 WHERE TypeId=2 AND  IsPrivate=1
INSERT INTO @VUZ
(
[Вид],
[Правовая форма],
[В БД],
[Всего],
[из них на регистрации],
[из них на согласовании],
[из них на доработке],
[из них действующие],
[из них отключенные]
)
SELECT 'ССУЗ','Государственный',@SSUZState,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM OrganizationRequest2010 OrgReq
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND OrgReq.TypeId=2 AND OrgReq.IsPrivate=0

UNION ALL
SELECT
'ССУЗ','Негосударственный',@SSUZPriv,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM OrganizationRequest2010 OrgReq
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND OrgReq.TypeId=2 AND OrgReq.IsPrivate=1
UNION ALL
SELECT
'ССУЗ','Всего',@SSUZPriv+@SSUZState,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM OrganizationRequest2010 OrgReq
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND OrgReq.TypeId=2

return
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportOrgActivation_SSUZ_Accred]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[ReportOrgActivation_SSUZ_Accred]()
RETURNS @VUZ TABLE 
(
	[Вид] nvarchar(500) null, 
	[Правовая форма] nvarchar(50) null, 
	[В БД] int null,
	[Всего] int null,
	[из них на регистрации] int null, 
	[из них на согласовании] int null,
	[из них на доработке] int null, 
	[из них действующие] int null,
	[из них отключенные] int null
)
AS
BEGIN
	
DECLARE @SSUZState INT
SELECT @SSUZState = COUNT(*) FROM Organization2010 Org WHERE Org.TypeId=2 AND  Org.IsPrivate=0
AND (Org.IsAccredited=1 
		OR (
			Org.AccreditationSertificate != '' 
			AND Org.AccreditationSertificate IS NOT NULL
			))
DECLARE @SSUZPriv INT
SELECT @SSUZPriv = COUNT(*) FROM Organization2010  Org WHERE Org.TypeId=2 AND  Org.IsPrivate=1
AND (Org.IsAccredited=1 
		OR (
			Org.AccreditationSertificate != '' 
			AND Org.AccreditationSertificate IS NOT NULL
			))
INSERT INTO @VUZ
(
[Вид],
[Правовая форма],
[В БД],
[Всего],
[из них на регистрации],
[из них на согласовании],
[из них на доработке],
[из них действующие],
[из них отключенные]
)
SELECT 'ССУЗ','Государственный',@SSUZState,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM Organization2010 Org
INNER JOIN OrganizationRequest2010 OrgReq ON Org.Id=OrgReq.OrganizationId
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND Org.TypeId=2 AND Org.IsPrivate=0
AND (Org.IsAccredited=1 
		OR (
			Org.AccreditationSertificate != '' 
			AND Org.AccreditationSertificate IS NOT NULL
			))

UNION ALL
SELECT
'ССУЗ','Негосударственный',@SSUZPriv,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM Organization2010 Org
INNER JOIN OrganizationRequest2010 OrgReq ON Org.Id=OrgReq.OrganizationId
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND Org.TypeId=2 AND Org.IsPrivate=1
AND (Org.IsAccredited=1 
		OR (
			Org.AccreditationSertificate != '' 
			AND Org.AccreditationSertificate IS NOT NULL
			))
UNION ALL
SELECT
'ССУЗ','Всего',@SSUZPriv+@SSUZState,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM Organization2010 Org
INNER JOIN OrganizationRequest2010 OrgReq ON Org.Id=OrgReq.OrganizationId
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND Org.TypeId=2
AND (Org.IsAccredited=1 
		OR (
			Org.AccreditationSertificate != '' 
			AND Org.AccreditationSertificate IS NOT NULL
			))

return
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportOrgActivation_VUZ]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[ReportOrgActivation_VUZ]()
RETURNS @VUZ TABLE 
(
	[Вид] nvarchar(500) null, 
	[Правовая форма] nvarchar(50) null, 
	[В БД] int null,
	[Всего] int null,
	[из них на регистрации] int null, 
	[из них на согласовании] int null,
	[из них на доработке] int null, 
	[из них действующие] int null,
	[из них отключенные] int null
)
AS
BEGIN
	
DECLARE @VUZStateMain INT
DECLARE @VUZStateFilial INT
DECLARE @VUZPrivMain INT
DECLARE @VUZPrivFilial INT

SELECT @VUZStateMain = COUNT(*) FROM Organization2010 WHERE TypeId=1 AND IsFilial=0 AND IsPrivate=0
SELECT @VUZStateFilial = COUNT(*) FROM Organization2010 WHERE TypeId=1 AND IsFilial=1 AND IsPrivate=0
SELECT @VUZPrivMain = COUNT(*) FROM Organization2010 WHERE TypeId=1 AND IsFilial=0 AND IsPrivate=1
SELECT @VUZPrivFilial = COUNT(*) FROM Organization2010 WHERE TypeId=1 AND IsFilial=1 AND IsPrivate=1

INSERT INTO @VUZ
(
[Вид],
[Правовая форма],
[В БД],
[Всего],
[из них на регистрации],
[из них на согласовании],
[из них на доработке],
[из них действующие],
[из них отключенные]
)
SELECT 'ВУЗ','Государственный',@VUZStateFilial+@VUZStateMain,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM  OrganizationRequest2010 OrgReq  
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND OrgReq.TypeId=1 AND OrgReq.IsPrivate=0
UNION ALL
SELECT
'ВУЗ основной','Государственный',@VUZStateMain,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM  OrganizationRequest2010 OrgReq  
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND OrgReq.TypeId=1 AND OrgReq.IsFilial=0 AND OrgReq.IsPrivate=0
UNION ALL
SELECT
'ВУЗ филиал','Государственный',@VUZStateFilial,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM  OrganizationRequest2010 OrgReq  
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND OrgReq.TypeId=1 AND OrgReq.IsFilial=1 AND OrgReq.IsPrivate=0
UNION ALL
SELECT 'ВУЗ','Негосударственный',@VUZPrivFilial+@VUZPrivMain,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM  OrganizationRequest2010 OrgReq  
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND OrgReq.TypeId=1 AND OrgReq.IsPrivate=1
UNION ALL
SELECT
'ВУЗ основной','Негосударственный',@VUZPrivMain,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM  OrganizationRequest2010 OrgReq  
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND OrgReq.TypeId=1 AND OrgReq.IsFilial=0 AND OrgReq.IsPrivate=1
UNION ALL
SELECT
'ВУЗ филиал','Негосударственный',@VUZPrivFilial,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM  OrganizationRequest2010 OrgReq  
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND OrgReq.TypeId=1 AND OrgReq.IsFilial=1 AND OrgReq.IsPrivate=1
UNION ALL
SELECT
'ВУЗ','Всего',@VUZStateMain+@VUZStateFilial+@VUZPrivMain+@VUZPrivFilial,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM  OrganizationRequest2010 OrgReq  
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND OrgReq.TypeId=1

return
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportOrgActivation_VUZ_Accred]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[ReportOrgActivation_VUZ_Accred]()
RETURNS @VUZ TABLE 
(
	[Вид] nvarchar(500) null, 
	[Правовая форма] nvarchar(50) null, 
	[В БД] int null,
	[Всего] int null,
	[из них на регистрации] int null, 
	[из них на согласовании] int null,
	[из них на доработке] int null, 
	[из них действующие] int null,
	[из них отключенные] int null
)
AS
BEGIN
	
DECLARE @VUZState INT
DECLARE @VUZPriv INT

SELECT @VUZState = COUNT(*) FROM Organization2010 Org WHERE Org.TypeId=1 AND Org.IsPrivate=0
AND (Org.IsAccredited=1 
		OR (
			Org.AccreditationSertificate != '' 
			AND Org.AccreditationSertificate IS NOT NULL
			))

SELECT @VUZPriv = COUNT(*) FROM Organization2010 Org WHERE Org.TypeId=1 AND Org.IsPrivate=1
AND (Org.IsAccredited=1 
		OR (
			Org.AccreditationSertificate != '' 
			AND Org.AccreditationSertificate IS NOT NULL
			))

INSERT INTO @VUZ
(
[Вид],
[Правовая форма],
[В БД],
[Всего],
[из них на регистрации],
[из них на согласовании],
[из них на доработке],
[из них действующие],
[из них отключенные]
)
SELECT 'ВУЗ','Государственный',@VUZState,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM Organization2010 Org
INNER JOIN OrganizationRequest2010 OrgReq ON Org.Id=OrgReq.OrganizationId
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND Org.TypeId=1 AND Org.IsPrivate=0
AND (Org.IsAccredited=1 
		OR (
			Org.AccreditationSertificate != '' 
			AND Org.AccreditationSertificate IS NOT NULL
			))

UNION ALL
SELECT 'ВУЗ','Негосударственный',@VUZPriv,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM Organization2010 Org
INNER JOIN OrganizationRequest2010 OrgReq ON Org.Id=OrgReq.OrganizationId
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND Org.TypeId=1 AND Org.IsPrivate=1
AND (Org.IsAccredited=1 
		OR (
			Org.AccreditationSertificate != '' 
			AND Org.AccreditationSertificate IS NOT NULL
			))

UNION ALL
SELECT
'ВУЗ','Всего',@VUZState+@VUZPriv,
COUNT(Acc.Status),
COUNT(CASE WHEN Acc.Status='registration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='consideration' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='revision' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='activated' THEN 1 ELSE NULL END) ,
COUNT(CASE WHEN Acc.Status='deactivated' THEN 1 ELSE NULL END) 
FROM Organization2010 Org
INNER JOIN OrganizationRequest2010 OrgReq ON Org.Id=OrgReq.OrganizationId
INNER JOIN Account Acc ON Acc.OrganizationId=OrgReq.Id
INNER JOIN GroupAccount GA ON GA.AccountId=Acc.Id
INNER JOIN [Group] G ON G.Id=GA.GroupId
WHERE G.Code='User' AND Org.TypeId=1
AND (Org.IsAccredited=1 
		OR (
			Org.AccreditationSertificate != '' 
			AND Org.AccreditationSertificate IS NOT NULL
			))

return
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportOrgsBASE]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[ReportOrgsBASE]()
	
	
RETURNS @report TABLE 
(
[Id] INT 
,[Полное наименование] NVARCHAR(4000) NULL
,[Краткое наименование] NVARCHAR(2000) null
,[Дата создания] datetime null
,[Создана из справочника] bit null
,[Имя ФО] nvarchar(255) null
,[Код ФО] int null
,[Имя региона] nvarchar(255) null
,[Код региона] nvarchar(255) null
,[Тип] nvarchar(255) null
,[Вид] nvarchar(255) null
,[ОПФ] nvarchar(50) null
,[Филиал] nvarchar(50) null
,[Аккредитация по справочнику] nvarchar(20) null
,[Свидетельство об аккредитации] nvarchar(255) null
,[Аккредитация по факту] nvarchar(255) null
,[ФИО руководителя] nvarchar(255) null
,[Должность руководителя] nvarchar(255) null
,[Ведомственная принадлежность] nvarchar(500) null
,[Фактический адрес] nvarchar(255) null
,[Юридический адрес] nvarchar(255) null
,[Код города] nvarchar(255) null
,[Телефон] nvarchar(255) null
,[EMail] nvarchar(255) null
,[ИНН] nvarchar(10) null
,[ОГРН] nvarchar(13) null
)
AS 
BEGIN
 
INSERT INTO @Report
SELECT 
Org.Id as [Id]
,Org.FullName AS [Полное наименование]
,ISNULL(Org.ShortName,'') AS [Краткое наименование]
,Org.CreateDate AS [Дата создания]
,Org.WasImportedAtStart AS [Создана из справочника]
,FD.[Name] AS [Имя ФО]
,FD.Code AS [Код ФО]
,Reg.[Name] AS [Имя региона]
,Reg.Code AS [Код региона]
,OrgType.[Name] AS [Тип]
,OrgKind.[Name] AS [Вид]
,REPLACE(REPLACE(Org.IsPrivate,1,'Частный'),0,'Гос-ный') AS [ОПФ]
,REPLACE(REPLACE(Org.IsFilial,1,'Да'),0,'Нет') AS [Филиал]
,CASE 
	WHEN (Org.IsAccredited IS NULL OR Org.IsAccredited=0)
	THEN 'Нет'
	ELSE 'Есть'
	END AS [Аккредитация по справочнику]
,ISNULL(Org.AccreditationSertificate,'') AS [Свидетельство об аккредитации]
,CASE 
	WHEN (Org.IsAccredited=1 OR (Org.AccreditationSertificate IS NOT NULL AND Org.AccreditationSertificate!= ''))
	THEN 'Есть'
	ELSE 'Нет'
	END AS [Аккредитация по факту] 	
,Org.DirectorFullName AS [ФИО руководителя]
,Org.DirectorPosition AS [Должность руководителя]
,Org.OwnerDepartment AS [Ведомственная принадлежность]
,Org.FactAddress AS [Фактический адрес]
,Org.LawAddress AS [Юридический адрес]
,Org.PhoneCityCode AS[Код города]
,Org.Phone AS [Телефон]
,Org.EMail AS [EMail]
,Org.INN AS [ИНН]
,Org.OGRN AS [ОГРН]



FROM 
Organization2010 Org 
INNER JOIN Region Reg 
ON Reg.Id=Org.RegionId
INNER JOIN FederalDistricts FD
ON FD.Id=Reg.FederalDistrictId
INNER JOIN OrganizationType2010 OrgType
ON OrgType.Id=Org.TypeId
INNER JOIN OrganizationKind OrgKind
ON OrgKind.Id=Org.KindId

RETURN
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportOrgsInfoTVF_WithoutChecks]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[ReportOrgsInfoTVF_WithoutChecks](
	@periodBegin DATETIME = NULL
	, @periodEnd DATETIME = NULL)
RETURNS @report TABLE 
(
[Полное наименование] NVARCHAR(4000) NULL
,[Краткое наименование] NVARCHAR(2000) null
--,[Дата создания] datetime null
--,[Создана из справочника] bit null
,[Наименование ФО] nvarchar(255) null
,[Код ФО] nvarchar(255) null
,[Наименование региона] nvarchar(255) null
,[Код региона] nvarchar(255) null
,[Тип] nvarchar(255) null
,[Вид] nvarchar(255) null
,[ОПФ] nvarchar(50) null
,[Филиал] nvarchar(50) null
,[Аккредитация по справочнику] nvarchar(20) null
,[Свидетельство об аккредитации] nvarchar(255) null
,[Аккредитация по факту] nvarchar(255) null
,[ФИО руководителя] nvarchar(255) null
,[Должность руководителя] nvarchar(255) null
,[Ведомственная принадлежность] nvarchar(500) null
,[Фактический адрес] nvarchar(255) null
,[Юридический адрес] nvarchar(255) null
,[Код города] nvarchar(255) null
,[Телефон] nvarchar(255) null
,[EMail] nvarchar(255) null
,[ИНН] nvarchar(10) null
,[ОГРН] nvarchar(13) null
,[Есть пользователи] nvarchar(255) null
,[Количество пользователей] INT NULL
,[Пользователей активировано] INT NULL
,[Пользователей на рассмотрении] INT NULL
,[Пользователей отключено] INT NULL
,[Пользователей на регистрации] INT NULL
,[Пользователей на доработке] INT NULL
,[Первый зарегистрирован] DATETIME NULL
,[Последний зарегистрирован] DATETIME NULL
--,[Количество проверок по номеру] int null
--,[Количество уникальных проверок по номеру] INT NULL
--,[Количество проверок по паспортным данным] INT NULL
--,[Количество уникальных проверок по паспортным данным] INT NULL
--,[Количество проверок по типографскому номеру] INT NULL
--,[Количество уникальных проверок по типографскому номеру] INT NULL
--,[Количество интерактивных проверок] INT NULL
--,[Количество уникальных интерактивных проверок] INT NULL
--,[Количество неправильных проверок] INT NULL
--,[Первая проверка] datetime null
--,[Последняя проверка] datetime null
--,[Работа с ФБС] NVARCHAR(20)
)
AS 
BEGIN

--если не определены временные границы, то указывается промежуток = 1 суткам
--IF(@periodBegin IS NULL OR @periodEnd IS NULL)
	SELECT @periodBegin = DATEADD(YEAR, -1, GETDATE()), @periodEnd = GETDATE()
 
DECLARE @UsersByOrgs TABLE (
OrganizationId INT
,UsersCount INT
)
INSERT INTO @UsersByOrgs
SELECT 
IOrgReq.OrganizationId AS OrganizationId
,COUNT(*) AS UsersCount
FROM 
OrganizationRequest2010 IOrgReq		
WHERE IOrgReq.OrganizationId IS NOT NULL 
AND IOrgReq.CreateDate BETWEEN @periodBegin and @periodEnd
GROUP BY IOrgReq.OrganizationId


DECLARE @StatusesAndOrgs TABLE
(
	OrganizationId int,
	[Status] nvarchar(50),
	UsersCount int
)
INSERT INTO @StatusesAndOrgs
SELECT 
IOrgReq.OrganizationId AS OrganizationId,IAcc.Status  AS [Status]
,COUNT(*) AS UsersCount
FROM 
OrganizationRequest2010 IOrgReq		
INNER JOIN Account IAcc
ON IAcc.OrganizationId=IOrgReq.Id
WHERE IOrgReq.OrganizationId IS NOT NULL 
AND IOrgReq.CreateDate BETWEEN @periodBegin and @periodEnd
GROUP BY IOrgReq.OrganizationId,IAcc.Status

DECLARE @StatusesByOrgs TABLE
(
	OrganizationId int,
	[activated] nvarchar(20),
	[deactivated] nvarchar(20),
	[consideration] nvarchar(20),
	[registration] nvarchar(20),
	[revision] nvarchar(20)
)
INSERT INTO @StatusesByOrgs
SELECT 
OrganizationId,
[activated],
[deactivated],
[consideration],
[registration],
[revision]
FROM @StatusesAndOrgs
PIVOT 
(SUM(UsersCount) 
FOR [Status] IN ([activated],[deactivated],[consideration],[registration],[revision])
) AS Piv 


DECLARE @CreatedByOrgs TABLE (
OrganizationId INT
,FirstCreated DATETIME
,LastCreated DATETIME
)
INSERT INTO @CreatedByOrgs
SELECT 
IOrgReq.OrganizationId AS OrganizationId
,MIN(IOrgReq.CreateDate) AS FirstCreated
,MAX(IOrgReq.CreateDate) AS LastCreated
FROM 
OrganizationRequest2010 IOrgReq		
WHERE IOrgReq.OrganizationId IS NOT NULL
AND IOrgReq.CreateDate BETWEEN @periodBegin and @periodEnd
GROUP BY IOrgReq.OrganizationId







INSERT INTO @Report
SELECT 
Org.[Полное наименование] AS [Полное наименование]
,ISNULL(Org.[Краткое наименование],'') AS [Краткое наименование]
--,Org.CreateDate AS [Дата создания]
--,Org.WasImportedAtStart AS [Создана из справочника]
,Org.[Имя ФО] AS [Имя ФО]
,Org.[Код ФО] AS [Код ФО]
,Org.[Имя региона] AS [Имя региона]
,Org.[Код региона] AS [Код региона]
,Org.[Тип] AS [Тип]
,Org.[Вид] AS [Вид]
,Org.[ОПФ] AS [ОПФ]
,Org.[Филиал] AS [Филиал]
,Org.[Аккредитация по справочнику] AS [Аккредитация по справочнику]
,Org.[Свидетельство об аккредитации] AS [Свидетельство об аккредитации]
,Org.[Аккредитация по факту] AS [Аккредитация по факту] 	
,Org.[ФИО руководителя] AS [ФИО руководителя]
,Org.[Должность руководителя] AS [Должность руководителя]
,Org.[Ведомственная принадлежность] AS [Ведомственная принадлежность]
,Org.[Фактический адрес] AS [Фактический адрес]
,Org.[Юридический адрес] AS [Юридический адрес]
,Org.[Код города] AS[Код города]
,Org.[Телефон] AS [Телефон]
,Org.[EMail] AS [EMail]
,Org.[ИНН] AS [ИНН]
,Org.[ОГРН] AS [ОГРН]

,CASE 
	WHEN (ISNULL(UsersCnt.UsersCount,0)=0)
	THEN 'Нет'
	ELSE 'Есть'
	END AS [Есть пользователи]
,ISNULL(UsersCnt.UsersCount,0) AS [Количество пользователей]
,ISNULL(StatusesCnt.activated,0) AS [Пользователей активировано]
,ISNULL(StatusesCnt.consideration,0) AS [Пользователей на рассмотрении]
,ISNULL(StatusesCnt.deactivated,0) AS [Пользователей отключено]
,ISNULL(StatusesCnt.registration,0) AS [Пользователей на регистрации]
,ISNULL(StatusesCnt.revision,0) AS [Пользователей на доработке]
,CreationDates.FirstCreated AS [Первый зарегистрирован]
,CreationDates.LastCreated AS [Последний зарегистрирован]
--
--,ISNULL(NumberChecks.UniqueNumberChecks,0) AS [Количество уникальных проверок по номеру] 
--,ISNULL(NumberChecks.TotalNumberChecks,0) AS[Количество проверок по номеру]  
--,ISNULL(PassportChecks.TotalPassportChecks,0) AS [Количество проверок по паспортным данным]  
--,ISNULL(PassportChecks.UniquePassportChecks,0) AS [Количество уникальных проверок по паспортным данным]  
--,ISNULL(TNChecks.TotalTNChecks,0) AS [Количество проверок по типографскому номеру] 
--,ISNULL(TNChecks.UniqueTNChecks,0) AS [Количество уникальных проверок по типографскому номеру] 
--,ISNULL(UIChecks.TotalUIChecks,0) AS [Количество интерактивных проверок]
--,ISNULL(UIChecks.UniqueUIChecks,0) AS [Количество уникальных интерактивных проверок]
--,ISNULL(WrongChecks.WrongChecks,0) AS [Количество неправильных проверок]
--
--,LimitDates.FirstCheck AS [Первая проверка]  
--,LimitDates.LastCheck AS [Последняя проверка] 

--,CASE WHEN 
--ISNULL(NumberChecks.UniqueNumberChecks,0)
--+ISNULL(PassportChecks.UniquePassportChecks,0)
--+ISNULL(TNChecks.UniqueTNChecks,0)
--+ISNULL(UIChecks.UniqueUIChecks,0) 
--= 0 
--THEN 'Не работает'
--WHEN 
--ISNULL(NumberChecks.UniqueNumberChecks,0)
--+ISNULL(PassportChecks.UniquePassportChecks,0)
--+ISNULL(TNChecks.UniqueTNChecks,0)
--+ISNULL(UIChecks.UniqueUIChecks,0) 
--< 10 
--THEN 'Работа неактивна'
--ELSE 'Работает'
--END
--AS [Работа с ФБС]

FROM 
ReportOrgsBASE() Org 
LEFT JOIN @UsersByOrgs UsersCnt
ON Org.Id=UsersCnt.OrganizationId
LEFT JOIN @StatusesByOrgs StatusesCnt
ON Org.Id=StatusesCnt.OrganizationId
LEFT JOIN @CreatedByOrgs CreationDates
ON Org.Id=CreationDates.OrganizationId

--LEFT JOIN @NumberChecksByOrg NumberChecks
--ON Org.Id=NumberChecks.OrganizationId
--LEFT JOIN @TNChecksByOrg TNChecks
--ON Org.Id=TNChecks.OrganizationId
--LEFT JOIN @PassportChecksByOrg PassportChecks
--ON Org.Id=PassportChecks.OrganizationId
--LEFT JOIN @UIChecksByOrg UIChecks
--ON Org.Id=UIChecks.OrganizationId
--LEFT JOIN @WrongChecksByOrg WrongChecks
--ON Org.Id=WrongChecks.OrganizationId
--LEFT JOIN @CheckLimitDatesByOrg LimitDates
--ON Org.Id=LimitDates.OrganizationId

RETURN
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportOrgsRegistrationBASE]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE function [dbo].[ReportOrgsRegistrationBASE] ( )
RETURNS @report TABLE
    (
      [Id] INT,
      [GUID ИСЛОД] nvarchar(50) null,
      [Полное наименование] NVARCHAR(4000) NULL,
      [Краткое наименование] NVARCHAR(2000) null,
      [ИД головной] INT null,
      [Должность руководителя] nvarchar(1000) null, 
      [Должность руководителя (кому)] nvarchar(1000) null, 
      [ФО] nvarchar(1000) null,
      [Код ФО] int null,
      [Субъект РФ] nvarchar(1000) null,
      [Код субъекта] nvarchar(1000) null,
      [Тип] nvarchar(1000) null,
      [Статус ОО] nvarchar(1000) null,
      [Вид] nvarchar(1000) null,
      [ОПФ] nvarchar(50) null,
      [Филиал] nvarchar(50) null,
      [ФИО руководителя] nvarchar(1000) null,
      [И.О. Фамилия Руководителя] nvarchar(1000) null,
      [Фамилия руководителя] nvarchar(1000) null,
      [Имя руководителя] nvarchar(1000) null,
      [Отчество руководителя] nvarchar(1000) null,
      [Учредитель] nvarchar(1000) null,
      [Фактический адрес] nvarchar(1000) null,
      [Юридический адрес] nvarchar(1000) null,
      [Код города] nvarchar(1000) null,
      [Телефон] nvarchar(1000) null,
      [EMail] nvarchar(1000) null,
      [ИНН] nvarchar(10) null,
      [КПП] nvarchar(9) null,
      [ОГРН] nvarchar(13) null,
      [Данные проверены] nvarchar(10) null,
      [Срок подключения к ЗКСПД] datetime null,
      [Срок предоставления информации в ФИС] datetime null,
      [Головная организация] nvarchar(MAX) null,
      [Регион Филиала(код)] int null,
      [Регион учредителя(код)] int null,
      [Статус регистрации] nvarchar(80) null,
      [Обязательность регистрации] nvarchar(20) null
     
    )
AS BEGIN
 
    INSERT  INTO @Report
            SELECT  Org.Id as [Id],
            Org.ISLOD_GUID as [GUID ИСЛОД],
                    Org.FullName AS [Полное наименование],
                    ISNULL(Org.ShortName, '') AS [Краткое наименование],
                    ISNULL(Org.MainId,'0') AS [ИД головной],
                    Org.DirectorPosition AS [Должность руководителя],
                    Org.DirectorPositionInGenetive AS [Должность руководителя (кому)],
                    FD.[Name] AS [ФО],
                    FD.Code AS [Код ФО],
                    Reg.[Name] AS [Субъект РФ],
                    Reg.Code AS [Код субъекта],
                    OrgType.[Name] AS [Тип],
                    Case when (Org.RegionId=1000) then 'За пределами РФ' when (Org.DepartmentId=9553 or Org.DepartmentId=9564 or Org.DepartmentId=9566 Or Org.Id=1335 Or Org.Id=1838) then 'Военный' WHEN Org.Id = 21750 then 'Крым' else 'Обычный' end [Статус ОО],
                    OrgKind.[Name] AS [Вид],
                    Case when OrgType.[Name] = 'РЦОИ' OR OrgType.[Name] = 'Учредитель'  THEN 'Не установлено' ELSE REPLACE(REPLACE(Org.IsPrivate, 1, 'Частный'), 0, 'Гос-ный') END AS [ОПФ],
                    Case when OrgType.[Name] = 'РЦОИ' OR OrgType.[Name] = 'Учредитель' THEN 'Не установлено' ELSE REPLACE(REPLACE(Org.IsFilial, 1, 'Да'), 0, 'Нет') END AS [Филиал],
                    Org.DirectorFullName AS [ФИО руководителя],
                    Org.DirectorFullNameInGenetive AS [И.О. Фамилия Руководителя],
                    Org.DirectorLastName AS [Фамилия руководителя],
                    Org.DirectorFirstName AS [Имя руководителя],
                    Org.DirectorPatronymicName AS [Отчество руководителя],
                    isnull(Dep.FullName,'Не установлено') AS [Ведомственная принадлежность],
                    Org.FactAddress AS [Фактический адрес],
                    Org.LawAddress AS [Юридический адрес],
                    Org.PhoneCityCode AS [Код города],
                    Org.Phone AS [Телефон],
                    Org.EMail AS [EMail],
                    Org.INN AS [ИНН],
                    Case when Org.KPP='000000000' then '' else Org.KPP end as [КПП],
                    Org.OGRN AS [ОГРН],
                    CASE WHEN Org.OUConfirmation=1 THEN 'Да' ELSE 'Нет' END AS [Данные проверены],
                    Org.TimeConnectionToSecureNetwork as [Срок подключения к ЗКСПД],
                    org.TimeEnterInformationInFIS as [Срок предоставления информации в ФИС],
                    CASE WHEN Filial.FullName IS NOT NULL THEN Filial.FullName ELSE Org.FullName END as [Головная организация],
                    CASE WHEN Filial.FullName IS NOT NULL THEN Filial.RegionId ELSE Org.RegionId END as [Регион филиала(код)],
					CASE WHEN Dep.FullName IS NOT NULL THEN Dep.RegionId ELSE 1000 END as [Регион учредителя(код)],
                    ISNULL(ORS.Status, 'Нет') [Статус регистрации],
             
                    ISNULL(MDL.ModelType, 'Подключение') [Обязательность регистрации]
            FROM    Organization2010 Org
                    INNER JOIN Region Reg ON Reg.Id = Org.RegionId
                    INNER JOIN FederalDistricts FD ON FD.Id = Reg.FederalDistrictId
                    INNER JOIN OrganizationType2010 OrgType ON OrgType.Id = Org.TypeId
                    INNER JOIN OrganizationKind OrgKind ON OrgKind.Id = Org.KindId
                    LEFT JOIN Organization2010 Dep ON Org.DepartmentId = Dep.Id
                    Left JOIN Organization2010 Filial ON Org.MainId=Filial.Id
                    LEFT JOIN ( select distinct
                                        orq.OrganizationId,
                                        'Да' as Status                                  
                                from    Account A
                                        inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
                                where   Status = 'deactivated'
                                        and orq.OrganizationId not in (
                                        select distinct
                                                orq.OrganizationId
                                        from    Account A
                                                inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
                                        where   Status = 'activated'
                                                or status = 'consideration'
                                                or Status = 'revision'
                                                or Status = 'registration' )
                                union all
                                select distinct
                                        orq.OrganizationId,
                                        'В процессе регистрации'
                                     
                                from    Account A
                                        inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
                                where   Status = 'registration'
                                        and orq.OrganizationId not in (
                                        select distinct
                                                orq.OrganizationId
                                        from    Account A
                                                inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
                                        where   Status = 'activated'
                                                or status = 'consideration'
                                                or Status = 'revision' )
                                union all
                                select distinct
                                        orq.OrganizationId,
                                        'В процессе регистрации'
                                       from    Account A
                                        inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
                                where   Status = 'revision'
                                        and orq.OrganizationId not in (
                                        select distinct
                                                orq.OrganizationId
                                        from    Account A
                                                inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
                                        where   Status = 'activated'
                                                or status = 'consideration' )
                                union all
                                select distinct
                                        orq.OrganizationId,
                                        'В процессе регистрации'
                                from    Account A
                                        inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
                                where   Status = 'consideration'
                                        and orq.OrganizationId not in (
                                        select  orq.OrganizationId
                                        from    Account A
                                                inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
                                        where   Status = 'activated' )
                                union all
                                select distinct
                                        orq.OrganizationId,
                                        'Да'
                                from    Account A
                                        inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
                                where   Status = 'activated')
                               ORS ON ORS.OrganizationId = Org.Id
                    LEFT JOIN 
						(select distinct a.Id, a.ModelType   
						 from 
							(
							select o.Id, 'Через головной' as ModelType 
							from Organization2010 O
							where o.IsFilial=1 and o.MainId in (
								select O.id 
								from Organization2010 O
									inner join RecruitmentCampaigns RC ON o.RCModel = RC.Id 
								where modeltype=3
							)
							union all
							select O.id, 'Через головной' 
							from Organization2010 O
								inner join RecruitmentCampaigns RC ON o.RCModel = RC.Id 
							where o.IsFilial=1 and modeltype=2
							) A
						) MDL ON MDL.Id = Org.Id 
            where   org.StatusId = 1 and Org.TypeId in (1,2)
		 order by OrgType.SortOrder

    RETURN
   END

GO
/****** Object:  UserDefinedFunction [dbo].[ReportOrgsRegistrationBASE_copy]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE function [dbo].[ReportOrgsRegistrationBASE_copy] ( )
RETURNS @report TABLE
    (
      [Id] INT,
      [GUID ИСЛОД] nvarchar(50) null,
	  [Реквизиты лицензии] nvarchar(500) null,
	  [Статус лицензии] nvarchar(500) null,
	  [Факс] nvarchar(500) null,
	  [Сайт] nvarchar(500) null,
      [Полное наименование] NVARCHAR(4000) NULL,
      [Краткое наименование] NVARCHAR(2000) null,
      [ИД головной] INT null,
      [Должность руководителя] nvarchar(1000) null, 
      [Должность руководителя (кому)] nvarchar(1000) null, 
      [ФО] nvarchar(1000) null,
      [Код ФО] int null,
      [Субъект РФ] nvarchar(1000) null,
      [Код субъекта] nvarchar(1000) null,
      [Тип] nvarchar(1000) null,
      [Категория ОО] nvarchar(1000) null,
      [Категория ОО 2] nvarchar(1000) null,
      [Статус ОО] nvarchar(1000) null,
      [Вид] nvarchar(1000) null,
      [ОПФ] nvarchar(50) null,
      [Филиал] nvarchar(50) null,
      [ФИО руководителя] nvarchar(1000) null,
      [И.О. Фамилия Руководителя] nvarchar(1000) null,
      [Фамилия руководителя] nvarchar(1000) null,
      [Имя руководителя] nvarchar(1000) null,
      [Отчество руководителя] nvarchar(1000) null,
      [Учредитель] nvarchar(1000) null,
      [Фактический адрес] nvarchar(1000) null,
      [Юридический адрес] nvarchar(1000) null,
      [Код города] nvarchar(1000) null,
      [Телефон] nvarchar(1000) null,
      [EMail] nvarchar(1000) null,
      [ИНН] nvarchar(10) null,
      [КПП] nvarchar(9) null,
      [ОГРН] nvarchar(13) null,
      [Данные проверены] nvarchar(10) null,
      [Срок подключения к ЗКСПД] datetime null,
      [Срок предоставления информации в ФИС] datetime null,
      [Головная организация] nvarchar(MAX) null,
      [Регион Филиала(код)] int null,
      [Регион учредителя(код)] int null,
      [Статус регистрации] nvarchar(80) null,
      [Обязательность регистрации] nvarchar(20) null
     
    )
AS BEGIN
 
    INSERT  INTO @Report
SELECT  Org.Id as [Id],
            Org.ISLOD_GUID as [GUID ИСЛОД],
		    ISL.License as [Реквизиты лицензии],
			ISL.Status as [Статус лицензии],
			Org.Fax as [Факс],
			Org.Site as [Сайт],
                    Org.FullName AS [Полное наименование],
                    ISNULL(Org.ShortName, '') AS [Краткое наименование],
                    ISNULL(Org.MainId,'0') AS [ИД головной],
                    Org.DirectorPosition AS [Должность руководителя],
                    Org.DirectorPositionInGenetive AS [Должность руководителя (кому)],
                    FD.[Name] AS [ФО],
                    FD.Code AS [Код ФО],
                    Reg.[Name] AS [Субъект РФ],
                    Reg.Code AS [Код субъекта],
                    OrgType.[Name] AS [Тип],
                    Case when (ISL.ZARUBEG=1) then 'За пределами РФ' when (ISL.ZARUBEG=0 and ISL.SIL=1) then 'Военный' WHEN (ISL.ZARUBEG=0 and ISL.RELIG=1) then 'Религиозное'  else 'Обычный' end [Категория ОО],
					Case when (ISL.VUZ=1) then 'ВУЗ' when (ISL.SUZ=1) then 'СУЗ' else 'Другие' end [Категория ОО 2],
                    Case when (Org.StatusId=1) then 'Действующая' when (Org.StatusId=2) then 

'Реорганизованная' else 'Ликвидированная' end [Статус ОО],
                    OrgKind.[Name] AS [Вид],
                    Case when OrgType.[Name] = 'РЦОИ' OR OrgType.[Name] = 'Учредитель'  THEN 'Не 

установлено' ELSE REPLACE(REPLACE(Org.IsPrivate, 1, 'Частный'), 0, 'Гос-ный') END AS [ОПФ],
                    Case when OrgType.[Name] = 'РЦОИ' OR OrgType.[Name] = 'Учредитель' THEN 'Не 

установлено' ELSE REPLACE(REPLACE(Org.IsFilial, 1, 'Да'), 0, 'Нет') END AS [Филиал],
                    Org.DirectorFullName AS [ФИО руководителя],
                    Org.DirectorFullNameInGenetive AS [И.О. Фамилия Руководителя],
                    Org.DirectorLastName AS [Фамилия руководителя],
                    Org.DirectorFirstName AS [Имя руководителя],
                    Org.DirectorPatronymicName AS [Отчество руководителя],
                    isnull(Dep.FullName,'Не установлено') AS [Ведомственная принадлежность],
                    Org.FactAddress AS [Фактический адрес],
                    Org.LawAddress AS [Юридический адрес],
                    Org.PhoneCityCode AS [Код города],
                    Org.Phone AS [Телефон],
                    Org.EMail AS [EMail],
                    Org.INN AS [ИНН],
                    Case when Org.KPP='000000000' then '' else Org.KPP end as [КПП],
                    Org.OGRN AS [ОГРН],
                    CASE WHEN Org.OUConfirmation=1 THEN 'Да' ELSE 'Нет' END AS [Данные проверены],
                    Org.TimeConnectionToSecureNetwork as [Срок подключения к ЗКСПД],
                    org.TimeEnterInformationInFIS as [Срок предоставления информации в ФИС],
                    CASE WHEN Filial.FullName IS NOT NULL THEN Filial.FullName ELSE '' END as [Головная 

организация],
                    CASE WHEN Filial.FullName IS NOT NULL THEN Filial.RegionId ELSE Org.RegionId END as 

[Регион филиала(код)],
					CASE WHEN Dep.FullName IS NOT NULL THEN Dep.RegionId ELSE 1000 

END as [Регион учредителя(код)],
                    ISNULL(ORS.Status, 'Нет') [Статус регистрации],
             
                    ISNULL(MDL.ModelType, 'Подключение') [Обязательность регистрации]
            FROM    Organization2010 Org
                    INNER JOIN Region Reg ON Reg.Id = Org.RegionId
                    INNER JOIN FederalDistricts FD ON FD.Id = Reg.FederalDistrictId
                    INNER JOIN OrganizationType2010 OrgType ON OrgType.Id = Org.TypeId
                    INNER JOIN OrganizationKind OrgKind ON OrgKind.Id = Org.KindId
                    LEFT JOIN Organization2010 Dep ON Org.DepartmentId = Dep.Id
                    Left JOIN Organization2010 Filial ON Org.MainId=Filial.Id
                    LEFT JOIN ( select distinct
                                        orq.OrganizationId,
                                        'Да' as Status                                  
                                from    Account A
                                        inner join OrganizationRequest2010 ORQ ON ORQ.id = 

A.organizationid
                                where   Status = 'deactivated'
                                        and orq.OrganizationId not in (
                                        select distinct
                                                orq.OrganizationId
                                        from    Account A
                                                inner join OrganizationRequest2010 ORQ ON ORQ.id = 

A.organizationid
                                        where   Status = 'activated'
                                                or status = 'consideration'
                                                or Status = 'revision'
                                                or Status = 'registration' )
                                union all
                                select distinct
                                        orq.OrganizationId,
                                        'В процессе регистрации'
                                     
                                from    Account A
                                        inner join OrganizationRequest2010 ORQ ON ORQ.id = 

A.organizationid
                                where   Status = 'registration'
                                        and orq.OrganizationId not in (
                                        select distinct
                                                orq.OrganizationId
                                        from    Account A
                                                inner join OrganizationRequest2010 ORQ ON ORQ.id = 

A.organizationid
                                        where   Status = 'activated'
                                                or status = 'consideration'
                                                or Status = 'revision' )
                                union all
                                select distinct
                                        orq.OrganizationId,
                                        'В процессе регистрации'
                                       from    Account A
                                        inner join OrganizationRequest2010 ORQ ON ORQ.id = 

A.organizationid
                                where   Status = 'revision'
                                        and orq.OrganizationId not in (
                                        select distinct
                                                orq.OrganizationId
                                        from    Account A
                                                inner join OrganizationRequest2010 ORQ ON ORQ.id = 

A.organizationid
                                        where   Status = 'activated'
                                                or status = 'consideration' )
                                union all
                                select distinct
                                        orq.OrganizationId,
                                        'В процессе регистрации'
                                from    Account A
                                        inner join OrganizationRequest2010 ORQ ON ORQ.id = 

A.organizationid
                                where   Status = 'consideration'
                                        and orq.OrganizationId not in (
                                        select  orq.OrganizationId
                                        from    Account A
                                                inner join OrganizationRequest2010 ORQ ON ORQ.id = 

A.organizationid
                                        where   Status = 'activated' )
                                union all
                                select distinct
                                        orq.OrganizationId,
                                        'Да'
                                from    Account A
                                        inner join OrganizationRequest2010 ORQ ON ORQ.id = 

A.organizationid
                                where   Status = 'activated')
                               ORS ON ORS.OrganizationId = Org.Id
                    LEFT JOIN 
						(select distinct a.Id, a.ModelType   
						 from 
							(
							select o.Id, 'Через головной' as ModelType 
							from Organization2010 O
							where o.IsFilial=1 and o.MainId in (
								select O.id 
								from Organization2010 O
									inner join RecruitmentCampaigns 

RC ON o.RCModel = RC.Id 
								where modeltype=3
							)
							union all
							select O.id, 'Через головной' 
							from Organization2010 O
								inner join RecruitmentCampaigns RC ON 

o.RCModel = RC.Id 
							where o.IsFilial=1 and modeltype=2
							) A
						) MDL ON MDL.Id = Org.Id
				LEFT JOIN dbRep.dbo.License_ISLOD ISL ON 
					Org.ISLOD_GUID = ISL.sys_guid
            where  /*org.StatusId = 1  and*/  Org.TypeId in (1,2) 
		 order by Org.Id

    RETURN
   END




GO
/****** Object:  UserDefinedFunction [dbo].[ReportOrgsRegistrationBASE_copy_2]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE function [dbo].[ReportOrgsRegistrationBASE_copy_2] ( )
RETURNS @report TABLE
    (
      [Id] INT,
      [GUID ИСЛОД] nvarchar(50) null,
	  [Реквизиты лицензии] nvarchar(500) null,
	  [Статус лицензии] nvarchar(500) null,
	  [Факс] nvarchar(500) null,
	  [Сайт] nvarchar(500) null,
      [Полное наименование] NVARCHAR(4000) NULL,
      [Краткое наименование] NVARCHAR(2000) null,
      [ИД головной] INT null,
      [Должность руководителя] nvarchar(1000) null, 
      [Должность руководителя (кому)] nvarchar(1000) null, 
      [ФО] nvarchar(1000) null,
      [Код ФО] int null,
      [Субъект РФ] nvarchar(1000) null,
      [Код субъекта] nvarchar(1000) null,
      [Тип] nvarchar(1000) null,
      [Категория ОО] nvarchar(1000) null,
      [Категория ОО 2] nvarchar(1000) null,
      [Статус ОО] nvarchar(1000) null,
      [Вид] nvarchar(1000) null,
      [ОПФ] nvarchar(50) null,
      [Филиал] nvarchar(50) null,
      [ФИО руководителя] nvarchar(1000) null,
      [И.О. Фамилия Руководителя] nvarchar(1000) null,
      [Фамилия руководителя] nvarchar(1000) null,
      [Имя руководителя] nvarchar(1000) null,
      [Отчество руководителя] nvarchar(1000) null,
      [Учредитель] nvarchar(1000) null,
      [Фактический адрес] nvarchar(1000) null,
      [Юридический адрес] nvarchar(1000) null,
      [Код города] nvarchar(1000) null,
      [Телефон] nvarchar(1000) null,
      [EMail] nvarchar(1000) null,
      [ИНН] nvarchar(10) null,
      [КПП] nvarchar(9) null,
      [ОГРН] nvarchar(13) null,
      [Данные проверены] nvarchar(10) null,
      [Срок подключения к ЗКСПД] datetime null,
      [Срок предоставления информации в ФИС] datetime null,
      [Головная организация] nvarchar(MAX) null,
      [Регион Филиала(код)] int null,
      [Регион учредителя(код)] int null,
      [Статус регистрации] nvarchar(80) null,
      [Обязательность регистрации] nvarchar(20) null
     
    )
AS BEGIN
 
    INSERT  INTO @Report
SELECT  Org.Id as [Id],
            Org.ISLOD_GUID as [GUID ИСЛОД],
		    ISL.License as [Реквизиты лицензии],
			ISL.Status as [Статус лицензии],
			Org.Fax as [Факс],
			Org.Site as [Сайт],
                    Org.FullName AS [Полное наименование],
                    ISNULL(Org.ShortName, '') AS [Краткое наименование],
                    ISNULL(Org.MainId,'0') AS [ИД головной],
                    Org.DirectorPosition AS [Должность руководителя],
                    Org.DirectorPositionInGenetive AS [Должность руководителя (кому)],
                    FD.[Name] AS [ФО],
                    FD.Code AS [Код ФО],
                    Reg.[Name] AS [Субъект РФ],
                    Reg.Code AS [Код субъекта],
                    OrgType.[Name] AS [Тип],
                    Case when (ISL.ZARUBEG=1) then 'За пределами РФ' when (ISL.ZARUBEG=0 and ISL.SIL=1) then 'Военный' WHEN (ISL.ZARUBEG=0 and ISL.RELIG=1) then 'Религиозное'  else 'Обычный' end [Категория ОО],
					Case when (ISL.VUZ=1) then 'ВУЗ' when (ISL.SUZ=1) then 'СУЗ' when (ISL.DRUGIE=1 OR (ISL.VUZ<>1 and ISL.SUZ<>1 and ISL.DRUGIE<>1 and Org.TypeId =5)) then 'Другие' else 'Не найден' end [Категория ОО 2],
                    Case when (Org.StatusId=1) then 'Действующая' when (Org.StatusId=2) then 
'Реорганизованная' else 'Ликвидированная' end [Статус ОО],
                    OrgKind.[Name] AS [Вид],
                    Case when OrgType.[Name] = 'РЦОИ' OR OrgType.[Name] = 'Учредитель'  THEN 'Не 
установлено' ELSE REPLACE(REPLACE(Org.IsPrivate, 1, 'Частный'), 0, 'Гос-ный') END AS [ОПФ],
                    Case when OrgType.[Name] = 'РЦОИ' OR OrgType.[Name] = 'Учредитель' THEN 'Не 
установлено' ELSE REPLACE(REPLACE(Org.IsFilial, 1, 'Да'), 0, 'Нет') END AS [Филиал],
                    Org.DirectorFullName AS [ФИО руководителя],
                    Org.DirectorFullNameInGenetive AS [И.О. Фамилия Руководителя],
                    Org.DirectorLastName AS [Фамилия руководителя],
                    Org.DirectorFirstName AS [Имя руководителя],
                    Org.DirectorPatronymicName AS [Отчество руководителя],
                    isnull(Dep.FullName,'Не установлено') AS [Ведомственная принадлежность],
                    Org.FactAddress AS [Фактический адрес],
                    Org.LawAddress AS [Юридический адрес],
                    Org.PhoneCityCode AS [Код города],
                    Org.Phone AS [Телефон],
                    Org.EMail AS [EMail],
                    Org.INN AS [ИНН],
                    Case when Org.KPP='000000000' then '' else Org.KPP end as [КПП],
                    Org.OGRN AS [ОГРН],
                    CASE WHEN Org.OUConfirmation=1 THEN 'Да' ELSE 'Нет' END AS [Данные проверены],
                    Org.TimeConnectionToSecureNetwork as [Срок подключения к ЗКСПД],
                    org.TimeEnterInformationInFIS as [Срок предоставления информации в ФИС],
                    CASE WHEN Filial.FullName IS NOT NULL THEN Filial.FullName ELSE '' END as [Головная 
организация],
                    CASE WHEN Filial.FullName IS NOT NULL THEN Filial.RegionId ELSE Org.RegionId END as 
[Регион филиала(код)],
					CASE WHEN Dep.FullName IS NOT NULL THEN Dep.RegionId ELSE 1000 
END as [Регион учредителя(код)],
                    ISNULL(ORS.Status, 'Нет') [Статус регистрации],
             
                    ISNULL(MDL.ModelType, 'Подключение') [Обязательность регистрации]
            FROM    Organization2010 Org
                    INNER JOIN Region Reg ON Reg.Id = Org.RegionId
                    INNER JOIN FederalDistricts FD ON FD.Id = Reg.FederalDistrictId
                    INNER JOIN OrganizationType2010 OrgType ON OrgType.Id = Org.TypeId
                    INNER JOIN OrganizationKind OrgKind ON OrgKind.Id = Org.KindId
                    LEFT JOIN Organization2010 Dep ON Org.DepartmentId = Dep.Id
                    Left JOIN Organization2010 Filial ON Org.MainId=Filial.Id
                    LEFT JOIN ( select distinct
                                        orq.OrganizationId,
                                        'Да' as Status                                  
                                from    Account A
                                        inner join OrganizationRequest2010 ORQ ON ORQ.id = 
A.organizationid
                                where   Status = 'deactivated'
                                        and orq.OrganizationId not in (
                                        select distinct
                                                orq.OrganizationId
                                        from    Account A
                                                inner join OrganizationRequest2010 ORQ ON ORQ.id = 
A.organizationid
                                        where   Status = 'activated'
                                                or status = 'consideration'
                                                or Status = 'revision'
                                                or Status = 'registration' )
                                union all
                                select distinct
                                        orq.OrganizationId,
                                        'В процессе регистрации'
                                     
                                from    Account A
                                        inner join OrganizationRequest2010 ORQ ON ORQ.id = 
A.organizationid
                                where   Status = 'registration'
                                        and orq.OrganizationId not in (
                                        select distinct
                                                orq.OrganizationId
                                        from    Account A
                                                inner join OrganizationRequest2010 ORQ ON ORQ.id = 
A.organizationid
                                        where   Status = 'activated'
                                                or status = 'consideration'
                                                or Status = 'revision' )
                                union all
                                select distinct
                                        orq.OrganizationId,
                                        'В процессе регистрации'
                                       from    Account A
                                        inner join OrganizationRequest2010 ORQ ON ORQ.id = 
A.organizationid
                                where   Status = 'revision'
                                        and orq.OrganizationId not in (
                                        select distinct
                                                orq.OrganizationId
                                        from    Account A
                                                inner join OrganizationRequest2010 ORQ ON ORQ.id = 
A.organizationid
                                        where   Status = 'activated'
                                                or status = 'consideration' )
                                union all
                                select distinct
                                        orq.OrganizationId,
                                        'В процессе регистрации'
                                from    Account A
                                        inner join OrganizationRequest2010 ORQ ON ORQ.id = 
A.organizationid
                                where   Status = 'consideration'
                                        and orq.OrganizationId not in (
                                        select  orq.OrganizationId
                                        from    Account A
                                                inner join OrganizationRequest2010 ORQ ON ORQ.id = 
A.organizationid
                                        where   Status = 'activated' )
                                union all
                                select distinct
                                        orq.OrganizationId,
                                        'Да'
                                from    Account A
                                        inner join OrganizationRequest2010 ORQ ON ORQ.id = 
A.organizationid
                                where   Status = 'activated')
                               ORS ON ORS.OrganizationId = Org.Id
                    LEFT JOIN 
						(select distinct a.Id, a.ModelType   
						 from 
							(
							select o.Id, 'Через головной' as ModelType 
							from Organization2010 O
							where o.IsFilial=1 and o.MainId in (
								select O.id 
								from Organization2010 O
									inner join RecruitmentCampaigns 
RC ON o.RCModel = RC.Id 
								where modeltype=3
							)
							union all
							select O.id, 'Через головной' 
							from Organization2010 O
								inner join RecruitmentCampaigns RC ON 
o.RCModel = RC.Id 
							where o.IsFilial=1 and modeltype=2
							) A
						) MDL ON MDL.Id = Org.Id
				--LEFT JOIN dbRep.dbo.License_ISLOD ISL ON 
				LEFT JOIN License_ISLOD ISL ON
					Org.ISLOD_GUID = ISL.sys_guid
            where  /*org.StatusId = 1  and*/  Org.TypeId in (1,2,5) 
		 order by Org.Id

    RETURN
   END



GO
/****** Object:  UserDefinedFunction [dbo].[ReportOrgsRegistrationBASE_copy_3]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE function [dbo].[ReportOrgsRegistrationBASE_copy_3] ( )
RETURNS @report TABLE
    (
      [Id] INT,
      [GUID ИСЛОД] nvarchar(500) null,
	  [Реквизиты лицензии] nvarchar(500) null,
	  [Статус лицензии] nvarchar(500) null,
	  [Факс] nvarchar(500) null,
	  [Сайт] nvarchar(500) null,
      [Полное наименование] NVARCHAR(4000) NULL,
      [Краткое наименование] NVARCHAR(2000) null,
      [ИД головной] INT null,
      [Должность руководителя] nvarchar(1000) null, 
      [Должность руководителя (кому)] nvarchar(1000) null, 
      [ФО] nvarchar(1000) null,
      [Код ФО] int null,
      [Субъект РФ] nvarchar(1000) null,
      [Код субъекта] nvarchar(1000) null,
      [Тип] nvarchar(1000) null,
      [Категория ОО] nvarchar(1000) null,
      [Категория ОО 2] nvarchar(1000) null,
      [Статус ОО] nvarchar(1000) null,
      [Вид] nvarchar(1000) null,
      [ОПФ] nvarchar(500) null,
      [Филиал] nvarchar(500) null,
      [ФИО руководителя] nvarchar(1000) null,
      [И.О. Фамилия Руководителя] nvarchar(1000) null,
      [Фамилия руководителя] nvarchar(1000) null,
      [Имя руководителя] nvarchar(1000) null,
      [Отчество руководителя] nvarchar(1000) null,
      [Учредитель] nvarchar(max) null,
      [Фактический адрес] nvarchar(1000) null,
      [Юридический адрес] nvarchar(1000) null,
      [Код города] nvarchar(1000) null,
      [Телефон] nvarchar(1000) null,
      [EMail] nvarchar(1000) null,
      [ИНН] nvarchar(100) null,
      [КПП] nvarchar(100) null,
      [ОГРН] nvarchar(100) null,
      [Данные проверены] nvarchar(100) null,
      [Срок подключения к ЗКСПД] datetime null,
      [Срок предоставления информации в ФИС] datetime null,
      [Головная организация] nvarchar(MAX) null,
      [Регион Филиала(код)] int null,
      [Регион учредителя(код)] int null,
      [Статус регистрации] nvarchar(80) null,
      [Обязательность регистрации] nvarchar(200) null
     
    )
AS BEGIN
 
    INSERT  INTO @Report
SELECT  distinct Org.Id as [Id],
            Org.ISLOD_GUID as [GUID ИСЛОД],
            CASE WHEN ORG.IsFilial = 0 THEN (SELECT TOP 1 '№ ' + L.RegNumber + ' от ' + CONVERT(VARCHAR,L.OrderDocumentDate,104)FROM License L WHERE L.OrganizationId = ORG.Id AND L.OrderDocumentDate IS NOT NULL
            ORDER BY L.OrderDocumentDate DESC) 
            WHEN Org.IsFilial = 1 THEN (SELECT TOP 1 '№ ' + L.RegNumber + ' от ' + CONVERT(VARCHAR,L.OrderDocumentDate,104) FROM LicenseSupplement LS INNER JOIN License L ON LS.LicenseId = L.Id WHERE LS.OrganizationId = ORG.Id 
            AND LS.OrderDocumentDate IS NOT NULL ORDER BY LS.OrderDocumentDate DESC)
            END AS [Реквизиты лицензии],
            
            CASE WHEN ORG.IsFilial = 0 THEN (SELECT TOP 1 StatusName FROM License L WHERE L.OrganizationId = ORG.Id AND L.OrderDocumentDate IS NOT NULL
            ORDER BY L.OrderDocumentDate DESC) 
            WHEN Org.IsFilial = 1 THEN (SELECT TOP 1 StatusName FROM LicenseSupplement LS WHERE LS.OrganizationId = ORG.Id 
            AND LS.OrderDocumentDate IS NOT NULL ORDER BY LS.OrderDocumentDate DESC)
            END AS [Статус лицензии],
		 --   ISL.License as [Реквизиты лицензии],
			--ISL.Status as [Статус лицензии],
			Org.Fax as [Факс],
			Org.Site as [Сайт],
                    Org.FullName AS [Полное наименование],
                    ISNULL(Org.ShortName, '') AS [Краткое наименование],
                    ISNULL(Org.MainId,'0') AS [ИД головной],
                    Org.DirectorPosition AS [Должность руководителя],
                    Org.DirectorPositionInGenetive AS [Должность руководителя (кому)],
                    FD.[Name] AS [ФО],
                    FD.Code AS [Код ФО],
                    Reg.[Name] AS [Субъект РФ],
                    Reg.Code AS [Код субъекта],
                    OrgType.[Name] AS [Тип],
                    Case when (org.RegionId = 1001) then 'За пределами РФ' when (org.IsLawEnforcmentOrganization = 1 and org.IsReligious =0 ) then 'Военный' WHEN (org.IsLawEnforcmentOrganization = 0 and org.IsReligious = 1 ) then 'Религиозное'  else 'Обычный' end [Категория ОО],
					Case when (org.TypeId = 1) then 'ВУЗ' when (org.TypeId = 2) then 'СУЗ' when (org.TypeId = 5) then 'Другие' else 'Не найден' end [Категория ОО 2],
                    Case when (Org.StatusId=1) then 'Действующая' when (Org.StatusId=2) then 

'Реорганизованная' else 'Ликвидированная' end [Статус ОО],
                    OrgKind.[Name] AS [Вид],
                    Case when OrgType.[Name] = 'РЦОИ' OR OrgType.[Name] = 'Учредитель'  THEN 'Не 

установлено' ELSE REPLACE(REPLACE(Org.IsPrivate, 1, 'Частный'), 0, 'Гос-ный') END AS [ОПФ],
                    Case when OrgType.[Name] = 'РЦОИ' OR OrgType.[Name] = 'Учредитель' THEN 'Не 

установлено' ELSE REPLACE(REPLACE(Org.IsFilial, 1, 'Да'), 0, 'Нет') END AS [Филиал],
                    Org.DirectorFullName AS [ФИО руководителя],
                    Org.DirectorFullNameInGenetive AS [И.О. Фамилия Руководителя],
                    Org.DirectorLastName AS [Фамилия руководителя],
                    Org.DirectorFirstName AS [Имя руководителя],
                    Org.DirectorPatronymicName AS [Отчество руководителя],
                    --isnull(Dep.FullName, 'Не установлено') AS [Учредитель],
					isnull(SUBSTRING((
                                select t.OrganizationFullName as [text()]
                                from (
                                    select ', ' + f.OrganizationFullName OrganizationFullName
                                            --,ROW_NUMBER() over(PARTITION BY f.Id order by f.id) is_distinct
                                    from
                                        OrganizationFounder fo --(NOLOCK) on org2.id = fo.OrganizationId
                                        INNER JOIN Founder f ON fo.FounderId = f.id
                                    where
                                        fo.OrganizationId = org.id
                                ) t 
                                --where t.is_distinct = 1 
                                ORDER by t.OrganizationFullName
                                for xml path('')
                            ), 3, 8000), N'Не установлено') AS [Учредитель],
                    Org.FactAddress AS [Фактический адрес],
                    Org.LawAddress AS [Юридический адрес],
                    Org.PhoneCityCode AS [Код города],
                    Org.Phone AS [Телефон],
                    Org.EMail AS [EMail],
                    Org.INN AS [ИНН],
                    Case when Org.KPP='000000000' then '' else Org.KPP end as [КПП],
                    Org.OGRN AS [ОГРН],
                    CASE WHEN Org.OUConfirmation=1 THEN 'Да' ELSE 'Нет' END AS [Данные проверены],
                    Org.TimeConnectionToSecureNetwork as [Срок подключения к ЗКСПД],
                    org.TimeEnterInformationInFIS as [Срок предоставления информации в ФИС],
                    CASE WHEN Filial.FullName IS NOT NULL THEN Filial.FullName ELSE '' END as [Головная организация],
                    CASE WHEN Filial.FullName IS NOT NULL THEN Filial.RegionId ELSE Org.RegionId END as [Регион филиала(код)],
					CASE WHEN Dep.FullName IS NOT NULL THEN Dep.RegionId ELSE 1000 END as [Регион учредителя(код)],
                    ISNULL(ORS.Status, 'Нет') [Статус регистрации],
             
                    ISNULL(MDL.ModelType, 'Подключение') [Обязательность регистрации]
					FROM    Organization2010 Org
                    INNER JOIN Region Reg ON Reg.Id = Org.RegionId
                    INNER JOIN FederalDistricts FD ON FD.Id = Reg.FederalDistrictId
                    INNER JOIN OrganizationType2010 OrgType ON OrgType.Id = Org.TypeId
                    INNER JOIN OrganizationKind OrgKind ON OrgKind.Id = Org.KindId
                    LEFT JOIN Organization2010 Dep ON Org.DepartmentId = Dep.Id
                    Left JOIN Organization2010 Filial ON Org.MainId=Filial.Id
                    LEFT JOIN ( select distinct
                                        orq.OrganizationId,
                                        'Да' as Status                                  
                                from    Account A
                                        inner join OrganizationRequest2010 ORQ ON ORQ.id = 

A.organizationid
                                where   Status = 'deactivated'
                                        and orq.OrganizationId not in (
                                        select distinct
                                                orq.OrganizationId
                                        from    Account A
                                                inner join OrganizationRequest2010 ORQ ON ORQ.id = 

A.organizationid
                                        where   Status = 'activated'
                                                or status = 'consideration'
                                                or Status = 'revision'
                                                or Status = 'registration' )
                                union all
                                select distinct
                                        orq.OrganizationId,
                                        'В процессе регистрации'
                                     
                                from    Account A
                                        inner join OrganizationRequest2010 ORQ ON ORQ.id = 

A.organizationid
                                where   Status = 'registration'
                                        and orq.OrganizationId not in (
                                        select distinct
                                                orq.OrganizationId
                                        from    Account A
                                                inner join OrganizationRequest2010 ORQ ON ORQ.id = 

A.organizationid
                                        where   Status = 'activated'
                                                or status = 'consideration'
                                                or Status = 'revision' )
                                union all
                                select distinct
                                        orq.OrganizationId,
                                        'В процессе регистрации'
                                       from    Account A
                                        inner join OrganizationRequest2010 ORQ ON ORQ.id = 

A.organizationid
                                where   Status = 'revision'
                                        and orq.OrganizationId not in (
                                        select distinct
                                                orq.OrganizationId
                                        from    Account A
                                                inner join OrganizationRequest2010 ORQ ON ORQ.id = 

A.organizationid
                                        where   Status = 'activated'
                                                or status = 'consideration' )
                                union all
                                select distinct
                                        orq.OrganizationId,
                                        'В процессе регистрации'
                                from    Account A
                                        inner join OrganizationRequest2010 ORQ ON ORQ.id = 

A.organizationid
                                where   Status = 'consideration'
                                        and orq.OrganizationId not in (
                                        select  orq.OrganizationId
                                        from    Account A
                                                inner join OrganizationRequest2010 ORQ ON ORQ.id = 

A.organizationid
                                        where   Status = 'activated' )
                                union all
                                select distinct
                                        orq.OrganizationId,
                                        'Да'
                                from    Account A
                                        inner join OrganizationRequest2010 ORQ ON ORQ.id = 

A.organizationid
                                where   Status = 'activated')
                               ORS ON ORS.OrganizationId = Org.Id
                    LEFT JOIN 
						(select distinct a.Id, a.ModelType   
						 from 
							(
							select o.Id, 'Через головной' as ModelType 
							from Organization2010 O
							inner join RecruitmentCampaigns RC ON 

o.RCModel = RC.Id 
							where o.IsFilial=1 and RC.Id != 6 and o.MainId in (
								select O.id 
								from Organization2010 O
									inner join RecruitmentCampaigns 

RC ON o.RCModel = RC.Id 
								where modeltype=3
							)
							union all
							(select O.id, 'Через головной' 
							from Organization2010 O
								inner join RecruitmentCampaigns RC ON 

o.RCModel = RC.Id 
							where o.IsFilial=1 and modeltype=2 and RC.Id = 5)
							
							union all
							(select O.id, 'ОО не ведет прием' 
							from Organization2010 O
								inner join RecruitmentCampaigns RC ON 

o.RCModel = RC.Id 
							where modeltype=2 and RC.Id = 6)
							) A
						) MDL ON MDL.Id = Org.Id
				--LEFT JOIN dbRep.dbo.License_ISLOD ISL ON 
				--	Org.ISLOD_GUID = ISL.sys_guid
            where  /*org.StatusId = 1  and*/  Org.TypeId in (1,2,5) 
		 order by Org.Id

    RETURN
   END






GO
/****** Object:  UserDefinedFunction [dbo].[ReportOrgsRegistrationBASE_copy_old]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE function [dbo].[ReportOrgsRegistrationBASE_copy_old] ( )
RETURNS @report TABLE
    (
      [Id] INT,
      [GUID ИСЛОД] nvarchar(50) null,
      [Полное наименование] NVARCHAR(4000) NULL,
      [Краткое наименование] NVARCHAR(2000) null,
      [ИД головной] INT null,
      [Должность руководителя] nvarchar(1000) null, 
      [Должность руководителя (кому)] nvarchar(1000) null, 
      [ФО] nvarchar(1000) null,
      [Код ФО] int null,
      [Субъект РФ] nvarchar(1000) null,
      [Код субъекта] nvarchar(1000) null,
      [Тип] nvarchar(1000) null,
      [Категория ОО] nvarchar(1000) null,
      [Статус ОО] nvarchar(1000) null,
      [Вид] nvarchar(1000) null,
      [ОПФ] nvarchar(50) null,
      [Филиал] nvarchar(50) null,
      [ФИО руководителя] nvarchar(1000) null,
      [И.О. Фамилия Руководителя] nvarchar(1000) null,
      [Фамилия руководителя] nvarchar(1000) null,
      [Имя руководителя] nvarchar(1000) null,
      [Отчество руководителя] nvarchar(1000) null,
      [Учредитель] nvarchar(1000) null,
      [Фактический адрес] nvarchar(1000) null,
      [Юридический адрес] nvarchar(1000) null,
      [Код города] nvarchar(1000) null,
      [Телефон] nvarchar(1000) null,
      [EMail] nvarchar(1000) null,
      [ИНН] nvarchar(10) null,
      [КПП] nvarchar(9) null,
      [ОГРН] nvarchar(13) null,
      [Данные проверены] nvarchar(10) null,
      [Срок подключения к ЗКСПД] datetime null,
      [Срок предоставления информации в ФИС] datetime null,
      [Головная организация] nvarchar(MAX) null,
      [Регион Филиала(код)] int null,
      [Регион учредителя(код)] int null,
      [Статус регистрации] nvarchar(80) null,
      [Обязательность регистрации] nvarchar(20) null
     
    )
AS BEGIN
 
    INSERT  INTO @Report
            SELECT  Org.Id as [Id],
            Org.ISLOD_GUID as [GUID ИСЛОД],
                    Org.FullName AS [Полное наименование],
                    ISNULL(Org.ShortName, '') AS [Краткое наименование],
                    ISNULL(Org.MainId,'0') AS [ИД головной],
                    Org.DirectorPosition AS [Должность руководителя],
                    Org.DirectorPositionInGenetive AS [Должность руководителя (кому)],
                    FD.[Name] AS [ФО],
                    FD.Code AS [Код ФО],
                    Reg.[Name] AS [Субъект РФ],
                    Reg.Code AS [Код субъекта],
                    OrgType.[Name] AS [Тип],
                    Case when (Org.RegionId=1000) then 'За пределами РФ' when (Org.DepartmentId=9553 or Org.DepartmentId=9564 or Org.DepartmentId=9566 Or Org.Id=1335 Or Org.Id=1838) then 'Военный' WHEN Org.Id = 21750 then 'Крым'  else 'Обычный' end [Категория ОО],
                    Case when (Org.StatusId=1) then 'Действующая' when (Org.StatusId=2) then 'Реорганизованная' else 'Ликвидированная' end [Статус ОО],
                    OrgKind.[Name] AS [Вид],
                    Case when OrgType.[Name] = 'РЦОИ' OR OrgType.[Name] = 'Учредитель'  THEN 'Не установлено' ELSE REPLACE(REPLACE(Org.IsPrivate, 1, 'Частный'), 0, 'Гос-ный') END AS [ОПФ],
                    Case when OrgType.[Name] = 'РЦОИ' OR OrgType.[Name] = 'Учредитель' THEN 'Не установлено' ELSE REPLACE(REPLACE(Org.IsFilial, 1, 'Да'), 0, 'Нет') END AS [Филиал],
                    Org.DirectorFullName AS [ФИО руководителя],
                    Org.DirectorFullNameInGenetive AS [И.О. Фамилия Руководителя],
                    Org.DirectorLastName AS [Фамилия руководителя],
                    Org.DirectorFirstName AS [Имя руководителя],
                    Org.DirectorPatronymicName AS [Отчество руководителя],
                    isnull(Dep.FullName,'Не установлено') AS [Ведомственная принадлежность],
                    Org.FactAddress AS [Фактический адрес],
                    Org.LawAddress AS [Юридический адрес],
                    Org.PhoneCityCode AS [Код города],
                    Org.Phone AS [Телефон],
                    Org.EMail AS [EMail],
                    Org.INN AS [ИНН],
                    Case when Org.KPP='000000000' then '' else Org.KPP end as [КПП],
                    Org.OGRN AS [ОГРН],
                    CASE WHEN Org.OUConfirmation=1 THEN 'Да' ELSE 'Нет' END AS [Данные проверены],
                    Org.TimeConnectionToSecureNetwork as [Срок подключения к ЗКСПД],
                    org.TimeEnterInformationInFIS as [Срок предоставления информации в ФИС],
                    CASE WHEN Filial.FullName IS NOT NULL THEN Filial.FullName ELSE '' END as [Головная организация],
                    CASE WHEN Filial.FullName IS NOT NULL THEN Filial.RegionId ELSE Org.RegionId END as [Регион филиала(код)],
					CASE WHEN Dep.FullName IS NOT NULL THEN Dep.RegionId ELSE 1000 END as [Регион учредителя(код)],
                    ISNULL(ORS.Status, 'Нет') [Статус регистрации],
             
                    ISNULL(MDL.ModelType, 'Подключение') [Обязательность регистрации]
            FROM    Organization2010 Org
                    INNER JOIN Region Reg ON Reg.Id = Org.RegionId
                    INNER JOIN FederalDistricts FD ON FD.Id = Reg.FederalDistrictId
                    INNER JOIN OrganizationType2010 OrgType ON OrgType.Id = Org.TypeId
                    INNER JOIN OrganizationKind OrgKind ON OrgKind.Id = Org.KindId
                    LEFT JOIN Organization2010 Dep ON Org.DepartmentId = Dep.Id
                    Left JOIN Organization2010 Filial ON Org.MainId=Filial.Id
                    LEFT JOIN ( select distinct
                                        orq.OrganizationId,
                                        'Да' as Status                                  
                                from    Account A
                                        inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
                                where   Status = 'deactivated'
                                        and orq.OrganizationId not in (
                                        select distinct
                                                orq.OrganizationId
                                        from    Account A
                                                inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
                                        where   Status = 'activated'
                                                or status = 'consideration'
                                                or Status = 'revision'
                                                or Status = 'registration' )
                                union all
                                select distinct
                                        orq.OrganizationId,
                                        'В процессе регистрации'
                                     
                                from    Account A
                                        inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
                                where   Status = 'registration'
                                        and orq.OrganizationId not in (
                                        select distinct
                                                orq.OrganizationId
                                        from    Account A
                                                inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
                                        where   Status = 'activated'
                                                or status = 'consideration'
                                                or Status = 'revision' )
                                union all
                                select distinct
                                        orq.OrganizationId,
                                        'В процессе регистрации'
                                       from    Account A
                                        inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
                                where   Status = 'revision'
                                        and orq.OrganizationId not in (
                                        select distinct
                                                orq.OrganizationId
                                        from    Account A
                                                inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
                                        where   Status = 'activated'
                                                or status = 'consideration' )
                                union all
                                select distinct
                                        orq.OrganizationId,
                                        'В процессе регистрации'
                                from    Account A
                                        inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
                                where   Status = 'consideration'
                                        and orq.OrganizationId not in (
                                        select  orq.OrganizationId
                                        from    Account A
                                                inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
                                        where   Status = 'activated' )
                                union all
                                select distinct
                                        orq.OrganizationId,
                                        'Да'
                                from    Account A
                                        inner join OrganizationRequest2010 ORQ ON ORQ.id = A.organizationid
                                where   Status = 'activated')
                               ORS ON ORS.OrganizationId = Org.Id
                    LEFT JOIN 
						(select distinct a.Id, a.ModelType   
						 from 
							(
							select o.Id, 'Через головной' as ModelType 
							from Organization2010 O
							where o.IsFilial=1 and o.MainId in (
								select O.id 
								from Organization2010 O
									inner join RecruitmentCampaigns RC ON o.RCModel = RC.Id 
								where modeltype=3
							)
							union all
							select O.id, 'Через головной' 
							from Organization2010 O
								inner join RecruitmentCampaigns RC ON o.RCModel = RC.Id 
							where o.IsFilial=1 and modeltype=2
							) A
						) MDL ON MDL.Id = Org.Id 
            where  /* org.StatusId = 1 and */ Org.TypeId in (1,2)
		 order by Org.Id

    RETURN
   END


GO
/****** Object:  UserDefinedFunction [dbo].[ReportOrgsStatusWithAccredTVF]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[ReportOrgsStatusWithAccredTVF](@periodBegin datetime,@periodEnd datetime)
RETURNS @report TABLE 
(
	[Тип ОУ] nvarchar(500) null, 
	[Правовая форма] nvarchar(50) null, 
	[Филиал] nvarchar(50) null,
	[В БД] int null,
	[Из них действующих] int null
)
AS
BEGIN

DECLARE @PreReport TABLE
(
	[Тип ОУ] nvarchar(500) null, 
	[Правовая форма] nvarchar(50) null, 
	[Филиал] nvarchar(50) null,
	[Аккредитация] nvarchar(50) null,
	[В БД] int null,
	[Из них действующих] int null
)
INSERT INTO @PreReport
SELECT 
OrgInfo.[Тип],
OrgInfo.[ОПФ],
OrgInfo.[Филиал],
OrgInfo.[Аккредитация по факту],
COUNT(*),
COUNT(CASE WHEN OrgInfo.[Пользователей активировано]>0 THEN 1 ELSE NULL END) 
FROM dbo.[ReportOrgsInfoTVF_WithoutChecks](null,null) AS OrgInfo
GROUP BY 
OrgInfo.[Тип],
OrgInfo.[ОПФ],
OrgInfo.[Филиал],
OrgInfo.[Аккредитация по факту]


INSERT INTO @Report
SELECT [Тип ОУ],'Государственный',[Филиал],SUM([В БД]),SUM([Из них действующих])
FROM @PreReport WHERE [Тип ОУ]='ВУЗ' AND [Правовая форма]='Гос-ный'
GROUP BY [Тип ОУ],[Правовая форма],[Филиал]
UNION ALL
SELECT 'ВУЗ','Государственный','Всего',SUM([В БД]),SUM([Из них действующих]) 
FROM @PreReport WHERE [Тип ОУ]='ВУЗ' AND [Правовая форма]='Гос-ный'

INSERT INTO @Report
SELECT [Тип ОУ],'Негосударственный',[Филиал],SUM([В БД]),SUM([Из них действующих])
FROM @PreReport WHERE [Тип ОУ]='ВУЗ' AND [Правовая форма]='Частный'
GROUP BY [Тип ОУ],[Правовая форма],[Филиал]
UNION ALL
SELECT 'ВУЗ','Негосударственный','Всего',SUM([В БД]),SUM([Из них действующих]) 
FROM @PreReport WHERE [Тип ОУ]='ВУЗ' AND [Правовая форма]='Частный'

INSERT INTO @Report
SELECT 'ВУЗ','Всего','-',SUM([В БД]),SUM([Из них действующих]) 
FROM @PreReport WHERE [Тип ОУ]='ВУЗ' 


INSERT INTO @Report
SELECT [Тип ОУ],'Государственный','-',SUM([В БД]),SUM([Из них действующих])
FROM @PreReport WHERE [Тип ОУ]='ССУЗ' AND [Правовая форма]='Гос-ный'
GROUP BY [Тип ОУ],[Правовая форма]

INSERT INTO @Report
SELECT [Тип ОУ],'Негосударственный','-',SUM([В БД]),SUM([Из них действующих])
FROM @PreReport WHERE [Тип ОУ]='ССУЗ' AND [Правовая форма]='Частный'
GROUP BY [Тип ОУ],[Правовая форма]

INSERT INTO @Report
SELECT 'ССУЗ','Всего','-',SUM([В БД]),SUM([Из них действующих]) 
FROM @PreReport WHERE [Тип ОУ]='ССУЗ' 


INSERT INTO @Report
SELECT [Тип ОУ],'-','-',SUM([В БД]),SUM([Из них действующих]) 
FROM @PreReport WHERE [Тип ОУ]<>'ССУЗ'  AND [Тип ОУ]<>'ВУЗ'
GROUP BY [Тип ОУ]

INSERT INTO @Report
SELECT 'Итого','-','-',SUM([В БД]),SUM([Из них действующих]) 
FROM @PreReport


INSERT INTO @Report
SELECT NULL, NULL, NULL, NULL, NULL
INSERT INTO @Report
SELECT NULL, NULL, NULL, NULL, NULL
INSERT INTO @Report
SELECT 'В разрезе наличия аккредитации', NULL, NULL, NULL, NULL

INSERT INTO @Report
SELECT 'Тип ОУ','Правовая форма','Аккредитация',NULL, NULL

INSERT INTO @Report
SELECT [Тип ОУ],'Государственный',[Аккредитация],SUM([В БД]),SUM([Из них действующих])
FROM @PreReport WHERE [Тип ОУ]='ВУЗ' AND [Правовая форма]='Гос-ный'
GROUP BY [Тип ОУ],[Правовая форма],[Аккредитация]
UNION ALL
SELECT 'ВУЗ','Государственный','Всего',SUM([В БД]),SUM([Из них действующих]) 
FROM @PreReport WHERE [Тип ОУ]='ВУЗ' AND [Правовая форма]='Гос-ный'

INSERT INTO @Report
SELECT [Тип ОУ],'Негосударственный',[Аккредитация],SUM([В БД]),SUM([Из них действующих])
FROM @PreReport WHERE [Тип ОУ]='ВУЗ' AND [Правовая форма]='Частный'
GROUP BY [Тип ОУ],[Правовая форма],[Аккредитация]
UNION ALL
SELECT 'ВУЗ','Негосударственный','Всего',SUM([В БД]),SUM([Из них действующих]) 
FROM @PreReport WHERE [Тип ОУ]='ВУЗ' AND [Правовая форма]='Частный'

INSERT INTO @Report
SELECT 'ВУЗ','Всего','-',SUM([В БД]),SUM([Из них действующих]) 
FROM @PreReport WHERE [Тип ОУ]='ВУЗ' 

INSERT INTO @Report
SELECT [Тип ОУ],'Государственный',[Аккредитация],SUM([В БД]),SUM([Из них действующих])
FROM @PreReport WHERE [Тип ОУ]='ССУЗ' AND [Правовая форма]='Гос-ный'
GROUP BY [Тип ОУ],[Правовая форма],[Аккредитация]
UNION ALL
SELECT 'ССУЗ','Государственный','Всего',SUM([В БД]),SUM([Из них действующих]) 
FROM @PreReport WHERE [Тип ОУ]='ССУЗ' AND [Правовая форма]='Гос-ный'

INSERT INTO @Report
SELECT [Тип ОУ],'Негосударственный',[Аккредитация],SUM([В БД]),SUM([Из них действующих])
FROM @PreReport WHERE [Тип ОУ]='ССУЗ' AND [Правовая форма]='Частный'
GROUP BY [Тип ОУ],[Правовая форма],[Аккредитация]
UNION ALL
SELECT 'ССУЗ','Негосударственный','Всего',SUM([В БД]),SUM([Из них действующих]) 
FROM @PreReport WHERE [Тип ОУ]='ССУЗ' AND [Правовая форма]='Частный'

INSERT INTO @Report
SELECT 'ССУЗ','Всего','-',SUM([В БД]),SUM([Из них действующих]) 
FROM @PreReport WHERE [Тип ОУ]='ССУЗ'  

INSERT INTO @Report
SELECT 'Итого','-','-',SUM([В БД]),SUM([Из них действующих]) 
FROM @PreReport 
WHERE  [Тип ОУ]='ССУЗ' OR [Тип ОУ]='ВУЗ'

return
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportRegistrationShortTVF]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
/****** Object:  UserDefinedFunction [dbo].[ReportRegistredOrgsTVF]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[ReportRegistredOrgsTVF](
	@periodBegin DATETIME = NULL
	, @periodEnd DATETIME = NULL)
RETURNS @report TABLE 
(
[Полное наименование] NVARCHAR(4000) NULL
,[Краткое наименование] NVARCHAR(2000) null
--,[Дата создания] datetime null
--,[Создана из справочника] bit null
,[Наименование региона] nvarchar(255) null
,[Код региона] nvarchar(255) null
,[Тип] nvarchar(255) null
,[Вид] nvarchar(255) null
,[ОПФ] nvarchar(50) null
,[Филиал] nvarchar(50) null
,[Аккредитация по справочнику] nvarchar(20) null
,[Свидетельство об аккредитации] nvarchar(255) null
,[Аккредитация по факту] nvarchar(255) null
,[ФИО руководителя] nvarchar(255) null
,[Должность руководителя] nvarchar(255) null
,[Ведомственная принадлежность] nvarchar(500) null
,[Фактический адрес] nvarchar(255) null
,[Юридический адрес] nvarchar(255) null
,[Код города] nvarchar(255) null
,[Телефон] nvarchar(255) null
,[EMail] nvarchar(255) null
,[ИНН] nvarchar(10) null
,[ОГРН] nvarchar(13) null
)
AS 
BEGIN

 
DECLARE @UsersByOrgs TABLE (
OrganizationId INT
,UsersCount INT
)
INSERT INTO @UsersByOrgs
SELECT 
IOrgReq.OrganizationId AS OrganizationId
,COUNT(*) AS UsersCount
FROM 
OrganizationRequest2010 IOrgReq		
WHERE IOrgReq.OrganizationId IS NOT NULL 
GROUP BY IOrgReq.OrganizationId


INSERT INTO @Report
SELECT 
Org.[Полное наименование]  
,Org.[Краткое наименование]  
,Org.[Имя региона]  
,Org.[Код региона]  
,Org.[Тип]  
,Org.[Вид]  
,Org.[ОПФ]  
,Org.[Филиал]  
,Org.[Аккредитация по справочнику]  
,Org.[Свидетельство об аккредитации]  
,Org.[Аккредитация по факту]  
,Org.[ФИО руководителя]  
,Org.[Должность руководителя]  
,Org.[Ведомственная принадлежность]  
,Org.[Фактический адрес]  
,Org.[Юридический адрес]  
,Org.[Код города]  
,Org.[Телефон]  
,Org.[EMail]  
,Org.[ИНН] 
,Org.[ОГРН] 

FROM dbo.ReportOrgsBASE() Org
WHERE Id IN 
	(SELECT OReq.OrganizationId 
	FROM OrganizationRequest2010 OReq
	WHERE OReq.OrganizationId  IS NOT NULL)

RETURN
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportStatisticSubordinateOrg]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE FUNCTION  [dbo].[ReportStatisticSubordinateOrg](
			@periodBegin datetime,
			@periodEnd datetime,
			@departmentId int)
RETURNS @Report TABLE
(
	Id int null,
	FullName nvarchar(Max) null,
	RegionId int null,
	RegionName nvarchar(255) null,
	AccreditationSertificate nvarchar(255) null,
	DirectorFullName nvarchar(255) null,
	CountUser int null,
	UserUpdateDate datetime null
)
AS BEGIN

SET @periodBegin = DATEADD(YEAR, -1, GETDATE())
SET @periodEnd = GETDATE()



INSERT INTO @Report
SELECT
	O.Id,
	O.FullName,
	O.RegionId,
	R.Name as RegionName,
	O.AccreditationSertificate,
	O.DirectorFullName,
	COUNT(A.Id) CountUser,
	MIN(A.UpdateDate) UserUpdateDate
from
	Organization2010 O
	INNER JOIN Region R on R.Id = O.RegionId
	LEFT JOIN OrganizationRequest2010 OrR on O.Id = OrR.OrganizationId
	LEFT JOIN Account A on A.OrganizationId = OrR.Id
where
	O.DepartmentId = @departmentId OR o.MainId = @departmentId
	
	
group by
	O.Id,
	O.FullName,
	O.RegionId,
	R.Name,
	O.AccreditationSertificate,
	O.DirectorFullName

RETURN
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportUserStatusAccredTVF]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[ReportUserStatusAccredTVF](@periodBegin datetime,@periodEnd datetime)
RETURNS @report TABLE 
(
	[ ] nvarchar(500) null, 
	[Правовая форма] nvarchar(50) null, 
	[Всего] int null,
	[из них на регистрации] int null, 
	[из них на согласовании] int null,
	[из них на доработке] int null, 
	[из них действующие] int null,
	[из них отключенные] int null
)
AS
BEGIN
	
DECLARE @Statuses TABLE
(
	[Name] NVARCHAR (50),
	Code NVARCHAR (50),
	[Order] INT
)
INSERT INTO @Statuses ([Name],Code,[Order])
SELECT 'На доработке','revision',3
UNION
SELECT 'На регистрации','registration',1
UNION
SELECT 'Отключен','deactivated',5
UNION
SELECT 'Активирован','activated',4
UNION
SELECT 'На согласовании','consideration',2
UNION
SELECT 'Всего','total',10

DECLARE @OPF TABLE
(
	[Name] NVARCHAR (50),
	Code BIT,
	[Order] INT
)
INSERT INTO @OPF ([Name],Code,[Order])
SELECT 'Негосударственный',1,1
UNION
SELECT 'Государственный',0,0

DECLARE @Combinations TABLE
(
	OrgTypeName NVARCHAR (50),
	OrgTypeCode INT,
	IsPrivateName NVARCHAR (50),
	IsPrivateCode INT,
	IsPrivateOrder INT,
	StatusName NVARCHAR(50),
	StatusCode NVARCHAR(50),
	StatusOrder INT
)

INSERT INTO @Combinations(OrgTypeName,OrgTypeCode,IsPrivateName,IsPrivateCode,IsPrivateOrder,StatusName,StatusCode,StatusOrder)
SELECT OrganizationType2010.[Name],OrganizationType2010.Id, OPFTable.[Name],OPFTable.Code,OPFTable.[Order], StatTable.[Name],StatTable.Code,
StatTable.[Order]
FROM OrganizationType2010,@OPF AS OPFTable,@Statuses AS StatTable
WHERE OrganizationType2010.Id<3
UNION
SELECT 'ВУЗ',1,'Всего',NULL,3, StatTable.[Name],StatTable.Code,StatTable.[Order] FROM @Statuses AS StatTable
UNION
SELECT 'ССУЗ',2,'Всего',NULL,3, StatTable.[Name],StatTable.Code,StatTable.[Order] FROM @Statuses AS StatTable
UNION
SELECT 'Итого',10,'-',NULL,3, StatTable.[Name],StatTable.Code,StatTable.[Order] FROM @Statuses AS StatTable

DELETE FROM @Combinations WHERE OrgTypeCode IN(3,4) AND IsPrivateCode=1

--SELECT * FROM @Combinations
DECLARE @Users TABLE
(
	OrgTypeName NVARCHAR (50),
	OrgTypeCode NVARCHAR (50),
	IsPrivateName NVARCHAR (50),
	IsPrivateOrder NVARCHAR (50),
	StatusName NVARCHAR(50),
	UsersCount INT
)

INSERT INTO @Users(OrgTypeName,OrgTypeCode,IsPrivateName,IsPrivateOrder,StatusName,UsersCount)
SELECT Comb.OrgTypeName,Comb.OrgTypeCode,Comb.IsPrivateName,Comb.IsPrivateOrder,Comb.StatusName,COUNT(Acc.Id) FROM dbo.Account Acc 
LEFT JOIN dbo.OrganizationRequest2010 OReq 
ON Acc.OrganizationId=OReq.Id
INNER JOIN dbo.Organization2010 Org 
ON (Org.Id=OReq.OrganizationId 
	AND (
		Org.IsAccredited=1 
		OR (
			Org.AccreditationSertificate != '' 
			AND Org.AccreditationSertificate IS NOT NULL
			)
		)
	)
INNER JOIN dbo.OrganizationType2010 OType
ON OReq.TypeId=OType.Id
RIGHT JOIN @Combinations Comb 
ON (Acc.Status=Comb.StatusCode 
	AND (
		OReq.TypeId=Comb.OrgTypeCode 
		AND (
			(OReq.IsPrivate=Comb.IsPrivateCode AND Comb.IsPrivateCode IS NOT NULL)
			OR
			(Comb.IsPrivateCode IS NULL)
		)
		AND Comb.OrgTypeCode!=10
	))
OR (
	(Acc.Status=Comb.StatusCode)
	AND (
		Comb.OrgTypeCode=10
	)
	AND (
		OReq.TypeId IS NOT NULL
	)
)
OR (
	Comb.StatusCode='total'
	AND ((
		OReq.TypeId=Comb.OrgTypeCode 
		AND (
			(OReq.IsPrivate=Comb.IsPrivateCode AND Comb.IsPrivateCode IS NOT NULL)
			OR
			(Comb.IsPrivateCode IS NULL)
		)
		AND Comb.OrgTypeCode!=10
	)
	OR
	((Comb.OrgTypeCode=10)AND(OReq.TypeId IS NOT NULL)))
)

GROUP BY Comb.OrgTypeName,Comb.OrgTypeCode,IsPrivateName,IsPrivateOrder,StatusName

DECLARE  @PreResult TABLE
(	
	MainOrder INT,
	OrgTypeName NVARCHAR (50),
	IsPrivateName NVARCHAR (50),
	[Всего] INT,
	[Активирован] INT,
	[На регистрации] INT,
	[На доработке] INT,
	[На согласовании] INT,
	[Отключен] INT
)
DECLARE @days INT
SET @days=  DATEDIFF(DAY,@periodBegin,@periodEnd)

INSERT INTO @PreResult
SELECT OrgTypeCode*100+IsPrivateOrder AS MainOrder,
OrgTypeName AS [Вид],
IsPrivateName AS [Правовая форма],
ISNULL([Всего],0) AS [Всего] ,
ISNULL([Активирован],0) AS [Активирован], 
ISNULL([На регистрации],0) AS [На регистрации], 
ISNULL([На доработке],0) AS [На доработке], 
ISNULL([На согласовании],0) AS [На согласовании], 
ISNULL([Отключен],0) AS [Отключен]
FROM @Users PIVOT
(
  SUM(UsersCount)
  FOR [StatusName] IN ([Активирован],[На регистрации],[На доработке],[На согласовании],[Отключен],[Всего]) 
) AS P
UNION

SELECT 
2000
,'В том числе на '+convert(varchar(16),@periodEnd, 120)+' за ' 
+ case @days when 1 then '24 часа' else cast(@days as varchar(10)) + ' дней' end
+':' 
, '-'
, SUM([Всего]) 
, SUM([Активирован]) 
, SUM([На регистрации]) 
, SUM([На доработке]) 
, SUM([На согласовании]) 
, SUM([Отключен])


FROM(
	SELECT 
		1 AS [Всего],
		case when A.[Status]='activated' then 1 else 0 end AS [Активирован],
		case when A.[Status]='registration' then 1 else 0 end AS [На регистрации],
		case when A.[Status]='revision' then 1 else 0 end AS [На доработке],
		case when A.[Status]='consideration' then 1 else 0 end AS [На согласовании],
		case when A.[Status]='deactivated' then 1 else 0 end AS [Отключен]
		
	FROM dbo.Account A 
	INNER JOIN OrganizationRequest2010 OReq ON OReq.Id=A.OrganizationId
	INNER JOIN Organization2010 Org ON 
	(
		Org.Id=OReq.OrganizationId 
		AND (
			Org.IsAccredited=1 
			OR (
				Org.AccreditationSertificate != '' 
				AND Org.AccreditationSertificate IS NOT NULL
				)
			)
	)
	INNER JOIN dbo.GroupAccount G ON A.ID=G.AccountId AND G.GroupID in (6,7)
	INNER JOIN 	(SELECT DISTINCT AccountID
		FROM dbo.AccountLog 
		WHERE (IsStatusChange=1 OR (Status='registration' and VersionId=1)) AND UpdateDate between @periodBegin and @periodEnd 
	) F ON A.ID=F.AccountID
	UNION ALL
	SELECT 0,0,0,0,0,0
) T  
ORDER BY MainOrder

INSERT INTO @report
SELECT OrgTypeName,IsPrivateName,[Всего],[На регистрации],[На согласовании],[На доработке],[Активирован],[Отключен]
FROM @PreResult

return
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportUserStatusAccredTVF_New]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[ReportUserStatusAccredTVF_New](@periodBegin datetime,@periodEnd datetime)
RETURNS @report TABLE 
(
	[ ] nvarchar(500) null, 
	[Правовая форма] nvarchar(50) null, 
	[В БД] int null,
	[Всего] int null,
	[из них на регистрации] int null, 
	[из них на согласовании] int null,
	[из них на доработке] int null, 
	[из них действующие] int null,
	[из них отключенные] int null
)
AS
BEGIN
	
DECLARE @days INT
SET @days=  DATEDIFF(DAY,@periodBegin,@periodEnd)

INSERT INTO @report
SELECT * FROM dbo.ReportOrgActivation_VUZ_Accred()
UNION ALL
SELECT * FROM dbo.ReportOrgActivation_SSUZ_Accred()

INSERT INTO @report
SELECT 'Итого','-',
SUM(ISNULL([В БД],0)),
SUM(ISNULL([Всего],0)),
SUM(ISNULL([из них на регистрации],0)),
SUM(ISNULL([из них на согласовании],0)),
SUM(ISNULL([из них на доработке],0)),
SUM(ISNULL([из них действующие],0)),
SUM(ISNULL([из них отключенные],0)) 
FROM @report WHERE [Правовая форма]='Всего'

INSERT INTO @report
SELECT 
'В том числе на '+convert(varchar(16),@periodEnd, 120)+' за ' 
+ case @days when 1 then '24 часа' else cast(@days as varchar(10)) + ' дней' end
+':' 
, '-'
, 0
, SUM([Всего]) 
, SUM([На регистрации]) 
, SUM([На согласовании]) 
, SUM([На доработке]) 
, SUM([Активирован]) 
, SUM([Отключен])


FROM(
	SELECT 
		1 AS [Всего],
		case when A.[Status]='activated' then 1 else 0 end AS [Активирован],
		case when A.[Status]='registration' then 1 else 0 end AS [На регистрации],
		case when A.[Status]='revision' then 1 else 0 end AS [На доработке],
		case when A.[Status]='consideration' then 1 else 0 end AS [На согласовании],
		case when A.[Status]='deactivated' then 1 else 0 end AS [Отключен]
		
	FROM dbo.Account A 
	INNER JOIN OrganizationRequest2010 OReq ON OReq.Id=A.OrganizationId
	INNER JOIN Organization2010 Org ON 
	(
		Org.Id=OReq.OrganizationId 
		AND (
			Org.IsAccredited=1 
			OR (
				Org.AccreditationSertificate != '' 
				AND Org.AccreditationSertificate IS NOT NULL
				)
			)
	)
	INNER JOIN dbo.GroupAccount G ON A.ID=G.AccountId AND G.GroupID=1
	INNER JOIN 	(SELECT DISTINCT AccountID
		FROM dbo.AccountLog 
		WHERE (IsStatusChange=1 OR (Status='registration' and VersionId=1)) AND UpdateDate between @periodBegin and @periodEnd 
	) F ON A.ID=F.AccountID
	UNION ALL
	SELECT 0,0,0,0,0,0
) T  

return
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportUserStatusWithAccredTVF]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[ReportUserStatusWithAccredTVF](@periodBegin datetime,@periodEnd datetime)
RETURNS @report TABLE 
(
	[ ] nvarchar(500) null, 
	[Правовая форма] nvarchar(50) null, 
	[Всего] int null,
	[из них на регистрации] int null, 
	[из них на согласовании] int null,
	[из них на доработке] int null, 
	[из них действующие] int null,
	[из них отключенные] int null
)
AS
BEGIN
	
DECLARE @Statuses TABLE
(
	[Name] NVARCHAR (50),
	Code NVARCHAR (50),
	[Order] INT
)
INSERT INTO @Statuses ([Name],Code,[Order])
SELECT 'На доработке','revision',3
UNION
SELECT 'На регистрации','registration',1
UNION
SELECT 'Отключен','deactivated',5
UNION
SELECT 'Активирован','activated',4
UNION
SELECT 'На согласовании','consideration',2
UNION
SELECT 'Всего','total',10

DECLARE @OPF TABLE
(
	[Name] NVARCHAR (50),
	Code BIT,
	[Order] INT
)
INSERT INTO @OPF ([Name],Code,[Order])
SELECT 'Негосударственный',1,1
UNION
SELECT 'Государственный',0,0

DECLARE @Combinations TABLE
(
	OrgTypeName NVARCHAR (50),
	OrgTypeCode INT,
	IsPrivateName NVARCHAR (50),
	IsPrivateCode INT,
	IsPrivateOrder INT,
	StatusName NVARCHAR(50),
	StatusCode NVARCHAR(50),
	StatusOrder INT
)

INSERT INTO @Combinations(OrgTypeName,OrgTypeCode,IsPrivateName,IsPrivateCode,IsPrivateOrder,StatusName,StatusCode,StatusOrder)
SELECT OrganizationType2010.[Name],OrganizationType2010.Id, OPFTable.[Name],OPFTable.Code,OPFTable.[Order], StatTable.[Name],StatTable.Code,
StatTable.[Order]
FROM OrganizationType2010,@OPF AS OPFTable,@Statuses AS StatTable
UNION
SELECT 'ВУЗ',1,'Всего',NULL,3, StatTable.[Name],StatTable.Code,StatTable.[Order] FROM @Statuses AS StatTable
UNION
SELECT 'ССУЗ',2,'Всего',NULL,3, StatTable.[Name],StatTable.Code,StatTable.[Order] FROM @Statuses AS StatTable
UNION
SELECT 'Итого',10,'-',NULL,3, StatTable.[Name],StatTable.Code,StatTable.[Order] FROM @Statuses AS StatTable

DELETE FROM @Combinations WHERE OrgTypeCode IN(3,4) AND IsPrivateCode=1

--SELECT * FROM @Combinations
DECLARE @Users TABLE
(
	OrgTypeName NVARCHAR (50),
	OrgTypeCode NVARCHAR (50),
	IsPrivateName NVARCHAR (50),
	IsPrivateOrder NVARCHAR (50),
	StatusName NVARCHAR(50),
	UsersCount INT
)

INSERT INTO @Users(OrgTypeName,OrgTypeCode,IsPrivateName,IsPrivateOrder,StatusName,UsersCount)
SELECT Comb.OrgTypeName,Comb.OrgTypeCode,Comb.IsPrivateName,Comb.IsPrivateOrder,Comb.StatusName,COUNT(Acc.Id) FROM dbo.Account Acc 
LEFT JOIN dbo.OrganizationRequest2010 OReq 
INNER JOIN dbo.OrganizationType2010 OType
ON OReq.TypeId=OType.Id
ON Acc.OrganizationId=OReq.Id
RIGHT JOIN @Combinations Comb 
ON (Acc.Status=Comb.StatusCode 
	AND (
		OReq.TypeId=Comb.OrgTypeCode 
		AND (
			(OReq.IsPrivate=Comb.IsPrivateCode AND Comb.IsPrivateCode IS NOT NULL)
			OR
			(Comb.IsPrivateCode IS NULL)
		)
		AND Comb.OrgTypeCode!=10
	))
OR (
	(Acc.Status=Comb.StatusCode)
	AND (
		Comb.OrgTypeCode=10
	)
	AND (
		OReq.TypeId IS NOT NULL
	)
)
OR (
	Comb.StatusCode='total'
	AND ((
		OReq.TypeId=Comb.OrgTypeCode 
		AND (
			(OReq.IsPrivate=Comb.IsPrivateCode AND Comb.IsPrivateCode IS NOT NULL)
			OR
			(Comb.IsPrivateCode IS NULL)
		)
		AND Comb.OrgTypeCode!=10
	)
	OR
	((Comb.OrgTypeCode=10)AND(OReq.TypeId IS NOT NULL)))
)

GROUP BY Comb.OrgTypeName,Comb.OrgTypeCode,IsPrivateName,IsPrivateOrder,StatusName

DECLARE  @PreResult TABLE
(	
	MainOrder INT,
	OrgTypeName NVARCHAR (50),
	IsPrivateName NVARCHAR (50),
	[Всего] INT,
	[Активирован] INT,
	[На регистрации] INT,
	[На доработке] INT,
	[На согласовании] INT,
	[Отключен] INT
)
DECLARE @days INT
SET @days=  DATEDIFF(DAY,@periodBegin,@periodEnd)

INSERT INTO @PreResult
SELECT OrgTypeCode*100+IsPrivateOrder AS MainOrder,
OrgTypeName AS [Вид],
IsPrivateName AS [Правовая форма],
ISNULL([Всего],0) AS [Всего] ,
ISNULL([Активирован],0) AS [Активирован], 
ISNULL([На регистрации],0) AS [На регистрации], 
ISNULL([На доработке],0) AS [На доработке], 
ISNULL([На согласовании],0) AS [На согласовании], 
ISNULL([Отключен],0) AS [Отключен]
FROM @Users PIVOT
(
  SUM(UsersCount)
  FOR [StatusName] IN ([Активирован],[На регистрации],[На доработке],[На согласовании],[Отключен],[Всего]) 
) AS P
UNION

SELECT 
2000
,'В том числе на '+convert(varchar(16),@periodEnd, 120)+' за ' 
+ case @days when 1 then '24 часа' else cast(@days as varchar(10)) + ' дней' end
+':' 
, '-'
, SUM([Всего]) 
, SUM([Активирован]) 
, SUM([На регистрации]) 
, SUM([На доработке]) 
, SUM([На согласовании]) 
, SUM([Отключен])


FROM(
	SELECT 
		1 AS [Всего],
		case when A.[Status]='activated' then 1 else 0 end AS [Активирован],
		case when A.[Status]='registration' then 1 else 0 end AS [На регистрации],
		case when A.[Status]='revision' then 1 else 0 end AS [На доработке],
		case when A.[Status]='consideration' then 1 else 0 end AS [На согласовании],
		case when A.[Status]='deactivated' then 1 else 0 end AS [Отключен]
		
	FROM dbo.Account A 
	INNER JOIN dbo.GroupAccount G ON A.ID=G.AccountId AND G.GroupID in (6,7)
	INNER JOIN 	(SELECT DISTINCT AccountID
		FROM dbo.AccountLog 
		WHERE (IsStatusChange=1 OR (Status='registration' and VersionId=1)) AND UpdateDate between @periodBegin and @periodEnd 
	) F ON A.ID=F.AccountID
	UNION ALL
	SELECT 0,0,0,0,0,0
) T  
ORDER BY MainOrder

INSERT INTO @report
SELECT OrgTypeName,IsPrivateName,[Всего],[На регистрации],[На согласовании],[На доработке],[Активирован],[Отключен]
FROM @PreResult
UNION ALL
SELECT 
NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL
UNION ALL
SELECT 
'Из них аккредитованных',NULL,NULL,NULL,NULL,NULL,NULL,NULL
UNION ALL
SELECT * FROM
dbo.ReportUserStatusAccredTVF (@periodBegin ,@periodEnd)

return
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportUserStatusWithAccredTVF_New]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[ReportUserStatusWithAccredTVF_New](@periodBegin datetime,@periodEnd datetime)
RETURNS @report TABLE 
(
	[ ] nvarchar(500) null, 
	[Правовая форма] nvarchar(50) null, 
	[В БД] int null,
	[Всего] int null,
	[из них на регистрации] int null, 
	[из них на согласовании] int null,
	[из них на доработке] int null, 
	[из них действующие] int null,
	[из них отключенные] int null
)
AS
BEGIN
	
DECLARE @days INT
SET @days=  DATEDIFF(DAY,@periodBegin,@periodEnd)



INSERT INTO @report
SELECT * FROM dbo.ReportOrgActivation_VUZ()
UNION ALL
SELECT * FROM dbo.ReportOrgActivation_SSUZ()
UNION ALL
SELECT * FROM dbo.ReportOrgActivation_OTHER()

INSERT INTO @report
SELECT 'Итого','-',
SUM(ISNULL([В БД],0)),
SUM(ISNULL([Всего],0)),
SUM(ISNULL([из них на регистрации],0)),
SUM(ISNULL([из них на согласовании],0)),
SUM(ISNULL([из них на доработке],0)),
SUM(ISNULL([из них действующие],0)),
SUM(ISNULL([из них отключенные],0)) 
FROM @report WHERE [Правовая форма]='Всего' OR [ ]='РЦОИ' OR [ ]='Орган управления образованием' OR [ ]='Другое'


INSERT INTO @report
SELECT 
'В том числе на '+convert(varchar(16),@periodEnd, 120)+' за ' 
+ case @days when 1 then '24 часа' else cast(@days as varchar(10)) + ' дней' end
+':' 
, '-'
, 0
, SUM([Всего]) 
, SUM([На регистрации]) 
, SUM([На согласовании]) 
, SUM([На доработке]) 
, SUM([Активирован]) 
, SUM([Отключен])


FROM(
	SELECT 
		1 AS [Всего],
		case when A.[Status]='activated' then 1 else 0 end AS [Активирован],
		case when A.[Status]='registration' then 1 else 0 end AS [На регистрации],
		case when A.[Status]='revision' then 1 else 0 end AS [На доработке],
		case when A.[Status]='consideration' then 1 else 0 end AS [На согласовании],
		case when A.[Status]='deactivated' then 1 else 0 end AS [Отключен]
		
	FROM dbo.Account A 
	INNER JOIN OrganizationRequest2010 OReq ON OReq.Id=A.OrganizationId
	INNER JOIN Organization2010 Org ON Org.Id=OReq.OrganizationId 
	INNER JOIN dbo.GroupAccount G ON A.ID=G.AccountId AND G.GroupID=1
	INNER JOIN 	(SELECT DISTINCT AccountID
		FROM dbo.AccountLog 
		WHERE (IsStatusChange=1 OR (Status='registration' and VersionId=1)) AND UpdateDate between @periodBegin and @periodEnd 
	) F ON A.ID=F.AccountID
	UNION ALL
	SELECT 0,0,0,0,0,0
) T  
UNION ALL
SELECT 
NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL
UNION ALL
SELECT 
'Из них аккредитованных',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL
UNION ALL
SELECT * FROM
dbo.ReportUserStatusAccredTVF_New (@periodBegin ,@periodEnd)

return
END
GO
/****** Object:  UserDefinedFunction [dbo].[ReportXMLSubordinateOrg]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--Функция по подведомственным учреждениям для экспорта в XML
CREATE FUNCTION  [dbo].[ReportXMLSubordinateOrg](
			@periodBegin datetime,
			@periodEnd datetime,
			@departmentId int)
RETURNS @Report TABLE
(
	[Код ОУ] int null,
	[Полное наименование] nvarchar(Max) null,
	[Код региона] int null,
	[Наименование региона] nvarchar(255) null,
	[Свидетельство об аккредитации] nvarchar(255) null,
	[ФИО руководителя] nvarchar(255) null,
	[Количество пользователей] int null,
	[Дата активации пользователя] datetime null
)
AS BEGIN
INSERT INTO @Report
SELECT
	Id [Код ОУ],
	FullName [Полное наименование],
	RegionId [Код региона] ,
	RegionName [Наименование региона],
	AccreditationSertificate [Свидетельство об аккредитации],
	DirectorFullName [ФИО руководителя],
	CountUser [Количество пользователей],
	UserUpdateDate [Дата активации]
FROM
	dbo.ReportStatisticSubordinateOrg ( @periodBegin, @periodEnd, @departmentId)
RETURN
END

GO
/****** Object:  UserDefinedFunction [dbo].[ufn_ut_SplitFromString]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create function [dbo].[ufn_ut_SplitFromString] 
(
	@string nvarchar(max),
	@delimeter nvarchar(1) = ' '
)
returns @ret table (nam nvarchar(1000) )
as
begin
	if len(@string)=0 
		return 
	declare @s int, @e int
	set @s = 0
	while charindex(@delimeter,@string,@s) <> 0
	begin
		set @e = charindex(@delimeter,@string,@s)
		insert @ret values (rtrim(ltrim(substring(@string,@s,@e - @s))))
		set @s = @e + 1
	end
	insert @ret values (rtrim(ltrim(substring(@string,@s,300))))
	return
end
GO
/****** Object:  Table [dbo].[Account]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[UpdateId] [uniqueidentifier] NOT NULL,
	[EditorAccountId] [bigint] NULL,
	[EditorIp] [nvarchar](255) NOT NULL,
	[Login] [nvarchar](255) NOT NULL,
	[PasswordHash] [nvarchar](255) NULL,
	[LastName] [nvarchar](255) NULL,
	[FirstName] [nvarchar](255) NULL,
	[PatronymicName] [nvarchar](255) NULL,
	[OrganizationId] [bigint] NULL,
	[IsOrganizationOwner] [bit] NULL,
	[ConfirmYear] [int] NULL,
	[Phone] [nvarchar](255) NULL,
	[Email] [nvarchar](255) NULL,
	[RegistrationDocument] [image] NULL,
	[RegistrationDocumentContentType] [nvarchar](255) NULL,
	[AdminComment] [ntext] NULL,
	[IsActive] [bit] NOT NULL,
	[Status] [nvarchar](255) NOT NULL,
	[IpAddresses] [nvarchar](4000) NULL,
	[HasFixedIp] [bit] NOT NULL,
	[Position] [nvarchar](510) NULL,
	[HasCrocEgeIntegration] [int] NULL,
 CONSTRAINT [PK7] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Organization2010]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Organization2010](
	[Id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[FullName] [nvarchar](max) NOT NULL,
	[ShortName] [nvarchar](max) NULL,
	[RegionId] [int] NOT NULL,
	[TypeId] [int] NOT NULL,
	[KindId] [int] NOT NULL,
	[INN] [nvarchar](50) NULL,
	[OGRN] [nvarchar](50) NULL,
	[OwnerDepartment] [nvarchar](2000) NULL,
	[IsPrivate] [bit] NOT NULL,
	[IsFilial] [bit] NOT NULL,
	[DirectorPosition] [nvarchar](2000) NULL,
	[DirectorFullName] [nvarchar](2000) NULL,
	[IsAccredited] [bit] NULL,
	[AccreditationSertificate] [nvarchar](2000) NULL,
	[LawAddress] [nvarchar](max) NULL,
	[FactAddress] [nvarchar](max) NULL,
	[PhoneCityCode] [nvarchar](2000) NULL,
	[Phone] [nvarchar](2000) NULL,
	[Fax] [nvarchar](2000) NULL,
	[EMail] [nvarchar](2000) NULL,
	[Site] [nvarchar](2000) NULL,
	[WasImportedAtStart] [bit] NOT NULL,
	[CNFederalBudget] [int] NOT NULL,
	[CNTargeted] [int] NOT NULL,
	[CNLocalBudget] [int] NOT NULL,
	[CNPaying] [int] NOT NULL,
	[CNFullTime] [int] NOT NULL,
	[CNEvening] [int] NOT NULL,
	[CNPostal] [int] NOT NULL,
	[RCModel] [int] NULL,
	[RCDescription] [nvarchar](400) NULL,
	[MainId] [int] NULL,
	[DepartmentId] [int] NULL,
	[StatusId] [int] NOT NULL,
	[NewOrgId] [int] NULL,
	[Version] [int] NOT NULL,
	[DateChangeStatus] [datetime] NULL,
	[Reason] [nvarchar](100) NULL,
	[ReceptionOnResultsCNE] [bit] NULL,
	[ISLOD_GUID] [varchar](50) NULL,
	[KPP] [nvarchar](50) NULL,
	[LetterToReschedule] [image] NULL,
	[LetterToRescheduleName] [nvarchar](255) NULL,
	[LetterToRescheduleContentType] [nvarchar](255) NULL,
	[TimeConnectionToSecureNetwork] [datetime] NULL,
	[TimeEnterInformationInFIS] [datetime] NULL,
	[ConnectionSchemeId] [int] NOT NULL,
	[ConnectionStatusId] [int] NOT NULL,
	[IsAgreedTimeConnection] [bit] NULL,
	[IsAgreedTimeEnterInformation] [bit] NULL,
	[DirectorPositionInGenetive] [nvarchar](2000) NULL,
	[DirectorFullNameInGenetive] [nvarchar](2000) NULL,
	[DirectorFirstName] [nvarchar](100) NULL,
	[DirectorLastName] [nvarchar](100) NULL,
	[DirectorPatronymicName] [nvarchar](100) NULL,
	[OUConfirmation] [bit] NOT NULL,
	[UpdatedByUser] [bit] NOT NULL,
	[Eiis_Id] [nvarchar](50) NULL,
	[TownName] [nvarchar](250) NULL,
	[IsReligious] [bit] NOT NULL,
	[IsLawEnforcmentOrganization] [bit] NOT NULL,
	[OrganizationISId] [int] NULL,
	[IsAnotherName] [varchar](255) NULL,
 CONSTRAINT [PK__Organization2010__24F84F52] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrganizationKind]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrganizationKind](
	[Id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Name] [nvarchar](30) NOT NULL,
	[SortOrder] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrganizationRequest2010]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrganizationRequest2010](
	[Id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[FullName] [nvarchar](max) NOT NULL,
	[ShortName] [nvarchar](max) NULL,
	[RegionId] [int] NOT NULL,
	[TypeId] [int] NOT NULL,
	[KindId] [int] NULL,
	[INN] [nvarchar](50) NULL,
	[OGRN] [nvarchar](50) NULL,
	[OwnerDepartment] [nvarchar](2000) NULL,
	[IsPrivate] [bit] NULL,
	[IsFilial] [bit] NULL,
	[DirectorPosition] [nvarchar](2000) NULL,
	[DirectorFullName] [nvarchar](2000) NULL,
	[IsAccredited] [bit] NULL,
	[AccreditationSertificate] [nvarchar](2000) NULL,
	[LawAddress] [nvarchar](max) NULL,
	[FactAddress] [nvarchar](max) NULL,
	[PhoneCityCode] [nvarchar](2000) NULL,
	[Phone] [nvarchar](2000) NULL,
	[Fax] [nvarchar](2000) NULL,
	[EMail] [nvarchar](2000) NULL,
	[Site] [nvarchar](2000) NULL,
	[OrganizationId] [int] NULL,
	[StatusID] [int] NOT NULL,
	[RegistrationDocument] [image] NULL,
	[RegistrationDocumentContentType] [nvarchar](255) NULL,
	[IsForActivation] [bit] NOT NULL,
	[RCModelID] [int] NULL,
	[RCDescription] [nchar](400) NULL,
	[ReceptionOnResultsCNE] [bit] NULL,
	[KPP] [nvarchar](50) NULL,
	[DirectorPositionInGenetive] [nvarchar](2000) NULL,
	[DirectorFullNameInGenetive] [nvarchar](2000) NULL,
	[DirectorFirstName] [nvarchar](100) NULL,
	[DirectorLastName] [nvarchar](100) NULL,
	[DirectorPatronymicName] [nvarchar](100) NULL,
	[OUConfirmation] [bit] NOT NULL,
	[TownName] [nvarchar](250) NULL,
 CONSTRAINT [PK__OrganizationRequ__2BA54CE1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrganizationRequestAccount]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrganizationRequestAccount](
	[OrgRequestID] [int] NOT NULL,
	[AccountID] [bigint] NOT NULL,
	[GroupID] [int] NOT NULL,
	[Id] [bigint] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
 CONSTRAINT [PK_OrganizationRequestAccount] PRIMARY KEY CLUSTERED 
(
	[OrgRequestID] ASC,
	[AccountID] ASC,
	[GroupID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrganizationType2010]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrganizationType2010](
	[Id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Name] [nvarchar](30) NOT NULL,
	[SortOrder] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RecruitmentCampaigns]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RecruitmentCampaigns](
	[Id] [int] NOT NULL,
	[ModelName] [nvarchar](400) NOT NULL,
	[ModelType] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Region]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Region](
	[Id] [int] NOT NULL,
	[Code] [nvarchar](255) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[InOrganization] [bit] NOT NULL,
	[InCertificate] [bit] NOT NULL,
	[SortIndex] [tinyint] NOT NULL,
	[InOrganizationEtalon] [bit] NOT NULL,
	[FederalDistrictId] [int] NOT NULL,
	[Eiis_Id] [nvarchar](50) NULL,
 CONSTRAINT [PK_Region] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[Большой отчет]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Большой отчет]
AS
SELECT     TOP (100) PERCENT dbo.Organization2010.FullName AS [Полное название организации], dbo.Organization2010.ShortName AS [Краткое название организации], 
                      dbo.OrganizationType2010.Name AS [Тип ОУ], dbo.OrganizationKind.Name AS [Вид ОУ], dbo.Region.Name AS Регион, dbo.Organization2010.INN, 
                      dbo.Organization2010.OGRN, dbo.Organization2010.IsPrivate, dbo.Organization2010.IsFilial, dbo.Organization2010.DirectorPosition, 
                      dbo.Organization2010.DirectorFullName, dbo.Organization2010.LawAddress, dbo.Organization2010.FactAddress, dbo.Organization2010.PhoneCityCode, 
                      dbo.Organization2010.Phone, dbo.Organization2010.Fax, dbo.Organization2010.EMail, dbo.Organization2010.RCModel, dbo.Organization2010.MainId, 
                      dbo.Organization2010.DepartmentId, dbo.Organization2010.Id AS OrgID, COUNT(dbo.Account.Id) AS [Количество учетных записей], 
                      Organization2010_2.FullName AS [Головной ОУ], Organization2010_1.FullName AS Учредитель, RecruitmentCampaigns_1.ModelName
FROM         dbo.Organization2010 INNER JOIN
                      dbo.RecruitmentCampaigns ON dbo.Organization2010.RCModel = dbo.RecruitmentCampaigns.Id INNER JOIN
                      dbo.Region ON dbo.Organization2010.RegionId = dbo.Region.Id INNER JOIN
                      dbo.OrganizationType2010 ON dbo.Organization2010.TypeId = dbo.OrganizationType2010.Id INNER JOIN
                      dbo.OrganizationKind ON dbo.Organization2010.KindId = dbo.OrganizationKind.Id INNER JOIN
                      dbo.RecruitmentCampaigns AS RecruitmentCampaigns_1 ON dbo.Organization2010.RCModel = RecruitmentCampaigns_1.Id LEFT OUTER JOIN
                      dbo.Organization2010 AS Organization2010_1 ON dbo.Organization2010.DepartmentId = Organization2010_1.Id LEFT OUTER JOIN
                      dbo.Organization2010 AS Organization2010_2 ON dbo.Organization2010.MainId = Organization2010_2.Id LEFT OUTER JOIN
                      dbo.OrganizationRequestAccount INNER JOIN
                      dbo.Account ON dbo.OrganizationRequestAccount.AccountID = dbo.Account.Id INNER JOIN
                      dbo.OrganizationRequest2010 ON dbo.OrganizationRequestAccount.OrgRequestID = dbo.OrganizationRequest2010.Id ON 
                      dbo.Organization2010.Id = dbo.OrganizationRequest2010.OrganizationId
GROUP BY dbo.Organization2010.FullName, dbo.Organization2010.ShortName, dbo.OrganizationType2010.Name, dbo.OrganizationKind.Name, dbo.Region.Name, 
                      dbo.Organization2010.INN, dbo.Organization2010.OGRN, dbo.Organization2010.DirectorPosition, dbo.Organization2010.DirectorFullName, 
                      dbo.Organization2010.LawAddress, dbo.Organization2010.FactAddress, dbo.Organization2010.PhoneCityCode, dbo.Organization2010.Phone, 
                      dbo.Organization2010.Fax, dbo.Organization2010.EMail, dbo.Organization2010.RCModel, dbo.Organization2010.MainId, dbo.Organization2010.DepartmentId, 
                      dbo.Organization2010.Id, Organization2010_2.FullName, Organization2010_1.FullName, dbo.Organization2010.IsPrivate, dbo.Organization2010.IsFilial, 
                      dbo.Organization2010.TypeId, RecruitmentCampaigns_1.ModelName
HAVING      (dbo.Organization2010.TypeId = 1) OR
                      (dbo.Organization2010.TypeId = 2)
ORDER BY Учредитель, [Головной ОУ]
GO
/****** Object:  UserDefinedFunction [dbo].[GetEventParam]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Возвращает значение параметра по его номеру, 
-- из строки с разделителем |.
-- v.1.0: Created by Sedov Anton 15.05.2008
-- Измение размера выходного массива.
-- =============================================
create function [dbo].[GetEventParam]
	(
	@eventParams nvarchar(4000)
	, @index int
	)
returns nvarchar(4000) with schemabinding
as
begin
	declare 
		@delimiterIndex int
		, @startIndex int
		, @sourceParams nvarchar(4000)
		, @result nvarchar(4000)
	
	set @delimiterIndex = 1
	set @startIndex = 1
	set @sourceParams = Convert(nvarchar(4000), @eventParams) 
	set @delimiterIndex = charindex('|', isnull(@sourceParams, ''))
	
	if @delimiterIndex = 0
		return @sourceParams

	set @delimiterIndex = 1

	while @index <> 0
	begin
		set @startIndex = 1
		set @delimiterIndex = charindex('|', isnull(@sourceParams, ''))

		if @delimiterIndex = 0
			set @result = @sourceParams
		else
		begin
			set @result = substring(@sourceParams, @startIndex, @delimiterIndex - @startIndex)
			set @startIndex = @delimiterIndex
			set @sourceParams = substring(@sourceParams
					, @startIndex + 1, len(@sourceParams) - @startIndex) 
		end

		set @index = @index - 1
	end	
	
	return @result
end
GO
/****** Object:  Table [dbo].[EventLog]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EventLog](
	[Id] [bigint] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Date] [datetime] NOT NULL,
	[AccountId] [bigint] NULL,
	[Ip] [nvarchar](255) NOT NULL,
	[EventCode] [nvarchar](100) NOT NULL,
	[SourceEntityId] [bigint] NULL,
	[EventParams] [nvarchar](4000) NULL,
	[UpdateId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_EventLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[AuthenticationEventLog]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Лог событий входов  в систему
-- v.1.0: Created by Sedov Anton 21.05.2008
-- =============================================
CREATE view [dbo].[AuthenticationEventLog] with schemabinding 
as
select
	event_log.Id EventId 
	, event_log.Date Date
	, event_log.Ip Ip
	, event_log.SourceEntityId AccountId
	, case 
		when event_log.EventCode = 'USR_VERIFY'  
			then Convert(bit, dbo.GetEventParam(event_log.EventParams, 4)) 
			else 1
	end IsPasswordValid
	, case 
		when event_log.EventCode = 'USR_VERIFY'
			then Convert(bit, dbo.GetEventParam(event_log.EventParams, 5)) 
			else 1
	end IsIpValid
from   
	dbo.EventLog event_log
where 
	((event_log.EventCode = 'USR_VERIFY'
			and not event_log.AccountId is null)
		or (event_log.EventCode = 'USR_REG'
			and event_log.AccountId is null))
GO
/****** Object:  Table [dbo].[Role]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[Id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Code] [nvarchar](255) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GroupRole]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GroupRole](
	[Id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[RoleId] [int] NOT NULL,
	[GroupId] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[IsActiveCondition] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GroupAccount]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GroupAccount](
	[Id] [bigint] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[GroupId] [int] NOT NULL,
	[AccountId] [bigint] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[AccountRole]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Роли пользователей
-- v.1.0: Created by Makarev Andrey 02.04.2008
-- =============================================
CREATE view [dbo].[AccountRole] with schemabinding
as
select 
	group_account.AccountId AccountId
	, [role].Code RoleCode
	, group_account.GroupId GroupId
	, group_role.RoleId RoleId
	, group_role.IsActiveCondition IsActiveCondition
from
	dbo.GroupAccount group_account
		inner join dbo.GroupRole group_role 
			on group_account.GroupId = group_role.GroupId
		inner join dbo.[Role] [role] 
			on [role].Id = group_role.RoleId
where
	group_role.IsActive = 1

GO
/****** Object:  Table [dbo].[AccountIp]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountIp](
	[Id] [bigint] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[AccountId] [bigint] NOT NULL,
	[Ip] [nvarchar](255) NULL,
 CONSTRAINT [PK12] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AccountKey]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountKey](
	[Id] [bigint] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[UpdateId] [uniqueidentifier] NOT NULL,
	[EditorAccountId] [bigint] NOT NULL,
	[EditorIp] [nvarchar](255) NOT NULL,
	[AccountId] [bigint] NOT NULL,
	[Key] [nvarchar](255) NOT NULL,
	[DateFrom] [datetime] NULL,
	[DateTo] [datetime] NULL,
	[IsActive] [bit] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AccountKeyLog]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountKeyLog](
	[AccountKeyId] [bigint] NOT NULL,
	[VersionId] [int] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[UpdateId] [uniqueidentifier] NOT NULL,
	[EditorAccountId] [bigint] NOT NULL,
	[EditorIp] [nvarchar](255) NOT NULL,
	[Key] [nvarchar](255) NOT NULL,
	[DateFrom] [datetime] NULL,
	[DateTo] [datetime] NULL,
	[IsActive] [bit] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AccountLog]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountLog](
	[AccountId] [bigint] NOT NULL,
	[VersionId] [int] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[UpdateId] [uniqueidentifier] NOT NULL,
	[EditorAccountId] [bigint] NULL,
	[EditorIp] [nvarchar](255) NOT NULL,
	[Login] [nvarchar](255) NOT NULL,
	[PasswordHash] [nvarchar](255) NULL,
	[OrganizationId] [bigint] NULL,
	[IsOrganizationOwner] [bit] NULL,
	[ConfirmYear] [int] NULL,
	[LastName] [nvarchar](255) NULL,
	[FirstName] [nvarchar](255) NULL,
	[PatronymicName] [nvarchar](255) NULL,
	[Phone] [nvarchar](255) NULL,
	[Email] [nvarchar](255) NULL,
	[AdminComment] [ntext] NULL,
	[IsActive] [bit] NOT NULL,
	[Status] [nvarchar](255) NULL,
	[IpAddresses] [nvarchar](4000) NULL,
	[IsActiveChange] [bit] NOT NULL,
	[IsStatusChange] [bit] NOT NULL,
	[IsEdit] [bit] NOT NULL,
	[IsPasswordChange] [bit] NOT NULL,
	[HasFixedIp] [bit] NOT NULL,
	[IsVpnEditorIp] [bit] NOT NULL,
 CONSTRAINT [PK7_1] PRIMARY KEY CLUSTERED 
(
	[AccountId] ASC,
	[VersionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AccountRoleActivity]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountRoleActivity](
	[CreateDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[AccountId] [bigint] NOT NULL,
	[RoleId] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AccountStatus]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountStatus](
	[StatusID] [int] NOT NULL,
	[Code] [nvarchar](510) NOT NULL,
	[Name] [nvarchar](510) NOT NULL,
 CONSTRAINT [PK_AccountStatus] PRIMARY KEY CLUSTERED 
(
	[StatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AllowedEducationalDirection]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AllowedEducationalDirection](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Eiis_Id] [nvarchar](50) NULL,
	[Name] [nvarchar](3000) NULL,
	[Code] [nvarchar](50) NULL,
	[MappedEducationalLevelId] [int] NULL,
	[QualificationCode] [nvarchar](2) NULL,
	[QualificationName] [nvarchar](1500) NULL,
	[QualificationGrade] [nvarchar](32) NULL,
	[LicenseSupplementId] [int] NULL,
	[EducationalDirectionId] [int] NULL,
	[EducationalDirectionTypeId] [int] NULL,
	[Period] [nvarchar](100) NULL,
 CONSTRAINT [PK_AllowedEducationalDirection] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AskedQuestion]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AskedQuestion](
	[Id] [bigint] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[UpdateId] [uniqueidentifier] NOT NULL,
	[EditorAccountId] [bigint] NOT NULL,
	[EditorIp] [nvarchar](10) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Question] [ntext] NOT NULL,
	[Answer] [ntext] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[ViewCount] [int] NOT NULL,
	[Popularity] [decimal](18, 4) NOT NULL,
	[ContextCodes] [nvarchar](4000) NULL,
 CONSTRAINT [PK28] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AskedQuestionContext]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AskedQuestionContext](
	[Id] [bigint] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[AskedQuestionId] [bigint] NOT NULL,
	[ContextId] [int] NOT NULL,
 CONSTRAINT [PK75] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ConnectionScheme]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ConnectionScheme](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](510) NOT NULL,
 CONSTRAINT [PK_ConnectionScheme] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ConnectionStatus]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ConnectionStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](510) NOT NULL,
 CONSTRAINT [PK_ConnectionStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Context]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Context](
	[Id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Code] [nvarchar](255) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK73] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Delivery]    Script Date: 27.11.2018 15:32:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Delivery](
	[Id] [bigint] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Title] [nvarchar](255) NOT NULL,
	[Message] [nvarchar](4000) NOT NULL,
	[TypeCode] [nvarchar](20) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[DeliveryDate] [datetime] NOT NULL,
	[Status] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DeliveryLog]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DeliveryLog](
	[DeliveryId] [bigint] NULL,
	[ReciverEMail] [nvarchar](255) NOT NULL,
	[Success] [bit] NOT NULL,
	[EventDate] [datetime] NOT NULL,
	[ErrorDescription] [nvarchar](1000) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DeliveryRecipients]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DeliveryRecipients](
	[RecipientCode] [int] NULL,
	[DeliveryId] [bigint] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DeliveryStatus]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DeliveryStatus](
	[Id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Name] [nvarchar](30) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DirectorPosition]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DirectorPosition](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PositionName] [nvarchar](255) NOT NULL,
	[PositionNameInGenetive] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_DirectorPosition] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Document]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Document](
	[Id] [bigint] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[UpdateId] [uniqueidentifier] NOT NULL,
	[EditorAccountId] [bigint] NOT NULL,
	[EditorIp] [nvarchar](255) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Description] [ntext] NOT NULL,
	[Content] [image] NOT NULL,
	[ContentSize] [int] NOT NULL,
	[ContentType] [nvarchar](255) NULL,
	[IsActive] [bit] NOT NULL,
	[ActivateDate] [datetime] NULL,
	[ContextCodes] [nvarchar](4000) NULL,
	[RelativeUrl] [nvarchar](255) NULL,
 CONSTRAINT [PK29] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DocumentContext]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DocumentContext](
	[Id] [bigint] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[DocumentId] [bigint] NOT NULL,
	[ContextId] [int] NOT NULL,
 CONSTRAINT [PK74] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EducationalDirection]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EducationalDirection](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Eiis_Id] [nvarchar](50) NULL,
	[Name] [nvarchar](3000) NULL,
	[Code] [nvarchar](50) NULL,
	[MappedEducationalLevelId] [int] NULL,
	[EducationalDirectionTypeId] [int] NULL,
	[EducationalDirectionGroupCode] [nvarchar](10) NULL,
	[EducationalDirectionGroupName] [nvarchar](255) NULL,
	[EducationalDirectionGroupId] [int] NULL,
	[QualificationCode] [nvarchar](2) NULL,
	[QualificationName] [nvarchar](1500) NULL,
	[QualificationGrade] [nvarchar](32) NULL,
	[Period] [nvarchar](100) NULL,
	[IsOutOfUse] [bit] NOT NULL,
	[DirectoryName] [nvarchar](500) NULL,
 CONSTRAINT [PK_EducationalDirection] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EducationalDirectionGroup]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EducationalDirectionGroup](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Eiis_Id] [nvarchar](50) NULL,
	[Name] [nvarchar](250) NULL,
	[Code] [nvarchar](10) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_EducationalDirectionGroup] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EducationalDirectionType]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EducationalDirectionType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Eiis_Id] [nvarchar](50) NULL,
	[Name] [nvarchar](250) NULL,
	[ShortName] [nvarchar](50) NULL,
 CONSTRAINT [PK_EducationalDirectionType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EducationalDirectionTypeEIIS]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EducationalDirectionTypeEIIS](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Eiis_Id] [nvarchar](50) NULL,
	[Name] [nvarchar](250) NULL,
	[ShortName] [nvarchar](50) NULL,
 CONSTRAINT [PK_EducationalDirectionTypeEIIS] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EducationalLevelEIISMap]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EducationalLevelEIISMap](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Eiis_Id] [nvarchar](50) NULL,
	[Name] [nvarchar](250) NULL,
	[ShortName] [nvarchar](50) NULL,
	[MappedEducationalLevelId] [int] NULL,
 CONSTRAINT [PK_EducationalLevelEIISMap] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[fbs_users]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[fbs_users](
	[Column 0] [nvarchar](50) NULL,
	[Column 1] [nvarchar](50) NULL,
	[Column 2] [nvarchar](50) NULL,
	[Column 3] [nvarchar](255) NULL,
	[Column 4] [nvarchar](50) NULL,
	[Column 5] [nvarchar](2000) NULL,
	[Column 6] [nvarchar](2000) NULL,
	[Column 7] [nvarchar](255) NULL,
	[Column 8] [nvarchar](255) NULL,
	[Column 9] [nvarchar](50) NULL,
	[Column 10] [nvarchar](50) NULL,
	[Column 11] [nvarchar](50) NULL,
	[Column 12] [nvarchar](50) NULL,
	[Column 13] [nvarchar](50) NULL,
	[Column 14] [nvarchar](50) NULL,
	[Column 15] [nvarchar](255) NULL,
	[Column 16] [nvarchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FederalDistricts]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FederalDistricts](
	[Id] [int] NOT NULL,
	[Code] [int] NULL,
	[Name] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Founder]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Founder](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Eiis_Id] [nvarchar](50) NULL,
	[TypeId] [int] NULL,
	[LawAddressRegionId] [int] NOT NULL,
	[FactAddressRegionId] [int] NOT NULL,
	[OrganizationFullName] [nvarchar](max) NULL,
	[OrganizationShortName] [nvarchar](max) NULL,
	[PersonLastName] [nvarchar](255) NULL,
	[PersonFirstName] [nvarchar](255) NULL,
	[PersonPatronymic] [nvarchar](255) NULL,
	[Phones] [nvarchar](255) NULL,
	[Faxes] [nvarchar](255) NULL,
	[Emails] [nvarchar](255) NULL,
	[Ogrn] [nvarchar](50) NULL,
	[Inn] [nvarchar](50) NULL,
	[Kpp] [nvarchar](50) NULL,
	[LawAddress] [nvarchar](max) NULL,
	[FactAddress] [nvarchar](max) NULL,
	[LawAddressDistrict] [nvarchar](255) NULL,
	[FactAddressDistrict] [nvarchar](255) NULL,
	[LawAddressTown] [nvarchar](255) NULL,
	[FactAddressTown] [nvarchar](255) NULL,
	[LawAddressStreet] [nvarchar](255) NULL,
	[FactAddressStreet] [nvarchar](255) NULL,
	[LawAddressHouseNumber] [nvarchar](255) NULL,
	[FactAddressHouseNumber] [nvarchar](255) NULL,
	[LawAddressPostalCode] [nvarchar](50) NULL,
	[FactAddressPostalCode] [nvarchar](50) NULL,
 CONSTRAINT [PK_Founder] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FounderType]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FounderType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Eiis_Id] [nvarchar](50) NULL,
	[Name] [nvarchar](1000) NULL,
	[Code] [nvarchar](255) NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_FounderType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Group]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Group](
	[Id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Code] [nvarchar](255) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[SystemID] [int] NOT NULL,
	[Default] [bit] NOT NULL,
	[IsUserIS] [bit] NOT NULL,
	[IsEduOrg] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[License]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[License](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Eiis_Id] [nvarchar](50) NULL,
	[RegionId] [int] NULL,
	[RegNumber] [nvarchar](10) NULL,
	[StatusName] [nvarchar](100) NULL,
	[IsTermless] [bit] NOT NULL,
	[EndDate] [datetime] NULL,
	[OrganizationId] [int] NOT NULL,
	[BaseDocumentTypeName] [nvarchar](250) NULL,
	[OrderDocumentNumber] [nvarchar](10) NULL,
	[OrderDocumentDate] [datetime] NULL,
	[ReasonOfSuspension] [nvarchar](100) NULL,
	[DateOfSuspension] [datetime] NULL,
	[OldLicenseId] [int] NULL,
	[AdministrativeSuspensionOrder] [nvarchar](150) NULL,
	[SuspensionDecision] [nvarchar](150) NULL,
	[CourtRevokingDecision] [nvarchar](max) NULL,
 CONSTRAINT [PK_License] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[License_ISLOD]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[License_ISLOD](
	[sys_guid] [nvarchar](50) NOT NULL,
	[SchoolName] [nvarchar](1460) NOT NULL,
	[LawAddress] [nvarchar](255) NULL,
	[RegionName] [nvarchar](120) NULL,
	[Inn] [nvarchar](50) NULL,
	[Ogrn] [nvarchar](50) NULL,
	[Kpp] [nvarchar](50) NULL,
	[Opf] [nvarchar](50) NULL,
	[Opf2] [nvarchar](50) NULL,
	[Gol_Fil] [nvarchar](50) NULL,
	[Type] [nvarchar](255) NULL,
	[License] [nvarchar](50) NULL,
	[Status] [nvarchar](125) NULL,
	[Sil] [nvarchar](50) NULL,
	[Relig] [nvarchar](50) NULL,
	[Vuz] [nvarchar](50) NULL,
	[Suz] [nvarchar](50) NULL,
	[Drugie] [nvarchar](50) NULL,
	[Zarubeg] [nvarchar](50) NULL,
 CONSTRAINT [PK__License___6E6899DA1367E606] PRIMARY KEY CLUSTERED 
(
	[sys_guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LicenseSupplement]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LicenseSupplement](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Eiis_Id] [nvarchar](50) NULL,
	[LicenseId] [int] NOT NULL,
	[Number] [nvarchar](8) NULL,
	[FormSerialNumber] [nvarchar](10) NULL,
	[FormNumber] [nvarchar](30) NULL,
	[IsBranch] [bit] NOT NULL,
	[StatusName] [nvarchar](100) NULL,
	[EndDate] [datetime] NULL,
	[OrganizationId] [int] NOT NULL,
	[BaseDocumentTypeName] [nvarchar](250) NULL,
	[OrderDocumentNumber] [nvarchar](10) NULL,
	[OrderDocumentDate] [datetime] NULL,
	[ReasonOfSuspension] [nvarchar](100) NULL,
	[DateOfSuspension] [datetime] NULL,
	[OldSupplementId] [int] NULL,
 CONSTRAINT [PK_LicenseSupplement] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Migrations]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Migrations](
	[MigrationVersion] [int] NOT NULL,
	[MigrationName] [varchar](200) NOT NULL,
	[CreateDate] [datetime] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[News]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[News](
	[Id] [bigint] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[UpdateId] [uniqueidentifier] NOT NULL,
	[EditorAccountId] [bigint] NOT NULL,
	[EditorIp] [nvarchar](255) NOT NULL,
	[Date] [datetime] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Description] [ntext] NOT NULL,
	[Text] [ntext] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK27] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OperatorLog]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OperatorLog](
	[CheckedUserID] [int] NOT NULL,
	[OperatorID] [int] NOT NULL,
	[Comments] [varchar](1024) NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastChange] [datetime] NOT NULL,
 CONSTRAINT [PK_OperatorLog_1] PRIMARY KEY CLUSTERED 
(
	[CheckedUserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrganisationMON]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrganisationMON](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[FullName] [nvarchar](1000) NOT NULL,
	[ShortName] [nvarchar](1000) NULL,
	[RegionId] [int] NOT NULL,
	[TypeId] [int] NOT NULL,
	[KindId] [int] NOT NULL,
	[INN] [nvarchar](10) NULL,
	[OGRN] [nvarchar](13) NULL,
	[OwnerDepartment] [nvarchar](1000) NULL,
	[IsPrivate] [bit] NOT NULL,
	[IsFilial] [bit] NOT NULL,
	[DirectorPosition] [nvarchar](1000) NULL,
	[DirectorFullName] [nvarchar](1000) NULL,
	[IsAccredited] [bit] NULL,
	[AccreditationSertificate] [nvarchar](1000) NULL,
	[LawAddress] [nvarchar](1000) NULL,
	[FactAddress] [nvarchar](1000) NOT NULL,
	[PhoneCityCode] [nvarchar](1000) NULL,
	[Phone] [nvarchar](1000) NULL,
	[Fax] [nvarchar](1000) NULL,
	[EMail] [nvarchar](1000) NULL,
	[Site] [nvarchar](1000) NULL,
	[WasImportedAtStart] [bit] NOT NULL,
	[CNFederalBudget] [int] NOT NULL,
	[CNTargeted] [int] NOT NULL,
	[CNLocalBudget] [int] NOT NULL,
	[CNPaying] [int] NOT NULL,
	[CNFullTime] [int] NOT NULL,
	[CNEvening] [int] NOT NULL,
	[CNPostal] [int] NOT NULL,
	[RCModel] [int] NULL,
	[RCDescription] [nvarchar](400) NULL,
	[MainId] [int] NULL,
	[DepartmentId] [int] NULL,
	[StatusId] [int] NOT NULL,
	[NewOrgId] [int] NULL,
	[Version] [int] NOT NULL,
	[DateChangeStatus] [datetime] NULL,
	[Reason] [nvarchar](100) NULL,
	[ReceptionOnResultsCNE] [bit] NULL,
	[ISLOD_GUID] [varchar](50) NULL,
	[KPP] [nvarchar](9) NULL,
	[LetterToReschedule] [image] NULL,
	[LetterToRescheduleName] [nvarchar](255) NULL,
	[LetterToRescheduleContentType] [nvarchar](255) NULL,
	[TimeConnectionToSecureNetwork] [datetime] NULL,
	[TimeEnterInformationInFIS] [datetime] NULL,
	[ConnectionSchemeId] [int] NOT NULL,
	[ConnectionStatusId] [int] NOT NULL,
	[IsAgreedTimeConnection] [bit] NULL,
	[IsAgreedTimeEnterInformation] [bit] NULL,
	[DirectorPositionInGenetive] [nvarchar](255) NULL,
	[DirectorFullNameInGenetive] [nvarchar](255) NULL,
	[DirectorFirstName] [nvarchar](100) NULL,
	[DirectorLastName] [nvarchar](100) NULL,
	[DirectorPatronymicName] [nvarchar](100) NULL,
	[OUConfirmation] [bit] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrganizationFounder]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrganizationFounder](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Eiis_Id] [nvarchar](50) NULL,
	[OrganizationId] [int] NOT NULL,
	[FounderId] [int] NOT NULL,
 CONSTRAINT [PK_OrganizationFounder] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrganizationIS]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrganizationIS](
	[Id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Name] [nvarchar](30) NOT NULL,
	[SortOrder] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrganizationKindEIISMap]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrganizationKindEIISMap](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Eiis_Id] [nvarchar](50) NULL,
	[Name] [nvarchar](400) NULL,
	[Code] [nvarchar](50) NULL,
	[OrganizationKindId] [int] NULL,
 CONSTRAINT [PK_OrganizationKindEIISMap] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrganizationLimitation]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrganizationLimitation](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Eiis_Id] [nvarchar](50) NULL,
	[OrganizationId] [int] NOT NULL,
	[DocumentName] [nvarchar](255) NULL,
	[DocumentNumber] [nvarchar](150) NULL,
	[DocumentDate] [datetime] NULL,
 CONSTRAINT [PK_OrganizationLimitation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrganizationLog]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrganizationLog](
	[OrganizationId] [bigint] NOT NULL,
	[VersionId] [int] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[UpdateId] [uniqueidentifier] NOT NULL,
	[EditorAccountId] [bigint] NULL,
	[EditorIp] [nvarchar](255) NOT NULL,
	[RegionId] [bigint] NOT NULL,
	[DepartmentOwnershipCode] [nvarchar](255) NULL,
	[Name] [nvarchar](2000) NOT NULL,
	[FounderName] [nvarchar](2000) NOT NULL,
	[Address] [nvarchar](500) NOT NULL,
	[ChiefName] [nvarchar](255) NOT NULL,
	[Fax] [nvarchar](255) NULL,
	[Phone] [nvarchar](255) NULL,
	[EducationInstitutionTypeId] [int] NULL,
 CONSTRAINT [PK71] PRIMARY KEY NONCLUSTERED 
(
	[OrganizationId] ASC,
	[VersionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrganizationOperatingStatus]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrganizationOperatingStatus](
	[Id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Name] [nvarchar](30) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrganizationRequestOperatorLog]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrganizationRequestOperatorLog](
	[OrganizationRequestID] [int] NOT NULL,
	[OperatorID] [bigint] NOT NULL,
	[Comments] [varchar](1024) NULL,
	[DTCreate] [datetime] NOT NULL,
	[DTLastChange] [datetime] NOT NULL,
 CONSTRAINT [PK_OperatorOrganizationRequestLog] PRIMARY KEY CLUSTERED 
(
	[OrganizationRequestID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrganizationStatusEIISMap]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrganizationStatusEIISMap](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Eiis_Id] [nvarchar](50) NULL,
	[Name] [nvarchar](400) NULL,
	[OrganizationOperatingStatusId] [int] NULL,
 CONSTRAINT [PK_OrganizationStatusEIISMap] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrganizationUpdateHistory]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrganizationUpdateHistory](
	[Id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[OriginalOrgId] [int] NULL,
	[UpdateDescription] [nvarchar](max) NULL,
	[Version] [int] NULL,
	[UpdateDate] [datetime] NULL,
	[CreateDate] [datetime] NOT NULL,
	[FullName] [nvarchar](max) NOT NULL,
	[ShortName] [nvarchar](max) NULL,
	[RegionId] [int] NULL,
	[TypeId] [int] NULL,
	[KindId] [int] NULL,
	[INN] [nvarchar](50) NULL,
	[OGRN] [nvarchar](50) NULL,
	[OwnerDepartment] [nvarchar](2000) NULL,
	[IsPrivate] [bit] NOT NULL,
	[IsFilial] [bit] NOT NULL,
	[DirectorPosition] [nvarchar](2000) NULL,
	[DirectorFullName] [nvarchar](2000) NULL,
	[IsAccredited] [bit] NULL,
	[AccreditationSertificate] [nvarchar](2000) NULL,
	[LawAddress] [nvarchar](max) NULL,
	[FactAddress] [nvarchar](max) NULL,
	[PhoneCityCode] [nvarchar](2000) NULL,
	[Phone] [nvarchar](2000) NULL,
	[Fax] [nvarchar](2000) NULL,
	[EMail] [nvarchar](2000) NULL,
	[Site] [nvarchar](2000) NULL,
	[RCModel] [int] NULL,
	[RCDescription] [nvarchar](400) NULL,
	[MainId] [int] NULL,
	[DepartmentId] [int] NULL,
	[CNFBFullTime] [int] NULL,
	[CNFBEvening] [int] NULL,
	[CNFBPostal] [int] NULL,
	[CNPayFullTime] [int] NULL,
	[CNPayEvening] [int] NULL,
	[CNPayPostal] [int] NULL,
	[CNFederalBudget] [int] NULL,
	[CNTargeted] [int] NULL,
	[CNLocalBudget] [int] NULL,
	[CNPaying] [int] NULL,
	[CNFullTime] [int] NULL,
	[CNEvening] [int] NULL,
	[CNPostal] [int] NULL,
	[NewOrgId] [int] NULL,
	[StatusId] [int] NULL,
	[EditorUserName] [nvarchar](50) NULL,
	[DateChangeStatus] [datetime] NULL,
	[Reason] [nvarchar](100) NULL,
	[ReceptionOnResultsCNE] [bit] NULL,
	[KPP] [nvarchar](50) NULL,
	[LetterToReschedule] [image] NULL,
	[LetterToRescheduleName] [nvarchar](255) NULL,
	[LetterToRescheduleContentType] [nvarchar](255) NULL,
	[TimeConnectionToSecureNetwork] [datetime] NULL,
	[TimeEnterInformationInFIS] [datetime] NULL,
	[ConnectionSchemeId] [int] NOT NULL,
	[ConnectionStatusId] [int] NOT NULL,
	[IsAgreedTimeConnection] [bit] NULL,
	[IsAgreedTimeEnterInformation] [bit] NULL,
	[DirectorPositionInGenetive] [nvarchar](2000) NULL,
	[DirectorFullNameInGenetive] [nvarchar](2000) NULL,
	[DirectorFirstName] [nvarchar](100) NULL,
	[DirectorLastName] [nvarchar](100) NULL,
	[DirectorPatronymicName] [nvarchar](100) NULL,
	[OUConfirmation] [bit] NOT NULL,
	[TownName] [nvarchar](250) NULL,
	[OrganizationISId] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrgISLOD]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrgISLOD](
	[SYS_GUID] [nvarchar](255) NULL,
	[fullname] [nvarchar](1000) NULL,
	[inn] [nvarchar](255) NULL,
	[kpp] [nvarchar](255) NULL,
	[filial] [nvarchar](255) NULL,
	[opf] [nvarchar](255) NULL,
	[type] [nvarchar](255) NULL,
	[sil] [nvarchar](255) NULL,
	[relig] [nvarchar](255) NULL,
	[licstat] [nvarchar](255) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReplicationData]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReplicationData](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[ReplicationTableName] [varchar](100) NOT NULL,
	[ReplicationRecordId] [varchar](50) NOT NULL,
	[ReplicationType] [char](1) NOT NULL,
	[ReplicationDate] [datetime] NOT NULL,
 CONSTRAINT [PK_ReplicationData] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Report]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Report](
	[id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[name] [varchar](100) NOT NULL,
	[created] [datetime] NOT NULL,
	[xml] [xml] NOT NULL,
	[dateFrom] [datetime] NULL,
	[dateTo] [datetime] NULL,
 CONSTRAINT [PK_Report] PRIMARY KEY CLUSTERED 
(
	[id] ASC,
	[name] ASC,
	[created] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[rpt_ConnectStat]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[rpt_ConnectStat](
	[ConnectStatID] [int] IDENTITY(1,1) NOT NULL,
	[InstEsrpID] [int] NOT NULL,
	[TypeOO] [varchar](255) NOT NULL,
	[NameOO] [varchar](max) NOT NULL,
	[INN] [varchar](255) NULL,
	[OGRN] [varchar](255) NULL,
	[KPP] [varchar](255) NULL,
	[OrderStat] [varchar](255) NULL,
	[ConnectStat] [varchar](255) NULL,
	[StatDate] [datetime] NOT NULL,
	[StatVers] [int] NOT NULL,
	[Scheme] [varchar](255) NULL,
 CONSTRAINT [PK_rpt_ConnectStat] PRIMARY KEY CLUSTERED 
(
	[ConnectStatID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[rpt_ConnectStatV2]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[rpt_ConnectStatV2](
	[InstEsrpID] [int] NOT NULL,
	[TypeOO] [varchar](255) NOT NULL,
	[NameOO] [varchar](max) NOT NULL,
	[INN] [varchar](255) NULL,
	[OGRN] [varchar](255) NULL,
	[KPP] [varchar](255) NULL,
	[OrderStat] [varchar](255) NULL,
	[ConnectStat] [varchar](255) NULL,
	[StatDate] [datetime] NULL,
	[StatVers] [int] NULL,
	[Scheme] [varchar](255) NULL,
	[ConnectStatID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [ConnectStatID] PRIMARY KEY CLUSTERED 
(
	[ConnectStatID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Settings]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Settings](
	[name] [nvarchar](50) NOT NULL,
	[value] [nvarchar](250) NULL,
 CONSTRAINT [PK_Settings] PRIMARY KEY CLUSTERED 
(
	[name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[System]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[System](
	[SystemID] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](510) NOT NULL,
	[Name] [nvarchar](510) NOT NULL,
	[FullName] [nvarchar](1000) NOT NULL,
	[AvailableRegistration] [bit] NOT NULL,
 CONSTRAINT [PK_System] PRIMARY KEY CLUSTERED 
(
	[SystemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Temp_ReportOrgsRegistrationBASE_copy_2]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Temp_ReportOrgsRegistrationBASE_copy_2](
	[Id] [int] NOT NULL,
	[GUID ИСЛОД] [nvarchar](500) NULL,
	[Реквизиты лицензии] [nvarchar](1000) NULL,
	[Статус лицензии] [nvarchar](1000) NULL,
	[Факс] [nvarchar](1000) NULL,
	[Сайт] [nvarchar](1000) NULL,
	[Полное наименование] [nvarchar](4000) NULL,
	[Краткое наименование] [nvarchar](2000) NULL,
	[ИД головной] [int] NULL,
	[Должность руководителя] [nvarchar](1000) NULL,
	[Должность руководителя (кому)] [nvarchar](1000) NULL,
	[ФО] [nvarchar](1000) NULL,
	[Код ФО] [int] NULL,
	[Субъект РФ] [nvarchar](1000) NULL,
	[Код субъекта] [nvarchar](1000) NULL,
	[Тип] [nvarchar](1000) NULL,
	[Категория ОО] [nvarchar](1000) NULL,
	[Категория ОО 2] [nvarchar](1000) NULL,
	[Статус ОО] [nvarchar](1000) NULL,
	[Вид] [nvarchar](1000) NULL,
	[ОПФ] [nvarchar](500) NULL,
	[Филиал] [nvarchar](500) NULL,
	[ФИО руководителя] [nvarchar](1000) NULL,
	[И.О. Фамилия Руководителя] [nvarchar](1000) NULL,
	[Фамилия руководителя] [nvarchar](1000) NULL,
	[Имя руководителя] [nvarchar](1000) NULL,
	[Отчество руководителя] [nvarchar](1000) NULL,
	[Учредитель] [nvarchar](1000) NULL,
	[Фактический адрес] [nvarchar](1000) NULL,
	[Юридический адрес] [nvarchar](1000) NULL,
	[Код города] [nvarchar](1000) NULL,
	[Телефон] [nvarchar](1000) NULL,
	[EMail] [nvarchar](1000) NULL,
	[ИНН] [nvarchar](100) NULL,
	[КПП] [nvarchar](100) NULL,
	[ОГРН] [nvarchar](100) NULL,
	[Данные проверены] [nvarchar](100) NULL,
	[Срок подключения к ЗКСПД] [datetime] NULL,
	[Срок предоставления информации в ФИС] [datetime] NULL,
	[Головная организация] [nvarchar](max) NULL,
	[Регион Филиала(код)] [int] NULL,
	[Регион учредителя(код)] [int] NULL,
	[Статус регистрации] [nvarchar](100) NULL,
	[Обязательность регистрации] [nvarchar](100) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Temp_ReportOrgsRegistrationBASE_copy_2_old]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Temp_ReportOrgsRegistrationBASE_copy_2_old](
	[Id] [int] NOT NULL,
	[GUID ИСЛОД] [nvarchar](50) NULL,
	[Реквизиты лицензии] [nvarchar](500) NULL,
	[Статус лицензии] [nvarchar](500) NULL,
	[Факс] [nvarchar](500) NULL,
	[Сайт] [nvarchar](500) NULL,
	[Полное наименование] [nvarchar](4000) NULL,
	[Краткое наименование] [nvarchar](2000) NULL,
	[ИД головной] [int] NULL,
	[Должность руководителя] [nvarchar](1000) NULL,
	[Должность руководителя (кому)] [nvarchar](1000) NULL,
	[ФО] [nvarchar](1000) NULL,
	[Код ФО] [int] NULL,
	[Субъект РФ] [nvarchar](1000) NULL,
	[Код субъекта] [nvarchar](1000) NULL,
	[Тип] [nvarchar](1000) NULL,
	[Категория ОО] [nvarchar](1000) NULL,
	[Категория ОО 2] [nvarchar](1000) NULL,
	[Статус ОО] [nvarchar](1000) NULL,
	[Вид] [nvarchar](1000) NULL,
	[ОПФ] [nvarchar](50) NULL,
	[Филиал] [nvarchar](50) NULL,
	[ФИО руководителя] [nvarchar](1000) NULL,
	[И.О. Фамилия Руководителя] [nvarchar](1000) NULL,
	[Фамилия руководителя] [nvarchar](1000) NULL,
	[Имя руководителя] [nvarchar](1000) NULL,
	[Отчество руководителя] [nvarchar](1000) NULL,
	[Учредитель] [nvarchar](1000) NULL,
	[Фактический адрес] [nvarchar](1000) NULL,
	[Юридический адрес] [nvarchar](1000) NULL,
	[Код города] [nvarchar](1000) NULL,
	[Телефон] [nvarchar](1000) NULL,
	[EMail] [nvarchar](1000) NULL,
	[ИНН] [nvarchar](10) NULL,
	[КПП] [nvarchar](9) NULL,
	[ОГРН] [nvarchar](13) NULL,
	[Данные проверены] [nvarchar](10) NULL,
	[Срок подключения к ЗКСПД] [datetime] NULL,
	[Срок предоставления информации в ФИС] [datetime] NULL,
	[Головная организация] [nvarchar](max) NULL,
	[Регион Филиала(код)] [int] NULL,
	[Регион учредителя(код)] [int] NULL,
	[Статус регистрации] [nvarchar](80) NULL,
	[Обязательность регистрации] [nvarchar](20) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TMP_Organization2010]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TMP_Organization2010](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](1000) NOT NULL,
	[INN] [nvarchar](14) NULL,
	[KPP] [nvarchar](13) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tmpAccount]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tmpAccount](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[UpdateId] [uniqueidentifier] NOT NULL,
	[EditorAccountId] [bigint] NULL,
	[EditorIp] [nvarchar](255) NOT NULL,
	[Login] [nvarchar](255) NOT NULL,
	[PasswordHash] [nvarchar](255) NULL,
	[LastName] [nvarchar](255) NULL,
	[FirstName] [nvarchar](255) NULL,
	[PatronymicName] [nvarchar](255) NULL,
	[OrganizationId] [bigint] NULL,
	[IsOrganizationOwner] [bit] NULL,
	[ConfirmYear] [int] NULL,
	[Phone] [nvarchar](255) NULL,
	[Email] [nvarchar](255) NULL,
	[AdminComment] [ntext] NULL,
	[IsActive] [bit] NOT NULL,
	[Status] [nvarchar](255) NOT NULL,
	[IpAddresses] [nvarchar](4000) NULL,
	[HasFixedIp] [bit] NOT NULL,
	[Position] [nvarchar](510) NULL,
	[HasCrocEgeIntegration] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tmpOrganizationRequst]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tmpOrganizationRequst](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[FullName] [nvarchar](1000) NOT NULL,
	[ShortName] [nvarchar](500) NULL,
	[RegionId] [int] NOT NULL,
	[TypeId] [int] NOT NULL,
	[KindId] [int] NULL,
	[INN] [nvarchar](10) NULL,
	[OGRN] [nvarchar](13) NULL,
	[OwnerDepartment] [nvarchar](500) NULL,
	[IsPrivate] [bit] NULL,
	[IsFilial] [bit] NULL,
	[DirectorPosition] [nvarchar](255) NULL,
	[DirectorFullName] [nvarchar](255) NULL,
	[IsAccredited] [bit] NULL,
	[AccreditationSertificate] [nvarchar](255) NULL,
	[LawAddress] [nvarchar](255) NULL,
	[FactAddress] [nvarchar](255) NULL,
	[PhoneCityCode] [nvarchar](10) NULL,
	[Phone] [nvarchar](100) NULL,
	[Fax] [nvarchar](100) NULL,
	[EMail] [nvarchar](100) NULL,
	[Site] [nvarchar](40) NULL,
	[OrganizationId] [int] NULL,
	[StatusID] [int] NOT NULL,
	[IsForActivation] [bit] NOT NULL,
	[RCModelID] [int] NULL,
	[RCDescription] [nchar](400) NULL,
	[ReceptionOnResultsCNE] [bit] NULL,
	[KPP] [nvarchar](9) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserAccountPassword]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserAccountPassword](
	[AccountId] [bigint] NOT NULL,
	[Password] [nvarchar](255) NOT NULL,
	[Id] [bigint] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
 CONSTRAINT [PK171] PRIMARY KEY CLUSTERED 
(
	[AccountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserAgent]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserAgent](
	[login] [nvarchar](255) NOT NULL,
	[userAgent] [nvarchar](1000) NOT NULL,
	[lastLoginDate] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VpnIp]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VpnIp](
	[Ip] [nvarchar](255) NOT NULL,
	[IsActive] [bit] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Delivery] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[Delivery] ADD  DEFAULT ((1)) FOR [Status]
GO
ALTER TABLE [dbo].[DeliveryLog] ADD  DEFAULT (getdate()) FOR [EventDate]
GO
ALTER TABLE [dbo].[DeliveryLog] ADD  DEFAULT (NULL) FOR [ErrorDescription]
GO
ALTER TABLE [dbo].[Group] ADD  CONSTRAINT [DF_Group_Default]  DEFAULT ((0)) FOR [Default]
GO
ALTER TABLE [dbo].[Group] ADD  DEFAULT ((1)) FOR [IsEduOrg]
GO
ALTER TABLE [dbo].[Migrations] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[OperatorLog] ADD  CONSTRAINT [DF_OperatorLog_DTCreate]  DEFAULT (getdate()) FOR [DTCreate]
GO
ALTER TABLE [dbo].[OperatorLog] ADD  CONSTRAINT [DF_OperatorLog_DTLastChange]  DEFAULT (getdate()) FOR [DTLastChange]
GO
ALTER TABLE [dbo].[Organization2010] ADD  CONSTRAINT [DF__Organizat__Creat__25EC738B]  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[Organization2010] ADD  CONSTRAINT [DF__Organizat__Updat__26E097C4]  DEFAULT (getdate()) FOR [UpdateDate]
GO
ALTER TABLE [dbo].[Organization2010] ADD  CONSTRAINT [DF__Organizat__WasIm__3AE79071]  DEFAULT ((0)) FOR [WasImportedAtStart]
GO
ALTER TABLE [dbo].[Organization2010] ADD  CONSTRAINT [org2010_col_fb]  DEFAULT ((0)) FOR [CNFederalBudget]
GO
ALTER TABLE [dbo].[Organization2010] ADD  CONSTRAINT [org2010_col_targ]  DEFAULT ((0)) FOR [CNTargeted]
GO
ALTER TABLE [dbo].[Organization2010] ADD  CONSTRAINT [org2010_col_lb]  DEFAULT ((0)) FOR [CNLocalBudget]
GO
ALTER TABLE [dbo].[Organization2010] ADD  CONSTRAINT [org2010_col_pay]  DEFAULT ((0)) FOR [CNPaying]
GO
ALTER TABLE [dbo].[Organization2010] ADD  CONSTRAINT [org2010_col_ft]  DEFAULT ((0)) FOR [CNFullTime]
GO
ALTER TABLE [dbo].[Organization2010] ADD  CONSTRAINT [org2010_col_even]  DEFAULT ((0)) FOR [CNEvening]
GO
ALTER TABLE [dbo].[Organization2010] ADD  CONSTRAINT [org2010_col_post]  DEFAULT ((0)) FOR [CNPostal]
GO
ALTER TABLE [dbo].[Organization2010] ADD  CONSTRAINT [org2010_col_model]  DEFAULT ((1)) FOR [RCModel]
GO
ALTER TABLE [dbo].[Organization2010] ADD  CONSTRAINT [DF__Organizat__Statu__288E7875]  DEFAULT ((1)) FOR [StatusId]
GO
ALTER TABLE [dbo].[Organization2010] ADD  CONSTRAINT [DF__Organizat__Versi__29829CAE]  DEFAULT ((1)) FOR [Version]
GO
ALTER TABLE [dbo].[Organization2010] ADD  DEFAULT ((1)) FOR [ConnectionSchemeId]
GO
ALTER TABLE [dbo].[Organization2010] ADD  DEFAULT ((1)) FOR [ConnectionStatusId]
GO
ALTER TABLE [dbo].[Organization2010] ADD  DEFAULT ((0)) FOR [OUConfirmation]
GO
ALTER TABLE [dbo].[Organization2010] ADD  DEFAULT ((0)) FOR [UpdatedByUser]
GO
ALTER TABLE [dbo].[Organization2010] ADD  DEFAULT ((0)) FOR [IsReligious]
GO
ALTER TABLE [dbo].[Organization2010] ADD  DEFAULT ((0)) FOR [IsLawEnforcmentOrganization]
GO
ALTER TABLE [dbo].[OrganizationRequest2010] ADD  CONSTRAINT [DF__Organizat__Creat__2C99711A]  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[OrganizationRequest2010] ADD  CONSTRAINT [DF__Organizat__Updat__2D8D9553]  DEFAULT (getdate()) FOR [UpdateDate]
GO
ALTER TABLE [dbo].[OrganizationRequest2010] ADD  CONSTRAINT [DF_OrganizationRequest2010_StatusID]  DEFAULT ((1)) FOR [StatusID]
GO
ALTER TABLE [dbo].[OrganizationRequest2010] ADD  CONSTRAINT [DF_OrganizationRequest2010_IsForActivation]  DEFAULT ((0)) FOR [IsForActivation]
GO
ALTER TABLE [dbo].[OrganizationRequest2010] ADD  DEFAULT ((0)) FOR [OUConfirmation]
GO
ALTER TABLE [dbo].[OrganizationUpdateHistory] ADD  DEFAULT ((1)) FOR [ConnectionSchemeId]
GO
ALTER TABLE [dbo].[OrganizationUpdateHistory] ADD  DEFAULT ((1)) FOR [ConnectionStatusId]
GO
ALTER TABLE [dbo].[OrganizationUpdateHistory] ADD  DEFAULT ((0)) FOR [OUConfirmation]
GO
ALTER TABLE [dbo].[Region] ADD  CONSTRAINT [DF_Region_InOrganizationEtalon]  DEFAULT ((1)) FOR [InOrganizationEtalon]
GO
ALTER TABLE [dbo].[Region] ADD  DEFAULT ((0)) FOR [FederalDistrictId]
GO
ALTER TABLE [dbo].[ReplicationData] ADD  DEFAULT (getdate()) FOR [ReplicationDate]
GO
ALTER TABLE [dbo].[Report] ADD  CONSTRAINT [DF_Report_created]  DEFAULT (getdate()) FOR [created]
GO
ALTER TABLE [dbo].[rpt_ConnectStat] ADD  CONSTRAINT [DF_rpt_ConnectStat_StatDate]  DEFAULT (getdate()) FOR [StatDate]
GO
ALTER TABLE [dbo].[rpt_ConnectStatV2] ADD  CONSTRAINT [DF_rpt_ConnectStatV2_StatDate]  DEFAULT (getdate()) FOR [StatDate]
GO
ALTER TABLE [dbo].[System] ADD  DEFAULT ('') FOR [FullName]
GO
ALTER TABLE [dbo].[System] ADD  DEFAULT ((1)) FOR [AvailableRegistration]
GO
ALTER TABLE [dbo].[AllowedEducationalDirection]  WITH CHECK ADD  CONSTRAINT [FK_AllowedEducationalDirection_EducationalDirection] FOREIGN KEY([EducationalDirectionId])
REFERENCES [dbo].[EducationalDirection] ([Id])
GO
ALTER TABLE [dbo].[AllowedEducationalDirection] CHECK CONSTRAINT [FK_AllowedEducationalDirection_EducationalDirection]
GO
ALTER TABLE [dbo].[AllowedEducationalDirection]  WITH CHECK ADD  CONSTRAINT [FK_AllowedEducationalDirection_EducationalDirectionType] FOREIGN KEY([EducationalDirectionTypeId])
REFERENCES [dbo].[EducationalDirectionType] ([Id])
GO
ALTER TABLE [dbo].[AllowedEducationalDirection] CHECK CONSTRAINT [FK_AllowedEducationalDirection_EducationalDirectionType]
GO
ALTER TABLE [dbo].[AllowedEducationalDirection]  WITH CHECK ADD  CONSTRAINT [FK_AllowedEducationalDirection_LicenseSupplement] FOREIGN KEY([LicenseSupplementId])
REFERENCES [dbo].[LicenseSupplement] ([Id])
GO
ALTER TABLE [dbo].[AllowedEducationalDirection] CHECK CONSTRAINT [FK_AllowedEducationalDirection_LicenseSupplement]
GO
ALTER TABLE [dbo].[Delivery]  WITH CHECK ADD FOREIGN KEY([Status])
REFERENCES [dbo].[DeliveryStatus] ([Id])
GO
ALTER TABLE [dbo].[DeliveryLog]  WITH CHECK ADD FOREIGN KEY([DeliveryId])
REFERENCES [dbo].[Delivery] ([Id])
GO
ALTER TABLE [dbo].[DeliveryRecipients]  WITH CHECK ADD FOREIGN KEY([DeliveryId])
REFERENCES [dbo].[Delivery] ([Id])
GO
ALTER TABLE [dbo].[EducationalDirection]  WITH CHECK ADD  CONSTRAINT [FK_EducationalDirection_EducationalDirectionGroup] FOREIGN KEY([EducationalDirectionGroupId])
REFERENCES [dbo].[EducationalDirectionGroup] ([Id])
GO
ALTER TABLE [dbo].[EducationalDirection] CHECK CONSTRAINT [FK_EducationalDirection_EducationalDirectionGroup]
GO
ALTER TABLE [dbo].[EducationalDirection]  WITH CHECK ADD  CONSTRAINT [FK_EducationalDirection_EducationalDirectionType] FOREIGN KEY([EducationalDirectionTypeId])
REFERENCES [dbo].[EducationalDirectionType] ([Id])
GO
ALTER TABLE [dbo].[EducationalDirection] CHECK CONSTRAINT [FK_EducationalDirection_EducationalDirectionType]
GO
ALTER TABLE [dbo].[Founder]  WITH CHECK ADD  CONSTRAINT [FK_Founder_FactAddressRegionId] FOREIGN KEY([FactAddressRegionId])
REFERENCES [dbo].[Region] ([Id])
GO
ALTER TABLE [dbo].[Founder] CHECK CONSTRAINT [FK_Founder_FactAddressRegionId]
GO
ALTER TABLE [dbo].[Founder]  WITH CHECK ADD  CONSTRAINT [FK_Founder_LawAddressRegionId] FOREIGN KEY([LawAddressRegionId])
REFERENCES [dbo].[Region] ([Id])
GO
ALTER TABLE [dbo].[Founder] CHECK CONSTRAINT [FK_Founder_LawAddressRegionId]
GO
ALTER TABLE [dbo].[Founder]  WITH CHECK ADD  CONSTRAINT [FK_Founder_TypeId] FOREIGN KEY([TypeId])
REFERENCES [dbo].[FounderType] ([Id])
GO
ALTER TABLE [dbo].[Founder] CHECK CONSTRAINT [FK_Founder_TypeId]
GO
ALTER TABLE [dbo].[Group]  WITH NOCHECK ADD  CONSTRAINT [FK_Group_System] FOREIGN KEY([SystemID])
REFERENCES [dbo].[System] ([SystemID])
GO
ALTER TABLE [dbo].[Group] CHECK CONSTRAINT [FK_Group_System]
GO
ALTER TABLE [dbo].[GroupAccount]  WITH NOCHECK ADD  CONSTRAINT [FK_GroupAccount_Account] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Account] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GroupAccount] CHECK CONSTRAINT [FK_GroupAccount_Account]
GO
ALTER TABLE [dbo].[GroupAccount]  WITH NOCHECK ADD  CONSTRAINT [FK_GroupAccount_Group] FOREIGN KEY([GroupId])
REFERENCES [dbo].[Group] ([Id])
GO
ALTER TABLE [dbo].[GroupAccount] CHECK CONSTRAINT [FK_GroupAccount_Group]
GO
ALTER TABLE [dbo].[License]  WITH CHECK ADD  CONSTRAINT [FK_License_OldLicense] FOREIGN KEY([OldLicenseId])
REFERENCES [dbo].[License] ([Id])
GO
ALTER TABLE [dbo].[License] CHECK CONSTRAINT [FK_License_OldLicense]
GO
ALTER TABLE [dbo].[License]  WITH CHECK ADD  CONSTRAINT [FK_License_Organization2010] FOREIGN KEY([OrganizationId])
REFERENCES [dbo].[Organization2010] ([Id])
GO
ALTER TABLE [dbo].[License] CHECK CONSTRAINT [FK_License_Organization2010]
GO
ALTER TABLE [dbo].[License]  WITH CHECK ADD  CONSTRAINT [FK_License_Region] FOREIGN KEY([RegionId])
REFERENCES [dbo].[Region] ([Id])
GO
ALTER TABLE [dbo].[License] CHECK CONSTRAINT [FK_License_Region]
GO
ALTER TABLE [dbo].[LicenseSupplement]  WITH CHECK ADD  CONSTRAINT [FK_LicenseSupplement_License] FOREIGN KEY([LicenseId])
REFERENCES [dbo].[License] ([Id])
GO
ALTER TABLE [dbo].[LicenseSupplement] CHECK CONSTRAINT [FK_LicenseSupplement_License]
GO
ALTER TABLE [dbo].[LicenseSupplement]  WITH CHECK ADD  CONSTRAINT [FK_LicenseSupplement_OldLicenseSupplement] FOREIGN KEY([OldSupplementId])
REFERENCES [dbo].[LicenseSupplement] ([Id])
GO
ALTER TABLE [dbo].[LicenseSupplement] CHECK CONSTRAINT [FK_LicenseSupplement_OldLicenseSupplement]
GO
ALTER TABLE [dbo].[LicenseSupplement]  WITH CHECK ADD  CONSTRAINT [FK_LicenseSupplement_Organization2010] FOREIGN KEY([OrganizationId])
REFERENCES [dbo].[Organization2010] ([Id])
GO
ALTER TABLE [dbo].[LicenseSupplement] CHECK CONSTRAINT [FK_LicenseSupplement_Organization2010]
GO
ALTER TABLE [dbo].[Organization2010]  WITH CHECK ADD  CONSTRAINT [FK__Organizat__KindI__29BD046F] FOREIGN KEY([KindId])
REFERENCES [dbo].[OrganizationKind] ([Id])
GO
ALTER TABLE [dbo].[Organization2010] CHECK CONSTRAINT [FK__Organizat__KindI__29BD046F]
GO
ALTER TABLE [dbo].[Organization2010]  WITH CHECK ADD  CONSTRAINT [FK__Organizat__Regio__27D4BBFD] FOREIGN KEY([RegionId])
REFERENCES [dbo].[Region] ([Id])
GO
ALTER TABLE [dbo].[Organization2010] CHECK CONSTRAINT [FK__Organizat__Regio__27D4BBFD]
GO
ALTER TABLE [dbo].[Organization2010]  WITH CHECK ADD  CONSTRAINT [FK__Organizat__TypeI__28C8E036] FOREIGN KEY([TypeId])
REFERENCES [dbo].[OrganizationType2010] ([Id])
GO
ALTER TABLE [dbo].[Organization2010] CHECK CONSTRAINT [FK__Organizat__TypeI__28C8E036]
GO
ALTER TABLE [dbo].[Organization2010]  WITH CHECK ADD  CONSTRAINT [FK__Organization_OrganizationISId] FOREIGN KEY([OrganizationISId])
REFERENCES [dbo].[OrganizationIS] ([Id])
GO
ALTER TABLE [dbo].[Organization2010] CHECK CONSTRAINT [FK__Organization_OrganizationISId]
GO
ALTER TABLE [dbo].[Organization2010]  WITH CHECK ADD  CONSTRAINT [FK_Organization2010_Organization2010] FOREIGN KEY([NewOrgId])
REFERENCES [dbo].[Organization2010] ([Id])
GO
ALTER TABLE [dbo].[Organization2010] CHECK CONSTRAINT [FK_Organization2010_Organization2010]
GO
ALTER TABLE [dbo].[Organization2010]  WITH CHECK ADD  CONSTRAINT [FK_Organization2010_OrganizationOperatingStatus] FOREIGN KEY([StatusId])
REFERENCES [dbo].[OrganizationOperatingStatus] ([Id])
GO
ALTER TABLE [dbo].[Organization2010] CHECK CONSTRAINT [FK_Organization2010_OrganizationOperatingStatus]
GO
ALTER TABLE [dbo].[Organization2010]  WITH CHECK ADD  CONSTRAINT [fk_OrzanizationDepartmentId] FOREIGN KEY([DepartmentId])
REFERENCES [dbo].[Organization2010] ([Id])
GO
ALTER TABLE [dbo].[Organization2010] CHECK CONSTRAINT [fk_OrzanizationDepartmentId]
GO
ALTER TABLE [dbo].[Organization2010]  WITH NOCHECK ADD  CONSTRAINT [Organization2010_fk] FOREIGN KEY([RCModel])
REFERENCES [dbo].[RecruitmentCampaigns] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Organization2010] CHECK CONSTRAINT [Organization2010_fk]
GO
ALTER TABLE [dbo].[Organization2010]  WITH CHECK ADD  CONSTRAINT [Organization2010_mainid_fk] FOREIGN KEY([MainId])
REFERENCES [dbo].[Organization2010] ([Id])
GO
ALTER TABLE [dbo].[Organization2010] CHECK CONSTRAINT [Organization2010_mainid_fk]
GO
ALTER TABLE [dbo].[OrganizationFounder]  WITH CHECK ADD  CONSTRAINT [FK_OrganizationFounder_FounderId] FOREIGN KEY([FounderId])
REFERENCES [dbo].[Founder] ([Id])
GO
ALTER TABLE [dbo].[OrganizationFounder] CHECK CONSTRAINT [FK_OrganizationFounder_FounderId]
GO
ALTER TABLE [dbo].[OrganizationFounder]  WITH CHECK ADD  CONSTRAINT [FK_OrganizationFounder_OrganizationId] FOREIGN KEY([OrganizationId])
REFERENCES [dbo].[Organization2010] ([Id])
GO
ALTER TABLE [dbo].[OrganizationFounder] CHECK CONSTRAINT [FK_OrganizationFounder_OrganizationId]
GO
ALTER TABLE [dbo].[OrganizationKindEIISMap]  WITH CHECK ADD  CONSTRAINT [FK_OrganizationKindEIISMap_OrganizationKind] FOREIGN KEY([OrganizationKindId])
REFERENCES [dbo].[OrganizationKind] ([Id])
GO
ALTER TABLE [dbo].[OrganizationKindEIISMap] CHECK CONSTRAINT [FK_OrganizationKindEIISMap_OrganizationKind]
GO
ALTER TABLE [dbo].[OrganizationLimitation]  WITH CHECK ADD  CONSTRAINT [FK_OrganizationLimitation_Organization2010] FOREIGN KEY([OrganizationId])
REFERENCES [dbo].[Organization2010] ([Id])
GO
ALTER TABLE [dbo].[OrganizationLimitation] CHECK CONSTRAINT [FK_OrganizationLimitation_Organization2010]
GO
ALTER TABLE [dbo].[OrganizationRequest2010]  WITH CHECK ADD  CONSTRAINT [FK__Organizat__KindI__306A01FE] FOREIGN KEY([KindId])
REFERENCES [dbo].[OrganizationKind] ([Id])
GO
ALTER TABLE [dbo].[OrganizationRequest2010] CHECK CONSTRAINT [FK__Organizat__KindI__306A01FE]
GO
ALTER TABLE [dbo].[OrganizationRequest2010]  WITH NOCHECK ADD  CONSTRAINT [FK__Organizat__Organ__315E2637] FOREIGN KEY([OrganizationId])
REFERENCES [dbo].[Organization2010] ([Id])
GO
ALTER TABLE [dbo].[OrganizationRequest2010] CHECK CONSTRAINT [FK__Organizat__Organ__315E2637]
GO
ALTER TABLE [dbo].[OrganizationRequest2010]  WITH CHECK ADD  CONSTRAINT [FK__Organizat__Regio__2E81B98C] FOREIGN KEY([RegionId])
REFERENCES [dbo].[Region] ([Id])
GO
ALTER TABLE [dbo].[OrganizationRequest2010] CHECK CONSTRAINT [FK__Organizat__Regio__2E81B98C]
GO
ALTER TABLE [dbo].[OrganizationRequest2010]  WITH CHECK ADD  CONSTRAINT [FK__Organizat__TypeI__2F75DDC5] FOREIGN KEY([TypeId])
REFERENCES [dbo].[OrganizationType2010] ([Id])
GO
ALTER TABLE [dbo].[OrganizationRequest2010] CHECK CONSTRAINT [FK__Organizat__TypeI__2F75DDC5]
GO
ALTER TABLE [dbo].[OrganizationRequest2010]  WITH CHECK ADD  CONSTRAINT [FK_OrganizationRequest2010_AccountStatus] FOREIGN KEY([StatusID])
REFERENCES [dbo].[AccountStatus] ([StatusID])
GO
ALTER TABLE [dbo].[OrganizationRequest2010] CHECK CONSTRAINT [FK_OrganizationRequest2010_AccountStatus]
GO
ALTER TABLE [dbo].[OrganizationRequest2010]  WITH NOCHECK ADD  CONSTRAINT [FK_OrganizationRequest2010_RecruitmentCampaigns] FOREIGN KEY([RCModelID])
REFERENCES [dbo].[RecruitmentCampaigns] ([Id])
GO
ALTER TABLE [dbo].[OrganizationRequest2010] NOCHECK CONSTRAINT [FK_OrganizationRequest2010_RecruitmentCampaigns]
GO
ALTER TABLE [dbo].[OrganizationRequestAccount]  WITH CHECK ADD  CONSTRAINT [FK_OrganizationRequestAccount_Account] FOREIGN KEY([AccountID])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[OrganizationRequestAccount] CHECK CONSTRAINT [FK_OrganizationRequestAccount_Account]
GO
ALTER TABLE [dbo].[OrganizationRequestAccount]  WITH NOCHECK ADD  CONSTRAINT [FK_OrganizationRequestAccount_Group] FOREIGN KEY([GroupID])
REFERENCES [dbo].[Group] ([Id])
GO
ALTER TABLE [dbo].[OrganizationRequestAccount] CHECK CONSTRAINT [FK_OrganizationRequestAccount_Group]
GO
ALTER TABLE [dbo].[OrganizationRequestAccount]  WITH NOCHECK ADD  CONSTRAINT [FK_OrganizationRequestAccount_OrganizationRequest2010] FOREIGN KEY([OrgRequestID])
REFERENCES [dbo].[OrganizationRequest2010] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrganizationRequestAccount] CHECK CONSTRAINT [FK_OrganizationRequestAccount_OrganizationRequest2010]
GO
ALTER TABLE [dbo].[OrganizationRequestOperatorLog]  WITH CHECK ADD  CONSTRAINT [FK_OperatorOrganizationRequestLog_Account] FOREIGN KEY([OperatorID])
REFERENCES [dbo].[Account] ([Id])
GO
ALTER TABLE [dbo].[OrganizationRequestOperatorLog] CHECK CONSTRAINT [FK_OperatorOrganizationRequestLog_Account]
GO
ALTER TABLE [dbo].[OrganizationRequestOperatorLog]  WITH CHECK ADD  CONSTRAINT [FK_OperatorOrganizationRequestLog_OperatorOrganizationRequestLog] FOREIGN KEY([OrganizationRequestID])
REFERENCES [dbo].[OrganizationRequestOperatorLog] ([OrganizationRequestID])
GO
ALTER TABLE [dbo].[OrganizationRequestOperatorLog] CHECK CONSTRAINT [FK_OperatorOrganizationRequestLog_OperatorOrganizationRequestLog]
GO
ALTER TABLE [dbo].[OrganizationStatusEIISMap]  WITH CHECK ADD  CONSTRAINT [FK_OrganizationStatusEIISMap_OrganizationOperatingStatus] FOREIGN KEY([OrganizationOperatingStatusId])
REFERENCES [dbo].[OrganizationOperatingStatus] ([Id])
GO
ALTER TABLE [dbo].[OrganizationStatusEIISMap] CHECK CONSTRAINT [FK_OrganizationStatusEIISMap_OrganizationOperatingStatus]
GO
ALTER TABLE [dbo].[OrganizationUpdateHistory]  WITH NOCHECK ADD  CONSTRAINT [FK_OrganizationUpdateHistory_RecruitmentCampaigns] FOREIGN KEY([RCModel])
REFERENCES [dbo].[RecruitmentCampaigns] ([Id])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[OrganizationUpdateHistory] NOCHECK CONSTRAINT [FK_OrganizationUpdateHistory_RecruitmentCampaigns]
GO
ALTER TABLE [dbo].[Region]  WITH CHECK ADD FOREIGN KEY([FederalDistrictId])
REFERENCES [dbo].[FederalDistricts] ([Id])
GO
/****** Object:  StoredProcedure [dbo].[AddNewGroupRole]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AddNewGroupRole]
@password NVARCHAR (255)=null, 
@organizationTypeId INT=null,
@orgRequestID INT=null,
@ListSystemId nvarchar(max),
@accountId int,
@isOlympic bit=0,
@error int=1 output
AS
set nocount on
begin try
	set @error=1
	begin tran	
		declare @tableSystemId table (SystemId int)
		insert @tableSystemId(SystemId)	
		select * from ufn_ut_SplitFromString(@ListSystemId,',') 	
	
		if exists(select * from @tableSystemId where SystemId = 0)
		begin
			delete OrganizationRequestAccount 
			where OrgRequestID = @orgRequestID AND AccountID = @accountId
			
			delete GroupAccount 
			where AccountId = @accountId
			
			delete @tableSystemId where SystemId = 0
		end

		-- установка группы пользователЯ.
		IF exists(select * from @tableSystemId where SystemId = 3)
		BEGIN					
			IF(@isOlympic = 1)
			BEGIN
				-- fbd_^authorizedstaff
				insert dbo.GroupAccount(GroupId, AccountID)
				select	27, @accountId 
				where not exists(select * from GroupAccount WHERE AccountId = @accountId AND GroupId =27)
				union all
				select	3, @accountId 
				where not exists(select * from GroupAccount WHERE AccountId = @accountId AND GroupId =3)			
		
				INSERT INTO OrganizationRequestAccount (OrgRequestID, AccountID, GroupID)
				select @orgRequestID, @accountId, 27 
				where not exists(select * from OrganizationRequestAccount where OrgRequestID = @orgRequestID AND AccountID = @accountId AND GroupId = 27)
				union all
				select @orgRequestID, @accountId, 3 
				where not exists(select * from OrganizationRequestAccount where OrgRequestID = @orgRequestID AND AccountID = @accountId AND GroupId = 3)			
			END
			ELSE
			BEGIN			
				-- fbd_^authorizedstaff
				insert dbo.GroupAccount(GroupId, AccountID)
				select	15, @accountId 
				where not exists(select * from GroupAccount WHERE AccountId = @accountId AND GroupId =15)
				union all
				select	3, @accountId 
				where not exists(select * from GroupAccount WHERE AccountId = @accountId AND GroupId =3)			
		
				INSERT INTO OrganizationRequestAccount (OrgRequestID, AccountID, GroupID)
				select @orgRequestID, @accountId, 15 
				where not exists(select * from OrganizationRequestAccount where OrgRequestID = @orgRequestID AND AccountID = @accountId AND GroupId = 15)
				union all
				select @orgRequestID, @accountId, 3 
				where not exists(select * from OrganizationRequestAccount where OrgRequestID = @orgRequestID AND AccountID = @accountId AND GroupId = 3)			
			END		
			delete @tableSystemId where SystemId = 3					
		END	
		IF exists(select * from @tableSystemId where SystemId = 2)
		BEGIN		

			-- ‚“‡
			IF(@organizationTypeId = 1)
			BEGIN				
				-- fbs_^vuz
				insert dbo.GroupAccount(GroupId, AccountID)
				select	6, @accountId 
				where not exists(select * from GroupAccount WHERE AccountId = @accountId AND GroupId = 6)			
					
				INSERT INTO OrganizationRequestAccount (OrgRequestID, AccountID, GroupID)
				select @orgRequestID, @accountId, 6 
				where not exists(select * from OrganizationRequestAccount where OrgRequestID = @orgRequestID AND AccountID = @accountId AND GroupId = 6)									
			END

			-- ‘‘“‡
			ELSE IF(@organizationTypeId = 2)
			BEGIN						
				-- fbs_^ssuz
				insert dbo.GroupAccount(GroupId, AccountID)
				select	7, @accountId 
				where not exists(select * from GroupAccount WHERE AccountId = @accountId AND GroupId = 7)

				INSERT INTO OrganizationRequestAccount (OrgRequestID, AccountID, GroupID)
				select @orgRequestID, @accountId, 7 
				where not exists(select * from OrganizationRequestAccount where OrgRequestID = @orgRequestID AND AccountID = @accountId AND GroupId = 7)									

			END

			-- ђ–Ћ€
			ELSE IF(@organizationTypeId = 3)
			BEGIN						
				-- fbs_^infoprocessing
				insert dbo.GroupAccount(GroupId, AccountID)
				select	8, @accountId 
				where not exists(select * from GroupAccount WHERE AccountId = @accountId AND GroupId = 8)		

				INSERT INTO OrganizationRequestAccount (OrgRequestID, AccountID, GroupID)
				select @orgRequestID, @accountId, 8 
				where not exists(select * from OrganizationRequestAccount where OrgRequestID = @orgRequestID AND AccountID = @accountId AND GroupId = 8)									

			END

			-- Ћрган управлениЯ образованием
			ELSE IF(@organizationTypeId = 4)
			BEGIN
				-- fbs_^direction
				insert dbo.GroupAccount(GroupId, AccountID)
				select	9, @accountId 
				where not exists(select * from GroupAccount WHERE AccountId = @accountId AND GroupId = 9)	

				INSERT INTO OrganizationRequestAccount (OrgRequestID, AccountID, GroupID)
				select @orgRequestID, @accountId, 9 
				where not exists(select * from OrganizationRequestAccount where OrgRequestID = @orgRequestID AND AccountID = @accountId AND GroupId = 9)									
			END

			-- „ругое
			ELSE IF(@organizationTypeId = 5)
			BEGIN							
				-- fbs_^other
				insert dbo.GroupAccount(GroupId, AccountID)				
				select	11, @accountId 
				where not exists(select * from GroupAccount WHERE AccountId = @accountId AND GroupId = 11)

				INSERT INTO OrganizationRequestAccount (OrgRequestID, AccountID, GroupID)
				select @orgRequestID, @accountId, 11 
				where not exists(select * from OrganizationRequestAccount where OrgRequestID = @orgRequestID AND AccountID = @accountId AND GroupId = 11)									
			END

			-- “чредитель
			ELSE IF(@organizationTypeId = 6)
			BEGIN		
				-- fbs_^founder
				insert dbo.GroupAccount(GroupId, AccountID)
				select	10, @accountId 
				where not exists(select * from GroupAccount WHERE AccountId = @accountId AND GroupId = 10)
				
				INSERT INTO OrganizationRequestAccount (OrgRequestID, AccountID, GroupID)
				select @orgRequestID, @accountId, 10 
				where not exists(select * from OrganizationRequestAccount where OrgRequestID = @orgRequestID AND AccountID = @accountId AND GroupId = 10)									
			END	
		
			delete @tableSystemId where SystemId = 2			
		END	
	
		IF exists(select * from @tableSystemId)
		begin
			
			insert dbo.GroupAccount(GroupId, AccountID)
			select	b.id, @accountId 
			from @tableSystemId a 
				join [Group] b on a.SystemId=b.SystemID and b.[Default]=1
			where not exists(select * 
							 from GroupAccount 
							 WHERE AccountId = @accountId 
									AND GroupId =b.id)

			INSERT INTO OrganizationRequestAccount (OrgRequestID, AccountID, GroupID)
			select @orgRequestID, @accountId,b.id 
			from @tableSystemId a
				join [Group] b on a.SystemId=b.SystemID and b.[Default]=1
			where not exists(select * 
							 from OrganizationRequestAccount 
							 WHERE OrgRequestID = @orgRequestID AND AccountID = @accountId 
									AND GroupId =b.id)
		end
						

		-- временно
		if isnull(@password, '') <> '' 
		begin
			if exists(select * 
					from dbo.UserAccountPassword user_account_password
					where user_account_password.AccountId = @accountId)
			BEGIN
				update user_account_password
					set [Password] = @password
						from dbo.UserAccountPassword user_account_password
				where user_account_password.AccountId = @accountId
			end	
			else		
				insert dbo.UserAccountPassword(AccountId, [Password])
				select @accountId, @password
		END
	
	if @@trancount > 0 
		commit tran 

end try
begin catch
	set @error=-1
	if @@trancount > 0
		rollback tran 
	declare @er nvarchar(4000)
	set @er=error_message()
	raiserror(@er,16,1) 
	return -1
end catch
GO
/****** Object:  StoredProcedure [dbo].[CheckAccountKey]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
-- exec dbo.CheckAccountKey
-- ====================================================
-- Проверить ключа.
-- v.1.0: Created by Fomin Dmitriy 29.08.2008
-- ====================================================
CREATE procedure [dbo].[CheckAccountKey]
	@key nvarchar(255)
	, @ip nvarchar(255)
as
begin
	declare
		@now datetime
		, @entityParams nvarchar(1000)
		, @sourceEntityIds nvarchar(255)
		, @accountId bigint
		, @isValid bit
		, @year int
		, @login nvarchar(255)

	set @now = convert(nvarchar(8), GetDate(), 112)
	set @year = Year(GetDate())

	select top 1
		@accountId = account.Id
		, @login = account.Login
	from dbo.Account account
		inner join dbo.AccountKey account_key
			on account.Id = account_key.AccountId
	where
		account_key.[Key] = @key
		and account_key.IsActive = 1
		and @now between isnull(account_key.DateFrom, @now) and isnull(account_key.DateTo, @now)
		and ((account.Id in (select group_account.AccountId
				from dbo.GroupAccount group_account
					inner join dbo.[Group] [group]
						on [group].Id = group_account.GroupId
				where [group].Code = 'User')
				and dbo.GetUserStatus(account.ConfirmYear, account.Status, @year 
						, account.RegistrationDocument) = 'activated')
			or (account.Id in (select group_account.AccountId
				from dbo.GroupAccount group_account
					inner join dbo.[Group] [group]
						on [group].Id = group_account.GroupId
				where [group].Code = 'Administrator')
				and account.IsActive = 1))

	if not @login is null
		set @isValid = 1
	else
		set @isValid = 0
		
	select
		@key [Key]
		, @login [Login]
		, @isValid IsValid

	set @entityParams = @key + N'|' +
			convert(nvarchar, @isValid)

	set @sourceEntityIds = convert(nvarchar(255), @accountId)

	exec dbo.RegisterEvent 
		@accountId = @accountId
		, @ip = @ip
		, @eventCode = N'USR_KEY_VERIFY'
		, @sourceEntityIds = @sourceEntityIds
		, @eventParams = @entityParams
		, @updateId = null

	return 0
end
GO
/****** Object:  StoredProcedure [dbo].[CheckLastAccountIp]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
-- exec dbo.CheckLastAccountIp
-- ====================================================
-- Процедура проверки последнего адресса пользователя,
-- под которым он авторизовался
-- v.1.0: Created by Sedov Anton 08.07.2008
-- v.1.1: Modified by Fomin Dmitriy 28.08.2008
-- Добавлена регистрация события авторизации.
-- ====================================================
CREATE procedure [dbo].[CheckLastAccountIp] 
	@accountLogin nvarchar(255)
	, @ip nvarchar(255)
as
begin
	declare
		@isLastIp bit
		, @entityParams nvarchar(1000)
		, @sourceEntityIds nvarchar(255)
		, @accountId bigint

	set @isLastIp = null

	select top 1
		@isLastIp = case when auth_event_log.Ip = @ip
				then 1
			else 0
		end
		, @accountId = account.Id
	from 
		dbo.AuthenticationEventLog auth_event_log
			left join dbo.Account account
				on account.Id = auth_event_log.AccountId
	where
		account.[Login] = @accountLogin
			and auth_event_log.IsPasswordValid = 1
			and auth_event_log.IsIpValid = 1
	order by 
		auth_event_log.Date desc
		

	select
		@accountLogin AccountLogin
		, @ip Ip
		, isnull(@isLastIp, 0) IsLastIp						

	set @entityParams = @accountLogin + N'|' +
			@ip + N'||' +
			convert(nvarchar, case 
					when @isLastIp is null then 0 
					else 1 
				end)  + '|' +
			convert(nvarchar, isnull(@isLastIp, 0))

	set @sourceEntityIds = convert(nvarchar(255), @accountId)

	if isnull(@isLastIp, 0) = 1
		exec dbo.RegisterEvent 
			@accountId = @accountId
			, @ip = @ip
			, @eventCode = N'USR_VERIFY'
			, @sourceEntityIds = @sourceEntityIds
			, @eventParams = @entityParams
			, @updateId = null

	return 0
end
GO
/****** Object:  StoredProcedure [dbo].[CheckNewLogin]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.CheckNewLogin

-- =============================================
-- Проверка нового логина.
-- v.1.0: Created by Makarev Andrey 10.04.2008
-- v.1.1: Modofied by Makarev Andrey 14.04.2008
-- Приведение к стандарту
-- =============================================
CREATE procedure [dbo].[CheckNewLogin]
	@login nvarchar(255)
as
begin
	declare @isExists bit

	if exists(select 1
			from 
				dbo.Account account with (nolock, fastfirstrow)
			where
				account.[Login] = @login)
		set @isExists = 1
	else
		set @isExists = 0

	select
		@login [Login]
		, @isExists IsExists

	return 0
end
GO
/****** Object:  StoredProcedure [dbo].[CheckNewUserAccountEmail]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- exec dbo.CheckNewUserAccountEmail
-- =============================================
-- Проверка email нового пользователя 
-- v.1.0: Created by Sedov A.G. 13.05.2008
-- =============================================
CREATE procedure [dbo].[CheckNewUserAccountEmail]
	@email nvarchar(255)
as
begin
	declare 
		@userGroupCode nvarchar(255)
		, @isValid bit
		, @currentYear int
		, @activatedStatus  nvarchar(255)

	set @userGroupCode = 'User'
	set @activatedStatus = 'activated'
	set @currentYear = Year(GetDate())

	if exists(select 1
			from 
				dbo.Account account with (nolock)
					inner join dbo.GroupAccount group_account with (nolock) 
						inner join dbo.[Group] [group] with (nolock)
							on [group].Id = group_account.GroupId
						on group_account.AccountId = account.Id
			where
				[group].Code = @userGroupCode
				and account.Email = @email
				and dbo.GetUserStatus(account.ConfirmYear, account.Status, @currentYear
					, account.RegistrationDocument) = @activatedStatus)
		set @isValid = 0
	else 
		set @isValid = 1

	select
		@email Email
		, @isValid IsValid
  return @isValid
end

GO
/****** Object:  StoredProcedure [dbo].[CheckUserAccountEmail]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CheckUserAccountEmail]
	@login nvarchar(255)
	,@email nvarchar(255)
	,@IsUniq bit out
AS
BEGIN
	-- если e-mail не меняется, то считаем его уникальным
	IF EXISTS(	SELECT 1 FROM dbo.Account WITH (NOLOCK) 
				WHERE Email = @email and [Login]=@login)
		SET @IsUniq = 1
	ELSE 
	IF EXISTS(	SELECT 1 FROM dbo.Account WITH (NOLOCK) 
				WHERE Email = @email and [Status]!='deactivated' and [Login]!=@login)
		SET @IsUniq = 0
	ELSE 
		SET @IsUniq = 1
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteAccount]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Сулиманов А.М.
-- Create date: 2009-05-07
-- Description:	Удаление из БД всего, что касается AccountId (не анализируются связи)
-- =============================================
CREATE PROCEDURE [dbo].[DeleteAccount]
(@AccountID int)
AS
BEGIN
	SET NOCOUNT ON;
	
	DELETE FROM dbo.AccountIp WHERE AccountId=@AccountID
	DELETE FROM dbo.AccountKey WHERE AccountId=@AccountID
	DELETE FROM dbo.AccountLog WHERE AccountId=@AccountID
	DELETE FROM dbo.AccountRoleActivity WHERE AccountId=@AccountID
	DELETE FROM dbo.GroupAccount WHERE AccountId=@AccountID
--	DELETE FROM dbo.Organization WHERE Id IN (SELECT OrganizationId FROM dbo.Account WHERE Id=@AccountID)
	DELETE FROM dbo.UserAccountPassword WHERE AccountId=@AccountID
	DELETE FROM dbo.Account WHERE Id=@AccountID
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteAskedQuestion]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.DeleteAskedQuestion

-- =============================================
-- Удаление вопросов.
-- v.1.0: Created by Makarev Andrey 19.04.2008
-- v.1.1: Modified by Makarev Andrey 23.04.2008
-- Изменение оформления.
-- =============================================
CREATE proc [dbo].[DeleteAskedQuestion]
	@ids nvarchar(255)
	, @editorLogin nvarchar(255)
	, @editorIp nvarchar(255)
as
begin
	declare @idTable table
		(
		[id] bigint
		)

	insert @idTable select dbo.GetInternalId(convert(bigint, [value])) from dbo.GetDelimitedValues(@ids)

	declare 
		@eventCode nvarchar(255)
		, @editorAccountId bigint
		, @updateId	uniqueidentifier
		, @innerIds nvarchar(4000)

	set @innerIds = ''
	
	select 
		@innerIds = @innerIds + convert(nvarchar, id_table.[id]) + N','
	from 
		@idTable id_table
	
	if len(@innerIds) > 0
		set @innerIds = left(@innerIds, len(@innerIds) - 1)

	set @updateId = newid()

	select 
		@editorAccountId = account.[Id]
	from
		dbo.Account account with (nolock, fastfirstrow)
	where 
		account.[Login] = @editorLogin
	
	set @eventCode = N'FAQ_DEL'

	begin tran delete_faq_tran

		delete asked_question_context
		from 
			dbo.AskedQuestionContext asked_question_context
				inner join @idTable idTable
					on asked_question_context.AskedQuestionId = idTable.[id]

		if (@@error <> 0)
			goto undo

		delete asked_question
		from 
			dbo.AskedQuestion asked_question
				inner join @idTable idTable
					on asked_question.[Id] = idTable.[id]

		if (@@error <> 0)
			goto undo

	if @@trancount > 0
		commit tran delete_faq_tran

	exec dbo.RegisterEvent 
		@accountId = @editorAccountId
		, @ip = @editorIp
		, @eventCode = @eventCode
		, @sourceEntityIds = @innerIds
		, @eventParams = null
		, @updateId = @updateId
		
	return 0

	undo:

	rollback tran delete_faq_tran

	return 1

end
GO
/****** Object:  StoredProcedure [dbo].[DeleteDeliveries]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.DeleteNews

-- =============================================
-- Удаление рассылок.
-- v.1.0: Created by Yusupov Kirill 19.04.2010
-- =============================================

CREATE proc [dbo].[DeleteDeliveries]
	@ids nvarchar(255)
	, @editorLogin nvarchar(255)
	, @editorIp nvarchar(255)
as
begin
	declare @idTable table
		(
		[id] bigint
		)

	insert @idTable select [value] from dbo.GetDelimitedValues(@ids)

	declare 
		@eventCode nvarchar(255)
		, @editorAccountId bigint
		, @updateId	uniqueidentifier
		, @innerIds nvarchar(4000)

	

	set @updateId = newid()

	select 
		@editorAccountId = account.[Id]
	from
		dbo.Account account with (nolock, fastfirstrow)
	where 
		account.[Login] = @editorLogin
	
	set @eventCode = N'DLV_DEL'

	--Удалим получателей рассылки
	delete recipient
		from 
			dbo.DeliveryRecipients recipient
				inner join @idTable idTable
					on recipient.DeliveryId = idTable.[id]

	--Удалим лог рассылки
	delete [log]
		from 
			dbo.DeliveryLog [log]
				inner join @idTable idTable
					on [log].DeliveryId = idTable.[id]

	--Удалим саму рассылку
	delete delivery
	from 
		dbo.Delivery delivery
			inner join @idTable idTable
				on delivery.Id = idTable.[id]


	exec dbo.RegisterEvent 
		@accountId = @editorAccountId
		, @ip = @editorIp
		, @eventCode = @eventCode
		, @sourceEntityIds = @ids
		, @eventParams = null
		, @updateId = @updateId
		
	return 0
end




GO
/****** Object:  StoredProcedure [dbo].[DeleteDocument]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.DeleteDocument

-- =============================================
-- Удаление документов.
-- v.1.0: Created by Makarev Andrey 19.04.2008
-- v.1.1: Modified by Makarev Andrey 21.04.2008
-- Вызов dbo.RegisterEvent с внутренними ИД.
-- v.1.2: Modified by Makarev Andrey 23.04.2008
-- Изменение оформления.
-- =============================================
CREATE proc [dbo].[DeleteDocument]
	@ids nvarchar(255)
	, @editorLogin nvarchar(255)
	, @editorIp nvarchar(255)
as
begin
	declare @idTable table
		(
		[id] bigint
		)

	insert @idTable select dbo.GetInternalId(convert(bigint, [value])) from dbo.GetDelimitedValues(@ids)

	declare 
		@eventCode nvarchar(255)
		, @editorAccountId bigint
		, @updateId	uniqueidentifier
		, @innerIds nvarchar(4000)

	set @innerIds = ''
	
	select 
		@innerIds = @innerIds + convert(nvarchar, id_table.[id]) + N','
	from 
		@idTable id_table
	
	if len(@innerIds) > 0
		set @innerIds = left(@innerIds, len(@innerIds) - 1)

	set @updateId = newid()

	select 
		@editorAccountId = account.[Id]
	from
		dbo.Account account with (nolock, fastfirstrow)
	where 
		account.[Login] = @editorLogin
	
	set @eventCode = N'DOC_DEL'

	begin tran delete_document_tran

		delete document_context
		from 
			dbo.DocumentContext document_context
				inner join @idTable idTable
					on document_context.DocumentId = idTable.[id]

		if (@@error <> 0)
			goto undo

		delete [document]
		from 
			dbo.[Document] [document]
				inner join @idTable idTable
					on [document].[Id] = idTable.[id]

		if (@@error <> 0)
			goto undo

	if @@trancount > 0
		commit tran delete_document_tran

	exec dbo.RegisterEvent 
		@accountId = @editorAccountId
		, @ip = @editorIp
		, @eventCode = @eventCode
		, @sourceEntityIds = @innerIds
		, @eventParams = null
		, @updateId = @updateId
		
	return 0

	undo:

	rollback tran delete_document_tran

	return 1

end
GO
/****** Object:  StoredProcedure [dbo].[DeleteNews]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.DeleteNews

-- =============================================
-- Удаление новостей.
-- v.1.0: Created by Makarev Andrey 19.04.2008
-- v.1.1: Modified by Makarev Andrey 21.04.2008
-- Вызов dbo.RegisterEvent с внутренними ИД.
-- v.1.2: Modified by Makarev Andrey 23.04.2008
-- Изменение оформления.
-- =============================================
CREATE proc [dbo].[DeleteNews]
	@ids nvarchar(255)
	, @editorLogin nvarchar(255)
	, @editorIp nvarchar(255)
as
begin
	declare @idTable table
		(
		[id] bigint
		)

	insert @idTable select dbo.GetInternalId(convert(bigint, [value])) from dbo.GetDelimitedValues(@ids)

	declare 
		@eventCode nvarchar(255)
		, @editorAccountId bigint
		, @updateId	uniqueidentifier
		, @innerIds nvarchar(4000)

	set @innerIds = ''
	
	select 
		@innerIds = @innerIds + convert(nvarchar, id_table.[id]) + N','
	from 
		@idTable id_table
	
	if len(@innerIds) > 0
		set @innerIds = left(@innerIds, len(@innerIds) - 1)

	set @updateId = newid()

	select 
		@editorAccountId = account.[Id]
	from
		dbo.Account account with (nolock, fastfirstrow)
	where 
		account.[Login] = @editorLogin
	
	set @eventCode = N'NWS_DEL'

	delete news
	from 
		dbo.News news
			inner join @idTable idTable
				on news.Id = idTable.[id]

	exec dbo.RegisterEvent 
		@accountId = @editorAccountId
		, @ip = @editorIp
		, @eventCode = @eventCode
		, @sourceEntityIds = @innerIds
		, @eventParams = null
		, @updateId = @updateId
		
	return 0

end
GO
/****** Object:  StoredProcedure [dbo].[GetAccount]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- exec dbo.GetAccount

-- =============================================
-- Получение информации об учетной записи 
-- внутреннего пользователя.
-- v.1.0: Created by Fomin Dmitriy 09.04.2008
-- v.1.1: Modified by Makarev Andrey 10.04.2008
-- Добавлено поле Account.IpAddresses
-- v.1.2: Modified by Fomin Dmitriy 15.05.2008
-- Добавлено поле HasFixedIp
-- =============================================
CREATE procedure [dbo].[GetAccount]
	@login nvarchar(255)
as
begin
	select
		account.[Login] [Login]
		, account.LastName LastName 
		, account.FirstName FirstName
		, account.PatronymicName PatronymicName
		, account.Email Email
		, account.Phone Phone
		, account.IsActive IsActive
		, account.IpAddresses IpAddresses
		, account.HasFixedIp HasFixedIp
		, account.PasswordHash PasswordHash
	from dbo.Account account with (nolock, fastfirstrow)
	where account.[Login] = @login

	return 0
end

GO
/****** Object:  StoredProcedure [dbo].[GetAccountAndLogin]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modified 29.01.2011
-- Группа пользователя вычислятся по типу учреждения, 
-- в котором он зарегистрирован
-- =============================================
CREATE PROCEDURE [dbo].[GetAccountAndLogin]
@login NVARCHAR (255)=null, 
@passwordHash NVARCHAR (255)=null, 
@lastName NVARCHAR (255)=null, 
@firstName NVARCHAR (255)=null, 
@patronymicName NVARCHAR (255)=null, 
@phone NVARCHAR (255)=null, 
@email NVARCHAR (255)=null, 
@position NVARCHAR (255)=null,
@ipAddresses NVARCHAR (4000)=null, 
@status NVARCHAR (255)=null, 
@registrationDocument IMAGE=null, 
@registrationDocumentContentType NVARCHAR (225)=null, 
@editorLogin NVARCHAR (255)=null, 
@editorIp NVARCHAR (255)=null, 
@password NVARCHAR (255)=null, 
@hasFixedIp BIT=null, 
@orgRequestID INT=null,
@error int=1 output
as
set nocount on
begin try
	set @error=1
	begin tran	
		if not exists(select  * from OrganizationRequest2010 where id=@orgRequestID)
			raiserror('Такая заявка не существует',16,1)
		
		if @status=''
			set @status=null
		if @login=''
			set @login=null
			
		declare @accountId bigint, @currentYear int, @editorAccountId bigint, @eventCode nvarchar(100), @updateId	uniqueidentifier, @useOnlyDocumentParam BIT
			, @userStatusBefore NVARCHAR(510), @isRegistrationDocumentExistsForUser BIT, @loginNew nvarchar(255)			
				
		declare @newIpAddress table (ip nvarchar(255))			
		declare @oldIpAddress table (ip nvarchar(255))				
					
		select @updateId = newid(), @currentYear = year(getdate())		
		
		select @editorAccountId = account.[Id], 
			   @isRegistrationDocumentExistsForUser = case when account.RegistrationDocument IS NULL then 0 else 1 end
		from dbo.Account account with (nolock, fastfirstrow)
		where account.[Login] = @editorLogin

		if @login is null
		begin 
			select @useOnlyDocumentParam = 1, @eventCode = N'USR_REG'

			select top 1 @loginNew = account.login	 
			from dbo.Account account with (nolock)
			where account.email = @email
			order by account.UpdateDate desc		
				
		end
		else
			select @useOnlyDocumentParam = 0, @eventCode = N'USR_EDIT', @loginNew = @login

		if @loginNew is null -- внесение нового пользователя
		begin			
			-- в качестве логина пользователя используем email
			select @loginNew = @email, @hasFixedIp = isnull(@hasFixedIp, 1)
			
			insert dbo.Account
				(
					CreateDate, UpdateDate, UpdateId, EditorAccountId, EditorIp, [Login], PasswordHash, LastName, FirstName, PatronymicName, OrganizationId,
					IsOrganizationOwner, ConfirmYear, Phone, Position, Email, RegistrationDocument, RegistrationDocumentContentType, AdminComment, IsActive,
					Status, IpAddresses, HasFixedIp
				)
			select GetDate(), GetDate(), @updateId, @editorAccountId, @editorIp, @loginNew, @passwordHash, @lastName, @firstName, @patronymicName, @orgRequestID,
				   1, @currentYear, @phone, @position, @email, @registrationDocument, @registrationDocumentContentType, null, 1, @status, @ipAddresses, 
				   @hasFixedIp

			select @accountId = scope_identity()
			
			if @hasFixedIp = 1
				insert @newIpAddress(ip)
				select ip_addresses.[value]
				from dbo.GetDelimitedValues(@ipAddresses) ip_addresses


			insert dbo.AccountIp(AccountId, Ip)
			select	@accountId, new_ip_address.ip
			from @newIpAddress new_ip_address			
		end
		else
		begin -- update существующего пользователя						
			declare @OrganizationId nvarchar(255)
			select @accountId = account.[Id],
				   @userStatusBefore = account.[Status],
				   @registrationDocument = isnull(@registrationDocument, 
					-- Если документ нельзя просмотривать, то считаем, что его нет.			   
				   case when dbo.CanViewUserAccountRegistrationDocument(account.ConfirmYear) = 0 
						or @useOnlyDocumentParam = 1 
						or isnull(datalength(account.RegistrationDocument),0)=0 
						then null
					else account.RegistrationDocument
				end)
				, @registrationDocumentContentType = case
					when not @registrationDocument is null then @registrationDocumentContentType
					-- Если документ нельзя просмотривать, то считаем, что его нет.
					when dbo.CanViewUserAccountRegistrationDocument(account.ConfirmYear) = 0 
						or @useOnlyDocumentParam = 1 				
						then null
					else account.RegistrationDocumentContentType
				end			
				-- берем последнюю поданную заявку			
				, @hasFixedIp = isnull(@hasFixedIp, account.HasFixedIp)
				, @OrganizationId=OrganizationId
			from dbo.Account account with (nolock, fastfirstrow)		
			where account.[Login] = @loginNew			
						
			declare @st nvarchar(255)		
			set @st = (select dbo.GetUserStatus(@currentYear, isnull(@status,account.[Status]), @currentYear, @registrationDocument) from dbo.Account account with (nolock, fastfirstrow) where account.[Login] = @loginNew)						

			if exists(select * from Account where [status] in ('registration','consideration','revision') and [login]=@loginNew and OrganizationId<>@orgRequestID)
			begin
				declare @dat nvarchar(255),@namest nvarchar(255)
				select @dat=convert(nvarchar(255),UpdateDate,104)+' '+convert(nvarchar(255),UpdateDate,108)  from Account where Id = @accountId	
				
				SELECT @namest=name FROM AccountStatus WHERE Code = @st
				
				raiserror(N'001Пользователь %s находится в статусе ''%s'' в заявке от %s.001', 16, 1, @email,@namest,@dat)						
			end
			
			update account set UpdateDate = GetDate(), UpdateId = @updateId, EditorAccountId = @editorAccountId, PasswordHash=isnull(@passwordHash,PasswordHash), 
							   EditorIp = @editorIp, LastName = @lastName, FirstName = @firstName, PatronymicName = @patronymicName, Phone = @phone,
							   Email = @email, ConfirmYear = @currentYear, [Status] = @st, IpAddresses = @ipAddresses, RegistrationDocument = @registrationDocument,
							   RegistrationDocumentContentType = @registrationDocumentContentType, HasFixedIp = @hasFixedIp, OrganizationId=@orgRequestID,position=@position
			from dbo.Account account with (rowlock)
			where account.[Id] = @accountId			
				
			if @hasFixedIp = 1
				insert @newIpAddress(ip)
				select ip_addresses.[value]
				from dbo.GetDelimitedValues(@ipAddresses) ip_addresses
			
			insert @oldIpAddress(ip)
			select account_ip.ip
			from dbo.AccountIp account_ip with (nolock, fastfirstrow)
			where account_ip.AccountId = @accountId
									
			if exists(select * from @oldIpAddress old_ip_address full join @newIpAddress new_ip_address on old_ip_address.ip = new_ip_address.ip
							where old_ip_address.ip is null	or new_ip_address.ip is null) 
			begin
				delete account_ip
				from dbo.AccountIp account_ip
				where AccountId = @accountId
			
				insert dbo.AccountIp(AccountId, Ip)
				select @accountId, ip
				from @newIpAddress
			end								
		end
		
		-- GVUZ-805 при приложении документа пользователю, если все пользователи имеют скан, то меняем статус заявки на consideration.
		-- определяем, что если это последний пользователь, которому приложили документ, то переводим заявку в статус Consideration
		IF @isRegistrationDocumentExistsForUser = 0 AND @registrationDocument IS NOT NULL	
			and NOT EXISTS(SELECT * FROM Account WHERE OrganizationId = @orgRequestID AND RegistrationDocument IS NULL)
				UPDATE OrganizationRequest2010 SET StatusID = case when StatusID<2 then 2 else StatusID end WHERE Id = @orgRequestID
	
		-- если все пользователи имеют сканы документов и статус на согласовании то меняет статус заявки на согласование
		IF NOT EXISTS(SELECT * FROM Account WHERE OrganizationId = @orgRequestID AND RegistrationDocument IS NULL)
			AND NOT exists(select * from dbo.Account account with (nolock, fastfirstrow) where OrganizationId = @orgRequestID and account.[Status] ='registration')
			UPDATE OrganizationRequest2010 SET StatusID = case when StatusID<2 then 2 else StatusID end WHERE Id = @orgRequestID
			
		exec dbo.RefreshRoleActivity @accountId = @accountId

		exec dbo.RegisterEvent 
			@accountId = @editorAccountId
			, @ip = @editorIp
			, @eventCode = @eventCode
			, @sourceEntityIds = @accountId
			, @eventParams = null
			, @updateId = @updateId

		select @loginNew [login], @accountId [accountId]
	if @@trancount > 0 
		commit tran 

end try
begin catch
	set @error=-1
	if @@trancount > 0
		rollback tran 
	declare @er nvarchar(4000)
	set @er=error_message()
	raiserror(@er,16,1) 
	return -1
end catch

GO
/****** Object:  StoredProcedure [dbo].[GetAccountGroup]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Получение группы пользователя.
-- v.1.0: Created by Fomin Dmitriy 09.04.2008
-- =============================================
CREATE procedure [dbo].[GetAccountGroup]
	@login nvarchar(50)
as
begin
	select --top 1
		account.[Login] [Login]
		, [group].Code GroupCode
		, [group].SystemID SystemID
	from dbo.GroupAccount group_account with (nolock, fastfirstrow)
		inner join dbo.Account account with (nolock, fastfirstrow)
			on group_account.AccountId = account.Id
		inner join dbo.[Group] [group] with (nolock, fastfirstrow)
			on [group].Id = group_account.GroupId
	where account.[Login] = @login
	ORDER BY [group].SystemID, group_account.GroupId		

	return 0
end
GO
/****** Object:  StoredProcedure [dbo].[GetAccountKey]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
-- exec dbo.GetAccountKey
-- ====================================================
-- Получение ключа.
-- v.1.0: Created by Fomin Dmitriy 29.08.2008
-- ====================================================
CREATE procedure [dbo].[GetAccountKey]
	@login nvarchar(255)
	, @key nvarchar(255)
as
begin
	select
		account.Login [Login]
		, account_key.[Key]
		, account_key.DateFrom
		, account_key.DateTo
		, account_key.IsActive
	from dbo.Account account
		inner join dbo.AccountKey account_key
			on account.Id = account_key.AccountId
	where
		account.Login = @login
		and account_key.[Key] = @key

	return 0
end
GO
/****** Object:  StoredProcedure [dbo].[GetAccountLog]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.GetAccountLog

-- =============================================
-- Получить лог учетной записи.
-- v.1.0: Created by Makarev Andrey 14.04.2008
-- v.1.1: Modified by Fomin Dmitriy 17.04.2008
-- Добавлены поля информации о редактировании.
-- v.1.2: Modified by Fomin Dmitriy 15.05.2008
-- Добавлены поля IsVpnEditorIp, HasFixedIp.
-- =============================================
CREATE procedure [dbo].[GetAccountLog]
	@login nvarchar(255)
	, @versionId int
as
begin

	select
		account_log.[Login] [Login]
		, account_log.VersionId VersionId
		, account_log.UpdateDate UpdateDate
		, editor.[Login] EditorLogin
		, editor.LastName EditorLastName
		, editor.FirstName EditorFirstName
		, editor.PatronymicName EditorPatronymicName
		, account_log.EditorIp EditorIp
		, account_log.IsVpnEditorIp IsVpnEditorIp
		, account_log.LastName LastName
		, account_log.FirstName FirstName
		, account_log.PatronymicName PatronymicName
		, account_log.Phone Phone
		, account_log.Email Email
		, account_log.IpAddresses IpAddresses
		, account_log.HasFixedIp HasFixedIp
		, account_log.IsActive IsActive
		, account_log.PasswordHash PasswordHash
	from
		dbo.AccountLog account_log with (nolock, fastfirstrow)
			inner join dbo.Account account with (nolock, fastfirstrow)
				on account_log.AccountId = account.[Id]
			left outer join dbo.Account editor with (nolock, fastfirstrow)
				on editor.Id = account_log.EditorAccountId
	where
		account.[Login] = @login
		and account_log.VersionId = @versionId

	return 0
end
GO
/****** Object:  StoredProcedure [dbo].[GetAccountRole]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.GetAccountRole

-- =============================================
-- Получить роли учетной записи.
-- v.1.0: Created by Makarev Andrey 10.04.2008
-- v.1.1: Modified by Makarev Andrey 14.04.2008
-- Приведение к стандарту.
-- v.1.2: Modified by Makarev Andrey 23.04.2008
-- Изменение офомления.
-- =============================================
CREATE procedure [dbo].[GetAccountRole]
	@login nvarchar(255)
as
begin

	select
		@login [Login]
		, account_role.RoleCode RoleCode
	from
		dbo.AccountRole account_role with (nolock, fastfirstrow)
			inner join dbo.Account account with (nolock, fastfirstrow)
				on account_role.AccountId = account.[Id]
	where
		account.[Login] = @login
		and (account_role.IsActiveCondition is null
			or exists(select 1
				from dbo.AccountRoleActivity activity
				where activity.AccountId = account_role.AccountId
					and activity.RoleId = account_role.RoleId
					and activity.UpdateDate >= account.UpdateDate))

	return 0
end
GO
/****** Object:  StoredProcedure [dbo].[GetAskedQuestion]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.GetAskedQuestion

-- =============================================
-- Получение вопроса.
-- Если @isViewCount = 1, то ViewCount увеличить на 1 для показываемой записи.
-- v.1.0: Created by Makarev Andrey 22.04.2008
-- =============================================
CREATE proc [dbo].[GetAskedQuestion]
	@id bigint
	, @isViewCount bit = 1
as
begin
	declare 
		@internalId bigint
	
	set @internalId = dbo.GetInternalId(@id)

	select
		@id [Id]
		, asked_question.Name [Name]
		, asked_question.Question Question
		, asked_question.Answer Answer
		, asked_question.IsActive IsActive
		, asked_question.ContextCodes ContextCodes
		, asked_question.Popularity Popularity
		, asked_question.ViewCount ViewCount
	from 
		dbo.AskedQuestion asked_question with (nolock, fastfirstrow)
	where
		asked_question.[Id] = @internalId

	if @isViewCount = 1
		update asked_question
		set 
			ViewCount = ViewCount + 1
		from 
			dbo.AskedQuestion asked_question with (rowlock)
		where
			asked_question.[Id] = @internalId

	return 0
end
GO
/****** Object:  StoredProcedure [dbo].[GetDelivery]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Получить информацию о рассылке.
-- v.1.0: Created by Yusupov Kirill 16.04.2010
-- =============================================
CREATE proc [dbo].[GetDelivery]
	@id bigint
as
begin
	select
		@id [Id]
		, delivery.Title Title
		, delivery.[Message] [Message]
		, delivery.[CreateDate] [CreateDate]
		, delivery.DeliveryDate DeliveryDate
		, delivery.TypeCode TypeCode
	from 
		dbo.Delivery delivery with (nolock, fastfirstrow)
	where
		delivery.[Id] = @id

	return 0
end




GO
/****** Object:  StoredProcedure [dbo].[GetDeliveryRecipients]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Получить информацию о рассылке.
-- v.1.0: Created by Yusupov Kirill 16.04.2010
-- =============================================
CREATE proc [dbo].[GetDeliveryRecipients]
	@id bigint
as
begin
	select
		recipients.RecipientCode RecipientCode
	from 
		dbo.DeliveryRecipients recipients with (nolock)
	where
		recipients.[DeliveryId] = @id

	return 0
end








GO
/****** Object:  StoredProcedure [dbo].[GetDocument]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.GetDocument

-- =============================================
-- Получение документа.
-- v.1.0: Created by Makarev Andrey 17.04.2008
-- v.1.1: Modified by Makarev Andrey 19.04.2008
-- Получение данных по внутреннему id.
-- v.1.2: Modified by Fomin Dmitriy 24.04.2008
-- Добавлено поле Alias.
-- v.1.3: Modified by Fomin Dmitriy 24.04.2008
-- Alias переименовано в RelativeUrl.
-- =============================================
CREATE proc [dbo].[GetDocument]
	@id bigint
as
begin

	declare @internalId bigint

	set @internalId = dbo.GetInternalId(@id)

	select
		@id [Id]
		, [document].[Name] [Name]
		, [document].Description Description
		, [document].[Content] [Content]
		, [document].ContentSize ContentSize
		, [document].ContentType ContentType
		, [document].IsActive IsActive
		, [document].ActivateDate ActivateDate
		, [document].ContextCodes ContextCodes
		, [document].RelativeUrl RelativeUrl
	from 
		dbo.[Document] [document] with (nolock, fastfirstrow)
	where
		[document].[Id] = @internalId

	return 0
end
GO
/****** Object:  StoredProcedure [dbo].[GetDocumentByUrl]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.GetDocumentByUrl

-- =============================================
-- Получение документа по относительному Url.
-- v.1.0: Created by Fomin Dmitriy 24.04.2008
-- =============================================
CREATE proc [dbo].[GetDocumentByUrl]
	@relativeUrl nvarchar(255)
as
begin
	select top 1
		dbo.GetExternalId([document].Id) Id
	from 
		dbo.Document [document] with (nolock, fastfirstrow)
	where 
		[document].RelativeUrl = @relativeUrl
		and [document].IsActive = 1

end
GO
/****** Object:  StoredProcedure [dbo].[GetLoginAttemptsInfo]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-------------------------------------------------
--Автор: Сулиманов А.М.
--Дата: 2009-06-02
--Проверка количества попыток залогинится
-------------------------------------------------
CREATE PROCEDURE [dbo].[GetLoginAttemptsInfo]
(	@IP varchar(32), 
	@TimeInterval int
)
AS
	SET NOCOUNT ON

	DECLARE @startDate datetime, @endDate datetime, @eventCode varchar(20)
	SET @endDate=GETDATE()
	SET @startDate=DATEADD(ss,-@TimeInterval,@endDate)
	SET @eventCode='USR_VERIFY'

	SELECT 
			ISNULL(MAX(Date),CAST('1900-01-01' as datetime)) LastLoginDate, 
			@endDate as CheckedDate, 
			--COUNT(*) Attempts, 
			ISNULL(SUM([LoginFailResult]),0) AttemptsFail
	FROM (
		SELECT 	
			--LEFT(EventParams,CHARINDEX('|',EventParams)-1) AS [Login],
			Date,
			CASE SUBSTRING(EventParams,LEN(EventParams)-2,1)
				WHEN '1' THEN 0
				ELSE 1
			END AS [LoginFailResult]
		FROM dbo.EventLog
		WHERE	(Date between @startDate and @endDate) 
				AND EventCode=@eventCode AND IP=@IP
	) T

GO
/****** Object:  StoredProcedure [dbo].[GetNewRequest]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetNewRequest]
@login NVARCHAR (255)=null, 
@organizationRegionId INT=null, 
@organizationFullName NVARCHAR (2000)=null,
@organizationShortName NVARCHAR (2000)=null, 
@organizationINN NVARCHAR (10)=null, 
@organizationOGRN NVARCHAR (13)=null, 
@organizationFounderName NVARCHAR (2000)=null, 
@organizationFactAddress NVARCHAR (500)=null,
@organizationLawAddress NVARCHAR (500)=null, 
@organizationTownName NVARCHAR(250)=null,
@organizationDirName NVARCHAR (255)=null,
@organizationDirPosition NVARCHAR (500)=null, 
@organizationPhoneCode NVARCHAR (255)=null,
@organizationFax NVARCHAR (255)=null,
@organizationIsAccred bit=0,
@organizationIsPrivate bit=0,
@organizationIsFilial bit=0,
@organizationAccredSert nvarchar(255)=null,
@organizationEMail nvarchar(255)=null,
@organizationSite nvarchar(255)=null, 
@organizationPhone NVARCHAR (255)=null,
@status NVARCHAR (255)=null,
@organizationTypeId INT=null,
@organizationKindId INT=null,
@organizationRcModelId INT=null,
@orgRCDescription NVARCHAR(400)=NULL,
@ExistingOrgId INT=NULL,
@orgRequestID INT= null,
@ReceptionOnResultsCNE BIT = null,
@registrationDocument IMAGE=null,
@organizationKPP NVARCHAR (9)=null,
@error int=1 output
as
set nocount on
begin try
	set @error=1
	begin tran
		DECLARE @statusID INT, @st nvarchar(255),@currentYear int	
		
		set @currentYear = year(getdate())
		
		if @status=''
			set @status=null
			
		if @login=''
			set @login=null
			
		if @login is null
			set @st=dbo.GetUserStatus(@currentYear, @status, @currentYear, @registrationDocument)
		else
			set @st = (select dbo.GetUserStatus(@currentYear, isnull(@status,account.[Status]), @currentYear, @registrationDocument) from dbo.Account account with (nolock, fastfirstrow) where account.[Login] = @login)	
	
		if @orgRequestID is null		
		begin						
-- определяем идентификатор статуса			
			SELECT @statusID = StatusID FROM AccountStatus WHERE Code = @st

-- заявка подается не зависимо от того, новый аккаунт создается или обновляется старый
			insert dbo.OrganizationRequest2010
				(
					FullName, ShortName, RegionId,	TypeId,	KindId,	INN, OGRN, OwnerDepartment, IsPrivate, IsFilial, DirectorPosition, DirectorFullName,
					IsAccredited, AccreditationSertificate, LawAddress,	FactAddress, PhoneCityCode,	Phone, Fax,	EMail, Site, OrganizationId, StatusID, RCModelID,
					RCDescription, ReceptionOnResultsCNE,KPP,TownName
				)
			select @organizationFullName, @organizationShortName, @organizationRegionId, @organizationTypeId, @organizationKindId, @organizationINN,
				   @organizationOGRN, @organizationFounderName, @organizationIsPrivate, @organizationIsFilial,	@organizationDirPosition, @organizationDirName,
				   @organizationIsAccred, @organizationAccredSert,	@organizationLawAddress, @organizationFactAddress, @organizationPhoneCode,
				   @organizationPhone,	@organizationFax, @organizationEMail, @organizationSite, @ExistingOrgId, @statusID, @organizationRcModelId,
				   @orgRCDescription, @ReceptionOnResultsCNE,@organizationKPP,@organizationTownName
		 
			set @orgRequestID = scope_identity()	

			select @orgRequestID [orgRequestID], @st [Status]
		end
		else
		begin		
			if exists(select * from dbo.Account account with (nolock, fastfirstrow) where OrganizationId = @orgRequestID and account.[Status] ='registration')
				set @st = 'registration'
			
			SELECT @statusID = StatusID FROM AccountStatus WHERE Code = @st			
					
			update OrganizationRequest2010 set registrationDocument=@registrationDocument,RegionId=@organizationRegionId,FullName=@organizationFullName,
											   ShortName=@organizationShortName,INN=@organizationINN,OGRN=@organizationOGRN,
											   OwnerDepartment=@organizationFounderName,FactAddress=@organizationFactAddress,
											   LawAddress=@organizationLawAddress,DirectorFullName=@organizationDirName,DirectorPosition=@organizationDirPosition,
											   PhoneCityCode=@organizationPhoneCode,Fax=@organizationFax,IsAccredited=@organizationIsAccred,
											   IsPrivate=@organizationIsPrivate,IsFilial=@organizationIsFilial,AccreditationSertificate=@organizationAccredSert,
											   EMail=@organizationEMail,Site=@organizationSite,Phone=@organizationPhone,
											   TypeId=@organizationTypeId,KindId=@organizationKindId,RcModelId=@organizationRcModelId,
											   RCDescription=@orgRCDescription,OrganizationId=@ExistingOrgId,ReceptionOnResultsCNE=@ReceptionOnResultsCNE,
											   StatusID=@StatusID,UpdateDate=getdate(),KPP=@organizationKPP,TownName=@organizationTownName
			where id=@orgRequestID
			
			select id [orgRequestID], Code [Status] from OrganizationRequest2010 a join AccountStatus b on a.StatusID=b.StatusID where id=@orgRequestID
		end
			
	if @@trancount > 0 
		commit tran 

end try
begin catch
	set @error=-1
	if @@trancount > 0
		rollback tran 
	declare @er nvarchar(4000)
	set @er=error_message()
	raiserror(@er,16,1) 
	return -1
end catch
GO
/****** Object:  StoredProcedure [dbo].[GetNews]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.GetNews

-- =============================================
-- Получить детальную информацию о новости.
-- v.1.0: Created by Makarev Andrey 19.04.2008
-- v.1.1: Modified by Makarev Andrey 22.04.2008
-- Выводим наименование новости.
-- =============================================
CREATE proc [dbo].[GetNews]
	@id bigint
as
begin
	declare @internalId bigint

	set @internalId = dbo.GetInternalId(@id)

	select
		@id [Id]
		, news.Date Date
		, news.Description Description
		, news.[Text] [Text]
		, news.IsActive IsActive
		, news.[Name] [Name]
	from 
		dbo.News news with (nolock, fastfirstrow)
	where
		news.[Id] = @internalId

	return 0
end
GO
/****** Object:  StoredProcedure [dbo].[GetOrganizationTypeReport]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetOrganizationTypeReport]
as 
begin
	SELECT 
	OrgType.Id,
	OrgType.[Name] AS TypeName,
	REPLACE(REPLACE(ISNULL(IsPrivate,''),'1','Негосударственный'),'0','Государственный') AS OPF,
	ISNULL(UsersCount ,0) AS UsersCount
	FROM
		(SELECT OrgReq.TypeId AS TypeId,CONVERT(NVARCHAR(5),OrgReq.IsPrivate) AS IsPrivate, COUNT(Acc.Id) AS UsersCount
		FROM dbo.Account Acc
		INNER JOIN dbo.OrganizationRequest2010 OrgReq
		ON Acc.OrganizationId=OrgReq.Id
		GROUP BY OrgReq.TypeId,OrgReq.IsPrivate
		) Rt
	RIGHT JOIN dbo.OrganizationType2010 OrgType
	ON OrgType.Id=TypeId
	UNION
	SELECT 6,'Итого','',COUNT(*) 
	FROM dbo.Account Acc 
	INNER JOIN dbo.OrganizationRequest2010 OrgReq
	ON Acc.OrganizationId=OrgReq.Id
	ORDER BY OrgType.Id
	--	declare
--		@year int
--
--	set @year = Year(GetDate())
--
--	select
--		[type].Name AS TypeName
--		, report.[Count]
--		, 0 IsSummary
--		, 0	IsTotal
--	from (select 
--			[type].Id OrganizationTypeId
--			, count(*) [Count]
--		from dbo.Account account
--			inner join dbo.OrganizationRequest2010 OrgReq
--				inner join dbo.OrganizationType2010 [type]
--					on [type].Id = OrgReq.TypeId
--				on OrgReq.Id = account.OrganizationId
--		where
--			account.Id in (select 
--						group_account.AccountId
--					from dbo.GroupAccount group_account
--						inner join dbo.[Group] [group]
--							on [group].Id = group_account.GroupId
--					where [group].Code = 'User')
--			and dbo.GetUserStatus(account.ConfirmYear, account.Status, @year 
--					, account.RegistrationDocument) = 'activated'
--		group by
--			[type].Id
--		with cube) report
--			left outer join dbo.OrganizationType2010 [type]
--				on [type].Id = report.OrganizationTypeId
--	return 0
end
GO
/****** Object:  StoredProcedure [dbo].[GetRemindAccount]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.GetRemindAccount

-- =============================================
-- Получить забытую учетную запись.
-- v.1.0: Created by Makarev Andrey
-- =============================================
CREATE procedure [dbo].[GetRemindAccount]
	@email nvarchar(255)
	, @editorLogin nvarchar(255)
	, @editorIp nvarchar(255)
as
begin
	
	declare
		@currentYear int
		, @eventCode nvarchar(255)
		, @editorAccountId bigint
		, @login nvarchar(255) 
		, @accountId bigint
		, @accountIds nvarchar(255)
		, @PasswordHash nvarchar(510)

	set @currentYear = year(getdate())
	set @eventCode = N'USR_REMIND'

	select 
		@editorAccountId = account.[Id]
	from
		dbo.Account account with (nolock, fastfirstrow)
	where 
		account.[Login] = @editorLogin

	select top 1
		@login = account.[Login] 
		, @accountId = account.[Id]
		, @PasswordHash = account.PasswordHash
	from 
		dbo.Account account with (nolock, fastfirstrow)
	where 
		account.email = @email
	order by 
		dbo.GetUserStatusOrder(dbo.GetUserStatus(account.ConfirmYear , account.Status
				, @currentYear, account.RegistrationDocument)) desc
		, account.UpdateDate desc

	select 
		@login [Login]
		, @email email
		, @PasswordHash

	set @accountIds = isnull(convert(nvarchar(255), @accountId), '')

	exec dbo.RegisterEvent 
		@accountId = @editorAccountId
		, @ip = @editorIp
		, @eventCode = @eventCode
		, @sourceEntityIds = @accountIds
		, @eventParams = @email
		, @updateId = null

	return 0
end
GO
/****** Object:  StoredProcedure [dbo].[GetSystemNameByLogin]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[GetSystemNameByLogin]
@login nvarchar(255) = null
AS
BEGIN
  SELECT [system].[Name]
  FROM [dbo].[Account] [account]
  JOIN [dbo].[GroupAccount] [groupAccount] ON groupAccount.[AccountId] = account.[Id]
  JOIN [dbo].[Group][group] ON [group].[Id] = [groupAccount].[GroupId] 
  JOIN [dbo].[System] [system] ON  [system].[SystemID] = [group].[SystemID]
  WHERE [account].[Login]=@login
END
GO
/****** Object:  StoredProcedure [dbo].[GetUserAccount]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetUserAccount] @login NVARCHAR(255)
AS 
BEGIN
    DECLARE @currentYear INT ,
        @accountId BIGINT--, @userGroupId int

    SET @currentYear = YEAR(GETDATE())

    SELECT  @accountId = account.[Id]
    FROM    dbo.Account account WITH ( NOLOCK, FASTFIRSTROW )
    WHERE   account.[Login] = @login

    SELECT  account.[Login] ,
            account.LastName ,
            account.FirstName ,
            account.PatronymicName ,
            region.[Id] OrganizationRegionId ,
            region.[Name] OrganizationRegionName ,
            OReq.Id OrganizationId ,
            OReq.FullName OrganizationName ,
            founderOrg.FullName OrganizationFounderName ,
            OReq.LawAddress OrganizationAddress ,
            OReq.DirectorFullName OrganizationChiefName ,
            OReq.Fax OrganizationFax ,
            OReq.Phone OrganizationPhone ,
            OReq.EMail OrganizationEmail,
    OReq.RCModelID RCModelID,
    OReq.RCDescription,
    OReq.KPP,
    OReq.ReceptionOnResultsCNE ReceptionOnResultsCNE,
            OReq.Site OrganizationSite ,
            OReq.ShortName OrganizationShortName ,
            OReq.FactAddress OrganizationFactAddress ,
            OReq.DirectorPosition OrganizationDirectorPosition ,
            OReq.IsPrivate OrganizationIsPrivate ,
            OReq.IsFilial OrganizationIsFilial ,
            OReq.PhoneCityCode OrganizationPhoneCode ,
            OReq.AccreditationSertificate AccreditationSertificate ,
            OReq.INN OrganizationINN ,
            OReq.OGRN OrganizationOGRN ,
            OReq.TownName,
            account.Phone ,
            account.Position ,
            account.Email ,
            account.IpAddresses IpAddresses ,
            account.Status ,
            --CASE WHEN account.CanViewUserAccountRegistrationDocument = 1
            --     THEN account.RegistrationDocument
            --     ELSE NULL
            --END RegistrationDocument ,
            account.RegistrationDocument RegistrationDocument ,
            --CASE WHEN account.CanViewUserAccountRegistrationDocument = 1
            --     THEN account.RegistrationDocumentContentType
            --     ELSE NULL
            --END RegistrationDocumentContentType ,
            account.RegistrationDocumentContentType RegistrationDocumentContentType ,
            account.AdminComment AdminComment ,
            dbo.CanEditUserAccount(account.Status, account.ConfirmYear,
                                   @currentYear) CanEdit ,
            dbo.CanEditUserAccountRegistrationDocument(account.Status) CanEditRegistrationDocument ,
            account.HasFixedIp HasFixedIp,
            OrgType.Id OrgTypeId ,
            OrgType.[Name] OrgTypeName ,
            OrgKind.Id OrgKindId ,
            OrgKind.[Name] OrgKindName ,
            OReq.OrganizationId OReqId ,
            RCModel.ModelName ,
            OReq.RCDescription,
    O.TimeConnectionToSecureNetwork,
    O.TimeEnterInformationInFIS,
    O.IsAgreedTimeConnection,
    O.IsAgreedTimeEnterInformation,
    account.PasswordHash PasswordHash

    FROM    ( SELECT    account.[Login] [Login] ,
                        account.LastName LastName ,
                        account.FirstName FirstName ,
                        account.PatronymicName PatronymicName ,
                        account.OrganizationId OrganizationId ,
                        account.Phone Phone ,
                        account.Position Position ,
                        account.Email Email ,
                        account.ConfirmYear ConfirmYear ,
                        account.RegistrationDocument RegistrationDocument ,
                        account.RegistrationDocumentContentType RegistrationDocumentContentType ,
                        account.AdminComment AdminComment ,
                        account.IpAddresses IpAddresses ,
                        account.HasFixedIp HasFixedIp ,
                        dbo.GetUserStatus(account.ConfirmYear,
                                          account.Status, @currentYear,
                                          account.RegistrationDocument) Status ,
                        dbo.CanViewUserAccountRegistrationDocument(account.ConfirmYear) CanViewUserAccountRegistrationDocument,
                        account.PasswordHash PasswordHash
              FROM      dbo.Account account WITH ( NOLOCK, FASTFIRSTROW )
              WHERE     account.[Id] = @accountId                  
            ) account 
            LEFT OUTER JOIN dbo.OrganizationRequest2010 OReq WITH ( NOLOCK, FASTFIRSTROW )
    JOIN dbo.Organization2010 O ON OReq.OrganizationId = O.Id
            LEFT OUTER JOIN dbo.Region region WITH ( NOLOCK, FASTFIRSTROW ) ON region.[Id] = OReq.RegionId
            LEFT OUTER JOIN dbo.OrganizationType2010 OrgType ON OReq.TypeId = OrgType.Id
            LEFT OUTER JOIN dbo.OrganizationKind OrgKind ON OReq.KindId = OrgKind.Id ON OReq.[Id] = account.OrganizationId
            LEFT OUTER JOIN dbo.RecruitmentCampaigns RCModel ON OReq.RCModelID = RCModel.Id
			LEFT OUTER JOIN dbo.Organization2010 founderOrg ON O.DepartmentId = founderOrg.Id
    RETURN 0
END
GO
/****** Object:  StoredProcedure [dbo].[GetUserAccountByRegionReport]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- exec dbo.GetUserAccountByRegionReport
-- ========================================================
-- Отчет пользователей по регионам.
-- ========================================================
CREATE procedure [dbo].[GetUserAccountByRegionReport]
as 
begin

	;with RegionUserCountCTE as
	(select 
		isnull(r.Code, '') RegionCode
		, isnull(r.Name, 'Не указано') RegionName
		, count(*) [Count]
	from dbo.Account a with(nolock)
		left join dbo.OrganizationRequest2010 OrgReq with(nolock) on OrgReq.Id = a.OrganizationId
		left join dbo.Region r with(nolock) on r.Id = OrgReq.RegionId
		inner join dbo.GroupAccount ga on ga.AccountId=a.id
	where ga.groupid=6 or ga.groupid=7
	group by
		r.Id, r.Code, r.Name
	)
	select *, 0 [IsTotal]
	from RegionUserCountCTE
	union all
	select NULL, NULL, sum(count), 1 from RegionUserCountCTE
	order by [IsTotal], RegionCode

	return 0
end

GO
/****** Object:  StoredProcedure [dbo].[GetUserAccountLog]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- exec dbo.GetUserAccountLog

-- =============================================
-- Получить лог учетной записи пользователя.
-- v.1.0: Created by Makarev Andrey 14.04.2008
-- v.1.1: Modified by Fomin Dmitriy 17.04.2008
-- Добавлены поля информации о редактировании.
-- v.1.2: Modified by Fomin Dmitriy 15.05.2008
-- Добавлены поля IsVpnEditorIp, HasFixedIp.
-- v.1.4: Modified by Sedov Anton 25.06.2008
-- Удалено поле RegistrationDocument
-- вместо него возвращается null
-- v.1.5: Modified by Sedov Anton 10.07.2008
-- В результат добавлено поле
-- EducationInstitutionTypeName
-- =============================================
CREATE procedure [dbo].[GetUserAccountLog]
	@login nvarchar(255)
	, @versionId int
as
begin
	declare
		@accountId bigint

	select @accountId = account.Id
	from dbo.Account account with (nolock, fastfirstrow)
	where account.[Login] = @login

	select
		account_log.[Login] [Login]
		, account_log.VersionId VersionId
		, account_log.UpdateDate UpdateDate
		, editor.[Login] EditorLogin
		, editor.LastName EditorLastName
		, editor.FirstName EditorFirstName
		, editor.PatronymicName EditorPatronymicName
		, account_log.EditorIp EditorIp
		, account_log.IsVpnEditorIp IsVpnEditorIp
		, account_log.LastName LastName
		, account_log.FirstName FirstName
		, account_log.PatronymicName PatronymicName
		, organization_log.RegionId OrganizationRegionId
		, region.[Name] OrganizationRegionName
		, organization_log.[Name] OrganizationName
		, organization_log.FounderName OrganizationFounderName
		, organization_log.Address OrganizationAddress
		, organization_log.ChiefName OrganizationChiefName
		, organization_log.Fax OrganizationFax
		, organization_log.Phone OrganizationPhone
		, account_log.Phone Phone
		, account_log.Email Email
		, account_log.IpAddresses IpAddresses
		, account_log.HasFixedIp HasFixedIp
		, null RegistrationDocument
		, account_log.AdminComment AdminComment
		, account_log.Status Status
		, education_institution_type.[Name] EducationInstitutionTypeName
		, account_log.PasswordHash PasswordHash
	from
		dbo.AccountLog account_log with (nolock, fastfirstrow)
			left outer join dbo.OrganizationLog organization_log with (nolock, fastfirstrow)
				left join dbo.OrganizationType2010 education_institution_type
					on education_institution_type.Id = organization_log.EducationInstitutionTypeId
				on account_log.OrganizationId = organization_log.OrganizationId
				and organization_log.UpdateId = (select top 1 last_linked_account_log.UpdateId
						from dbo.AccountLog last_linked_account_log with (nolock, fastfirstrow)
						where last_linked_account_log.AccountId = @accountId
							and last_linked_account_log.VersionId = (select max(inner_account_log.VersionId)
									from dbo.AccountLog inner_account_log with (nolock)
										inner join dbo.OrganizationLog inner_organization_log with (nolock)
											on inner_account_log.OrganizationId = inner_organization_log.OrganizationId
												and inner_account_log.UpdateId = inner_organization_log.UpdateId
									where inner_account_log.AccountId = @accountId
										and inner_account_log.VersionId <= @versionId))
			left outer join dbo.Region region with (nolock, fastfirstrow)
				on organization_log.RegionId = region.[Id]
			left outer join dbo.Account editor with (nolock, fastfirstrow)
				on editor.Id = account_log.EditorAccountId
	where
		account_log.AccountId = @accountId
		and account_log.VersionId = @versionId

	return 0
end
GO
/****** Object:  StoredProcedure [dbo].[GetYearsInRequests]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE proc [dbo].[GetYearsInRequests]
AS
BEGIN
SELECT DISTINCT year(CreateDate) as [Year] from [dbo].[OrganizationRequest2010]
ORDER BY year(CreateDate) DESC
END
GO
/****** Object:  StoredProcedure [dbo].[Operator_AddUserComment]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--------------------------------------------
-- Добавление комментария пользователю
-- Автор: Сулиманов А.М.
-- Дата: 2009-06-05
--------------------------------------------
CREATE PROCEDURE [dbo].[Operator_AddUserComment]
(@UserLogin nvarchar(255), @Comment varchar(1024))
AS 
	SET NOCOUNT ON

	UPDATE dbo.OperatorLog
	SET Comments=@Comment, DTLastChange=GETDATE()
	WHERE CheckedUserID IN (SELECT ID FROM dbo.Account WHERE [Login]=@UserLogin)
	
GO
/****** Object:  StoredProcedure [dbo].[Operator_GetNewUser]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--------------------------------------------
-- Получение 1-го "не обработанного пользователя"
-- Автор: Сулиманов А.М.
-- Дата: 2009-06-05
--------------------------------------------
CREATE PROCEDURE [dbo].[Operator_GetNewUser]
(	@OperatorLogin nvarchar(255), 
	@UserID int out, 
	@UserLogin nvarchar(255) out
)
AS 
	SET NOCOUNT ON
	DECLARE @OperatorID int
	DECLARE  @T TABLE(CheckedUserID int) 

	SELECT @OperatorID=ID FROM dbo.Account WHERE [Login]=@OperatorLogin

	INSERT INTO dbo.OperatorLog(CheckedUserID,OperatorID) 
		OUTPUT INSERTED.CheckedUserID INTO @T(CheckedUserID)
	SELECT TOP 1 
		A.ID CheckedUserID, 
		@OperatorID OperatorID
	FROM dbo.Account A
	INNER JOIN dbo.Organization2010 O ON A.OrganizationID=O.Id --AND O.EtalonOrgID IS NOT NULL
	INNER JOIN dbo.GroupAccount GA ON A.ID=GA.AccountId AND GA.GroupId=1
	LEFT JOIN dbo.OperatorLog OL ON A.ID=OL.CheckedUserID
	WHERE A.Status='consideration' AND OL.CheckedUserID IS NULL
	ORDER BY A.CreateDate 

	SELECT TOP 1 @UserID=A.ID, @UserLogin=A.[Login]
	FROM dbo.Account A
	WHERE A.ID IN (SELECT CheckedUserID FROM @T)
GO
/****** Object:  StoredProcedure [dbo].[Operator_GetUserInfo]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--------------------------------------------
-- Получение информации о пользователе
-- Автор: Сулиманов А.М.
-- Дата: 2009-06-05
--------------------------------------------
CREATE PROCEDURE [dbo].[Operator_GetUserInfo]
(	@OperatorLogin nvarchar(255), 
	@UserLogin nvarchar(255), 
	@IsMainOperator bit out, 
	@MainOperatorName varchar(255) out, 
	@Comments varchar(1024) out)
AS 
	SET NOCOUNT ON
	DECLARE @UserID int, @OperatorID int
	SELECT @OperatorID=ID FROM dbo.Account WHERE [Login]=@OperatorLogin
	SELECT @UserID=ID FROM dbo.Account WHERE [Login]=@UserLogin

	-- вставляем, если нет связи и другим оператором
	INSERT INTO dbo.OperatorLog(CheckedUserID,OperatorID) 
	SELECT A.ID CheckedUserID, @OperatorID OperatorID
	FROM dbo.Account A
	LEFT JOIN dbo.OperatorLog OL ON A.ID=OL.CheckedUserID
	WHERE A.ID=@UserID AND OL.CheckedUserID IS NULL

	-- данные о текущем пользователе
	SELECT 
		@IsMainOperator=CASE WHEN A.ID=@OperatorID THEN 1 ELSE 0 END,
		@MainOperatorName=A.LastName+' '+A.FirstName +'('+A.Login+')',
		@Comments=OL.Comments
	FROM dbo.OperatorLog OL
	INNER JOIN dbo.Account A ON OL.OperatorID=A.ID
	WHERE CheckedUserID=@UserID

	PRINT @@ROWCOUNT
	
	-- данные об остальных 'моих' пользователях
	SELECT A.Login, A.LastName+' '+FirstName FIO
	FROM dbo.OperatorLog OL
	INNER JOIN dbo.Account A ON OL.CheckedUserID=A.ID
	WHERE OL.OperatorID=@OperatorID AND A.ID<>@UserID 
		AND A.Status='consideration'
		AND (Comments IS NULL OR LEN(RTRIM(Comments))=0)
		

	-- данные об остальных 'моих' пользователях с комментариями
	SELECT A.Login, A.LastName+' '+FirstName FIO
	FROM dbo.OperatorLog OL
	INNER JOIN dbo.Account A ON OL.CheckedUserID=A.ID
	WHERE OL.OperatorID=@OperatorID AND A.ID<>@UserID
		AND A.Status='consideration'
		AND LEN(RTRIM(Comments))>0
GO
/****** Object:  StoredProcedure [dbo].[RefreshRoleActivity]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Обновить активность ролей.
-- v.1.0: Created by Fomin Dmitriy 13.06.2008
-- v.1.1: Modified by Makarev Andrey 23.06.2008
-- Добавлен параметр @accountLogin.
-- =============================================
CREATE proc [dbo].[RefreshRoleActivity]
	@accountId bigint = null
	, @accountLogin nvarchar(255) = null
as
begin
	declare
		@checkAccountId bigint
		, @checkRoleId int
		, @condition nvarchar(max)
		, @commandText nvarchar(max)

	declare @checkingAccount table
		(
		AccountId bigint
		, UpdateDate datetime
		)
	
	if @accountId is null
	begin
		if @accountLogin is null
			insert into @checkingAccount
			select
				account.Id
				, Account.UpdateDate
			from 
				dbo.Account account
			where 
				account.IsActive = 1
		else
			insert into @checkingAccount
			select
				account.Id
				, Account.UpdateDate
			from 
				dbo.Account account
			where 
				account.IsActive = 1
				and account.Login = @accountLogin
	end
	else
	begin
		if @accountLogin is null
			insert into @checkingAccount
			select
				account.Id
				, account.UpdateDate
			from dbo.Account account
			where account.IsActive = 1
				and account.Id = @accountId
		else
			insert into @checkingAccount
			select
				account.Id
				, account.UpdateDate
			from 
				dbo.Account account
			where 
				account.IsActive = 1
				and account.Id = @accountId
				and account.Login = @accountLogin				
	end

	create table #Activity 
		(
		AccountId bigint
		, RoleId int
		)

	declare activity_cursor cursor forward_only for 
	select
		account_role.AccountId
		, account_role.RoleId
		, account_role.IsActiveCondition
	from dbo.AccountRole account_role 
	where not account_role.IsActiveCondition is null
		and account_role.AccountId in (select AccountId from @checkingAccount)

	open activity_cursor
	fetch next from activity_cursor into @checkAccountId, @checkRoleId, @condition
	while @@fetch_status <> -1
	begin
		set @commandText = replace(replace(replace(
			'insert into #Activity 
			select
				activity.AccountId
				, <roleId> RoleId
			from (select 
					account.Id AccountId
					, case
						when <condition> then 1
						else 0
					end IsActive
				from dbo.Account account 
				where account.Id = <accountId>) activity 
			where activity.IsActive = 1 '
			, '<accountId>', @checkAccountId)
			, '<roleId>', @checkRoleId)
			, '<condition>', @condition)

		exec (@commandText)

		fetch next from activity_cursor into @checkAccountId, @checkRoleId, @condition
	end
	close activity_cursor
	deallocate activity_cursor

	if exists(select 1
			from (select
						account_activity.RoleId
						, account_activity.AccountId
					from dbo.AccountRoleActivity account_activity
					where account_activity.AccountId in (select AccountId from @checkingAccount)) account_activity
				full outer join #Activity activity
					on account_activity.RoleId = activity.RoleId
						and account_activity.AccountId = activity.AccountId)
	begin
		begin tran activity
			delete account_activity
			from dbo.AccountRoleActivity account_activity
			where 
				account_activity.AccountId in (select AccountId from @checkingAccount)
				and not exists(select 1
					from #Activity activity
					where account_activity.RoleId = activity.RoleId
						and account_activity.AccountId = activity.AccountId)

			update account_activity
			set UpdateDate = GetDate()
			from dbo.AccountRoleActivity account_activity with(rowlock)
				inner join @checkingAccount account
					on account.AccountId = account_activity.AccountId
				inner join #Activity activity
					on account_activity.RoleId = activity.RoleId
						and account_activity.AccountId = activity.AccountId
			where
				account.UpdateDate > account_activity.UpdateDate

			insert into dbo.AccountRoleActivity
				(
				CreateDate
				, UpdateDate
				, AccountId
				, RoleId
				)
			select
				GetDate()
				, GetDate()
				, activity.AccountId
				, activity.RoleId
			from #Activity activity
			where not exists(select 1
					from dbo.AccountRoleActivity account_activity
					where account_activity.RoleId = activity.RoleId
						and account_activity.AccountId = activity.AccountId)
		if @@trancount > 0 						
			commit tran activity
	end

	drop table #Activity 

	return 0
end
GO
/****** Object:  StoredProcedure [dbo].[RegisterEvent]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.RegisterEvent

-- =============================================
-- Добавление записи в таблицу EventLog.
-- v.1.0: Created by Makarev Andrey 02.04.2008
-- v.1.1: Modified by Makarev Andrey 14.04.2008
-- Добавлено поле UpdateId.
-- v.1.2: Modified by Makarev Andrey 18.04.2008
-- Регистрация событий по нескольким SourceEntityId.
-- v.1.3: Modified by Makarev Andrey 30.04.2008
-- Правильная работа с пустым @sourceEntityIds.
-- =============================================
CREATE proc [dbo].[RegisterEvent]
	@accountId bigint
	, @ip nvarchar(255)
	, @eventCode nvarchar(100)
	, @sourceEntityIds nvarchar(255)
	, @eventParams ntext
	, @updateId uniqueidentifier = null
as
begin
	insert dbo.EventLog
		(
		date
		, accountId
		, ip
		, eventCode
		, sourceEntityId
		, eventParams
		, UpdateId
		)
	select
		GetDate()
		, @accountId
		, @ip
		, @eventCode
		, ids.value
		, @eventParams
		, @updateId
	from
		dbo.GetDelimitedValues(@sourceEntityIds) ids

	return 0
end

GO
/****** Object:  StoredProcedure [dbo].[ReportCnecLoading]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[ReportCnecLoading] 
( @type varchar(10) = null)
as
begin

if(@type is null or @type not in ('month', 'week'))
	set @type = 'month'

select  
  day(n.value('date[1]', 'datetime')) day
, convert(varchar(10), n.value('date[1]', 'datetime'), 104) date
, n.value('cnecNew[1]', 'int') cnecNew
, n.value('cnecUpdated[1]', 'int') cnecUpdated
, n.value('cnecdNew[1]', 'int') cnecdNew
, n.value('cnecdUpdated[1]', 'int') cnecdUpdated
from report rp
cross apply rp.xml.nodes('unit') r(n)
where name = 'CnecLoading' + @type 
and rp.created = (select top 1 created from report where name = 'CnecLoading' + @type order by created desc)	

end
GO
/****** Object:  StoredProcedure [dbo].[ReportUserRegistration]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Получение отчета о регистрации пользователей.
-- =============================================
CREATE procedure [dbo].[ReportUserRegistration]
as
begin

DECLARE @StartDate DATETIME
SET @StartDate= '2010-05-15' -- dateadd(month, -1, getdate())

SELECT 
DAY(UpdateDay) AS [Day]
--, CONVERT(DATETIME,CONVERT(NVARCHAR(50),YEAR(@MonthAgo))+'/'+CONVERT(NVARCHAR(50),MONTH(GETDATE()))+'/'+CONVERT(NVARCHAR(50),UpdateDay)) AS [date]
, CONVERT(DATETIME,CONVERT(NVARCHAR(50),YEAR(UpdateDay))+'-'+CONVERT(NVARCHAR(50),MONTH(UpdateDay))+'-'+CONVERT(NVARCHAR(50),DAY(UpdateDay))) AS [date]
, SUM([Активирован]) AS [activated]
, SUM([На регистрации])  AS [registration]
, SUM([На доработке]) AS [revision]
, SUM([На согласовании]) AS [consideration]
, SUM([Отключен])AS [deactivated]


FROM(
	SELECT 
		CONVERT(NVARCHAR(4),YEAR(F.UpdateDate))+'-'+CONVERT(NVARCHAR(2),MONTH(F.UpdateDate))+'-'+CONVERT(NVARCHAR(2),DAY(F.UpdateDate)) 
	AS UpdateDay,
		case when F.[Status]='activated' then 1 else 0 end AS [Активирован],
		case when F.[Status]='registration' then 1 else 0 end AS [На регистрации],
		case when F.[Status]='revision' then 1 else 0 end AS [На доработке],
		case when F.[Status]='consideration' then 1 else 0 end AS [На согласовании],
		case when F.[Status]='deactivated' then 1 else 0 end AS [Отключен]
		
	FROM dbo.Account A 
	INNER JOIN dbo.GroupAccount G ON A.ID=G.AccountId AND (G.GroupID=6 OR G.GroupID=7)
	INNER JOIN 	(SELECT DISTINCT AccountID,UpdateDate,[Status]
		FROM dbo.AccountLog 
		WHERE (IsStatusChange=1 OR ([Status]='registration' and VersionId=1)) AND UpdateDate >= @StartDate 
	) F ON A.ID=F.AccountID
) T  
GROUP BY UpdateDay
ORDER BY [date]
end





GO
/****** Object:  StoredProcedure [dbo].[SaveUserAgent]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SaveUserAgent]
	@login nvarchar(255)
	,@userAgent nvarchar(1000)

as
begin
	MERGE UserAgent AS TARGET
	USING (SELECT @login AS [login], @userAgent AS userAgent) AS SOURCE
	ON (TARGET.[login] = SOURCE.[login])
	
	WHEN MATCHED THEN 
	UPDATE SET
		TARGET.userAgent = SOURCE.userAgent,
		TARGET.lastLoginDate = getdate()
		
	WHEN NOT MATCHED BY TARGET THEN
	INSERT
		([login], userAgent, lastLoginDate)
	VALUES
		(SOURCE.[login], SOURCE.userAgent, getdate());
		
	return 0
end

GO
/****** Object:  StoredProcedure [dbo].[SearchAccount]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.SearchAccount

-- =============================================
-- Поиск пользователей горячей линии.
-- v.1.0: Created by Makarev Andrey 09.04.2008
-- v.1.1: Modified by Makarev Andrey 11.04.2008
-- Приведение к стандарту.
-- v.1.2: Modified by Makarev Andrey 14.04.2008
-- Приведение к стандарту
-- v.1.3: Modified by Fomin Dmitriy 16.04.2008
-- Лишние хинты.
-- v.1.4: Modified by Sedov Anton 16.05.2008
-- добавлен параметр @email
-- =============================================
CREATE proc [dbo].[SearchAccount]
	@groupCode nvarchar(255)
	, @login nvarchar(255) = null
	, @lastName nvarchar(255) = null
	, @isActive bit = null
	, @email nvarchar(255) = null
	, @startRowIndex int = null
	, @maxRowCount int = null
	, @sortColumn nvarchar(20) = N'login'
	, @sortAsc bit = 1
	, @showCount bit = null
as
begin
	declare 
		@declareCommandText nvarchar(4000)
		, @params nvarchar(4000)
		, @commandText nvarchar(4000)
		, @viewCommandText nvarchar(4000)
		, @innerOrder nvarchar(1000)
		, @outerOrder nvarchar(1000)
		, @resultOrder nvarchar(1000)
		, @innerSelectHeader nvarchar(10)
		, @outerSelectHeader nvarchar(10)
		, @userGroupId int
		, @lastNameFormat nvarchar(255)

	if isnull(@lastName, N'') <> N''
		set @lastNameFormat = N'%' + replace(@lastName, N' ', '%') + N'%'

	select
		@userGroupId = [group].[Id]
	from
		dbo.[Group] [group] with (nolock, fastfirstrow)
	where
		[group].Code = @groupCode

	set @declareCommandText = ''
	set @commandText = ''
	set @viewCommandText = ''

	if isnull(@showCount, 0) = 0
		set @declareCommandText = 
			'declare @search table ' +
			'	( ' +
			'	Login nvarchar(255) ' +
			'	, LastName nvarchar(255) ' +
			'	, FirstName nvarchar(255) ' +
			'	, PatronymicName nvarchar(255) ' +
			'	, IsActive bit ' +
			'   , Email nvarchar(255) ' + 
			'	, Id bigint not null ' +
			'	) ' 

	if isnull(@showCount, 0) = 0
		set @commandText = 
				'select <innerHeader> ' +
				'	account.Login Login ' +
				'	, account.LastName LastName ' +
				'	, account.FirstName FirstName ' +
				'	, account.PatronymicName PatronymicName ' +
				'	, account.IsActive IsActive ' +
				'   , account.Email Email ' +
				'	, account.[Id] ' +
				'from dbo.Account account with (nolock) ' +
				'	inner join dbo.GroupAccount group_account with (nolock) ' +
				'		on account.[Id] = group_account.AccountId ' +
				'where ' +
				'	group_account.GroupId = @userGroupId '
	else
		set @commandText = 
				'select count(*) ' +
				'from dbo.Account account with (nolock, fastfirstrow) ' +
				'	inner join dbo.GroupAccount group_account with (nolock) ' +
				'		on account.[Id] = group_account.AccountId ' +
				'where ' + 
				'	group_account.GroupId = @userGroupId ' 
	
	if not @login is null
		set @commandText = @commandText + ' and account.Login = @login '

	if not @isActive is null
		set @commandText = @commandText + ' and account.IsActive = @isActive '

	if not @lastName is null
		set @commandText = @commandText + ' and account.LastName like @lastNameFormat '
	
	if not @email is null
		set @commandText = @commandText + ' and account.Email = @email '

	if isnull(@showCount, 0) = 0
	begin
		if @sortColumn = 'login'
		begin
			set @innerOrder = 'order by Login <orderDirection> '
			set @outerOrder = 'order by Login <orderDirection> '
			set @resultOrder = 'order by Login <orderDirection> '
		end
		else if @sortColumn = 'IsActive'
		begin
			set @innerOrder = 'order by IsActive <orderDirection> '
			set @outerOrder = 'order by IsActive <orderDirection> '
			set @resultOrder = 'order by IsActive <orderDirection> '
		end
		else if @sortColumn = 'name'
		begin
			set @innerOrder = 'order by LastName <orderDirection>, FirstName <orderDirection>, PatronymicName <orderDirection> '
			set @outerOrder = 'order by LastName <orderDirection>, FirstName <orderDirection>, PatronymicName <orderDirection> '
			set @resultOrder = 'order by LastName <orderDirection>, FirstName <orderDirection>, PatronymicName <orderDirection> '
		end
		else if @sortColumn = 'email'
		begin
			set @innerOrder = 'order by Email <orderDirection>, Id <orderDirection> '
			set @outerOrder = 'order by Email <orderDirection>, Id <orderDirection> '
			set @resultOrder = 'order by Email <orderDirection>, Id <orderDirection> '
		end 
		else if @sortColumn = 'Id'
		begin
			set @innerOrder = 'order by Id <orderDirection> '
			set @outerOrder = 'order by Id <orderDirection> '
			set @resultOrder = 'order by Id <orderDirection> ' 
		end 
		else 
		begin
			set @innerOrder = 'order by Login <orderDirection> '
			set @outerOrder = 'order by Login <orderDirection> '
			set @resultOrder = 'order by Login <orderDirection> '
		end

		if @sortAsc = 1
		begin
			set @innerOrder = replace(@innerOrder, '<orderDirection>', 'asc')
			set @outerOrder = replace(@outerOrder, '<orderDirection>', 'desc')
			set @resultOrder = replace(@resultOrder, '<orderDirection>', 'asc')
		end
		else
		begin
			set @innerOrder = replace(@innerOrder, '<orderDirection>', 'desc')
			set @outerOrder = replace(@outerOrder, '<orderDirection>', 'asc')
			set @resultOrder = replace(@resultOrder, '<orderDirection>', 'desc')
		end

		if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) <> 0
		begin
			set @innerSelectHeader = replace('top <count>', '<count>', @startRowIndex - 1 + @maxRowCount)
			set @outerSelectHeader = replace('top <count>', '<count>', @maxRowCount)
		end
		else if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) = 0
		begin
			set @innerSelectHeader = replace('top <count>', '<count>', @maxRowCount)
			set @outerSelectHeader = replace('top <count>', '<count>', @maxRowCount)
		end
		else if isnull(@maxRowCount, -1) = -1 
		begin
			set @innerSelectHeader = 'top 10000000'
			set @outerSelectHeader = 'top 10000000'
		end

		set @commandText = replace(replace(replace(
				N'insert into @search ' +
				N'select <outerHeader> * ' + 
				N'from (<innerSelect>) as innerSelect ' + @outerOrder
				, N'<innerSelect>', @commandText + @innerOrder)
				, N'<innerHeader>', @innerSelectHeader)
				, N'<outerHeader>', @outerSelectHeader)
	end
	set @commandText = @commandText + 
		'option (keepfixed plan) '

	if isnull(@showCount, 0) = 0
		set @viewCommandText = 
			'select ' +
			'	search.Login ' +
			'	, search.LastName ' +
			'	, search.FirstName ' +
			'	, search.PatronymicName ' +
			'	, search.IsActive ' +
			'	, search.Email ' + 
			'from @search search ' + @resultOrder

	set @commandText = @declareCommandText + @commandText + @viewCommandText 
	
	set @params = 
			'@userGroupId int ' +
			', @login nvarchar(255) ' +
			', @IsActive bit ' + 
			', @lastNameFormat nvarchar(255) ' +
			', @email nvarchar(255) ' 
	
	exec sp_executesql @commandText, @params, 
			@userGroupId
			, @login
			, @IsActive
			, @lastNameFormat
			, @email 

	return 0
end
GO
/****** Object:  StoredProcedure [dbo].[SearchAccountAuthenticationLog]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.SearchAccountAuthenticationLog
-- =============================================
-- Поиск в логе аутентификации записей об аккаунте
-- v.1.0: Created by Sedov A.G. 13.05.2008
-- v.1.1: Modified by Fomin Dmitriy 20.05.2008
-- Добавлено поле IsVpnIp.
-- v.1.2: Modified by Fomin Dmitriy 20.05.2008
-- Анонимное событие на регистрацию проводит 
-- неявную аутентификацию.
-- v.1.3: Modified by Sedov A.G. 22.05.2008
-- Переделана выборка данных, выборка теперь 
-- выполняется из dbo.AuthenticationEventLog 
-- =============================================
CREATE procedure [dbo].[SearchAccountAuthenticationLog]
	@login nvarchar(255)
	, @startRowIndex int = null 
	, @maxRowCount int = null 
	, @showCount bit = null 
as
begin
	declare
		@declareCommandText nvarchar(4000)
		, @params nvarchar(4000)
		, @commandText nvarchar(4000) 
		, @viewCommandText nvarchar(4000)
		, @innerOrder nvarchar(1000)
		, @outerOrder nvarchar(1000)
		, @resultOrder nvarchar(1000)
		, @innerSelectHeader nvarchar(10)
		, @outerSelectHeader nvarchar(10)
		, @sortAsc bit
		, @verifyEventCode nvarchar(255)
		, @registrationEventCode nvarchar(255)

	set @verifyEventCode = 'USR_VERIFY'
	set @registrationEventCode = 'USR_REG'

	set @declareCommandText = ''
	set @commandText = ''
	set @viewCommandText = '' 

	if isnull(@showCount, 0) = 0
		set @declareCommandText = 
			'declare @search table ' +
			'	( ' +
			'	Date datetime ' +
			'	, Ip nvarchar(255) ' +
			'	, IsPasswordValid bit ' +
			'	, IsIpValid bit ' +
			'	) ' 
	
	
	if isnull(@showCount, 0) = 0
		set @commandText = 
			'select <innerHeader> ' +
			'	auth_log.Date Date ' +
			'	, auth_log.Ip Ip ' + 
			'   , auth_log.IsPasswordValid ' + 
			'	, auth_log.IsIpValid ' + 
			'from ' + 
			'	dbo.AuthenticationEventLog auth_log with (nolock) ' + 
			'		inner join dbo.Account account with (nolock, fastfirstrow) ' + 
			'			on account.Id = auth_log.AccountId ' + 
			'where 1 = 1 ' 
	else
		set @commandText = 
			'select count(*) ' +
			'from ' + 
			'	dbo.AuthenticationEventLog auth_log with (nolock) ' +
			'		inner join dbo.Account account with (nolock, fastfirstrow) ' +
			'			on account.Id = auth_log.AccountId ' +
			'where 1 = 1 '

	set @commandText = @commandText +
		' and account.[Login] = @login '

	if isnull(@showCount, 0) = 0
	begin
		begin
			set @innerOrder = 'order by Date <orderDirection> '
			set @outerOrder = 'order by Date <orderDirection> '
			set @resultOrder = 'order by Date <orderDirection> '
		end
		
		if @sortAsc = 1
		begin
			set @innerOrder = replace(@innerOrder, '<orderDirection>', 'asc')
			set @outerOrder = replace(@outerOrder, '<orderDirection>', 'desc')
			set @resultOrder = replace(@resultOrder, '<orderDirection>', 'asc')
		end
		else
		begin
			set @innerOrder = replace(@innerOrder, '<orderDirection>', 'desc')
			set @outerOrder = replace(@outerOrder, '<orderDirection>', 'asc')
			set @resultOrder = replace(@resultOrder, '<orderDirection>', 'desc')
		end

		if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) <> 0
		begin
			set @innerSelectHeader = replace('top <count>', '<count>', @startRowIndex - 1 + @maxRowCount)
			set @outerSelectHeader = replace('top <count>', '<count>', @maxRowCount)
		end
		else if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) = 0
		begin
			set @innerSelectHeader = replace('top <count>', '<count>', @maxRowCount)
			set @outerSelectHeader = replace('top <count>', '<count>', @maxRowCount)
		end
		else if isnull(@maxRowCount, -1) = -1 
		begin
			set @innerSelectHeader = 'top 10000000'
			set @outerSelectHeader = 'top 10000000'
		end

		set @commandText = replace(replace(replace(
				N'insert into @search ' +
				N'select <outerHeader> * ' + 
				N'from (<innerSelect>) as innerSelect ' + @outerOrder
				, N'<innerSelect>', @commandText + @innerOrder)
				, N'<innerHeader>', @innerSelectHeader)
				, N'<outerHeader>', @outerSelectHeader)
	end 	

	set @commandText = @commandText + 
		'option (keepfixed plan) '
	
	if isnull(@showCount, 0) = 0
		set @viewCommandText = 
			'select ' +
			'	search.Date ' +
			'	, search.Ip ' +
			'	, search.IsPasswordValid ' +
			'	, search.IsIpValid ' +
			'	, case ' +
			'		when exists(select 1 ' +
			'				from dbo.VpnIp vpn_ip ' +
			'				where vpn_ip.Ip = search.Ip) then 1 ' +
			'		else 0 ' +
			'	end IsVpnIp ' +
			'from @search search ' + @resultOrder

	set @commandText = @declareCommandText + @commandText + @viewCommandText

	set @params = 
			'@login nvarchar(255) ' +  
			', @verifyEventCode varchar(100) ' +
			', @registrationEventCode varchar(100) '

	exec sp_executesql @commandText, @params
			, @login 
			, @verifyEventCode
			, @registrationEventCode

	return 0
end
GO
/****** Object:  StoredProcedure [dbo].[SearchAccountIS]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Поиск пользователей горячей линии.
-- v.1.0: Created by Makarev Andrey 09.04.2008
-- v.1.1: Modified by Makarev Andrey 11.04.2008
-- Приведение к стандарту.
-- v.1.2: Modified by Makarev Andrey 14.04.2008
-- Приведение к стандарту
-- v.1.3: Modified by Fomin Dmitriy 16.04.2008
-- Лишние хинты.
-- v.1.4: Modified by Sedov Anton 16.05.2008
-- добавлен параметр @email
-- =============================================
CREATE proc [dbo].[SearchAccountIS]
	@isAdmin BIT = 0
	, @login nvarchar(255) = null
	, @lastName nvarchar(255) = null
	, @isActive bit = null
	, @email nvarchar(255) = null
	, @startRowIndex int = null
	, @maxRowCount int = null
	, @sortColumn nvarchar(20) = N'login'
	, @sortAsc bit = 1
	, @showCount bit = null
as
begin
	declare 
		@declareCommandText nvarchar(4000)
		, @params nvarchar(4000)
		, @commandText nvarchar(4000)
		, @viewCommandText nvarchar(4000)
		, @innerOrder nvarchar(1000)
		, @outerOrder nvarchar(1000)
		, @resultOrder nvarchar(1000)
		, @innerSelectHeader nvarchar(10)
		, @outerSelectHeader nvarchar(10)
		, @lastNameFormat nvarchar(255)

	if isnull(@lastName, N'') <> N''
		set @lastNameFormat = N'%' + replace(@lastName, N' ', '%') + N'%'

	DECLARE @suff VARCHAR(1000)
	IF(@isAdmin = 1)
		SET @suff='1=1'
	ELSE
		SET @suff='ga.GroupId IN (2,  5)'



	set @declareCommandText = ''
	set @commandText = ''
	set @viewCommandText = ''

	if isnull(@showCount, 0) = 0
		set @declareCommandText = 
			'declare @search table ' +
			'	( ' +
			'	Login nvarchar(255) ' +
			'	, LastName nvarchar(255) ' +
			'	, FirstName nvarchar(255) ' +
			'	, PatronymicName nvarchar(255) ' +
			'	, IsActive bit ' +
			'   , Email nvarchar(255) ' + 
			'	, Id bigint not null ' +
			'	, GroupName nvarchar(255) ' +			
			'	) ' 

	if isnull(@showCount, 0) = 0
		set @commandText = 
				'select <innerHeader> ' +
				'	account.Login Login ' +
				'	, account.LastName LastName ' +
				'	, account.FirstName FirstName ' +
				'	, account.PatronymicName PatronymicName ' +
				'	, account.IsActive IsActive ' +
				'   , account.Email Email ' +
				'	, account.[Id] ' +
				'	, REPLACE((SELECT gr.Name + '', '' FROM GroupAccount ga INNER JOIN [Group] gr  ON gr.ID=ga.GroupId
							WHERE ga.AccountId=account.Id AND gr.IsUserIS=1 AND ' + @suff + '
				 	        ORDER BY  gr.Name
							FOR XML PATH('''')
							) + ''~'', '', ~'', '''') GroupName ' +
				'from dbo.Account account with (nolock) ' +
				'where (SELECT COUNT(*) FROM GroupAccount ga INNER JOIN [Group] g ON g.ID=ga.GroupId WHERE ga.AccountId=account.Id AND g.IsUserIS=1 AND ' + @suff +') > 0 '
	else
		set @commandText = 
				'select count(*) ' +
				'from dbo.Account account with (nolock, fastfirstrow) ' +
				'	inner join dbo.GroupAccount group_account with (nolock) ' +
				'		on account.[Id] = group_account.AccountId ' +
				'where (SELECT COUNT(*) FROM GroupAccount ga INNER JOIN [Group] g ON g.ID=ga.GroupId WHERE ga.AccountId=account.Id AND g.IsUserIS=1 AND ' + @suff +') > 0 '
	
	if not @login is null
		set @commandText = @commandText + ' and account.Login = @login '

	if not @isActive is null
		set @commandText = @commandText + ' and account.IsActive = @isActive '

	if not @lastName is null
		set @commandText = @commandText + ' and account.LastName like @lastNameFormat '
	
	if not @email is null
		set @commandText = @commandText + ' and account.Email = @email '

	if isnull(@showCount, 0) = 0
	begin
		if @sortColumn = 'login'
		begin
			set @innerOrder = 'order by Login <orderDirection> '
			set @outerOrder = 'order by Login <orderDirection> '
			set @resultOrder = 'order by Login <orderDirection> '
		end
		else if @sortColumn = 'IsActive'
		begin
			set @innerOrder = 'order by IsActive <orderDirection> '
			set @outerOrder = 'order by IsActive <orderDirection> '
			set @resultOrder = 'order by IsActive <orderDirection> '
		END
		else if @sortColumn = 'GroupName'
		begin
			set @innerOrder = 'order by GroupName <orderDirection> '
			set @outerOrder = 'order by GroupName <orderDirection> '
			set @resultOrder = 'order by GroupName <orderDirection> '
		end
		else if @sortColumn = 'name'
		begin
			set @innerOrder = 'order by LastName <orderDirection>, FirstName <orderDirection>, PatronymicName <orderDirection> '
			set @outerOrder = 'order by LastName <orderDirection>, FirstName <orderDirection>, PatronymicName <orderDirection> '
			set @resultOrder = 'order by LastName <orderDirection>, FirstName <orderDirection>, PatronymicName <orderDirection> '
		end
		else if @sortColumn = 'email'
		begin
			set @innerOrder = 'order by Email <orderDirection>, Id <orderDirection> '
			set @outerOrder = 'order by Email <orderDirection>, Id <orderDirection> '
			set @resultOrder = 'order by Email <orderDirection>, Id <orderDirection> '
		end 
		else if @sortColumn = 'Id'
		begin
			set @innerOrder = 'order by Id <orderDirection> '
			set @outerOrder = 'order by Id <orderDirection> '
			set @resultOrder = 'order by Id <orderDirection> ' 
		end 
		else 
		begin
			set @innerOrder = 'order by Login <orderDirection> '
			set @outerOrder = 'order by Login <orderDirection> '
			set @resultOrder = 'order by Login <orderDirection> '
		end

		if @sortAsc = 1
		begin
			set @innerOrder = replace(@innerOrder, '<orderDirection>', 'asc')
			set @outerOrder = replace(@outerOrder, '<orderDirection>', 'desc')
			set @resultOrder = replace(@resultOrder, '<orderDirection>', 'asc')
		end
		else
		begin
			set @innerOrder = replace(@innerOrder, '<orderDirection>', 'desc')
			set @outerOrder = replace(@outerOrder, '<orderDirection>', 'asc')
			set @resultOrder = replace(@resultOrder, '<orderDirection>', 'desc')
		end

		if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) <> 0
		begin
			set @innerSelectHeader = replace('top <count>', '<count>', @startRowIndex - 1 + @maxRowCount)
			set @outerSelectHeader = replace('top <count>', '<count>', @maxRowCount)
		end
		else if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) = 0
		begin
			set @innerSelectHeader = replace('top <count>', '<count>', @maxRowCount)
			set @outerSelectHeader = replace('top <count>', '<count>', @maxRowCount)
		end
		else if isnull(@maxRowCount, -1) = -1 
		begin
			set @innerSelectHeader = 'top 10000000'
			set @outerSelectHeader = 'top 10000000'
		end

		set @commandText = replace(replace(replace(
				N'insert into @search ' +
				N'select <outerHeader> * ' + 
				N'from (<innerSelect>) as innerSelect ' + @outerOrder
				, N'<innerSelect>', @commandText + @innerOrder)
				, N'<innerHeader>', @innerSelectHeader)
				, N'<outerHeader>', @outerSelectHeader)
	end
	set @commandText = @commandText + 
		'option (keepfixed plan) '

	if isnull(@showCount, 0) = 0
		set @viewCommandText = 
			'select ' +
			'	search.Login ' +
			'	, search.LastName ' +
			'	, search.FirstName ' +
			'	, search.PatronymicName ' +
			'	, search.IsActive ' +
			'	, search.Email ' + 
			'	, search.GroupName ' + 
			'from @search search ' + @resultOrder

	set @commandText = @declareCommandText + @commandText + @viewCommandText 
	
	set @params = 
			' @login nvarchar(255) ' +
			', @IsActive bit ' + 
			', @lastNameFormat nvarchar(255) ' +
			', @email nvarchar(255) ' 
	
	exec sp_executesql @commandText, @params
			, @login
			, @IsActive
			, @lastNameFormat
			, @email 

	return 0
end
GO
/****** Object:  StoredProcedure [dbo].[SearchAccountKey]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
-- exec dbo.SearchAccountKey
-- ====================================================
-- Поиск ключей.
-- v.1.0: Created by Fomin Dmitriy 28.08.2008
-- ====================================================
CREATE procedure [dbo].[SearchAccountKey]
	@login nvarchar(255)
as
begin
	declare
		@now datetime

	set @now = convert(nvarchar(8), GetDate(), 112)

	select
		account_key.[Key]
		, account_key.DateFrom
		, account_key.DateTo
		, account_key.IsActive
	from dbo.Account account
		inner join dbo.AccountKey account_key
			on account.Id = account_key.AccountId
	where
		account.Login = @login
	order by
		case
			when @now < account_key.DateFrom then 2
			when @now > account_key.DateTo then 1
			else 0
		end asc

	return 0
end
GO
/****** Object:  StoredProcedure [dbo].[SearchAccountLog]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.SearchAccountLog

-- =============================================
-- Список историй изменений учетной записи.
-- v.1.0: Created by Makarev Andrey 11.04.2008
-- v.1.1: Modified by Fomin Dmitriy 11.04.2008
-- Добавлено поле UpdateDate.
-- v.1.2: Modified by Makarev Andrey 14.04.2008
-- Приведение к стандарту
-- v.1.3: Modified by Fomin Dmitriy 16.04.2008
-- Лишние хинты.
-- v.1.4: Modified by Fomin Dmitriy 17.04.2008
-- Добавлены поля информации о редактировании.
-- v.1.5: Modified by Fomin Dmitriy 15.05.2008
-- Добавлено поле IsVpnEditorIp.
-- =============================================
CREATE proc [dbo].[SearchAccountLog]
	@login nvarchar(255)
	, @startRowIndex int = null
	, @maxRowCount int = null
	, @showCount bit = null
as
begin
	declare 
		@declareCommandText nvarchar(4000)
		, @commandText nvarchar(4000)
		, @viewCommandText nvarchar(4000)
		, @innerOrder nvarchar(1000)
		, @outerOrder nvarchar(1000)
		, @resultOrder nvarchar(1000)
		, @innerSelectHeader nvarchar(10)
		, @outerSelectHeader nvarchar(10)
		, @sortColumn nvarchar(20)
		, @sortAsc bit
		, @accountId bigint

	select 
		@accountId = account.[Id]
	from 
		dbo.Account account with (nolock, fastfirstrow)
	where
		account.[Login] = @login

	select 
		@sortColumn  = N'VersionId'
		, @sortAsc = 0

	set @declareCommandText = ''
	set @commandText = ''
	set @viewCommandText = ''

	if isnull(@showCount, 0) = 0
		set @declareCommandText = 
			'declare @search table ' +
			'	( ' +
			'	Login nvarchar(255) ' +
			'	, VersionId int ' +
			'	, UpdateDate datetime ' +
			'	, EditorLogin nvarchar(50) ' +
			'	, EditorLastName nvarchar(255) ' +
			'	, EditorFirstName nvarchar(255) ' +
			'	, EditorPatronymicName nvarchar(255) ' +
			'	, EditorIp nvarchar(255) ' +
			'	, IsVpnEditorIp bit ' +
			'	, IsActiveChange bit ' +
			'	, IsStatusChange bit ' +
			'	, IsEdit bit ' +
			'	, IsPasswordChange bit ' +
			'	) ' 

	if isnull(@showCount, 0) = 0
		set @commandText = 
				'select <innerHeader> ' +
				'	account_log.Login Login ' +
				'	, account_log.VersionId VersionId' +
				'	, account_log.UpdateDate UpdateDate ' +
				'	, editor.Login EditorLogin ' +
				'	, editor.LastName EditorLastName ' +
				'	, editor.FirstName EditorFirstName ' +
				'	, editor.PatronymicName EditorPatronymicName ' +
				'	, account_log.EditorIp EditorIp ' +
				'	, account_log.IsVpnEditorIp IsVpnEditorIp ' +
				'	, account_log.IsActiveChange IsActiveChange ' +
				'	, account_log.IsStatusChange IsStatusChange ' +
				'	, account_log.IsEdit IsEdit ' +
				'	, account_log.IsPasswordChange IsPasswordChange ' +
				'from dbo.AccountLog account_log with (nolock) ' +
				'	left outer join dbo.Account editor with (nolock) ' +
				'		on editor.Id = account_log.EditorAccountId ' +
				'where account_log.AccountId = @accountId ' 
	else
		set @commandText = 
				'select count(*) ' +
				'from dbo.AccountLog account_log with (nolock) ' +
				'where account_log.AccountId = @accountId ' 
	
	if isnull(@showCount, 0) = 0
	begin
		begin
			set @innerOrder = 'order by VersionId <orderDirection> '
			set @outerOrder = 'order by VersionId <orderDirection> '
			set @resultOrder = 'order by VersionId <orderDirection> '
		end

		if @sortAsc = 1
		begin
			set @innerOrder = replace(@innerOrder, '<orderDirection>', 'asc')
			set @outerOrder = replace(@outerOrder, '<orderDirection>', 'desc')
			set @resultOrder = replace(@resultOrder, '<orderDirection>', 'asc')
		end
		else
		begin
			set @innerOrder = replace(@innerOrder, '<orderDirection>', 'desc')
			set @outerOrder = replace(@outerOrder, '<orderDirection>', 'asc')
			set @resultOrder = replace(@resultOrder, '<orderDirection>', 'desc')
		end

		if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) <> 0
		begin
			set @innerSelectHeader = replace('top <count>', '<count>', @startRowIndex - 1 + @maxRowCount)
			set @outerSelectHeader = replace('top <count>', '<count>', @maxRowCount)
		end
		else if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) = 0
		begin
			set @innerSelectHeader = replace('top <count>', '<count>', @maxRowCount)
			set @outerSelectHeader = replace('top <count>', '<count>', @maxRowCount)
		end
		else if isnull(@maxRowCount, -1) = -1 
		begin
			set @innerSelectHeader = 'top 10000000'
			set @outerSelectHeader = 'top 10000000'
		end

		set @commandText = replace(replace(replace(
				N'insert into @search ' +
				N'select <outerHeader> * ' + 
				N'from (<innerSelect>) as innerSelect ' + @outerOrder
				, N'<innerSelect>', @commandText + @innerOrder)
				, N'<innerHeader>', @innerSelectHeader)
				, N'<outerHeader>', @outerSelectHeader)
	end
	set @commandText = @commandText + 
		'option (keepfixed plan) '

	if isnull(@showCount, 0) = 0
		set @viewCommandText = 
			'select ' +
			'	search.Login ' +
			'	, search.VersionId ' +
			'	, search.UpdateDate ' +
			'	, search.EditorLogin ' +
			'	, search.EditorLastName ' +
			'	, search.EditorFirstName ' +
			'	, search.EditorPatronymicName ' +
			'	, search.EditorIp ' +
			'	, search.IsVpnEditorIp ' +
			'	, search.IsActiveChange ' +
			'	, search.IsStatusChange ' +
			'	, search.IsEdit ' +
			'	, search.IsPasswordChange ' +
			'from @search search ' + @resultOrder

	set @commandText = @declareCommandText + @commandText + @viewCommandText 

	exec sp_executesql @commandText, N'@accountId bigint', @accountId

	return 0
end
GO
/****** Object:  StoredProcedure [dbo].[SearchAccountOU]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Поиск пользователей горячей линии.
-- v.1.0: Created by Makarev Andrey 09.04.2008
-- v.1.1: Modified by Makarev Andrey 11.04.2008
-- Приведение к стандарту.
-- v.1.2: Modified by Makarev Andrey 14.04.2008
-- Приведение к стандарту
-- v.1.3: Modified by Fomin Dmitriy 16.04.2008
-- Лишние хинты.
-- v.1.4: Modified by Sedov Anton 16.05.2008
-- добавлен параметр @email
-- =============================================
CREATE proc [dbo].[SearchAccountOU]
	@orgID int = -1
	, @login nvarchar(255) = null
	, @lastName nvarchar(255) = null
	, @isActive bit = null
	, @email nvarchar(255) = null
	, @startRowIndex int = null
	, @maxRowCount int = null
	, @sortColumn nvarchar(20) = N'login'
	, @sortAsc bit = 1
	, @showCount bit = null
as
begin
	declare 
		@declareCommandText nvarchar(4000)
		, @params nvarchar(4000)
		, @commandText nvarchar(4000)
		, @viewCommandText nvarchar(4000)
		, @innerOrder nvarchar(1000)
		, @outerOrder nvarchar(1000)
		, @resultOrder nvarchar(1000)
		, @innerSelectHeader nvarchar(10)
		, @outerSelectHeader nvarchar(10)
		, @lastNameFormat nvarchar(255)

	if isnull(@lastName, N'') <> N''
		set @lastNameFormat = N'%' + replace(@lastName, N' ', '%') + N'%'

	DECLARE @suff VARCHAR(1000)

	set @declareCommandText = ''
	set @commandText = ''
	set @viewCommandText = ''

	if isnull(@showCount, 0) = 0
		set @declareCommandText = 
			'declare @search table ' +
			'	( ' +
			'	Login nvarchar(255) ' +
			'	, LastName nvarchar(255) ' +
			'	, FirstName nvarchar(255) ' +
			'	, PatronymicName nvarchar(255) ' +
			'	, IsActive bit ' +
			'   , Email nvarchar(255) ' + 
			'	, Id bigint not null ' +
			'	) ' 

	if isnull(@showCount, 0) = 0
		set @commandText = 
				'select <innerHeader> ' +
				'	account.Login Login ' +
				'	, account.LastName LastName ' +
				'	, account.FirstName FirstName ' +
				'	, account.PatronymicName PatronymicName ' +
				'	, account.IsActive IsActive ' +
				'   , account.Email Email ' +
				'	, account.[Id] ' +
				'from dbo.Account account with (nolock) ' +
				'where ' +
				'	account.OrganizationID=@orgID '
	else
		set @commandText = 
				'select count(*) ' +
				'from dbo.Account account with (nolock, fastfirstrow) ' +
				'where ' + 
				'	account.OrganizationID=@orgID '
	
	if not @login is null
		set @commandText = @commandText + ' and account.Login = @login '

	if not @isActive is null
		set @commandText = @commandText + ' and account.IsActive = @isActive '

	if not @lastName is null
		set @commandText = @commandText + ' and account.LastName like @lastNameFormat '
	
	if not @email is null
		set @commandText = @commandText + ' and account.Email = @email '

	if isnull(@showCount, 0) = 0
	begin
		if @sortColumn = 'login'
		begin
			set @innerOrder = 'order by Login <orderDirection> '
			set @outerOrder = 'order by Login <orderDirection> '
			set @resultOrder = 'order by Login <orderDirection> '
		end
		else if @sortColumn = 'IsActive'
		begin
			set @innerOrder = 'order by IsActive <orderDirection> '
			set @outerOrder = 'order by IsActive <orderDirection> '
			set @resultOrder = 'order by IsActive <orderDirection> '
		END
		else if @sortColumn = 'name'
		begin
			set @innerOrder = 'order by LastName <orderDirection>, FirstName <orderDirection>, PatronymicName <orderDirection> '
			set @outerOrder = 'order by LastName <orderDirection>, FirstName <orderDirection>, PatronymicName <orderDirection> '
			set @resultOrder = 'order by LastName <orderDirection>, FirstName <orderDirection>, PatronymicName <orderDirection> '
		end
		else if @sortColumn = 'email'
		begin
			set @innerOrder = 'order by Email <orderDirection>, Id <orderDirection> '
			set @outerOrder = 'order by Email <orderDirection>, Id <orderDirection> '
			set @resultOrder = 'order by Email <orderDirection>, Id <orderDirection> '
		end 
		else if @sortColumn = 'Id'
		begin
			set @innerOrder = 'order by Id <orderDirection> '
			set @outerOrder = 'order by Id <orderDirection> '
			set @resultOrder = 'order by Id <orderDirection> ' 
		end 
		else 
		begin
			set @innerOrder = 'order by Login <orderDirection> '
			set @outerOrder = 'order by Login <orderDirection> '
			set @resultOrder = 'order by Login <orderDirection> '
		end

		if @sortAsc = 1
		begin
			set @innerOrder = replace(@innerOrder, '<orderDirection>', 'asc')
			set @outerOrder = replace(@outerOrder, '<orderDirection>', 'desc')
			set @resultOrder = replace(@resultOrder, '<orderDirection>', 'asc')
		end
		else
		begin
			set @innerOrder = replace(@innerOrder, '<orderDirection>', 'desc')
			set @outerOrder = replace(@outerOrder, '<orderDirection>', 'asc')
			set @resultOrder = replace(@resultOrder, '<orderDirection>', 'desc')
		end

		if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) <> 0
		begin
			set @innerSelectHeader = replace('top <count>', '<count>', @startRowIndex - 1 + @maxRowCount)
			set @outerSelectHeader = replace('top <count>', '<count>', @maxRowCount)
		end
		else if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) = 0
		begin
			set @innerSelectHeader = replace('top <count>', '<count>', @maxRowCount)
			set @outerSelectHeader = replace('top <count>', '<count>', @maxRowCount)
		end
		else if isnull(@maxRowCount, -1) = -1 
		begin
			set @innerSelectHeader = 'top 10000000'
			set @outerSelectHeader = 'top 10000000'
		end

		set @commandText = replace(replace(replace(
				N'insert into @search ' +
				N'select <outerHeader> * ' + 
				N'from (<innerSelect>) as innerSelect ' + @outerOrder
				, N'<innerSelect>', @commandText + @innerOrder)
				, N'<innerHeader>', @innerSelectHeader)
				, N'<outerHeader>', @outerSelectHeader)
	end
	set @commandText = @commandText + 
		'option (keepfixed plan) '

	if isnull(@showCount, 0) = 0
		set @viewCommandText = 
			'select ' +
			'	search.Login ' +
			'	, search.LastName ' +
			'	, search.FirstName ' +
			'	, search.PatronymicName ' +
			'	, search.IsActive ' +
			'	, search.Email ' + 
			'from @search search ' + @resultOrder

	set @commandText = @declareCommandText + @commandText + @viewCommandText 
	
	set @params = 
			' @login nvarchar(255) ' +
			', @IsActive bit ' + 
			', @lastNameFormat nvarchar(255) ' +
			', @email nvarchar(255) '  +
			', @orgID int ' 
	
	exec sp_executesql @commandText, @params
			, @login
			, @IsActive
			, @lastNameFormat
			, @email 
			, @orgID

	return 0
end
GO
/****** Object:  StoredProcedure [dbo].[SearchAskedQuestion]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.SearchAskedQuestion

-- =============================================
-- Получение списка вопросов.
-- v.1.0: Created by Makarev Andrey 22.04.2008
-- =============================================
CREATE proc [dbo].[SearchAskedQuestion]
	@name nvarchar(255) = null
	, @isActive bit = null
	, @contextCodes nvarchar(4000) = null
	, @startRowIndex int = null
	, @maxRowCount int = null
	, @sortColumn nvarchar(20)
	, @sortAsc bit = 1
	, @showCount bit = null
as
begin
	declare 
		@nameFormat nvarchar(255)
		, @declareCommandText nvarchar(4000)
		, @commandText nvarchar(4000)
		, @viewCommandText nvarchar(4000)
		, @innerOrder nvarchar(1000)
		, @outerOrder nvarchar(1000)
		, @resultOrder nvarchar(1000)
		, @innerSelectHeader nvarchar(10)
		, @outerSelectHeader nvarchar(10)

	if isnull(@name, '') <> ''
		set @nameFormat = '%' + replace(@name, ' ', '%') + '%'

	set @declareCommandText = ''
	set @commandText = ''
	set @viewCommandText = ''

	if isnull(@showCount, 0) = 0
		set @declareCommandText = 
			'declare @search table ' +
			'	( ' +
			'	Id bigint ' +
			'	, Name nvarchar(255) ' +
			'	, Question ntext ' +
			'	, IsActive bit ' +
			'	, Popularity decimal(18,4) ' +
			'	) ' 

	if isnull(@contextCodes, '') <> ''
		set @declareCommandText = @declareCommandText + 
			'declare @codes table '+
			'	( ' +
			'	Code nvarchar(255) ' +
			'	) ' +
			'insert @codes select value from dbo.GetDelimitedValues(@contextCodes) '

	if isnull(@showCount, 0) = 0
		set @commandText = 
				'select <innerHeader> ' +
				'	asked_question.Id Id ' +
				'	, asked_question.Name Name ' +
				'	, asked_question.Question Question ' +
				'	, asked_question.IsActive IsActive ' +
				'	, asked_question.Popularity Popularity ' +
				'from dbo.AskedQuestion asked_question with (nolock) ' +
				'where 1 = 1 ' 
	else
		set @commandText = 
				'select count(*) ' +
				'from dbo.AskedQuestion asked_question with (nolock) ' +
				'where 1 = 1 ' 

	if not @nameFormat is null	
		set @commandText = @commandText + ' and asked_question.Name like @nameFormat '

	if not @isActive is null
		set @commandText = @commandText + ' and asked_question.IsActive = @isActive '

	if not @contextCodes is null
		set @commandText = @commandText + ' and not exists(select 1 ' +
				'		from @codes context_codes ' +
				'			inner join dbo.Context context ' +
				'				on context.Code = context_codes.Code ' +
				'			left outer join dbo.AskedQuestionContext asked_question_context with(nolock) ' +
				'				on asked_question_context.ContextId = context.Id ' +
				'					and asked_question_context.AskedQuestionId = asked_question.Id ' +
				'		where asked_question_context.Id is null) '

	if isnull(@showCount, 0) = 0
	begin
		if @sortColumn = 'Name'
		begin
			set @innerOrder = 'order by Name <orderDirection>, Id <orderDirection> '
			set @outerOrder = 'order by Name <orderDirection>, Id <orderDirection> '
			set @resultOrder = 'order by Name <orderDirection>, Id <orderDirection> '
		end
		else if @sortColumn = 'IsActive'
		begin
			set @innerOrder = 'order by IsActive <orderDirection>, Id <orderDirection> '
			set @outerOrder = 'order by IsActive <orderDirection>, Id <orderDirection> '
			set @resultOrder = 'order by IsActive <orderDirection>, Id <orderDirection> '
		end
		else
		begin
			set @innerOrder = 'order by Popularity <orderDirection>, Id <orderDirection> '
			set @outerOrder = 'order by Popularity <orderDirection>, Id <orderDirection> '
			set @resultOrder = 'order by Popularity <orderDirection>, Id <orderDirection> '
		end

		if @sortAsc = 1
		begin
			set @innerOrder = replace(@innerOrder, '<orderDirection>', 'asc')
			set @outerOrder = replace(@outerOrder, '<orderDirection>', 'desc')
			set @resultOrder = replace(@resultOrder, '<orderDirection>', 'asc')
		end
		else
		begin
			set @innerOrder = replace(@innerOrder, '<orderDirection>', 'desc')
			set @outerOrder = replace(@outerOrder, '<orderDirection>', 'asc')
			set @resultOrder = replace(@resultOrder, '<orderDirection>', 'desc')
		end

		if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) <> 0
		begin
			set @innerSelectHeader = replace('top <count>', '<count>', @startRowIndex - 1 + @maxRowCount)
			set @outerSelectHeader = replace('top <count>', '<count>', @maxRowCount)
		end
		else if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) = 0
		begin
			set @innerSelectHeader = replace('top <count>', '<count>', @maxRowCount)
			set @outerSelectHeader = replace('top <count>', '<count>', @maxRowCount)
		end
		else if isnull(@maxRowCount, -1) = -1 
		begin
			set @innerSelectHeader = 'top 10000000'
			set @outerSelectHeader = 'top 10000000'
		end

		set @commandText = replace(replace(replace(
				N'insert into @search ' +
				N'select <outerHeader> * ' + 
				N'from (<innerSelect>) as innerSelect ' + @outerOrder
				, N'<innerSelect>', @commandText + @innerOrder)
				, N'<innerHeader>', @innerSelectHeader)
				, N'<outerHeader>', @outerSelectHeader)
	end
	set @commandText = @commandText + 
		'option (keepfixed plan) '

	if isnull(@showCount, 0) = 0
		set @viewCommandText = 
			'select ' +
			'	dbo.GetExternalId(search.Id) Id ' +
			'	, search.Name ' +
			'	, search.Question ' +
			'	, search.IsActive ' +
			'	, search.Popularity ' +
			'from @search search ' + @resultOrder

	set @commandText = @declareCommandText + @commandText + @viewCommandText 

	exec sp_executesql @commandText
		, N'@nameFormat nvarchar(255), @isActive bit, @contextCodes nvarchar(4000)'
		, @nameFormat
		, @IsActive
		, @contextCodes

	return 0
end
GO
/****** Object:  StoredProcedure [dbo].[SearchContext]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.SearchContext

-- =============================================
-- Поиск контекстов.
-- v.1.0: Created by Makarev Andrey 19.04.2008
-- v.1.1: Modified by Fomin Dmitriy 22.04.2008
-- Приведение к стандарту.
-- =============================================
CREATE proc [dbo].[SearchContext]
as
begin

	select 
		context.Code Code
		, context.[Name] [Name]
	from 
		dbo.Context context with (nolock)

	return 0
end
GO
/****** Object:  StoredProcedure [dbo].[SearchDeliveries]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Получение списка рассылок.
-- v.1.0: Created by Yusupov Kirill 16.04.2010
-- =============================================
CREATE proc [dbo].[SearchDeliveries]
	@title nvarchar(255) = null
	, @createDateFrom datetime = null
	, @createDateTo datetime = null
	, @deliveryDateFrom datetime = null
	, @deliveryDateTo datetime = null
	, @status int = null
	, @startRowIndex int = null
	, @maxRowCount int = null
	, @sortColumn nvarchar(20) = null
	, @sortAsc bit = 1
	, @showCount bit = null
as
begin
	declare 
		@declareCommandText nvarchar(4000)
		, @commandText nvarchar(4000)
		, @viewCommandText nvarchar(4000)
		, @innerOrder nvarchar(1000)
		, @outerOrder nvarchar(1000)
		, @resultOrder nvarchar(1000)
		, @innerSelectHeader nvarchar(10)
		, @outerSelectHeader nvarchar(10)
		, @titleFormat nvarchar(255)

	if isnull(@title, '') <> ''
		set @titleFormat = '%' + replace(@title, ' ' , '%') + '%'

	set @declareCommandText = ''
	set @commandText = ''
	set @viewCommandText = ''

	if isnull(@showCount, 0) = 0
		set @declareCommandText = 
			'declare @search table ' +
			'	( ' +
			'	Id bigint ' +
			'	, CreateDate datetime ' +
			'	, DeliveryDate datetime ' +
			'	, TypeCode nvarchar(20) ' +
			'	, Title nvarchar(255) ' +
			'	, Status int ' +
			'	, StatusName nvarchar(255) ' +
			'	) ' 

	if isnull(@showCount, 0) = 0
		set @commandText = 
				'select <innerHeader> ' +
				'	delivery.Id Id ' +
				'	, delivery.CreateDate CreateDate ' +
				'	, delivery.DeliveryDate DeliveryDate ' +
				'	, delivery.TypeCode TypeCode ' +
				'	, delivery.Title Title ' +
				'	, delivery.Status Status ' +
				'	, status.Name StatusName ' +
				'from dbo.Delivery delivery with (nolock) ' +
				'inner join DeliveryStatus status on delivery.Status=status.Id '+
				'where 1 = 1 ' 
	else
		set @commandText = 
				'select count(*) ' +
				'from dbo.Delivery delivery with (nolock) ' +
				'where 1 = 1 ' 
	
	if not @status is null
		set @commandText = @commandText + ' and delivery.Status = @status '

	if not @createDateFrom is null
		set @commandText = @commandText + ' and delivery.CreateDate >= @createDateFrom '

	if not @createDateTo is null
		set @commandText = @commandText + ' and delivery.CreateDate <= @createDateTo '

	if not @deliveryDateFrom is null
		set @commandText = @commandText + ' and delivery.DeliveryDate >= @deliveryDateFrom '

	if not @deliveryDateTo is null
		set @commandText = @commandText + ' and delivery.DeliveryDate <= @deliveryDateTo '

	if not @title is null
		set @commandText = @commandText + ' and delivery.Title like @titleFormat '

	if isnull(@showCount, 0) = 0
	begin
		if @sortColumn = N'Title'
		begin
			set @innerOrder = 'order by Title <orderDirection>, DeliveryDate <backOrderDirection>, Id <orderDirection> '
			set @outerOrder = 'order by Title <orderDirection>, DeliveryDate <backOrderDirection>, Id <orderDirection> '
			set @resultOrder = 'order by Title <orderDirection>, DeliveryDate <backOrderDirection>, Id <orderDirection> '
		end
		else if @sortColumn = N'StatusName'
		begin
			set @innerOrder = 'order by StatusName <orderDirection>, DeliveryDate <backOrderDirection>, Id <orderDirection> '
			set @outerOrder = 'order by StatusName <orderDirection>, DeliveryDate <backOrderDirection>, Id <orderDirection> '
			set @resultOrder = 'order by StatusName <orderDirection>, DeliveryDate <backOrderDirection>, Id <orderDirection> '
		end
		else if @sortColumn = N'CreateDate'
		begin
			set @innerOrder = 'order by CreateDate <orderDirection>, DeliveryDate <backOrderDirection>, Id <orderDirection> '
			set @outerOrder = 'order by CreateDate <orderDirection>, DeliveryDate <backOrderDirection>, Id <orderDirection> '
			set @resultOrder = 'order by CreateDate <orderDirection>, DeliveryDate <backOrderDirection>, Id <orderDirection> '
		end
		else
		begin 
			set @innerOrder = 'order by DeliveryDate <orderDirection>, Id <orderDirection> '
			set @outerOrder = 'order by DeliveryDate <orderDirection>, Id <orderDirection> '
			set @resultOrder = 'order by DeliveryDate <orderDirection>, Id <orderDirection> '

		end

		if @sortAsc = 1
		begin
			set @innerOrder = replace(replace(@innerOrder, '<orderDirection>', 'asc'), '<backOrderDirection>', 'desc')
			set @outerOrder = replace(replace(@outerOrder, '<orderDirection>', 'desc'), '<backOrderDirection>', 'asc')
			set @resultOrder = replace(replace(@resultOrder, '<orderDirection>', 'asc'), '<backOrderDirection>', 'desc')
		end
		else
		begin
			set @innerOrder = replace(replace(@innerOrder, '<orderDirection>', 'desc'), '<backOrderDirection>', 'asc')
			set @outerOrder = replace(replace(@outerOrder, '<orderDirection>', 'asc'), '<backOrderDirection>', 'desc')
			set @resultOrder = replace(replace(@resultOrder, '<orderDirection>', 'desc'), '<backOrderDirection>', 'asc')
		end

		if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) <> 0
		begin
			set @innerSelectHeader = replace('top <count>', '<count>', @startRowIndex - 1 + @maxRowCount)
			set @outerSelectHeader = replace('top <count>', '<count>', @maxRowCount)
		end
		else if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) = 0
		begin
			set @innerSelectHeader = replace('top <count>', '<count>', @maxRowCount)
			set @outerSelectHeader = replace('top <count>', '<count>', @maxRowCount)
		end
		else if isnull(@maxRowCount, -1) = -1 
		begin
			set @innerSelectHeader = 'top 10000000'
			set @outerSelectHeader = 'top 10000000'
		end

		set @commandText = replace(replace(replace(
				N'insert into @search ' +
				N'select <outerHeader> * ' + 
				N'from (<innerSelect>) as innerSelect ' + @outerOrder
				, N'<innerSelect>', @commandText + @innerOrder)
				, N'<innerHeader>', @innerSelectHeader)
				, N'<outerHeader>', @outerSelectHeader)
	end
	set @commandText = @commandText + 
		'option (keepfixed plan) '

	if isnull(@showCount, 0) = 0
		set @viewCommandText = 
			'select ' +
			'	search.Id Id ' +
			'	, search.CreateDate ' +
			'	, search.DeliveryDate ' +
			'	, search.TypeCode ' +
			'	, search.Title ' +
			'	, search.Status ' +
			'	, search.StatusName ' +
			'from @search search ' + @resultOrder

	set @commandText = @declareCommandText + @commandText + @viewCommandText 

	exec sp_executesql @commandText
		, N'@status int, @createDateFrom datetime, @createDateTo datetime, @deliveryDateFrom datetime, @deliveryDateTo datetime, @titleFormat nvarchar(255)'
		, @status
		, @createDateFrom
		, @createDateTo
		, @deliveryDateFrom
		, @deliveryDateTo
		, @titleFormat

	return 0
end







GO
/****** Object:  StoredProcedure [dbo].[SearchDocument]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.SearchDocument

-- =============================================
-- Получение списка документов.
-- v.1.0: Created by Makarev Andrey 16.04.2008
-- v.1.1: Modified by Fomin Dmitriy 18.04.2008
-- Добавлена фильтрация по наименованию.
-- v.1.2: Modified by Makarev Andrey 19.04.2008
-- Правильный вывод ИД.
-- v.1.3: Modified by Fomin Dmitriy 21.04.2008
-- Убраны лишние поля.
-- v.1.4: Modified by Fomin Dmitriy 24.04.2008
-- Добавлено поле RelativeUrl.
-- =============================================
CREATE proc [dbo].[SearchDocument]
	@isActive bit = null
	, @contextCodes nvarchar(4000) = null
	, @name nvarchar(255) = null
	, @startRowIndex int = null
	, @maxRowCount int = null
	, @sortColumn nvarchar(20) = N'Id'
	, @sortAsc bit = 0
	, @showCount bit = null
as
begin
	declare 
		@declareCommandText nvarchar(4000)
		, @commandText nvarchar(4000)
		, @viewCommandText nvarchar(4000)
		, @innerOrder nvarchar(1000)
		, @outerOrder nvarchar(1000)
		, @resultOrder nvarchar(1000)
		, @innerSelectHeader nvarchar(10)
		, @outerSelectHeader nvarchar(10)
		, @nameFormat nvarchar(255)

	if isnull(@name, '') <> ''
		set @nameFormat = '%' + replace(@name, ' ' , '%') + '%'

	set @declareCommandText = ''
	set @commandText = ''
	set @viewCommandText = ''

	if isnull(@showCount, 0) = 0
		set @declareCommandText = 
			'declare @search table ' +
			'	( ' +
			'	Id bigint ' +
			'	, Name nvarchar(255) ' +
			'	, Description ntext ' +
			'	, IsActive bit ' +
			'	, ActivateDate datetime ' +
			'	, ContextCodes nvarchar(4000) ' +
			'	, RelativeUrl nvarchar(255) ' +
			'   , Date datetime ' +
			'	) ' 

	if isnull(@contextCodes, '') <> ''
		set @declareCommandText = @declareCommandText + 
			'declare @codes table '+
			'	( ' +
			'	Code nvarchar(255) ' +
			'	) ' +
			'insert @codes select value from dbo.GetDelimitedValues(@contextCodes) '

	if isnull(@showCount, 0) = 0
		set @commandText = 
				'select <innerHeader> ' +
				'	document.Id Id ' +
				'	, document.Name Name ' +
				'	, document.Description Description ' +
				'	, document.IsActive IsActive ' +
				'	, document.ActivateDate ActivateDate ' +
				'	, document.ContextCodes ContextCodes ' +
				'	, document.RelativeUrl RelativeUrl ' +
				'	, document.UpdateDate Date ' +
				'from dbo.Document document with (nolock) ' +
				'where 1 = 1 ' 
	else
		set @commandText = 
				'select count(*) ' +
				'from dbo.Document document with (nolock) ' +
				'where 1 = 1 ' 
	
	if not @isActive is null
		set @commandText = @commandText + ' and document.IsActive = @isActive '

	if not @contextCodes is null
		set @commandText = @commandText + ' and not exists(select 1 ' +
				'		from @codes context_codes ' +
				'			inner join dbo.Context context ' +
				'				on context.Code = context_codes.Code ' +
				'			left outer join dbo.DocumentContext document_context with(nolock) ' +
				'				on document_context.ContextId = context.Id ' +
				'					and document_context.DocumentId = document.Id ' +
				'		where document_context.Id is null) '

	if not @nameFormat is null
		set @commandText = @commandText + ' and document.Name like @nameFormat '

	if isnull(@showCount, 0) = 0
	begin
		if @sortColumn = 'Name'
		begin
			set @innerOrder = 'order by Name <orderDirection>, Id <orderDirection> '
			set @outerOrder = 'order by Name <orderDirection>, Id <orderDirection> '
			set @resultOrder = 'order by Name <orderDirection>, Id <orderDirection> '
		end
		else if @sortColumn = 'IsActive'
		begin
			set @innerOrder = 'order by IsActive <orderDirection>, Id <orderDirection> '
			set @outerOrder = 'order by IsActive <orderDirection>, Id <orderDirection> '
			set @resultOrder = 'order by IsActive <orderDirection>, Id <orderDirection> '
		end
		else if @sortColumn = 'Date'
		begin
			set @innerOrder = 'order by Date <orderDirection>, Id <orderDirection> '
			set @outerOrder = 'order by Date <orderDirection>, Id <orderDirection> '
			set @resultOrder = 'order by Date <orderDirection>, Id <orderDirection> '
		end
		else
		begin
			set @innerOrder = 'order by Id <orderDirection> '
			set @outerOrder = 'order by Id <orderDirection> '
			set @resultOrder = 'order by Id <orderDirection> '
		end

		if @sortAsc = 1
		begin
			set @innerOrder = replace(@innerOrder, '<orderDirection>', 'asc')
			set @outerOrder = replace(@outerOrder, '<orderDirection>', 'desc')
			set @resultOrder = replace(@resultOrder, '<orderDirection>', 'asc')
		end
		else
		begin
			set @innerOrder = replace(@innerOrder, '<orderDirection>', 'desc')
			set @outerOrder = replace(@outerOrder, '<orderDirection>', 'asc')
			set @resultOrder = replace(@resultOrder, '<orderDirection>', 'desc')
		end

		if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) <> 0
		begin
			set @innerSelectHeader = replace('top <count>', '<count>', @startRowIndex - 1 + @maxRowCount)
			set @outerSelectHeader = replace('top <count>', '<count>', @maxRowCount)
		end
		else if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) = 0
		begin
			set @innerSelectHeader = replace('top <count>', '<count>', @maxRowCount)
			set @outerSelectHeader = replace('top <count>', '<count>', @maxRowCount)
		end
		else if isnull(@maxRowCount, -1) = -1 
		begin
			set @innerSelectHeader = 'top 10000000'
			set @outerSelectHeader = 'top 10000000'
		end

		set @commandText = replace(replace(replace(
				N'insert into @search ' +
				N'select <outerHeader> * ' + 
				N'from (<innerSelect>) as innerSelect ' + @outerOrder
				, N'<innerSelect>', @commandText + @innerOrder)
				, N'<innerHeader>', @innerSelectHeader)
				, N'<outerHeader>', @outerSelectHeader)
	end
	set @commandText = @commandText + 
		'option (keepfixed plan) '

	if isnull(@showCount, 0) = 0
		set @viewCommandText = 
			'select ' +
			'	dbo.GetExternalId(search.Id) Id ' +
			'	, search.Name ' +
			'	, search.Description ' +
			'	, search.IsActive ' +
			'	, search.ActivateDate ' +
			'	, search.ContextCodes ' +
			'	, search.RelativeUrl ' +
			'	, search.Date ' +
			'from @search search ' + @resultOrder

	set @commandText = @declareCommandText + @commandText + @viewCommandText 

	exec sp_executesql @commandText
		, N'@isActive bit, @contextCodes nvarchar(4000), @nameFormat nvarchar(255)'
		, @IsActive
		, @contextCodes
		, @nameFormat

	return 0
end
GO
/****** Object:  StoredProcedure [dbo].[SearchNews]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.SearchNews

-- =============================================
-- Получение списка новостей.
-- v.1.0: Created by Makarev Andrey 19.04.2008
-- v.1.1: Modified by Makarev Andrey 22.04.2008
-- Выводим название новости.
-- v.1.2: Modified by Makarev Andrey 23.04.2008
-- Название новости добавлено в фильтр.
-- =============================================
CREATE proc [dbo].[SearchNews]
	@isActive bit = null
	, @dateFrom datetime = null
	, @dateTo datetime = null
	, @name nvarchar(255) = null
	, @startRowIndex int = null
	, @maxRowCount int = null
	, @sortColumn nvarchar(20) = null
	, @sortAsc bit = 1
	, @showCount bit = null
as
begin
	declare 
		@declareCommandText nvarchar(4000)
		, @commandText nvarchar(4000)
		, @viewCommandText nvarchar(4000)
		, @innerOrder nvarchar(1000)
		, @outerOrder nvarchar(1000)
		, @resultOrder nvarchar(1000)
		, @innerSelectHeader nvarchar(10)
		, @outerSelectHeader nvarchar(10)
		, @nameFormat nvarchar(255)

	if isnull(@name, '') <> ''
		set @nameFormat = '%' + replace(@name, ' ' , '%') + '%'

	set @declareCommandText = ''
	set @commandText = ''
	set @viewCommandText = ''

	if isnull(@showCount, 0) = 0
		set @declareCommandText = 
			'declare @search table ' +
			'	( ' +
			'	Id bigint ' +
			'	, Date datetime ' +
			'	, Description ntext ' +
			'	, Name nvarchar(255) ' +
			'	, IsActive bit ' +
			'	) ' 

	if isnull(@showCount, 0) = 0
		set @commandText = 
				'select <innerHeader> ' +
				'	news.Id Id ' +
				'	, news.Date Date ' +
				'	, news.Description Description ' +
				'	, news.Name Name ' +
				'	, news.IsActive IsActive ' +
				'from dbo.News news with (nolock) ' +
				'where 1 = 1 ' 
	else
		set @commandText = 
				'select count(*) ' +
				'from dbo.News news with (nolock) ' +
				'where 1 = 1 ' 
	
	if not @isActive is null
		set @commandText = @commandText + ' and news.IsActive = @isActive '

	if not @dateFrom is null
		set @commandText = @commandText + ' and news.Date >= @dateFrom '

	if not @dateTo is null
		set @commandText = @commandText + ' and news.Date <= @dateTo '

	if not @name is null
		set @commandText = @commandText + ' and news.Name like @nameFormat '

	if isnull(@showCount, 0) = 0
	begin
		if @sortColumn = N'Name'
		begin
			set @innerOrder = 'order by Name <orderDirection>, Date <backOrderDirection>, Id <orderDirection> '
			set @outerOrder = 'order by Name <orderDirection>, Date <backOrderDirection>, Id <orderDirection> '
			set @resultOrder = 'order by Name <orderDirection>, Date <backOrderDirection>, Id <orderDirection> '
		end
		else if @sortColumn = N'IsActive'
		begin
			set @innerOrder = 'order by IsActive <orderDirection>, Date <backOrderDirection>, Id <orderDirection> '
			set @outerOrder = 'order by IsActive <orderDirection>, Date <backOrderDirection>, Id <orderDirection> '
			set @resultOrder = 'order by IsActive <orderDirection>, Date <backOrderDirection>, Id <orderDirection> '
		end
		else 
		begin
			set @innerOrder = 'order by Date <orderDirection>, Id <orderDirection> '
			set @outerOrder = 'order by Date <orderDirection>, Id <orderDirection> '
			set @resultOrder = 'order by Date <orderDirection>, Id <orderDirection> '
		end

		if @sortAsc = 1
		begin
			set @innerOrder = replace(replace(@innerOrder, '<orderDirection>', 'asc'), '<backOrderDirection>', 'desc')
			set @outerOrder = replace(replace(@outerOrder, '<orderDirection>', 'desc'), '<backOrderDirection>', 'asc')
			set @resultOrder = replace(replace(@resultOrder, '<orderDirection>', 'asc'), '<backOrderDirection>', 'desc')
		end
		else
		begin
			set @innerOrder = replace(replace(@innerOrder, '<orderDirection>', 'desc'), '<backOrderDirection>', 'asc')
			set @outerOrder = replace(replace(@outerOrder, '<orderDirection>', 'asc'), '<backOrderDirection>', 'desc')
			set @resultOrder = replace(replace(@resultOrder, '<orderDirection>', 'desc'), '<backOrderDirection>', 'asc')
		end

		if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) <> 0
		begin
			set @innerSelectHeader = replace('top <count>', '<count>', @startRowIndex - 1 + @maxRowCount)
			set @outerSelectHeader = replace('top <count>', '<count>', @maxRowCount)
		end
		else if isnull(@maxRowCount, -1) <> -1 and isnull(@startRowIndex, 0) = 0
		begin
			set @innerSelectHeader = replace('top <count>', '<count>', @maxRowCount)
			set @outerSelectHeader = replace('top <count>', '<count>', @maxRowCount)
		end
		else if isnull(@maxRowCount, -1) = -1 
		begin
			set @innerSelectHeader = 'top 10000000'
			set @outerSelectHeader = 'top 10000000'
		end

		set @commandText = replace(replace(replace(
				N'insert into @search ' +
				N'select <outerHeader> * ' + 
				N'from (<innerSelect>) as innerSelect ' + @outerOrder
				, N'<innerSelect>', @commandText + @innerOrder)
				, N'<innerHeader>', @innerSelectHeader)
				, N'<outerHeader>', @outerSelectHeader)
	end
	set @commandText = @commandText + 
		'option (keepfixed plan) '

	if isnull(@showCount, 0) = 0
		set @viewCommandText = 
			'select ' +
			'	dbo.GetExternalId(search.Id) Id ' +
			'	, search.Date ' +
			'	, search.Description ' +
			'	, search.Name ' +
			'	, search.IsActive ' +
			'from @search search ' + @resultOrder

	set @commandText = @declareCommandText + @commandText + @viewCommandText 

	exec sp_executesql @commandText
		, N'@isActive bit, @dateFrom datetime, @dateTo datetime, @nameFormat nvarchar(255)'
		, @IsActive
		, @dateFrom
		, @dateTo
		, @nameFormat

	return 0
end
GO
/****** Object:  StoredProcedure [dbo].[SearchRegion]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[SearchRegion]
as
begin
	select
		region.[Id] RegionId
		, region.[Name] [Name]
	from dbo.Region region
	where region.InOrganization = 1
	order by region.[Name] --region.SortIndex

	return 0
end
GO
/****** Object:  StoredProcedure [dbo].[SelectInformationOrg]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SelectInformationOrg]
 @Id INT
AS
BEGIN
	SELECT
		O.Id,O.[Version],O.FullName,O.ShortName,O.INN,O.OGRN,O.KPP, O.OwnerDepartment,O.IsPrivate,O.IsFilial,
		O.DirectorFullName,O.DirectorFullNameInGenetive,O.DirectorFirstName,O.DirectorLastName,O.DirectorPatronymicName,
		O.DirectorPositionInGenetive,O.DirectorPosition,O.AccreditationSertificate,O.LawAddress,O.FactAddress,
		O.OUConfirmation,O.PhoneCityCode,O.Phone,O.Fax,O.EMail,O.[Site],
		Reg.Id as RegionId, [Type].Id as TypeId, Kind.Id as KindId,
		Reg.Code as RegionCode,
		Reg.Name as RegionName, [Type].Name as TypeName, Kind.Name as KindName,
		O.RCModel as RCModelId, RC.ModelName as RCModelName, O.RCDescription,
		O.CNFederalBudget, O.CNTargeted, O.CNLocalBudget, O.CNPaying, O.CNFullTime, O.CNEvening, O.CNPostal,
		O.MainId, MO.FullName as MainFullName, MO.ShortName as MainShortName,
		O.StatusId, [Status].Name as StatusName,
		O.NewOrgId, [NO].FullName as NewOrgFullName, [NO].ShortName as NewOrgShortName,
		O.DepartmentId, DO.FullName as DepartmentFullName, DO.ShortName as DepartmentShortName,
		O.CreateDate, O.UpdateDate, O.DateChangeStatus, O.Reason, O.ReceptionOnResultsCNE, O.TimeConnectionToSecureNetwork, O.TimeEnterInformationInFIS,
		sch.Id as ConnectionSchemeId, sch.Name as ConnectionSchemeName, conStatus.Id as ConnectionStatusId, conStatus.Name as ConnectionStatusName,
		O.LetterToReschedule, O.LetterToRescheduleContentType, O.LetterToRescheduleName,
		O.IsAgreedTimeConnection, O.IsAgreedTimeEnterInformation, O.ISLOD_GUID,
		O.TownName, Lic.RegNumber as LicenseRegNumber, Lic.OrderDocumentDate as LicenseOrderDocumentDate, Lic.StatusName as LicenseStatusName,
		OIS.Id AS OrganizationISId, ois.Name AS OrganizationISName,
		Sup.Number AS SupplementNumber, Sup.OrderDocumentDate AS SupplementOrderDocumentDate, Sup.StatusName AS SupplementStatusName,O.IsAnotherName as IsAnotherName
	FROM 
		dbo.Organization2010 O
	INNER JOIN 
		dbo.Region Reg ON Reg.Id=O.RegionId 
	INNER JOIN 
		dbo.OrganizationType2010 [Type] ON [Type].Id=O.TypeId
	INNER JOIN 
		dbo.OrganizationKind Kind ON Kind.Id=O.KindId 
	LEFT JOIN 
		dbo.ConnectionScheme sch ON sch.Id=O.ConnectionSchemeId
	LEFT JOIN 
		dbo.ConnectionStatus conStatus ON conStatus.Id=O.ConnectionStatusId
	LEFT JOIN 
		[dbo].[RecruitmentCampaigns] RC on O.RCModel = RC.Id
	LEFT JOIN 
		[dbo].[OrganizationOperatingStatus] [Status] on O.StatusId = [Status].Id
	LEFT JOIN
		[dbo].[Organization2010] MO on O.MainId = MO.Id
	LEFT JOIN 
		[dbo].[Organization2010] [NO] on O.NewOrgId = [NO].Id
	LEFT JOIN
		[dbo].[Organization2010] DO on O.DepartmentId = DO.Id
	LEFT JOIN
		[dbo].[OrganizationIS] OIS on O.OrganizationISId = OIS.Id
	OUTER APPLY 
		(SELECT TOP 1 * FROM [dbo].[License] Lic WHERE Lic.OrganizationId = ISNULL(MO.Id,O.Id) ORDER BY Lic.OrderDocumentDate DESC) Lic
	OUTER APPLY 
		(SELECT TOP 1 * FROM [dbo].[LicenseSupplement] Sup WHERE Lic.id = Sup.LicenseId AND Sup.OrganizationId = O.Id ORDER BY Sup.OrderDocumentDate DESC) Sup	
							
	WHERE 
		O.Id=@id
END



GO
/****** Object:  StoredProcedure [dbo].[SelectInformationSystems]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[SelectInformationSystems]
AS
BEGIN
SELECT s.Name as ShortName, s.SystemID as SystemID, COUNT(g.SystemID) as NumberGroups, s.FullName, s.AvailableRegistration
	FROM dbo.System s
	LEFT JOIN [Group] g ON s.SystemID=g.SystemID
	GROUP BY s.Name, s.SystemID, s.FullName, s.AvailableRegistration
END
GO
/****** Object:  StoredProcedure [dbo].[SetActiveAskedQuestion]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.SetActiveAskedQuestion

-- =============================================
-- Установка активности вопроса.
-- v.1.0: Created by Makarev Andrey 21.04.2008
-- v.1.1: Modified by Makarev Andrey 23.04.2008
-- Изменение оформления.
-- =============================================
CREATE proc [dbo].[SetActiveAskedQuestion]
	@ids nvarchar(255)
	, @isActive bit
	, @editorLogin nvarchar(255)
	, @editorIp nvarchar(255)
as
begin
	declare @idTable table
		(
		[id] bigint
		)

	insert @idTable select dbo.GetInternalId(convert(bigint, [value])) from dbo.GetDelimitedValues(@ids)

	declare 
		@eventCode nvarchar(255)
		, @editorAccountId bigint
		, @updateId	uniqueidentifier
		, @currentDate datetime
		, @innerIds nvarchar(4000)

	set @innerIds = ''
	
	select 
		@innerIds = @innerIds + convert(nvarchar, id_table.[id]) + N','
	from 
		@idTable id_table
	
	if len(@innerIds) > 0
		set @innerIds = left(@innerIds, len(@innerIds) - 1)

	set @updateId = newid()
	set @currentDate = getdate()

	select 
		@editorAccountId = account.[Id]
	from
		dbo.Account account with (nolock, fastfirstrow)
	where 
		account.[Login] = @editorLogin
	
	if @isActive = 1
		set @eventCode = N'FAQ_PUBLIC'
	else
		set	@eventCode = N'FAQ_UNPUBLIC'

	update asked_question
	set
		UpdateDate = @currentDate
		, UpdateId = @updateId
		, EditorAccountId = @editorAccountId
		, EditorIp = @editorIp
		, IsActive = @isActive
	from 
		dbo.AskedQuestion asked_question with (rowlock)
			inner join @idTable idTable
				on asked_question.[id] = idTable.[id]

	exec dbo.RegisterEvent 
		@accountId = @editorAccountId
		, @ip = @editorIp
		, @eventCode = @eventCode
		, @sourceEntityIds = @innerIds
		, @eventParams = null
		, @updateId = @updateId
		
	return 0
end
GO
/****** Object:  StoredProcedure [dbo].[SetActiveDocument]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.SetActiveDocument

-- =============================================
-- Установка активности документа.
-- v.1.0: Created by Makarev Andrey 19.04.2008
-- v.1.1: Modified by Makarev Andrey 21.04.2008
-- Добавлены хинты.
-- v.1.2: Modified by Makarev Andrey 21.04.2008
-- Вызов dbo.RegisterEvent с внутренними ИД.
-- v.1.3: Modified by Makarev Andrey 23.04.2008
-- Изменение оформления.
-- =============================================
CREATE proc [dbo].[SetActiveDocument]
	@ids nvarchar(255)
	, @isActive bit
	, @editorLogin nvarchar(255)
	, @editorIp nvarchar(255)
as
begin
	declare @idTable table
		(
		[id] bigint
		)

	insert @idTable select dbo.GetInternalId(convert(bigint, [value])) from dbo.GetDelimitedValues(@ids)

	declare 
		@eventCode nvarchar(255)
		, @editorAccountId bigint
		, @updateId	uniqueidentifier
		, @activateDate datetime
		, @currentDate datetime
		, @innerIds nvarchar(4000)

	set @innerIds = ''
	
	select 
		@innerIds = @innerIds + convert(nvarchar, id_table.[id]) + N','
	from 
		@idTable id_table
	
	if len(@innerIds) > 0
		set @innerIds = left(@innerIds, len(@innerIds) - 1)

	set @updateId = newid()
	set @currentDate = getdate()

	select 
		@editorAccountId = account.[Id]
	from
		dbo.Account account with (nolock, fastfirstrow)
	where 
		account.[Login] = @editorLogin
	
	if @isActive = 1
		select 
			@eventCode = N'DOC_PUBLIC'
			, @activateDate = @currentDate
	else
		select
			@eventCode = N'DOC_UNPUBLIC'
			, @activateDate = null

	update [document]
	set
		UpdateDate = @currentDate
		, UpdateId = @updateId
		, EditorAccountId = @editorAccountId
		, EditorIp = @editorIp
		, IsActive = @isActive
		, ActivateDate = @activateDate
	from 
		dbo.[Document] [document] with (rowlock)
			inner join @idTable idTable
				on [document].[id] = idTable.[id]

	exec dbo.RegisterEvent 
		@accountId = @editorAccountId
		, @ip = @editorIp
		, @eventCode = @eventCode
		, @sourceEntityIds = @innerIds
		, @eventParams = null
		, @updateId = @updateId
		
	return 0
end
GO
/****** Object:  StoredProcedure [dbo].[SetActiveNews]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.SetActiveNews

-- =============================================
-- Установка активности новости.
-- v.1.0: Created by Makarev Andrey 21.04.2008
-- v.1.1: Modified by Makarev Andrey 21.04.2008
-- Вызов dbo.RegisterEvent с внутренними ИД.
-- v.1.2: Modified by Makarev Andrey 23.04.2008
-- Изменение оформления.
-- =============================================
CREATE proc [dbo].[SetActiveNews]
	@ids nvarchar(255)
	, @isActive bit
	, @editorLogin nvarchar(255)
	, @editorIp nvarchar(255)
as
begin
	declare @idTable table
		(
		[id] bigint
		)

	insert @idTable select dbo.GetInternalId(convert(bigint, [value])) from dbo.GetDelimitedValues(@ids)

	declare 
		@eventCode nvarchar(255)
		, @editorAccountId bigint
		, @updateId	uniqueidentifier
		, @currentDate datetime
		, @innerIds nvarchar(4000)

	set @innerIds = ''
	
	select 
		@innerIds = @innerIds + convert(nvarchar, id_table.[id]) + N','
	from 
		@idTable id_table
	
	if len(@innerIds) > 0
		set @innerIds = left(@innerIds, len(@innerIds) - 1)

	set @updateId = newid()
	set @currentDate = getdate()

	select 
		@editorAccountId = account.[Id]
	from
		dbo.Account account with (nolock, fastfirstrow)
	where 
		account.[Login] = @editorLogin
	
	if @isActive = 1
		set @eventCode = N'NWS_PUBLIC'
	else
		set	@eventCode = N'NWS_UNPUBLIC'

	update news
	set
		UpdateDate = @currentDate
		, UpdateId = @updateId
		, EditorAccountId = @editorAccountId
		, EditorIp = @editorIp
		, IsActive = @isActive
	from 
		dbo.News news with (rowlock)
			inner join @idTable idTable
				on news.[id] = idTable.[id]

	exec dbo.RegisterEvent 
		@accountId = @editorAccountId
		, @ip = @editorIp
		, @eventCode = @eventCode
		, @sourceEntityIds = @innerIds
		, @eventParams = null
		, @updateId = @updateId
		
	return 0
end
GO
/****** Object:  StoredProcedure [dbo].[UpdateAccount]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.UpdateAccount
-- =============================================
-- Сохранить учетную запись.
-- v.1.0: Created by Makarev Andrey 10.04.2008
-- v.1.1: Modified by Makarev Andrey 14.04.2008
-- Добавлен идентификатор обновления UpdateId.
-- v.1.2: Modified by Makarev Andrey 14.04.2008
-- Приведение к стандарту.
-- v.1.3: Modified by Makarev Andrey 16.04.2008
-- Измение процедуры GetDelimitedValues().
-- v.1.4: Modified by Makarev Andrey 18.04.2008
-- Изменение параметров ХП dbo.RegisterEvent.
-- v.1.5: Modified by Makarev Andrey 21.04.2008
-- Добавлены хинты.
-- =============================================
CREATE procedure [dbo].[UpdateAccount]
	@login nvarchar(255)
	, @passwordHash nvarchar(255) = null
	, @lastName nvarchar(255)
	, @firstName nvarchar(255)
	, @patronymicName nvarchar(255)
	, @phone nvarchar(255)
	, @email nvarchar(255)
	, @isActive bit
	, @ipAddresses nvarchar(4000) = null
	, @groupCode nvarchar(255) = null
	, @editorLogin nvarchar(255)
	, @editorIp nvarchar(255)
	, @hasFixedIp bit = null
as
begin
	declare @exists table([login] nvarchar(255), isExists bit)

	insert @exists exec dbo.CheckNewLogin @login = @login
	
	declare 
		@isExists bit
		, @eventCode nvarchar(255)
		, @editorAccountId bigint
		, @accountId bigint
		, @status nvarchar(255)
		, @innerStatus nvarchar(255)
		, @confirmYear int
		, @currentYear int
		, @userGroupId int
		, @updateId	uniqueidentifier
		, @accountIds nvarchar(255)

	set @updateId = newid()

	select @userGroupId = [group].[Id]
	from dbo.[Group] [group] with (nolock, fastfirstrow)
	where [group].[Code] = @groupCode

	select @isExists = user_exists.isExists
	from  @exists user_exists

	select @editorAccountId = account.[Id]
	from  dbo.Account account with (nolock, fastfirstrow)
	where account.[Login] = @editorLogin

	select @accountId = account.[Id]
	from dbo.Account account with (nolock, fastfirstrow)
	where account.[Login] = @login

	set @currentYear = year(getdate())

	set @confirmYear = @currentYear

	declare @oldIpAddress table (ip nvarchar(255))

	declare @newIpAddress table (ip nvarchar(255))

--если логина нет - добавляем запись и добавляем пользователя в группу
--если логин есть - меняем данные
	if @isExists = 0  -- внесение нового пользователя
	begin
		select 
			@status = case when @groupCode='User' then  null else 'activated' end,
			@hasFixedIp = isnull(@hasFixedIp, 1), @eventCode = N'USR_REG'

		select @innerStatus = dbo.GetUserStatus(@confirmYear, @status, @currentYear, null)
	end
	else
	begin -- update существующего пользователя
		select 
			@accountId = account.[Id]
			, @hasFixedIp = isnull(@hasFixedIp, account.HasFixedIp)
		from 
			dbo.Account account with (nolock, fastfirstrow)
		where
			account.[Login] = @login

		insert @oldIpAddress
			(
			ip
			)
		select
			account_ip.Ip
		from
			dbo.AccountIp account_ip with (nolock, fastfirstrow)
		where
			account_ip.AccountId = @accountId

		set @eventCode = N'USR_EDIT'
	end

	if @hasFixedIp = 1
		insert @newIpAddress
			(
			ip
			)
		select 
			ip_addresses.[value]
		from 
			dbo.GetDelimitedValues(@ipAddresses) ip_addresses

	begin tran insert_update_account_tran

		if @isExists = 0  -- внесение нового пользователя
		begin
			insert dbo.Account
				(
				CreateDate
				, UpdateDate
				, UpdateId
				, EditorAccountId
				, EditorIp
				, [Login]
				, PasswordHash
				, LastName
				, FirstName
				, PatronymicName
				, OrganizationId
				, IsOrganizationOwner
				, ConfirmYear
				, Phone
				, Email
				, RegistrationDocument
				, AdminComment
				, IsActive
				, Status
				, IpAddresses
				, HasFixedIp
				)
			select
				getdate()
				, getdate()
				, @updateId
				, @editorAccountId
				, @editorIp
				, @login
				, @passwordHash
				, @lastName
				, @firstName
				, @patronymicName
				, null
				, 0
				, @confirmYear
				, @phone
				, @email
				, null
				, null
				, @isActive
				, @status
				, @ipAddresses
				, @hasFixedIp

			if (@@error <> 0)
				goto undo

			select @accountId = scope_identity()

			if (@@error <> 0)
				goto undo

			insert dbo.AccountIp
				(
				AccountId
				, Ip
				)
			select
				@accountId
				, new_ip_address.ip
			from 
				@newIpAddress new_ip_address

			if (@@error <> 0)
				goto undo

			IF(@userGroupId IS NOT NULL)
			BEGIN
			insert dbo.GroupAccount
				(
				GroupId
				, AccountID
				)
			select
				@userGroupId
				, @accountId
			END
			if (@@error <> 0)
				goto undo

		end
		else
		begin -- update существующего пользователя
			update account
			set
				UpdateDate = getdate()
				, UpdateId = @updateId
				, EditorAccountID = @editorAccountId
				, EditorIp = @editorIp
				, LastName = @lastName
				, FirstName = @firstName
				, PatronymicName = @patronymicName 
				, phone = @phone
				, email = @email
				, IsActive = @isActive
				, IpAddresses = @ipAddresses
				, HasFixedIp = @hasFixedIp
			from
				dbo.Account account with (rowlock)
			where
				account.[Id] = @accountId

			if (@@error <> 0)
				goto undo

			if exists(select 
						1
					from
						@oldIpAddress old_ip_address
							full outer join @newIpAddress new_ip_address
								on old_ip_address.ip = new_ip_address.ip
					where
						old_ip_address.ip is null
						or new_ip_address.ip is null) 
			begin
				delete account_ip
				from 
					dbo.AccountIp account_ip
				where
					account_ip.AccountId = @accountId

				if (@@error <> 0)
					goto undo

				insert dbo.AccountIp
					(
					AccountId
					, Ip
					)
				select
					@accountId
					, new_ip_address.ip
				from 
					@newIpAddress new_ip_address

				if (@@error <> 0)
					goto undo
			end
		end

	if @@trancount > 0
		commit tran insert_update_account_tran

	set @accountIds = convert(nvarchar(255), @accountId)

	exec dbo.RegisterEvent 
		@accountId = @editorAccountId
		, @ip = @editorIp
		, @eventCode = @eventCode
		, @sourceEntityIds = @accountIds
		, @eventParams = null
		, @updateId = @updateId

	return 0

	undo:

	rollback tran insert_update_account_tran

	return 1
end


GO
/****** Object:  StoredProcedure [dbo].[UpdateAccountKey]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
-- exec dbo.UpdateAccountKey
-- ====================================================
-- Проверить ключа.
-- v.1.0: Created by Fomin Dmitriy 01.09.2008
-- ====================================================
CREATE procedure [dbo].[UpdateAccountKey]
	@login nvarchar(255)
	, @key nvarchar(255)
	, @dateFrom datetime
	, @dateTo datetime
	, @isActive bit
	, @editorLogin nvarchar(255)
	, @editorIp nvarchar(255)
as
begin
	declare
		@accountId bigint
		, @editorAccountId bigint
		, @eventCode nvarchar(255)
		, @keyId bigint
		, @updateId	uniqueidentifier
		, @keyIds nvarchar(255)

	set @updateId = newid()
	
	select @accountId = account.Id
	from dbo.Account account
	where account.[Login] = @login

	select @editorAccountId = account.Id
	from dbo.Account account
	where account.[Login] = @editorLogin

	select @keyId = account_key.Id
	from dbo.AccountKey account_key
	where account_key.[Key] = @key
		and account_key.AccountId = @accountId

	if @keyId is null
	begin
		insert into dbo.AccountKey
			(
			CreateDate
			, UpdateDate
			, UpdateId
			, EditorAccountId
			, EditorIp
			, AccountId
			, [Key]
			, DateFrom
			, DateTo
			, IsActive
			)
		select
			GetDate()
			, GetDate()
			, @updateId
			, @editorAccountId
			, @editorip
			, @accountId
			, @key
			, @dateFrom
			, @dateTo
			, @isActive

		set @keyId = scope_identity()

		set @eventCode = 'USR_KEY_CREATE'
	end
	else
	begin
		update account_key
		set
			UpdateDate = GetDate()
			, UpdateId = @updateId
			, EditorAccountId = @editorAccountId
			, EditorIp = @editorIp
			, DateFrom = @dateFrom
			, DateTo = @dateTo
			, IsActive = @isActive
		from dbo.AccountKey account_key
		where account_key.Id = @keyId

		set @eventCode = 'USR_KEY_EDIT'
	end

	set @keyIds = @keyId

	exec dbo.RegisterEvent 
		@accountId = @editorAccountId
		, @ip = @editorIp
		, @eventCode = @eventCode
		, @sourceEntityIds = @keyIds
		, @eventParams = null
		, @updateId = @updateId

	return 0
end
GO
/****** Object:  StoredProcedure [dbo].[UpdateAccountPassword]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- exec dbo.UpdateAccountPassword

-- =============================================
-- Сохранить пароль пользователя.
-- v.1.0: Created by Makarev Andrey 02.04.2008
-- v.1.1: Modified by Makarev Andrey 14.04.2008
-- Добавлен идентификатор обновления UpdateId
-- v.1.2: Modified by Makarev Andrey 14.04.2008
-- Приведение к стандарту
-- v.1.3: Modified by Makarev Andrey 18.04.2008
-- Изменение параметров ХП dbo.RegisterEvent.
-- v.1.4: Modified by Makarev Andrey 04.05.2008
-- Добавлен параметр password для обратной совместимости систем.
-- =============================================
CREATE proc [dbo].[UpdateAccountPassword]
	@login nvarchar(255)
	, @passwordHash nvarchar(255)
	, @editorLogin nvarchar(255)
	, @editorIp nvarchar(255)
	, @password nvarchar(255) = null -- !временно
as
begin

	declare
		@editorAccountId bigint
		, @accountId bigint
		, @updateId	uniqueidentifier
		, @accountIds nvarchar(255)

	set @updateId = newid()

	select 
		@accountId = account.[Id]
	from
		dbo.Account account with (nolock, fastfirstrow)
	where
		account.[Login] = @login

	select 
		@editorAccountId = account.[Id]
	from
		dbo.Account account with (nolock, fastfirstrow)
	where
		account.[Login] = @editorLogin

	update account
	set
		PasswordHash = @passwordHash
		, EditorAccountId = @editorAccountId
		, EditorIp = @editorIp
		, UpdateDate = GetDate()
		, UpdateId = @updateId
	from
		dbo.Account account with (rowlock)
	where
		account.[Id] = @accountId

/*
* -- GVUZ-785 закомментирован блок по причине ошибки. Ошибка происходит, в случае, если у пользователя несколько групп
* 
* 
* -- временно
	if isnull(@password, '') <> '' and N'User' = (select 
						[group].[code]
					from
						dbo.[Group] [group]
							inner join dbo.GroupAccount group_account
								on [group].[Id] = group_account.GroupId
					where
						group_account.AccountId = @accountId)
	begin*/
		if exists(select 
					1
				from
					dbo.UserAccountPassword user_account_password
				where
					user_account_password.AccountId = @accountId)
		begin
			update user_account_password
			set
				[Password] = @password
			from
				dbo.UserAccountPassword user_account_password
			where
				user_account_password.AccountId = @accountId
		end
		else
		begin
			insert dbo.UserAccountPassword
				(
				AccountId
				, [Password]
				)
			select 
				@accountId
				, @password
		end
	/*end*/

	exec dbo.RefreshRoleActivity @accountId = @accountId

	set @accountIds = convert(nvarchar(255), @accountId)

	exec dbo.RegisterEvent 
		@accountId = @editorAccountId
		, @ip = @editorIp
		, @eventCode = N'USR_PASSW'
		, @sourceEntityIds = @accountIds
		, @eventParams = null
		, @updateId = @updateId

	return 0
end

GO
/****** Object:  StoredProcedure [dbo].[UpdateAskedQuestion]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.UpdateAskedQuestion

-- =============================================
-- Сохранение изменений вопроса.
-- v.1.0: Created by Makarev Andrey 22.04.2008
-- =============================================
CREATE proc [dbo].[UpdateAskedQuestion]
	@id bigint output
	, @name nvarchar(255)
	, @question ntext
	, @answer ntext
	, @isActive bit
	, @contextCodes nvarchar(4000)
	, @editorLogin nvarchar(255) = null
	, @editorIp nvarchar(255) = null
as
begin
	declare 
		@newAskedQuestion bit
		, @currentDate datetime
		, @editorAccountId bigint
		, @eventCode nvarchar(100)
		, @updateId	uniqueidentifier
		, @ids nvarchar(255)
		, @oldIsActive bit
		, @public bit
		, @publicEventCode nvarchar(100)
		, @internalId bigint

	set @updateId = newid()

	declare @oldContextId table 
		(
		ContextId int
		)

	declare @newContextId table 
		(
		ContextId int
		)

	insert @newContextId
	select 
		context.Id
	from 
		dbo.GetDelimitedValues(@contextCodes) codes
			inner join dbo.Context context
				on context.Code = codes.[value]

	set @currentDate = getdate()

	select
		@editorAccountId = account.[Id]
	from
		dbo.Account account with (nolock, fastfirstrow)
	where
		account.[Login] = @editorLogin

	if isnull(@id, 0) = 0 -- новая новость
	begin
		set @newAskedQuestion = 1
		set @eventCode = N'FAQ_CREATE'
	end
	else
	begin -- update существующей новости
		set @internalId = dbo.GetInternalId(@id)

		select 
			@oldIsActive = asked_question.IsActive
		from 
			dbo.AskedQuestion asked_question with (nolock, fastfirstrow)
		where
			asked_question.[Id] = @internalId

		insert @oldContextId
			(
			ContextId
			)
		select
			asked_question_context.ContextId
		from
			dbo.AskedQuestionContext asked_question_context with (nolock)
		where
			asked_question_context.AskedQuestionId = @internalId

		set @eventCode = N'FAQ_EDIT'

		if @oldIsActive <> @isActive
		begin
			set @public = 1
			if @isActive = 1
				set @publicEventCode = N'FAQ_PUBLIC'
			else
				set @publicEventCode = N'FAQ_UNPUBLIC'
		end
	end

	begin tran insert_update_faq_tran

		if @newAskedQuestion = 1 -- новый документ
		begin
			insert dbo.AskedQuestion
				(
				CreateDate
				, UpdateDate
				, UpdateId
				, EditorAccountId
				, EditorIp
				, [Name]
				, Question
				, Answer
				, IsActive
				, ViewCount
				, Popularity
				, ContextCodes
				)
			select
				getdate()
				, getdate()
				, @updateId
				, @editorAccountId
				, @editorIp
				, @name
				, @question
				, @answer
				, @isActive
				, 0
				, 0
				, @contextCodes

			if (@@error <> 0)
				goto undo

			set @internalId = scope_identity()
			set @id = dbo.GetExternalId(@internalId)

			if (@@error <> 0)
				goto undo

			insert dbo.AskedQuestionContext
				(
				AskedQuestionId
				, ContextId
				)
			select
				@internalId
				, new_context_id.ContextId
			from 
				@newContextId new_context_id

			if (@@error <> 0)
				goto undo

		end	
		else 
		begin -- update существующего вопроса

			update asked_question
			set
				UpdateDate = GetDate()
				, UpdateId = @updateId
				, EditorAccountId = @editorAccountId
				, EditorIp = @editorIp
				, [Name] = @name
				, Question = @question
				, Answer = @answer
				, IsActive = @isActive
				, ContextCodes = @contextCodes
			from
				dbo.AskedQuestion asked_question with (rowlock)
			where
				asked_question.[Id] = @internalId

			if (@@error <> 0)
				goto undo

			if exists(select 
						1
					from
						@oldContextId old_context_id
							full outer join @newContextId new_context_id
								on old_context_id.ContextId = new_context_id.ContextId
					where
						old_context_id.ContextId is null
						or new_context_id.ContextId is null) 
			begin
				delete asked_question_context
				from 
					dbo.AskedQuestionContext asked_question_context
				where
					asked_question_context.AskedQuestionId = @internalId

				if (@@error <> 0)
					goto undo

				insert dbo.AskedQuestionContext
					(
					AskedQuestionId
					, ContextId
					)
				select
					@internalId
					, new_context_id.ContextId
				from 
					@newContextId new_context_id

				if (@@error <> 0)
					goto undo
			end
		end	

	if @@trancount > 0
		commit tran insert_update_faq_tran

	set @ids = convert(nvarchar(255), @internalId)

	exec dbo.RegisterEvent 
		@accountId = @editorAccountId
		, @ip = @editorIp
		, @eventCode = @eventCode
		, @sourceEntityIds = @ids
		, @eventParams = null
		, @updateId = @updateId

	if @public = 1
		exec dbo.RegisterEvent 
			@accountId = @editorAccountId
			, @ip = @editorIp
			, @eventCode = @publicEventCode
			, @sourceEntityIds = @ids
			, @eventParams = null
			, @updateId = @updateId

	return 0

	undo:

	rollback tran insert_update_faq_tran

	return 1

end
GO
/****** Object:  StoredProcedure [dbo].[UpdateConnectStatV2Scheme]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateConnectStatV2Scheme]
AS
BEGIN
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 21524;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 376;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 376;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 21986;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2581;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 341;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 20942;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2398;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 753;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1594;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2129;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 271;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3296;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 193;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 21721;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 101;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1018;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1022;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1025;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1029;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1031;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1032;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1061;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1063;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1068;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1074;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1082;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1083;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1084;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1085;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1088;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1088;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1093;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1095;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1097;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 11;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 11;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 11;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 11;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 11;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 9129;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 11;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1102;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1112;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1112;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1119;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1119;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 113;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1134;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1145;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1147;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 115;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1154;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1155;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 116;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1162;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1165;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1166;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1173;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1174;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 118;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 12;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1200;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1205;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1215;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1229;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 123;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1231;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1241;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1247;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 125;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 125;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1254;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 129;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1294;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 13;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 13;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1304;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1307;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1318;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1328;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1330;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1333;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1336;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 138;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1389;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 139;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1391;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1392;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1396;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1397;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1399;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 140;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 141;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1413;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1415;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1433;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1434;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 147;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1479;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1480;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1481;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 150;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1509;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1522;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1527;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1531;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1533;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1538;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1550;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1554;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1555;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1557;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1562;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 157;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 158;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 159;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1595;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1595;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1600;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 161;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 162;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1621;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1635;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1638;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1657;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1663;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1665;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1689;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 169;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1693;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1696;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 170;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1718;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1726;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1728;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1729;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1731;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1738;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1739;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1740;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1747;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1775;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1776;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 179;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1790;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1794;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1794;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1795;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1796;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1804;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1805;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 181;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1829;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 183;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1837;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1838;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 184;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1840;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1848;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1848;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1848;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1854;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1858;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1862;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1865;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1868;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1869;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1871;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 188;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 189;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 190;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 190;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1906;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1912;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1921;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1929;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1930;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1932;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1935;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1939;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1941;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1943;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1951;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1955;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1959;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1960;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1961;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1963;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 19646;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 197;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1991;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1992;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1992;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2105;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1996;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1998;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 20;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2003;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2005;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 201;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2015;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2029;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 203;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2035;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 204;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2051;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2056;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2058;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 206;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2063;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2068;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2069;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2074;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2089;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2091;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 20910;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 20937;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 20942;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 20942;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 20942;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2095;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 20979;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 20986;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 20994;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 210;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2127;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2127;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2128;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 214;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 214;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 215;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2150;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2162;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2163;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 21635;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2171;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 21714;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2181;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2182;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2195;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 22;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2222;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2226;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2257;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2261;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2262;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2273;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2273;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2273;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2283;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2287;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 23;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 23;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 23;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 231;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2324;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2325;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2333;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2334;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2340;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2342;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2345;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2371;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2375;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2376;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2377;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2378;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2385;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2389;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2390;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2399;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 240;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2403;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2416;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2436;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2438;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2441;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2444;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2448;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2454;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2457;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2459;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 246;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 247;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 247;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 247;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2472;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2473;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2476;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2477;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 248;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2484;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2486;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2489;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2489;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2490;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2496;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2500;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2509;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 251;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2516;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2525;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2527;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2539;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 254;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2547;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 256;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2561;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 257;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2581;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2585;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2598;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2624;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2634;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2635;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2639;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2640;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2641;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2644;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 265;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2650;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 266;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2663;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 267;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2686;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2693;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 27;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 27;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 27;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 27;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2705;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2706;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 271;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2716;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 273;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 274;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2741;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2765;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2772;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2777;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2779;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2795;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 282;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 282;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 282;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 282;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 282;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 282;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 282;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 282;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 282;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 282;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2833;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2835;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2837;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2842;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2842;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 285;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 286;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 286;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 286;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2867;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2872;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 288;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2880;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2887;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 289;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 289;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 289;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 289;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 290;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 292;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 292;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 293;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2937;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2937;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2937;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2937;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2937;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2938;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2938;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2938;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 294;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 294;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2941;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2941;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2941;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2941;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2945;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2952;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 296;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 296;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2962;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2963;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2966;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2968;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 297;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2971;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2973;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 298;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2980;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2980;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2980;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2980;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2987;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2989;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2993;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2996;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2999;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 30;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3001;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3001;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3008;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3013;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3015;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3018;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3019;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3023;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3025;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3027;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3033;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3038;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 304;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3042;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3044;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3053;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3059;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3073;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3077;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 311;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 311;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3113;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3113;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3117;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 312;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3120;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3121;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3122;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3123;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3128;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 313;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3139;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 314;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3158;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3167;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3169;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 317;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3170;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 318;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 319;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3198;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 32;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3200;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 323;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3233;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3234;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 324;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3250;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3256;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3257;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3258;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3259;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3259;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 326;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3264;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3269;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3284;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3287;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3287;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3291;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3293;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3294;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 33;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3312;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3312;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3313;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3319;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 333;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3330;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3331;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 334;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 335;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 335;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 335;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 335;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3363;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 337;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3370;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3376;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3377;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3382;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3385;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3388;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 339;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3392;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3404;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3406;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3413;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3416;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3424;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3431;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3441;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3443;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3446;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3457;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 346;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3470;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 351;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 351;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 351;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 351;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 351;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 351;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 351;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3510;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3511;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 356;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 357;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 358;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 358;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 358;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 36;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 364;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 366;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 368;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 37;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 370;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 374;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 380;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 383;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 387;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 39;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 392;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3938;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 394;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 399;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 402;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 412;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 413;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 419;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 424;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 428;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 43;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 430;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 432;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 432;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 433;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 434;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 435;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 44;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 440;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 446;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 448;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 45;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 450;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 450;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 451;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 452;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 461;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 471;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 475;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 476;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 483;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 485;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 488;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 49;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 506;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 509;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 512;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 517;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 523;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 527;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 530;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 538;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 5398;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 540;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 541;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 551;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 552;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 553;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 554;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 556;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 560;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 564;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 568;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 568;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 572;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 573;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 574;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 581;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 589;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 590;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 591;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 592;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 596;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 60;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 604;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 607;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 609;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 610;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 62;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 62;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 625;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 625;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 63;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 630;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 631;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 633;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 636;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 64;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 64;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 642;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 643;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 65;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 650;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 654;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 664;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 664;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 664;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 668;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 669;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 669;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 670;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 671;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 672;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 673;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 677;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 679;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 680;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 681;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 683;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 69;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 690;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 699;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 703;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 705;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 706;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 715;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 718;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 719;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 728;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 729;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 731;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 733;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 733;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 740;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 751;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 752;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 755;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 756;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 759;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 76;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 761;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 762;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 770;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 78;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 790;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 797;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 802;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 804;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 813;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 814;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 814;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 814;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 815;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 824;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 8286;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 8290;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 8297;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 8304;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 835;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 845;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 847;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 847;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 85;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 858;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 860;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 8700;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 8735;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 8747;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 8757;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 8768;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 8781;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 8788;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 8806;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 8823;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 887;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 888;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 8903;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 891;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 892;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 8923;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 8927;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 893;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 8938;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 8942;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 8943;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 895;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 9;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 90;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 9171;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 9172;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 933;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 94;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 9400;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 9402;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 9406;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 9420;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 9424;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 9425;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 9433;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 9434;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 9439;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 9451;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 9453;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 9455;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 9540;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 977;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 977;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 977;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 978;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 988;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 99;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 994;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 10;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1065;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1076;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1090;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1107;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 111;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 114;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1201;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1209;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1253;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1263;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1295;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1296;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1308;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1311;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1314;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1335;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1359;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1414;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1420;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1424;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1435;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1455;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 156;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1598;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1752;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 176;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1798;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1799;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1832;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 191;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1936;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 198;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 202;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 202;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 202;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 202;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2076;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2092;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 20971;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 21002;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2132;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 21389;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 21469;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2147;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 21501;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 21517;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 21521;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 21525;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2186;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2191;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2238;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2326;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2373;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2386;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2426;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2431;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2434;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2436;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2460;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2478;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2496;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 253;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2541;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2586;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 259;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2610;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2658;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2711;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2722;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2732;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2739;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 274;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2778;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2801;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2864;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2873;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2958;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2958;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2958;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2958;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2972;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1512;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3065;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3124;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3136;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 318;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3197;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3218;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3290;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3332;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3335;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 334;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3393;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3437;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3442;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3458;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3472;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 355;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 357;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3757;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 458;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 495;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 496;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 501;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 519;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 534;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 535;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 570;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 605;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 606;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 654;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 654;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 666;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 674;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 6877;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 713;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 732;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 805;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 82;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 8200;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 8698;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 8701;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 8767;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 8789;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 8789;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 8891;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 9396;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 9436;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 9440;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 9458;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 9460;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 989;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 989;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1279;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 542;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 21759;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 21685;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 21757;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1397;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1594;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 21730;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1234;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 21739;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 844;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1045;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 8696;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 562;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2069;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1991;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1750;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2352;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 729;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3385;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2015;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2893;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3397;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1525;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 9399;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1064;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 717;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 889;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3307;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 21043;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2379;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 21525;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3019;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1030;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 21669;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2638;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 8765;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2181;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 718;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2648;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2590;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2287;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1208;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 8931;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2500;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 21664;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 655;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2483;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 621;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 21789;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2182;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 21799;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2370;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2738;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 21812;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 21798;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 21806;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 389;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1504;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1221;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 8723;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 419;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 307;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 332;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 308;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 612;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1186;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3166;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1757;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 9526;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 20942;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 419;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3019;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2137;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2502;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1083;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 135;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 1954;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2765;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3020;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3020;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 3107;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 367;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 557;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 8767;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 9451;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 1' where InstEsrpID= 2693;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 6;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 7;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 10;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 19;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 37;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 38;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 39;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 42;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 50;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 56;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 57;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 59;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 66;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 67;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 71;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 72;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 86;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 92;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 101;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 106;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 107;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 109;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 111;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 115;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 134;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 143;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 145;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 150;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 153;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 169;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 170;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 179;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 180;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 186;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 188;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 190;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 204;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 209;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 210;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 217;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 227;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 228;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 230;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 232;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 243;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 249;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 261;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 262;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 264;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 282;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 307;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 309;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 310;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 311;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 313;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 314;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 321;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 322;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 324;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 326;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 327;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 329;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 330;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 331;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 334;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 336;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 338;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 342;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 344;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 352;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 367;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 375;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 378;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 381;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 392;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 396;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 398;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 409;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 414;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 432;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 435;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 439;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 443;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 447;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 456;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 457;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 458;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 459;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 468;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 469;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 470;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 475;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 476;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 480;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 481;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 482;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 489;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 495;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 499;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 502;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 503;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 511;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 516;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 532;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 533;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 542;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 547;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 557;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 561;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 562;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 563;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 567;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 572;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 582;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 586;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 588;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 589;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 591;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 593;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 595;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 610;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 611;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 615;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 617;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 618;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 647;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 650;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 656;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 658;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 660;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 664;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 677;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 699;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 728;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 730;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 731;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 763;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 798;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 831;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 846;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 848;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 849;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 853;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 929;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 987;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 1001;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 1015;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 1028;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 1052;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 1077;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 1085;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 1105;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 1128;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 1157;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 1167;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 1186;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 1206;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 1221;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 1228;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 1230;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 1280;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 1304;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 1332;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 1390;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 1409;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 1413;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 1414;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 1435;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 1436;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 1494;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 1556;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 1558;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 1624;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 1659;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 1690;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 1692;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 1739;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 1752;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 1760;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 1780;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 1796;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 1797;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 1802;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 1833;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 1854;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 1855;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 1863;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 1936;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 1952;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 1954;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 1992;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 1993;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 2005;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 2045;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 2063;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 2074;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 2083;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 2182;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 2231;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 2262;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 2282;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 2312;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 2326;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 2333;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 2337;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 2434;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 2477;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 2478;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 2483;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 2496;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 2498;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 2541;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 2546;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 2593;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 2640;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 2643;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 2666;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 2671;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 2680;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 2706;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 2715;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 2716;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 2727;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 2762;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 2776;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 2801;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 2807;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 2866;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 2945;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 2946;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 2947;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 2948;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 2961;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 2970;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 2985;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 2986;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 3004;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 3006;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 3007;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 3010;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 3011;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 3012;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 3015;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 3017;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 3024;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 3036;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 3039;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 3040;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 3045;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 3051;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 3053;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 3055;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 3060;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 3064;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 3080;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 3140;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 3186;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 3191;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 3197;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 3236;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 3238;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 3292;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 3379;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 3431;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 3441;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 3461;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 3483;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 4715;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 8723;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 8735;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 8747;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 8763;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 8772;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 8824;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 8831;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 8834;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 8902;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 8942;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 9172;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 9399;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 9515;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 9526;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 19646;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 19910;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 20970;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 20979;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 21459;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 21714;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 21855;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 21929;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 21967;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 21974;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 21978;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 21991;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 21992;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 21996;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 22009;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 22014;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 22015;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 22017;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 22029;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 22050;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 22051;
	update [esrp_prod].[dbo].[rpt_ConnectStatV2] set Scheme='По схеме № 1 вариант 2' where InstEsrpID= 22086;
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateDelivery]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Создание или редактирование рассылки.
-- v.1.0: Created by Yusupov Kirill 16.04.2010
-- =============================================
CREATE proc [dbo].[UpdateDelivery]
	@id bigint output
	, @title nvarchar(255)
	, @message nvarchar(4000)
	, @deliveryDate datetime
	, @deliveryType nvarchar(20)
	, @recipientIds nvarchar(max)
	, @editorLogin nvarchar(255) 
	, @editorIp nvarchar(255)
as
begin
	declare @eventCode nvarchar(100)
	
	if isnull(@id, 0) = 0 -- новая рассылка
	begin
		insert dbo.Delivery
			(
			Title
			, [Message]
			, DeliveryDate
			, TypeCode
			)
		select
			@title
			, @message
			, @deliveryDate
			, @deliveryType

		set @id = scope_identity()
		set @eventCode= N'DLV_CREATE'
	end	
	else 
	begin -- update существующей рассылки
		update delivery
		set
			Title = @title
			, [Message] = @message
			, DeliveryDate = @deliveryDate
			, TypeCode = @deliveryType
		from
			dbo.Delivery delivery with (rowlock)
		where
			delivery.[Id] = @id
		
		set @eventCode= N'DLV_EDIT'
	end	

	--Удалим старых получателей рассылки
	delete from dbo.DeliveryRecipients where DeliveryId = @id
	
	if (@recipientIds is not null)
	begin
		--[value] - recipientCode, @internalId - Id рассылки
		insert into dbo.DeliveryRecipients select [value],@id from dbo.GetDelimitedValues(@recipientIds)
	end

	declare @editorAccountId bigint
	select
		@editorAccountId = account.[Id]
	from
		dbo.Account account with (nolock, fastfirstrow)
	where
		account.[Login] = @editorLogin

	declare @updateId uniqueidentifier
	set @updateId = newid()

	exec dbo.RegisterEvent 
		@accountId = @editorAccountId
		, @ip = @editorIp
		, @eventCode = @eventCode
		, @sourceEntityIds = @id
		, @eventParams = null
		, @updateId = @updateId

	return 0
end




GO
/****** Object:  StoredProcedure [dbo].[UpdateDocument]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.UpdateDocument

-- =============================================
-- Сохранение изменений документа.
-- v.1.0: Created by Makarev Andrey 18.04.2008
-- v.1.1: Modified by Fomin Dmitriy 22.04.2008
-- Передаются не ИД контекстов, а коды.
-- v.1.2: Modified by Fomin Dmitriy 24.04.2008
-- Добавлено поле Alias.
-- v.1.3: Modified by Fomin Dmitriy 24.04.2008
-- Alias переименовано в RelativeUrl.
-- =============================================
CREATE proc [dbo].[UpdateDocument]
	@id bigint output
	, @name nvarchar(255)
	, @description ntext
	, @content image
	, @contentSize int
	, @contentType nvarchar(255)
	, @isActive bit
	, @contextCodes nvarchar(4000)
	, @relativeUrl nvarchar(255)
	, @editorLogin nvarchar(255) = null
	, @editorIp nvarchar(255) = null
as
begin
	declare 
		@newDocument bit
		, @currentDate datetime
		, @editorAccountId bigint
		, @eventCode nvarchar(100)
		, @updateId	uniqueidentifier
		, @ids nvarchar(255)
		, @activateDate datetime
		, @oldIsActive bit
		, @public bit
		, @publicEventCode nvarchar(100)
		, @internalId bigint

	set @updateId = newid()
	
	declare @oldContextId table 
		(
		ContextId int
		)

	declare @newContextId table 
		(
		ContextId int
		)

	insert @newContextId
	select 
		context.Id
	from 
		dbo.GetDelimitedValues(@contextCodes) codes
			inner join dbo.Context context
				on context.Code = codes.[value]

	set @currentDate = getdate()

	select
		@editorAccountId = account.[Id]
	from
		dbo.Account account with (nolock, fastfirstrow)
	where
		account.[Login] = @editorLogin

	if isnull(@id, 0) = 0 -- новый документ
	begin
		set @newDocument = 1
		set @eventCode = N'DOC_CREATE'
		if @isActive = 1
			set @activateDate = @currentDate
	end
	else
	begin -- update существующего документа
		set @internalId = dbo.GetInternalId(@id)

		select 
			@oldIsActive = [document].IsActive
		from 
			dbo.[Document] [document] with (nolock, fastfirstrow)
		where
			[document].[Id] = @internalId

		insert @oldContextId
			(
			ContextId
			)
		select
			document_context.ContextId
		from
			dbo.DocumentContext document_context with (nolock)
		where
			document_context.DocumentId = @internalId

		set @eventCode = N'DOC_EDIT'

		if @oldIsActive <> @isActive
		begin
			set @public = 1
			if @isActive = 1
				select 
					@publicEventCode = N'DOC_PUBLIC'
					, @activateDate = @currentDate
			else
				select 
					@publicEventCode = N'DOC_UNPUBLIC'
					, @activateDate = null
		end
	end

	begin tran insert_update_document_tran

		if @newDocument = 1 -- новый документ
		begin
			insert dbo.[Document]
				(
				CreateDate
				, UpdateDate
				, UpdateId
				, EditorAccountId
				, EditorIp
				, [Name]
				, Description
				, [Content]
				, ContentSize
				, ContentType
				, IsActive
				, ActivateDate
				, ContextCodes
				, RelativeUrl
				)
			select
				getdate()
				, getdate()
				, @updateId
				, @editorAccountId
				, @editorIp
				, @name
				, @description
				, @content
				, @contentSize
				, @contentType
				, @isActive
				, @activateDate
				, @contextCodes
				, @relativeUrl

			if (@@error <> 0)
				goto undo

			set @internalId = scope_identity()
			set @id = dbo.GetExternalId(@internalId)

			if (@@error <> 0)
				goto undo

			insert dbo.DocumentContext
				(
				DocumentId
				, ContextId
				)
			select
				@internalId
				, new_context_id.ContextId
			from 
				@newContextId new_context_id

			if (@@error <> 0)
				goto undo

		end	
		else 
		begin -- update существующего документа

			update [document]
			set
				UpdateDate = GetDate()
				, UpdateId = @updateId
				, EditorAccountId = @editorAccountId
				, EditorIp = @editorIp
				, [Name] = @name
				, Description = @description
				, [Content] = @content
				, ContentSize = @contentSize
				, ContentType = @contentType
				, IsActive = @isActive
				, ActivateDate = case
						when @public = 1 then @activateDate
						else [document].ActivateDate
				end
				, ContextCodes = @contextCodes
				, RelativeUrl = @relativeUrl
			from
				dbo.[Document] [document] with (rowlock)
			where
				[document].[Id] = @internalId

			if (@@error <> 0)
				goto undo

			if exists(select 
						1
					from
						@oldContextId old_context_id
							full outer join @newContextId new_context_id
								on old_context_id.ContextId = new_context_id.ContextId
					where
						old_context_id.ContextId is null
						or new_context_id.ContextId is null) 
			begin
				delete document_context
				from 
					dbo.DocumentContext document_context
				where
					document_context.DocumentId = @internalId

				if (@@error <> 0)
					goto undo

				insert dbo.DocumentContext
					(
					DocumentId
					, ContextId
					)
				select
					@internalId
					, new_context_id.ContextId
				from 
					@newContextId new_context_id

				if (@@error <> 0)
					goto undo
			end
		end	

	if @@trancount > 0
		commit tran insert_update_document_tran

	set @ids = convert(nvarchar(255), @internalId)

	exec dbo.RegisterEvent 
		@accountId = @editorAccountId
		, @ip = @editorIp
		, @eventCode = @eventCode
		, @sourceEntityIds = @ids
		, @eventParams = null
		, @updateId = @updateId

	if @public = 1
		exec dbo.RegisterEvent 
			@accountId = @editorAccountId
			, @ip = @editorIp
			, @eventCode = @publicEventCode
			, @sourceEntityIds = @ids
			, @eventParams = null
			, @updateId = @updateId

	return 0

	undo:

	rollback tran insert_update_document_tran

	return 1

end
GO
/****** Object:  StoredProcedure [dbo].[UpdateNews]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- exec dbo.UpdateNews

-- =============================================
-- Сохранение изменений новости.
-- v.1.0: Created by Makarev Andrey 21.04.2008
-- =============================================
CREATE proc [dbo].[UpdateNews]
	@id bigint output
	, @date datetime
	, @name nvarchar(255)
	, @description ntext
	, @text ntext
	, @isActive bit
	, @editorLogin nvarchar(255) = null
	, @editorIp nvarchar(255) = null
as
begin
	declare 
		@newNews bit
		, @currentDate datetime
		, @editorAccountId bigint
		, @eventCode nvarchar(100)
		, @updateId	uniqueidentifier
		, @ids nvarchar(255)
		, @oldIsActive bit
		, @public bit
		, @publicEventCode nvarchar(100)
		, @internalId bigint

	set @updateId = newid()
	
	set @currentDate = getdate()

	select
		@editorAccountId = account.[Id]
	from
		dbo.Account account with (nolock, fastfirstrow)
	where
		account.[Login] = @editorLogin

	if isnull(@id, 0) = 0 -- новая новость
	begin
		set @newNews = 1
		set @eventCode = N'NWS_CREATE'
	end
	else
	begin -- update существующего документа
		set @internalId = dbo.GetInternalId(@id)

		select 
			@oldIsActive = news.IsActive
		from 
			dbo.News news with (nolock, fastfirstrow)
		where
			news.[Id] = @internalId

		set @eventCode = N'NWS_EDIT'

		if @oldIsActive <> @isActive
		begin
			set @public = 1
			if @isActive = 1
				set @publicEventCode = N'DOC_PUBLIC'
			else
				set @publicEventCode = N'DOC_UNPUBLIC'
		end
	end

	begin tran insert_update_news_tran

		if @newNews = 1 -- новая новость
		begin
			insert dbo.News
				(
				CreateDate
				, UpdateDate
				, UpdateId
				, EditorAccountId
				, EditorIp
				, Date
				, [Name]
				, Description
				, [Text]
				, IsActive
				)
			select
				getdate()
				, getdate()
				, @updateId
				, @editorAccountId
				, @editorIp
				, @date
				, @name
				, @description
				, @text
				, @isActive

			if (@@error <> 0)
				goto undo

			set @internalId = scope_identity()
			set @id = dbo.GetExternalId(@internalId)

			if (@@error <> 0)
				goto undo
		end	
		else 
		begin -- update существующей новости

			update news
			set
				UpdateDate = GetDate()
				, UpdateId = @updateId
				, EditorAccountId = @editorAccountId
				, EditorIp = @editorIp
				, Date = @date
				, [Name] = @name
				, Description = @description
				, [Text] = @text
				, IsActive = @isActive
			from
				dbo.News news with (rowlock)
			where
				news.[Id] = @internalId

			if (@@error <> 0)
				goto undo
		end	

	if @@trancount > 0
		commit tran insert_update_news_tran

	set @ids = convert(nvarchar(255), @internalId)

	exec dbo.RegisterEvent 
		@accountId = @editorAccountId
		, @ip = @editorIp
		, @eventCode = @eventCode
		, @sourceEntityIds = @ids
		, @eventParams = null
		, @updateId = @updateId

	if @public = 1
		exec dbo.RegisterEvent 
			@accountId = @editorAccountId
			, @ip = @editorIp
			, @eventCode = @publicEventCode
			, @sourceEntityIds = @ids
			, @eventParams = null
			, @updateId = @updateId

	return 0

	undo:

	rollback tran insert_update_news_tran

	return 1

end
GO
/****** Object:  StoredProcedure [dbo].[UpdateOrganizationRequestStatus]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UpdateOrganizationRequestStatus]	
	  @orgRequestID INT
	, @statusID INT
	, @needConsiderLinkedUsers BIT = 1 -- если 1, то меняем статусы прикрепленных пользователей к заявке
	, @comment VARCHAR(MAX)
	, @editorLogin nvarchar(255)
	, @editorIp nvarchar(255)
AS
BEGIN	
	DECLARE
		@curRequestStatus INT,
		@statusCode NVARCHAR(1020),
		@editorID INT
	
	SELECT @editorID = id FROM Account WHERE [login] = @editorLogin 
	SELECT @statusCode = Code FROM AccountStatus WHERE StatusID = @statusID		
	SELECT @curRequestStatus = or1.StatusID
	FROM OrganizationRequest2010 or1
	WHERE or1.Id = @statusID	
		
	if @statusID=5 and not exists(SELECT * 
				  FROM OrganizationRequest2010 or1          
						join Organization2010 org on org.id=or1.OrganizationId       
						join Organization2010 orgm on orgm.id=org.MainId
						join OrganizationRequest2010 orm on orgm.id=orm.OrganizationId
						join Account ac on ac.OrganizationId=orm.Id 
                  Where or1.Id = @orgRequestID and (ac.Status = 'activated' OR ac.Status='deactivated'))
				  and exists(SELECT * 
				  FROM OrganizationRequest2010 or1          
						join Organization2010 org on org.id=or1.OrganizationId       
				  where org.MainId is not null and or1.Id = @orgRequestID)
    begin
		RAISERROR('$Невозможно активировать пользователя, так как у головного учреждения нет активированных пользователей', 18, 1)
		RETURN -1     
    end
                    		
	-- если активируют проверить на допустимые статусы
	IF (@statusID = 5 AND NOT EXISTS(SELECT * FROM OrganizationRequest2010 or1 
	                                 WHERE or1.Id = @orgRequestID AND or1.StatusID IN (2,3,6)))
	BEGIN
		RAISERROR('$Невозможно активировать заявку в текущем статусе.', 18, 1)
		RETURN -1
	END		
			
	-- GVUZ-595 Исключить подтверждение заявки без приложенных документов.
	IF (@statusID = 5 AND EXISTS(SELECT * FROM Account WHERE OrganizationId = @orgRequestID AND RegistrationDocument IS NULL))
	BEGIN
		RAISERROR('$Невозможно активировать заявку, т.к. не приложены сканы документов для регистрируемых пользователей.', 18, 1)
		RETURN -1
	END			

	-- если деактивируют проверить на допустимые статусы
	IF (@statusID = 6 AND NOT EXISTS(SELECT * FROM OrganizationRequest2010 or1 
	                                 WHERE or1.Id = @orgRequestID AND or1.StatusID IN (1,2,3,5)))
	BEGIN
		RAISERROR('$Невозможно деактивировать заявку в текущем статусе.', 18, 1)
		RETURN -1
	END		

	-- если отправляют на доработку проверить на допустимые статусы
	IF (@statusID = 3 AND NOT EXISTS(SELECT * FROM OrganizationRequest2010 or1 
	                                 WHERE or1.Id = @orgRequestID AND or1.StatusID IN (1,2,3,5,6)))
	BEGIN
		RAISERROR('$Невозможно отправить на доработку заявку в текущем статусе.', 18, 1)
		RETURN -1
	END
	
	BEGIN TRAN	
		-- смена статуса заявления
		-- GVUZ-810 При отправке на доработку ('revision') из статусов Registration и Revision статус не меняется.
		UPDATE OrganizationRequest2010 
		SET StatusID = CASE WHEN @statusCode = 'revision' AND StatusID IN (1, 3) THEN StatusID ELSE @statusID END		
		WHERE id = @orgRequestID
		
		DECLARE 
			@suggestedRCModelID INT,
			@suggestedRCDescription NVARCHAR(400),
			@orgID INT
		
		IF (@statusID = 5)
		BEGIN
			SELECT @orgID = OrganizationId, @suggestedRCModelID = RCModelID, @suggestedRCDescription = RCDescription			
			FROM dbo.OrganizationRequest2010
			WHERE Id = @orgRequestID
			
			UPDATE dbo.Organization2010
			SET
				RCModel = CASE WHEN @suggestedRCModelID IS NULL THEN 999 ELSE @suggestedRCModelID END,
				RCDescription = @suggestedRCDescription
			WHERE Id = @orgID
		END
		
		if (@@error <> 0) goto undo
				
		-- если учитываем пользователей в заявке, то идем по всем пользователям заявления и меняем им статус на новый статус для заявления
		if(@needConsiderLinkedUsers = 1)
		BEGIN
			DECLARE @curUserLogin NVARCHAR(510)			
			DECLARE linkedUsers CURSOR FOR
			  SELECT a.[login] FROM Account a WHERE a.OrganizationId = @orgRequestID
			
			OPEN linkedUsers
			FETCH NEXT FROM linkedUsers INTO @curUserLogin
			
			WHILE(@@FETCH_STATUS = 0)
			BEGIN				
				EXEC [UpdateUserAccountStatus] @login = @curUserLogin, @status = @statusCode, @adminComment = @comment,
				  @editorLogin = @editorLogin, @editorIp = @editorIp, @changeStatusByOrganizationRequest = 1
				if (@@error <> 0) goto undo
				FETCH NEXT FROM linkedUsers INTO @curUserLogin
			END			
			
			CLOSE linkedUsers
			DEALLOCATE linkedUsers
		END
		
	if @@trancount > 0 commit tran

	DECLARE @requestIds nvarchar(1024), @eventCode nvarchar(255), @updateId	UNIQUEIDENTIFIER
	set @updateId = newid()
	set @eventCode = N'REQ_STATUS'	
	set @requestIds = convert(nvarchar(1024), @orgRequestID)	
	exec dbo.RegisterEvent 
		  @accountId = @editorId
		, @ip = @editorIp
		, @eventCode = @eventCode
		, @sourceEntityIds = @requestIds
		, @eventParams = null
		, @updateId = @updateId
	
	RETURN 0
		
	undo:
		CLOSE linkedUsers
		DEALLOCATE linkedUsers	
		if @@trancount > 0 rollback
		return 1
END

GO
/****** Object:  StoredProcedure [dbo].[UpdateUserAccountRegistrationDocument]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- exec dbo.UpdateUserAccountRegistrationDocument

-- =============================================
-- Изменить регистрационный документ пользователя.
-- v.1.0: Created by Makarev Andrey 03.04.2008
-- v.1.1: Modified by Makarev Andrey 14.04.2008
-- Добавлен идентификатор обновления UpdateId
-- v.1.2: Modified by Makarev Andrey 14.04.2008
-- Приведение к стандарту
-- v.1.3: Modified by Makarev Andrey 18.04.2008
-- Изменение параметров ХП dbo.RegisterEvent.
-- v.1.4: Modified by Makarev Andrey 21.04.2008
-- Добавлены хинты.
-- v.1.5: Modified by Fomin Dmitriy 15.05.2008
-- Добавлены Status output-параметр.
-- =============================================
CREATE proc [dbo].[UpdateUserAccountRegistrationDocument]
	@login nvarchar(255)
	, @registrationDocument image
	, @registrationDocumentContentType nvarchar(255)
	, @status nvarchar(255) output
	, @editorLogin nvarchar(255)
	, @editorIp nvarchar(255)
as
begin
	declare
		@accountId bigint
		, @editorAccountId bigint
		, @currentYear int
		, @updateId	uniqueidentifier
		, @accountIds nvarchar(255)

	set @updateId = newid()

	set @currentYear = Year(GetDate())

	select
		@accountId = a.[Id]
		, @status = dbo.GetUserStatus(a.ConfirmYear, isnull(@status, a.Status), @currentYear, @registrationDocument)
	from 
		dbo.Account a with (nolock, fastfirstrow)
	where 
		a.[Login] = @login

	DECLARE @orgRequestID INT, @isRegistrationDocumentExistsForUser bit
	select
		@editorAccountId = account.[Id],
		@orgRequestID = account.OrganizationId,
		@isRegistrationDocumentExistsForUser = CASE WHEN account.RegistrationDocument is null THEN 0 ELSE 1 END
	from 
		dbo.Account account with (nolock, fastfirstrow)
	where 
		account.[Login] = @editorLogin	
	
	BEGIN TRAN
		update account
		SET
			UpdateDate = GetDate()
			, UpdateId = @updateId
			, EditorAccountId = @editorAccountId
			, EditorIp = @editorIp
			, RegistrationDocument = @registrationDocument
			, RegistrationDocumentContentType = @registrationDocumentContentType
			, [Status] = @status
		from 
			dbo.Account account with (rowlock)
		where 
			account.[Id] = @accountId
		if (@@error <> 0)
			goto undo			
				
		-- GVUZ-805 при приложении документа пользователю, если все пользователи имеют скан, то меняем статус заявки на consideration.
		-- определяем, что если это последний пользователь, которому приложили документ, то переводим заявку в статус Consideration
		IF(@isRegistrationDocumentExistsForUser = 0 AND @registrationDocument IS NOT NULL)
		BEGIN
			IF NOT EXISTS(SELECT * FROM Account WHERE OrganizationId = @orgRequestID AND RegistrationDocument IS NULL)
			BEGIN
				UPDATE OrganizationRequest2010 SET StatusID = 2 WHERE Id = @orgRequestID
			END
		END
				
		exec dbo.RefreshRoleActivity @accountId = @accountId

		set @accountIds = convert(nvarchar(255), @accountId)
	
	if @@trancount > 0 commit TRAN	

	exec dbo.RegisterEvent 
		@accountId = @editorAccountId
		, @ip = @editorIp
		, @eventCode = N'USR_EDIT'
		, @sourceEntityIds = @accountIds
		, @eventParams = null
		, @updateId = @updateId

	if @registrationDocument is not null
	begin
		RAISERROR (N'
		Загружена новая заявка на регистрацию:
		Пользователь: %s (https://www.fbsege.ru/Administration/Accounts/Users/View.aspx?login=%s)
		

		----------------------------------------
		Данное письмо не является сообщением об ошибке, а служит для оповещения операторов.
		', 7, 2, @login, @login) with log
	END

	return 0
	
	undo:
		rollback tran
		return 1
	
end

GO
/****** Object:  StoredProcedure [dbo].[UpdateUserAccountStatus]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =========================================================================
-- exec dbo.UpdateUserAccountStatus

-- =============================================
-- Изменить статус пользователя.
-- v.1.0: Created by Makarev Andrey 08.04.2008
-- v.1.1: Modified by Fomin Dmitriy 10.04.2008
-- Рефакторинг: выделена функция, изменеа логика 
-- означивания полей.
-- v.1.2: Modified by Fomin Dmitriy 11.04.2008
-- Удаляется документ регистрации, если он устарел.
-- v.1.3: Modified by Makarev Andrey 14.04.2008
-- Добавлен идентификатор обновления UpdateId.
-- v.1.4: Modified by Makarev Andrey 14.04.2008
-- Приведение к стандарту.
-- v.1.5: Modified by Makarev Andrey 18.04.2008
-- Изменение параметров ХП dbo.RegisterEvent.
-- =============================================
CREATE proc [dbo].[UpdateUserAccountStatus]
  @login nvarchar(255)
  , @status nvarchar(255)
  , @adminComment ntext 
  , @editorLogin nvarchar(255)
  , @editorIp nvarchar(255)
  , @changeStatusByOrganizationRequest BIT = 0 -- 1 если статус пользователя меняется через заявку на регистрацию
                         -- в этом случае игнорируем часть проверок
as
BEGIN
  declare
    @isActive bit
    , @eventCode nvarchar(255)
    , @accountId bigint
    , @editorAccountId bigint
    , @currentYear int
    , @updateId uniqueidentifier
    , @accountIds nvarchar(255)
    , @isValidEmail BIT
    , @userEmail NVARCHAR(510)
    , @orgRequestID int
      

  set @updateId = newid()
  set @eventCode = N'USR_STATE'
  set @currentYear = Year(GetDate())
  
  select
    @editorAccountId = account.[Id],
    @userEmail = account.Email
  from
    dbo.Account account with (nolock, fastfirstrow)
  where
    account.[Login] = @editorLogin

  SELECT
    @accountId = account.[Id],
    @orgRequestID = account.OrganizationId    
    /*
    * старый функционал. в процедуру передается статус, который уже необходимо установить пользьвателю, дополнительных проверок не требуется.
    * , @status = case when @changeStatusByOrganizationRequest = 1 then @status else dbo.GetUserStatus(@currentYear, @status, @currentYear, 
        account.RegistrationDocument) END*/
  from
    dbo.Account account with (nolock, fastfirstrow)
  where
    account.[Login] = @login  

  if(@changeStatusByOrganizationRequest = 0)
  BEGIN 
    -- если деактивируют проверить на допустимые статусы
    IF (@status = 'deactivated' AND NOT EXISTS(SELECT * FROM Account a WHERE a.[Login] = @login AND a.[Status] IN ('activated', 'readonly')))
    BEGIN
      RAISERROR('$Невозможно деактивировать пользователя в текущем статусе.', 18, 1)
      RETURN -1
    END 
    
    IF (@status = 'activated' AND NOT EXISTS(SELECT * FROM Account a WHERE a.[Login] = @login AND a.[Status] IN ('deactivated', 'readonly', 'consideration')))
    BEGIN 
      RAISERROR('$Невозможно активировать пользователя в текущем статусе.', 18, 1)
      RETURN -1
    END   
  END
  
  -- проверка на сущестование пользователя с указанным email
  EXEC @isValidEmail = CheckNewUserAccountEmail @email = @userEmail
  IF(@isValidEmail = 0)
  BEGIN
    RAISERROR('$Существуют пользователи с таким же e-mail.', 18, 1)
    RETURN -1
  END 
  
  -- при установке статуса "Активировать" проверка на наличие скана заявки
  IF(@status = 'activated' AND EXISTS(SELECT * FROM Account WHERE [Login] = @login AND RegistrationDocument IS NULL))
  BEGIN
    RAISERROR('$Пользователь не приложил скан заявки.', 18, 1)
    RETURN -1
  END 
  
  -- GVUZ-595. При работе с заблокированной учетной записью уполномоченного сотрудника (статус «Deactivated») исключить возможность ее активации, 
    -- если для этого же ОУ есть учетная запись уполномоченного сотрудника со статусом, отличным от значения «Deactivated». (редактирование)                
    -- Правило работает только для УС ОУ ФБД GVUZ-780.
    IF (@status = 'activated' AND 
    -- заблокированный пользователь является УС ОУ ФБД для организации
    EXISTS(
      SELECT * 
      FROM Account a JOIN GroupAccount ga ON ga.AccountId = a.Id
        JOIN [Group] g ON g.Id = ga.GroupID
      WHERE a.[Login] = @login AND a.[Status] = 'deactivated' AND g.Code = 'fbd_^authorizedstaff'
    ) AND
    -- есть незаблокированный УС ОУ ФБД для организации данного пользователя
    EXISTS(
      SELECT or1.OrganizationId, orgUser.[Login]
    FROM OrganizationRequest2010 or1 
      JOIN OrganizationRequest2010 orReqUsr ON orReqUsr.OrganizationId = or1.OrganizationId
      JOIN account orgUser ON orgUser.OrganizationId = orReqUsr.Id
      JOIN GroupAccount ga ON ga.AccountId = orgUser.Id
      JOIN [Group] g ON ga.GroupID = g.Id
      WHERE or1.Id = @orgRequestID AND orgUser.[Status] <> 'deactivated' AND g.Code = 'fbd_^authorizedstaff' AND orReqUsr.Id <> @orgRequestID))
    BEGIN
    RAISERROR('$Невозможно активировать заблокированного уполномоченного сотрудника для доступа к ФБД, т.к. для данного ОУ уже есть учетная запись незаблокированного уполномоченного сотрудника.', 18, 1)
    RETURN -1     
    END
    
    -- GVUZ-624. Исключается возможность активации учетной записи пользователя, если заявление, с которым она связана, не активировано.
    IF (@status = 'activated' AND NOT exists(
        SELECT *
      FROM Account a JOIN OrganizationRequest2010 or1 ON a.OrganizationId = or1.Id
      WHERE a.Id = @accountId AND or1.StatusID = 5 -- активировано
      ))
    BEGIN
    RAISERROR('$Невозможно активировать пользователя, т.к. заявление для данного пользователя не активировано.', 18, 1)
    RETURN -1
    END

  BEGIN TRAN
  
    -- GVUZ-761 при активации пользователя УС ОУ в ФБД - старого блокируем, нового активируем.
    IF(@status = 'activated')
    BEGIN
      DECLARE @existOrgUserLogin nvarchar(255), @organizationID INT
      SELECT @organizationID = or1.OrganizationId 
      FROM Account a JOIN OrganizationRequest2010 or1 ON a.OrganizationId = or1.id
      WHERE a.[Login] = @login
      
      -- активируемый пользователь должен входить в группу УС ОУ для ФБД
      IF EXISTS(
        SELECT *
        FROM OrganizationRequest2010 or1 JOIN OrganizationRequestAccount ora ON ora.OrgRequestID = or1.Id
          JOIN Account a ON a.Id = ora.AccountID
          JOIN [Group] g ON g.Id = ora.GroupID
        WHERE or1.OrganizationId = @organizationID AND a.Id = @accountId AND g.code IN ('fbd_^authorizedstaff'))
      BEGIN
        -- находим существующего активного УС ОУ для ФБД.
        SELECT @existOrgUserLogin = a.[login]
        FROM OrganizationRequest2010 or1 JOIN OrganizationRequestAccount ora ON ora.OrgRequestID = or1.Id
          JOIN Account a ON a.Id = ora.AccountID
          JOIN [Group] g ON g.Id = ora.GroupID
        WHERE or1.OrganizationId = @organizationID AND a.Id <> @accountId AND a.[Status] IN ('activated', 'readonly')
          AND g.code IN ('fbd_^authorizedstaff')
        
        -- если нашли активного УС ОУ для ФБД, то блокируем его.
        IF(@existOrgUserLogin IS NOT NULL)
        BEGIN
          exec UpdateUserAccountStatus @login = @existOrgUserLogin, @status = 'deactivated', 
          @adminComment = 'заблокирован по причине активации нового УС ОУ для ФБД', 
          @editorLogin = @editorLogin, @editorIp = @editorIp, @changeStatusByOrganizationRequest = 1      
          if (@@error <> 0)
            goto undo
        END       
      END
    END     
  
    update account
    set 
      -- GVUZ-810 При отправке на доработку из статусов Registration и Revision статус не меняется.
      Status = CASE WHEN @status = 'revision' AND [Status] IN ('registration', 'revision') THEN [Status] ELSE @status END
      , AdminComment = @adminComment
      , IsActive = dbo.GetUserIsActive(@status)
      , UpdateDate = GetDate()
      , UpdateId = @updateId
      , ConfirmYear = @currentYear
      -- Удаляем документ регистрации, если он устарел.
      , RegistrationDocument = case
        when dbo.CanViewUserAccountRegistrationDocument(account.ConfirmYear) = 0 then null
        else account.RegistrationDocument
      end
      , EditorAccountId = @editorAccountId
      , EditorIp = @editorIp
    from 
      dbo.Account account with (rowlock)
    where
      account.[Id] = @accountId
    if (@@error <> 0)
      goto undo
                
  if @@trancount > 0 COMMIT TRAN

  exec dbo.RefreshRoleActivity @accountId = @accountId

  set @accountIds = convert(nvarchar(255), @accountId)

  exec dbo.RegisterEvent 
    @accountId = @editorAccountId
    , @ip = @editorIp
    , @eventCode = @eventCode
    , @sourceEntityIds = @accountIds
    , @eventParams = null
    , @updateId = @updateId

  return 0
  
  undo:
    if @@trancount > 0 rollback tran
    return 1  
end
GO
/****** Object:  StoredProcedure [dbo].[VerifyAccount]    Script Date: 27.11.2018 15:32:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Проверка пользователя.
-- v.1.0: Created by Makarev Andrey 02.04.2008
-- v.1.1: Modified by Makarev Andrey 14.04.2008
-- Измененение параметров ХП dbo.RegisterEvent. 
-- v.1.2: Modified by Makarev Andrey 14.04.2008
-- Приведение к стандарту.
-- v.1.3: Modified by Makarev Andrey 18.04.2008
-- Изменение параметров ХП dbo.RegisterEvent.
-- v.1.4: Modified by Makarev Andrey 05.05.2008
-- Добавлен параметр @password.
-- Код временный. Строки помеченные "-- временно"
-- и кусок кода ограниченный "--! временно" и "--!"
-- v.1.5: Modified by Fomin Dmitriy 14.05.2008
-- Проверка по IP только после активирования пользователя.
-- v.1.5: Modified by Fomin Dmitriy 14.05.2008
-- ИД объекта в событии заполнять. если нашли логин.
-- v.1.6: Modified by Fomin Dmitriy 14.05.2008
-- Проверку IP для пользователя отключать, если 
-- может редактировать свои рег. данные.
-- v.1.7: Modified by Fomin Dmitriy 15.05.2008
-- Проверка по IP только после активирования пользователя.
-- v.1.8: Modified by Fomin Dmitriy 15.05.2008
-- Если пользователь не имеет постоянного IP, проверять
-- для IP VPN.
-- v.2.0 Сулиманов: Проверяем только хэш пароля и "незаблокированность" пользователя
-- =============================================
CREATE PROC [dbo].[VerifyAccount]
	@login nvarchar(255)
	, @ip nvarchar(255)
	, @passwordHash nvarchar(255)
	, @password nvarchar(255) = NULL	
AS
BEGIN

	DECLARE @isLoginValid bit
		, @isIpValid bit
		, @accountId bigint
		, @entityParams nvarchar(1000)
		, @sourceEntityIds nvarchar(255)
		, @status NVARCHAR(510)
		

	SELECT @isLoginValid = 0, @isIpValid = 0

	-- восстанавливаем ХЭШ, если он пустой
	--EXEC dbo.RecreateEmptyPasswordHash @login --TODO

	SELECT @accountId = [Id], 
			@isLoginValid = 
				CASE 
					WHEN passwordHash = @passwordHash and [Status] <> 'deactivated' 
					THEN 1 
					ELSE 0 
				END,
			@status = [Status]
	FROM dbo.Account with (nolock)
	WHERE [Login] = @login

	-- IP не проверяем - он валидный при валидном пароле
	SET @isIpValid=@isLoginValid

	SET @entityParams = @login + N'|' +	@ip + N'|' +@passwordHash + '|' +
			CONVERT(nvarchar, @isLoginValid)  + '|' +
			CONVERT(nvarchar, @isIpValid)

	SET @sourceEntityIds = CONVERT(nvarchar(255), ISNULL(@accountId,0))

	SELECT @login [Login], @ip Ip , @passwordHash PasswordHash
		, @isLoginValid IsLoginValid, @isIpValid IsIpValid, @status UserStatus
	EXEC dbo.RegisterEvent 
		@accountId = @accountId
		, @ip = @ip
		, @eventCode = N'USR_VERIFY'
		, @sourceEntityIds = @sourceEntityIds
		, @eventParams = @entityParams
		, @updateId = null

	RETURN 0
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Organization2010"
            Begin Extent = 
               Top = 99
               Left = 51
               Bottom = 287
               Right = 243
            End
            DisplayFlags = 280
            TopColumn = 27
         End
         Begin Table = "RecruitmentCampaigns"
            Begin Extent = 
               Top = 404
               Left = 347
               Bottom = 482
               Right = 498
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Region"
            Begin Extent = 
               Top = 218
               Left = 491
               Bottom = 326
               Right = 672
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "OrganizationType2010"
            Begin Extent = 
               Top = 391
               Left = 15
               Bottom = 484
               Right = 166
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "OrganizationKind"
            Begin Extent = 
               Top = 396
               Left = 166
               Bottom = 489
               Right = 317
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Account"
            Begin Extent = 
               Top = 139
               Left = 968
               Bottom = 247
               Right = 1217
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "OrganizationRequest2010"
            Begin Extent = 
               Top = 1
               Left = 285
               Bottom = 109
             ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Большой отчет'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'  Right = 534
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "OrganizationRequestAccount"
            Begin Extent = 
               Top = 6
               Left = 856
               Bottom = 99
               Right = 1007
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Organization2010_1"
            Begin Extent = 
               Top = 32
               Left = 635
               Bottom = 140
               Right = 827
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Organization2010_2"
            Begin Extent = 
               Top = 174
               Left = 750
               Bottom = 282
               Right = 942
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "RecruitmentCampaigns_1"
            Begin Extent = 
               Top = 129
               Left = 457
               Bottom = 207
               Right = 608
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 27
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 3000
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 1440
         Alias = 2880
         Table = 2880
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Большой отчет'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Большой отчет'
GO
