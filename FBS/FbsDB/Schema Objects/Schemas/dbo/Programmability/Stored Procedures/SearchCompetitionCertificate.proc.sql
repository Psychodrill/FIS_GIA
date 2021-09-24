
-- exec dbo.SearchCompetitionCertificate
-- =============================================
-- Процедура  поиска олимпиадников.
-- Created by Sedov Anton 10.07.2008
-- Modified by Fomin Dmitriy 23.07.2008
-- Поле SubjectId заменено CompetitionTypeId.
-- Тип олимпиады необязателен.
-- =============================================
CREATE procedure dbo.SearchCompetitionCertificate
	@competitionTypeId int = null
	, @lastName nvarchar(255)
	, @firstName nvarchar(255)
	, @patronymicName nvarchar(255)
	, @regionId int = null
	, @login nvarchar(255)
	, @ip nvarchar(255)
as
begin
	declare 
		@year int 
	
	set @year = Year(GetDate())

	select
		searching_competition_certificate.CompetitionTypeId CompetitionTypeId
		, competition_type.[Name] CompetitionTypeName
		, searching_competition_certificate.LastName LastName
		, searching_competition_certificate.FirstName FirstName
		, searching_competition_certificate.PatronymicName PatronymicName
		, competition_certificate.Degree Degree
		, isnull(region.[Name], searching_region.[Name]) RegionName
		, competition_certificate.City City
		, competition_certificate.School School
		, competition_certificate.Class Class
		, case 
			when competition_certificate.Id is null then 0
			else 1
		end IsExist 
	from
		(select
			@competitionTypeId CompetitionTypeId
			, @lastName LastName
			, @firstName FirstName
			, @patronymicName PatronymicName
			, @regionId RegionId) as searching_competition_certificate
			left join dbo.CompetitionCertificate competition_certificate
				left join dbo.CompetitionType competition_type
					on competition_certificate.CompetitionTypeId = competition_type.Id
				left join dbo.Region region
					on competition_certificate.RegionId = region.Id
				on searching_competition_certificate.LastName = competition_certificate.LastName
					and searching_competition_certificate.FirstName = competition_certificate.FirstName
					and searching_competition_certificate.PatronymicName = competition_certificate.PatronymicName
					and competition_certificate.[Year] = @year
					-- Лучше использовать диманический SQL, но здесь не критично.
					and (searching_competition_certificate.CompetitionTypeId = competition_certificate.CompetitionTypeId
						or searching_competition_certificate.CompetitionTypeId is null) 
					and (searching_competition_certificate.RegionId = competition_certificate.RegionId
						or searching_competition_certificate.RegionId is null) 
			left join dbo.Region searching_region
				on searching_competition_certificate.RegionId = region.Id

	declare 
		@eventCode nvarchar(255)
		, @editorAccountId bigint
		, @updateId uniqueidentifier
	
	select
		@editorAccountId = account.[Id]
	from 
		dbo.Account account with (nolock, fastfirstrow)
	where 
		account.[Login] = @login 

	set @eventCode = 'SCC_FND'
	set @updateId = NewId()			
	
	exec dbo.RegisterEvent 
		@accountId = @editorAccountId
		, @ip = @ip
		, @eventCode = @eventCode
		, @sourceEntityIds = null
		, @eventParams = null
		, @updateId = @updateId
				
	return 0
end
