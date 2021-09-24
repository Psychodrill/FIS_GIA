


---------------------------------------------------
-- Получение имени БД с последней версией данных
-- v.1.0: Created by Sedov Anton 25.06.2008
---------------------------------------------------
CREATE function [dbo].[GetDataDbName]
	(
	@onlyCertificate bit = null
	, @onlyCertificateDeny bit = null
	)
returns nvarchar(255)
as
begin
	declare 
		@resultDbName nvarchar(255)
		, @defaultDbName nvarchar(255)
		, @stateRestoring int

	declare @code_values table
		(
		Code nvarchar(255)
		)

	declare @dbName table
		(
		Id int
		, [Name] nvarchar(255)
		)

	insert into @dbName values (1, 'fbs')
	insert into @dbName values (2, 'fbs_check_db')

	set @defaultDbName = 'fbs'
	set @stateRestoring = 1

	if isnull(@onlyCertificate, 0) = 1
	begin
		insert into @code_values ( Code )	
		values ('Certificate')
		insert into @code_values ( Code )	
		values ('CertificateSubject')
	end

	if isnull(@onlyCertificateDeny, 0) = 1
	begin
		insert into @code_values ( Code )
		values ('CertificateDeny')
	end
		
	select top 1
		@resultDbName = bulk_file_db_subscription.DbName
	from	
		task_db.dbo.BulkFile bulk_file with(nolock)
			inner join task_db.dbo.BulkFileDbSubscription bulk_file_db_subscription with(nolock)
				inner join @dbName [db_name]
					on [db_name].[Name] collate cyrillic_general_ci_ai = bulk_file_db_subscription.DbName collate cyrillic_general_ci_ai
				inner join sys.databases [database]
					on [database].[Name] collate cyrillic_general_ci_ai = bulk_file_db_subscription.DbName collate cyrillic_general_ci_ai
				on bulk_file.Id = bulk_file_db_subscription.FileId
	where 
		bulk_file.Code in (select Code from @code_values)
			and [database].state <> @stateRestoring
	order by 
		bulk_file.Date desc, [db_name].Id asc


	if @resultDbName is null
		select 
			@resultDbName = @defaultDbName	 
		from 
			sys.databases [database]
		where 
			[database].[Name] collate cyrillic_general_ci_ai = @defaultDbName collate cyrillic_general_ci_ai
				and [database].state <> @stateRestoring
	
	if @resultDbName is null			  
		set @resultDbName = Db_Name()
			 
	return @resultDbName
end


