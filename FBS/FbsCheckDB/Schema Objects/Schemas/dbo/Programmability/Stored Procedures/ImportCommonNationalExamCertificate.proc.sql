
-- exec dbo.ImportCommonNationalExamCertificate
-- =============================================
-- Загрузка сертификатов ЕГЭ одной секции
-- v.1.0: Created by Sedov Anton 11.06.2008
-- v.1.1: Modified by Fomin Dmitriy 12.06.2008
-- Исправление ошибок. Объединение ХПшек в одну.
-- ============================================= 
CREATE procedure [dbo].[ImportCommonNationalExamCertificate]
	@importCertificateFilePath nvarchar(255) 
	, @importCertificateSubjectFilePath nvarchar(255) 
as
begin
	-- Подготовка секций.
	declare 
		@year int
		, @yearFrom int
		, @yearTo int
		, @commandText nvarchar(4000)
		, @partition int

	set @year = Year(GetDate())

	select
		@yearFrom = actuality.YearFrom
		, @yearTo = actuality.YearTo
	from dbo.GetCommonNationalExamCertificateActuality() actuality 

	-- Удаление устаревших секций сертификатов.
	if exists(select 1
			from dbo.CommonNationalExamCertificateActivePartition active_partition
			where active_partition.[Year] < @yearFrom)
	begin
		declare partition_cursor cursor forward_only for 
		select
			deprecated_partition.[Year] [Partition]
		from 
			dbo.CommonNationalExamCertificateActivePartition deprecated_partition
		where 
			deprecated_partition.[Year] < @yearFrom
		order by 
			[Partition] asc
		
		open partition_cursor
		fetch next from partition_cursor into @partition
		while @@fetch_status <> -1
		begin 
			if not object_id('dbo.DeprecatedCommonNationalExamCertificate') is null
				drop table dbo.DeprecatedCommonNationalExamCertificate

			create table dbo.DeprecatedCommonNationalExamCertificate
				(
				Id bigint not null
				, CreateDate datetime not null
				, UpdateDate datetime not null
				, UpdateId uniqueidentifier not null
				, EditorAccountId bigint not null
				, EditorIp nvarchar(255) not null
				, Number nvarchar(255) not null
				, EducationInstitutionCode nvarchar(255) not null
				, [Year] int not null 
				, LastName nvarchar(255) not null
				, FirstName nvarchar(255)  not null
				, PatronymicName nvarchar(255) not null
				, Sex bit not null
				, Class nvarchar(255) not null
				, InternalPassportSeria nvarchar(255) not null
				, PassportSeria nvarchar(255) not null
				, PassportNumber nvarchar(255) not null
				, EntrantNumber nvarchar(255) null
				, RegionId int not null 
				)

			alter table dbo.DeprecatedCommonNationalExamCertificate
			add constraint DeprecatedCertificatePK
				primary key clustered ([Year], Id)

			alter table dbo.CommonNationalExamCertificate
			switch partition $partition.CommonNationalExamCertificateLeftPartition(@partition) 
				to dbo.DeprecatedCommonNationalExamCertificate

			insert into dbo.CommonNationalExamCertificateArchive
			select
				deprecate_cne_certificate.CreateDate
				, deprecate_cne_certificate.UpdateDate
				, deprecate_cne_certificate.UpdateId 
				, deprecate_cne_certificate.EditorAccountId
				, deprecate_cne_certificate.EditorIp
				, deprecate_cne_certificate.Number
				, deprecate_cne_certificate.EducationInstitutionCode
				, deprecate_cne_certificate.[Year]
				, deprecate_cne_certificate.LastName
				, deprecate_cne_certificate.FirstName
				, deprecate_cne_certificate.PatronymicName
				, deprecate_cne_certificate.Sex
				, deprecate_cne_certificate.Class
				, deprecate_cne_certificate.InternalPassportSeria
				, deprecate_cne_certificate.PassportSeria
				, deprecate_cne_certificate.PassportNumber
				, deprecate_cne_certificate.EntrantNumber
				, deprecate_cne_certificate.RegionId
			from 
				dbo.DeprecatedCommonNationalExamCertificate deprecate_cne_certificate
			drop table dbo.DeprecatedCommonNationalExamCertificate

			alter partition scheme CommonNationalExamCertificatePartitionScheme
			next used [primary]
			
			alter partition function CommonNationalExamCertificateLeftPartition()
			merge range(@partition)

			fetch next from partition_cursor into @partition
		end
		close partition_cursor
		deallocate partition_cursor

		delete from dbo.CommonNationalExamCertificateActivePartition
		where [Year] < @yearFrom
	end

	-- Удаление устаревших секций предметов сертификатов.
	if exists(select 1
			from dbo.CommonNationalExamCertificateSubjectActivePartition active_partition
			where active_partition.[Year] < @yearFrom)
	begin
		declare partition_cursor cursor forward_only for 
		select
			deprecated_partition.[Year] [Partition]
		from 
			dbo.CommonNationalExamCertificateSubjectActivePartition deprecated_partition
		where 
			deprecated_partition.[Year] < @yearFrom
		order by 
			[Partition] asc
		
		open partition_cursor
		fetch next from partition_cursor into @partition
		while @@fetch_status <> -1
		begin 
			if not object_id('dbo.DeprecatedCommonNationalExamCertificateSubject') is null
				drop table dbo.DeprecatedCommonNationalExamCertificateSubject

			create table dbo.DeprecatedCommonNationalExamCertificateSubject
				(
				Id bigint not null
				, CertificateId bigint not null
				, SubjectId  bigint not null
				, Mark numeric(5,1) not null
				, HasAppeal bit not null
				, [Year] int not null
				, RegionId int not null
				)

			alter table dbo.DeprecatedCommonNationalExamCertificateSubject
			add constraint DeprecatedCertificateSubjectPK
				primary key clustered ([Year], CertificateId, SubjectId)

			alter table dbo.CommonNationalExamCertificateSubject
			switch partition $partition.CommonNationalExamCertificateSubjectLeftPartition(@partition) 
				to dbo.DeprecatedCommonNationalExamCertificateSubject


			insert into dbo.CommonNationalExamCertificateSubjectArchive
			select 
				deprecate_cne_certificate_subject.CertificateId
				, deprecate_cne_certificate_subject.SubjectId
				, deprecate_cne_certificate_subject.Mark
				, deprecate_cne_certificate_subject.HasAppeal
				, deprecate_cne_certificate_subject.[Year]
				, deprecate_cne_certificate_subject.RegionId
			from 
				dbo.DeprecatedCommonNationalExamCertificateSubject deprecate_cne_certificate_subject

			drop table dbo.DeprecatedCommonNationalExamCertificateSubject

			alter partition scheme CommonNationalExamCertificateSubjectPartitionScheme
			next used [primary]
			
			alter partition function CommonNationalExamCertificateSubjectLeftPartition()
			merge range(@partition)

			fetch next from partition_cursor into @partition
		end
		close partition_cursor
		deallocate partition_cursor

		delete from dbo.CommonNationalExamCertificateSubjectActivePartition
		where [Year] < @yearFrom
	end

	-- Добавление новых секций сертификатов.
	if not exists(select 1
			from dbo.CommonNationalExamCertificateActivePartition [partition]
			where [partition].[Year] = @year)
	begin
		alter partition scheme CommonNationalExamCertificatePartitionScheme
		next used [primary]
			
		alter partition function CommonNationalExamCertificateLeftPartition()
		split range (@year)

		insert into dbo.CommonNationalExamCertificateActivePartition
			(
			[Year]
			)
		values 
			(
			@year
			)
	end  
	
	-- Добавление новых секций предметов сертификатов.
	if not exists(select 1
			from dbo.CommonNationalExamCertificateSubjectActivePartition [partition]
			where [partition].[Year] = @year)
	begin
		alter partition scheme CommonNationalExamCertificateSubjectPartitionScheme
		next used [primary]
			
		alter partition function CommonNationalExamCertificateSubjectLeftPartition()
		split range (@year)

		insert into dbo.CommonNationalExamCertificateSubjectActivePartition
			(
			[Year]
			)
		values 
			(
			@year
			)
	end  
		
	-- Импорт данных сертификатов.
	if not object_id('dbo.ImportingCommonNationalExamCertificate') is null
		drop table dbo.ImportingCommonNationalExamCertificate

	create table dbo.ImportingCommonNationalExamCertificate
		(
		Id bigint not null
		, CreateDate datetime not null
		, UpdateDate datetime not null
		, UpdateId uniqueidentifier not null
		, EditorAccountId bigint not null
		, EditorIp nvarchar(255) not null
		, Number nvarchar(255) not null
		, EducationInstitutionCode nvarchar(255) not null
		, [Year] int not null 
		, LastName nvarchar(255) not null
		, FirstName nvarchar(255)  not null
		, PatronymicName nvarchar(255) not null
		, Sex bit not null
		, Class nvarchar(255) not null
		, InternalPassportSeria nvarchar(255) not null
		, PassportSeria nvarchar(255) not null
		, PassportNumber nvarchar(255) not null
		, EntrantNumber nvarchar(255) null
		, RegionId int not null 
		)

	set @commandText = 
		N'bulk insert dbo.ImportingCommonNationalExamCertificate
		from ''<importFile>'' with (codepage = 1251) ' 

	set @commandText = replace(@commandText, '<importFile>', 
		replace(@importCertificateFilePath, '''', '''''')) 

	exec(@commandText) 
	
	set @commandText = replace(
		'alter table dbo.ImportingCommonNationalExamCertificate
		with check
		add constraint ImportingCertificateCK
			check ([Year] = <year>) ', '<year>', @year)

	exec(@commandText) 

	alter table dbo.ImportingCommonNationalExamCertificate
	add constraint ImportingCertificatePK
		primary key clustered ([Year], Id)

	create nonclustered index IdxImportingCertificateNumber
	on dbo.ImportingCommonNationalExamCertificate ([Year], Number)

	create nonclustered index IdxImportingCertificateOwner
	on dbo.ImportingCommonNationalExamCertificate ([Year], LastName, FirstName, PatronymicName)

	if not object_id('dbo.DeprecatedCommonNationalExamCertificate') is null
		drop table dbo.DeprecatedCommonNationalExamCertificate

	create table dbo.DeprecatedCommonNationalExamCertificate
		(
		Id bigint not null
		, CreateDate datetime not null
		, UpdateDate datetime not null
		, UpdateId uniqueidentifier not null
		, EditorAccountId bigint not null
		, EditorIp nvarchar(255) not null
		, Number nvarchar(255) not null
		, EducationInstitutionCode nvarchar(255) not null
		, [Year] int not null 
		, LastName nvarchar(255) not null
		, FirstName nvarchar(255)  not null
		, PatronymicName nvarchar(255) not null
		, Sex bit not null
		, Class nvarchar(255) not null
		, InternalPassportSeria nvarchar(255) not null
		, PassportSeria nvarchar(255) not null
		, PassportNumber nvarchar(255) not null
		, EntrantNumber nvarchar(255) null
		, RegionId int not null 
		)

	alter table dbo.DeprecatedCommonNationalExamCertificate
	add constraint DeprecatedCertificatePK
		primary key clustered ([Year], Id)

	-- Импорт данных предметов сертификатов.
	if not object_id('dbo.ImportingCommonNationalExamCertificateSubject') is null
		drop table dbo.ImportingCommonNationalExamCertificateSubject

	create table dbo.ImportingCommonNationalExamCertificateSubject
		(
		Id bigint not null
		, CertificateId bigint not null
		, SubjectId  bigint not null
		, Mark numeric(5,1) not null
		, HasAppeal bit not null
		, [Year] int not null
		, RegionId int not null
		)

	set @commandText = 
		N'bulk insert dbo.ImportingCommonNationalExamCertificateSubject
		from ''<importFile>'' with (codepage = 1251) ' 

	set @commandText = replace(@commandText, '<importFile>', 
		replace(@importCertificateSubjectFilePath, '''', '''''')) 

	exec(@commandText) 

	set @commandText = replace(
		'alter table dbo.ImportingCommonNationalExamCertificateSubject
		with check
		add constraint ImportingCertificateSubjectCK
			check ([Year] = <year>) ', '<year>', @year)

	exec(@commandText) 

	alter table dbo.ImportingCommonNationalExamCertificateSubject
	add constraint ImportingCertificateSubjectPK
		primary key clustered ([Year], CertificateId, SubjectId)

	if not object_id('dbo.DeprecatedCommonNationalExamCertificateSubject') is null
		drop table dbo.DeprecatedCommonNationalExamCertificateSubject

	create table dbo.DeprecatedCommonNationalExamCertificateSubject
		(
		Id bigint not null
		, CertificateId bigint not null
		, SubjectId  bigint not null
		, Mark numeric(5,1) not null
		, HasAppeal bit not null
		, [Year] int not null
		, RegionId int not null
		)

	alter table dbo.DeprecatedCommonNationalExamCertificateSubject
	add constraint DeprecatedCertificateSubjectPK
		primary key clustered ([Year], CertificateId, SubjectId)

	-- Перенос данных в таблицы.
	alter table dbo.CommonNationalExamCertificate
	switch partition $partition.CommonNationalExamCertificateLeftPartition(@year) 
		to dbo.DeprecatedCommonNationalExamCertificate

	alter table dbo.CommonNationalExamCertificateSubject
	switch partition $partition.CommonNationalExamCertificateSubjectLeftPartition(@year) 
		to dbo.DeprecatedCommonNationalExamCertificateSubject

	alter table dbo.ImportingCommonNationalExamCertificate
	switch to dbo.CommonNationalExamCertificate
		partition $partition.CommonNationalExamCertificateLeftPartition(@year) 

	alter table dbo.ImportingCommonNationalExamCertificateSubject
	switch to dbo.CommonNationalExamCertificateSubject
		partition $partition.CommonNationalExamCertificateSubjectLeftPartition(@year) 

	-- Очистка.
	drop table dbo.DeprecatedCommonNationalExamCertificate
	drop table dbo.ImportingCommonNationalExamCertificate
	drop table dbo.DeprecatedCommonNationalExamCertificateSubject
	drop table dbo.ImportingCommonNationalExamCertificateSubject

	return 0
end

