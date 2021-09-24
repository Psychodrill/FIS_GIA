-- exec dbo.SearchSameUserAccount

-- =============================================
-- Поиск похожих учетных записей.
-- v.1.0: Created by Fomin Dmitriy 17.04.2008
-- v.1.1: Modified by Fomin Dmitriy 06.05.2008
-- Поле наименования организации увеличено.
-- =============================================
CREATE procedure dbo.SearchSameUserAccount
	@organizationName nvarchar(2000)
as
begin
	declare
		@userGroupId bigint
		, @currentYear int
		, @sameLavel decimal(18, 4)
		, @matchCount int
		, @shortOrganizationName nvarchar(2000)

	select 
		@userGroupId = [group].Id
	from 
		dbo.[Group] [group]
	where 
		[group].Code = 'User'

	set @currentYear = Year(GetDate())
	set @sameLavel = 0.7
	set @matchCount = 3
	set @shortOrganizationName = dbo.GetShortOrganizationName(@organizationName)

	select top 100
		search.[Login]
		, search.OrganizationName
		, search.LastName 
		, search.Status 
	from (select
			search.[Login]
			, search.OrganizationName
			, search.LastName 
			, search.Status 
			, dbo.CompareStrings(search.ShortOrganizationName
					, @shortOrganizationName, @matchCount) SameLevel
		from (select
				account.[Login] [Login]
				, account.LastName LastName 
				, dbo.GetUserStatus(account.ConfirmYear, account.Status
							, @currentYear, account.RegistrationDocument) Status 
				, Organization.[Name] OrganizationName
				, Organization.[ShortName] ShortOrganizationName
			from 
				dbo.Account account with (nolock)
					inner join dbo.Organization organization with (nolock)
						on organization.Id = account.OrganizationId
			where 
				account.IsActive = 1
				and account.Id in (select group_account.AccountId
						from dbo.GroupAccount group_account with (nolock)
						where group_account.GroupId = @userGroupId)) search) search
	where
		search.SameLevel >= @sameLavel
	order by
		search.SameLevel desc

	return 0
end
