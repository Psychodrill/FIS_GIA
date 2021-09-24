
-- exec dbo.RefreshCommonNationalExamCertificateFormPartition
-- ================================================
-- Обновить секционирование бланков сертификатов ЕГЭ.
-- v.1.0: Created by Fomin Dmitriy 08.06.2008
-- v.1.1: Modified by Sedov Anton 20.06.2008
-- v.1.2: Modified by Sedov Anton 12.08.2008
-- Добавлено сохранение записей бланков в архивной таблице
-- Добавлено обновление секций предметов сертификатов
-- ================================================
CREATE procedure [dbo].[RefreshCommonNationalExamCertificateFormPartition]
as
begin
	declare
		@declareCommandText nvarchar(max)
		, @commandText nvarchar(max)
		, @clearCommandText nvarchar(max)
		, @deletePartition nvarchar(max)
		, @deleteSubjectPartition nvarchar(max)
		, @addSubjectPartition nvarchar(max)
	
	set @declareCommandText =
		N'
		declare
			@year int
			, @yearFrom int
			, @yearTo int
			, @partition bigint
			, @regionId int
		'

	set @deletePartition = 
		N'
		select
			@yearFrom = actuality.YearFrom
			, @yearTo = actuality.YearTo
		from dbo.GetCommonNationalExamCertificateActuality() actuality

		declare @actualYear table
			(
			[Year] int
			)

		set @year = @yearFrom
		while @year <= @yearTo
		begin
			insert into @actualYear
			values (@year)

			set @year = @year + 1
		end

		set @year = Year(GetDate())
	
		if not exists(select 1
				from dbo.CommonNationalExamCertificateFormActivePartition active_partition
					full outer join @actualYear actual_year 
						on active_partition.[Year] = actual_year.[Year]
				where 
					active_partition.[Year] is null
					or actual_year.[Year] is null)
			goto quitproc

		declare partition_cursor cursor forward_only for  
		select
			deprecated_partition.Partition [Partition]
		from 
			dbo.CommonNationalExamCertificateFormActivePartition deprecated_partition
		where 
			deprecated_partition.[Year] < @yearFrom
		order by 
			[Partition] asc

		open partition_cursor
		fetch next from partition_cursor into @partition
		while @@fetch_status <> -1
		begin
			if not object_id(''dbo.DeprecatedCommonNationalExamCertificateForm'') is null
				drop table dbo.DeprecatedCommonNationalExamCertificateForm
		
			create table dbo.DeprecatedCommonNationalExamCertificateForm 
				(
					Id bigint identity(1,1) not null
					, CreateDate datetime not null
					, UpdateDate datetime not null
					, UpdateId uniqueidentifier not null
					, [Year] int not null
					, RegionId int not null
					, [Partition] bigint not null
					, Number nvarchar(255) not null
					, CertificateNumber nvarchar(255)
					, LastName nvarchar(255)
					, FirstName nvarchar(255)
					, PatronymicName nvarchar(255)
					, PassportSeria nvarchar(255)
					, PassportNumber nvarchar(255)
					, IsBlank bit not null
					, IsDeny bit not null
					, IsDuplicate bit not null
					, IsValid bit
					, SourceLastName nvarchar(255)
					, SourceFirstName nvarchar(255)
					, SourcePatronymicName nvarchar(255)
					, SourcePassportSeria nvarchar(255)
					, SourcePassportNumber nvarchar(255)
					, IsCertificateExist bit
					, IsCertificateDeny bit
				) 

			alter table dbo.CommonNationalExamCertificateForm
			switch partition $partition.CommonNationalExamCertificateFormPartitionFunction(@partition)  
			to dbo.DeprecatedCommonNationalExamCertificateForm
				
			alter partition function CommonNationalExamCertificateFormPartitionFunction()
			merge range(@partition)

			delete from dbo.CommonNationalExamCertificateFormActivePartition
			where [Partition] = @partition

			insert into dbo.CommonNationalExamCertificateFormArchive
			select
				cne_certificate_form.CreateDate
				, cne_certificate_form.UpdateDate
				, cne_certificate_form.UpdateId
				, cne_certificate_form.[Year]
				, cne_certificate_form.RegionId
				, cne_certificate_form.[Partition]
				, cne_certificate_form.Number
				, cne_certificate_form.CertificateNumber
				, cne_certificate_form.LastName
				, cne_certificate_form.FirstName
				, cne_certificate_form.PatronymicName
				, cne_certificate_form.PassportSeria
				, cne_certificate_form.PassportNumber
				, cne_certificate_form.IsBlank
				, cne_certificate_form.IsDeny
				, cne_certificate_form.IsDuplicate
				, cne_certificate_form.IsValid
				, cne_certificate_form.SourceLastName
				, cne_certificate_form.SourceFirstName
				, cne_certificate_form.SourcePatronymicName
				, cne_certificate_form.SourcePassportSeria
				, cne_certificate_form.SourcePassportNumber
				, cne_certificate_form.IsCertificateExist
				, cne_certificate_form.IsCertificateDeny
			from  
				dbo.DeprecatedCommonNationalExamCertificateForm cne_certificate_form

			drop table dbo.DeprecatedCommonNationalExamCertificateForm

			fetch next from partition_cursor into @partition
		end
		close partition_cursor
		deallocate partition_cursor

		delete from dbo.CommonNationalExamCertificateFormActivePartition
		where [Year] < @yearFrom
		'
	
	set @commandText = 
		N'
		if exists(select 1
			from dbo.CommonNationalExamCertificateFormActivePartition [partition]
			where [partition].[Year] = @year) goto quitproc 

		declare partition_cursor cursor forward_only for  
		select
			region.Id RegionId
			, dbo.GetCommonNationalExamCertificateFormPartition(region.Id, @year) [Partition]
		from dbo.Region region
		where 
			region.InCertificate = 1
		order by 
			[Partition] asc

		open partition_cursor
		fetch next from partition_cursor into @regionId, @partition
		while @@fetch_status <> -1
		begin
			alter partition scheme CommonNationalExamCertificateFormPartitionScheme 
			next used [primary] 
			
			alter partition function CommonNationalExamCertificateFormPartitionFunction()
			split range (@partition)

			fetch next from partition_cursor into @regionId, @partition
		end
		close partition_cursor
		deallocate partition_cursor

		insert into dbo.CommonNationalExamCertificateFormActivePartition
			(
			[Year]
			, [RegionId]
			, [Partition]
			)
		select 
			@year
			, region.Id RegionId
			, dbo.GetCommonNationalExamCertificateFormPartition(region.Id, @year) [Partition]
		from 
			dbo.Region region
		where 
			region.InCertificate = 1
		order by 
			[Partition] asc
		'

	set @deleteSubjectPartition =
		N'
		if not exists(select 1
				from dbo.CommonNationalExamCertificateSubjectFormActivePartition active_subject_partition
					full outer join @actualYear actual_year 
						on active_subject_partition.[Year] = actual_year.[Year]
				where 
					active_subject_partition.[Year] is null
					or actual_year.[Year] is null) goto quitproc

		declare partition_subject_cursor cursor forward_only for  
		select
			deprecated_subject_partition.Partition [Partition]
		from 
			dbo.CommonNationalExamCertificateSubjectFormActivePartition deprecated_subject_partition
		where 
			deprecated_subject_partition.[Year] < @yearFrom
		order by 
			[Partition] asc

		open partition_subject_cursor
		fetch next from partition_subject_cursor into @partition
		while @@fetch_status <> -1
		begin
			if not object_id(''dbo.DeprecatedCommonNationalExamCertificateSubjectForm'') is null
				drop table dbo.DeprecatedCommonNationalExamCertificateSubjectForm
		
			create table dbo.DeprecatedCommonNationalExamCertificateSubjectForm 
				(
					Id bigint identity(1,1) not null
					, [Year] int not null
					, RegionId int not null
					, [Partition] bigint not null
					, FormId bigint
					, SubjectId int
					, Mark numeric(5,1)
				) 

			alter table dbo.CommonNationalExamCertificateSubjectForm
			switch partition $partition.CommonNationalExamCertificateSubjectFormPartitionFunction(@partition)  
			to dbo.DeprecatedCommonNationalExamCertificateSubjectForm
				
			alter partition function CommonNationalExamCertificateSubjectFormPartitionFunction()
			merge range(@partition)

			delete from dbo.CommonNationalExamCertificateSubjectFormActivePartition
			where [Partition] = @partition

			insert into dbo.CommonNationalExamCertificateSubjectFrom 
			select 
				deprecate_cne_certificate_subject_form.[Year]
				, deprecate_cne_certificate_subject_form.RegionId
				, deprecate_cne_certificate_subject_form.[Partition]
				, deprecate_cne_certificate_subject_form.FormId
				, deprecate_cne_certificate_subject_form.SubjectId
				, deprecate_cne_certificate_subject_form.Mark
			from 
				dbo.DeprecateCommonNationalExamCertificateSubjectForm deprecate_cne_certificate_subject_form

			drop table dbo.DeprecatedCommonNationalExamCertificateSubjectForm

			fetch next from partition_subject_cursor into @partition
		end
		close partition_subject_cursor
		deallocate partition_subject_cursor

		delete from dbo.CommonNationalExamCertificateSubjectFormActivePartition
		where [Year] < @yearFrom
		' 
		
	set @addSubjectPartition =
		N'
		if exists(select 1
			from dbo.CommonNationalExamCertificateSubjectFormActivePartition [partition]
			where [partition].[Year] = @year) goto quitproc
		
		declare partition_subject_cursor cursor forward_only for  
		select
			region.Id RegionId
			, dbo.GetCommonNationalExamCertificateFormPartition(region.Id, @year) [Partition]
		from dbo.Region region
		where 
			region.InCertificate = 1
		order by 
			[Partition] asc

		open partition_subject_cursor
		fetch next from partition_subject_cursor into @regionId, @partition
		while @@fetch_status <> -1
		begin
			alter partition scheme CommonNationalExamCertificateSubjectFormPartitionScheme 
			next used [primary] 
			
			alter partition function CommonNationalExamCertificateSubjectFormPartitionFunction()
			split range (@partition)

			fetch next from partition_subject_cursor into @regionId, @partition
		end
		close partition_subject_cursor
		deallocate partition_subject_cursor

		insert into dbo.CommonNationalExamCertificateSubjectFormActivePartition
			(
			[Year]
			, [RegionId]
			, [Partition]
			)
		select 
			@year
			, region.Id RegionId
			, dbo.GetCommonNationalExamCertificateFormPartition(region.Id, @year) [Partition]
		from 
			dbo.Region region
		where 
			region.InCertificate = 1
		order by 
			[Partition] asc

		quitproc:
		'

	exec (@declareCommandText + @deletePartition + @commandText + @deleteSubjectPartition + 
		@addSubjectPartition)

	return 0	
end

