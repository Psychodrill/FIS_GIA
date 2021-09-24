
-- exec dbo.ImportCommonNationalExamCertificateDeny
-- ================================================
-- Загрузка отмен сертификатов ЕГЭ одной секции
-- v.1.0: Create by Sedov Anton 11.06.2008
-- ================================================
CREATE procedure dbo.ImportCommonNationalExamCertificateDeny
	@importFilePath nvarchar(255) 
as
begin
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

	-- Удаление устаревших секций запрещений сертификатов.
	if exists(select 1
			from dbo.CommonNationalExamCertificateDenyActivePartition active_partition
			where active_partition.[Year] < @yearFrom)
	begin
		declare partition_cursor cursor forward_only for 
		select
			deprecated_partition.[Year] [Partition]
		from 
			dbo.CommonNationalExamCertificateDenyActivePartition deprecated_partition
		where 
			deprecated_partition.[Year] < @yearFrom
		order by 
			[Partition] asc
		
		open partition_cursor
		fetch next from partition_cursor into @partition
		while @@fetch_status <> -1
		begin 
			if not object_id('dbo.DeprecatedCommonNationalExamCertificateDeny') is null
				drop table dbo.DeprecatedCommonNationalExamCertificateDeny

			create table dbo.DeprecatedCommonNationalExamCertificateDeny
				(
				Id bigint not null
				, CreateDate datetime not null
				, UpdateDate datetime not null
				, UpdateId  uniqueidentifier
				, [Year] int not null
				, CertificateNumber nvarchar(255) not null
				, Comment ntext not null
				, NewCertificateNumber nvarchar(255)
				)

			alter table dbo.DeprecatedCommonNationalExamCertificateDeny
			add constraint DeprecatedCertificateDenyPK
				primary key clustered ([Year], CertificateNumber)

			alter table dbo.CommonNationalExamCertificateDeny
			switch partition $partition.CommonNationalExamCertificateDenyLeftPartition(@partition) 
				to dbo.DeprecatedCommonNationalExamCertificateDeny

			insert into dbo.CommonNationalExamCertificateDenyArchive
			select
				cne_certificate_deny.CreateDate
				, cne_certificate_deny.UpdateDate
				, cne_certificate_deny.UpdateId
				, cne_certificate_deny.[Year]
				, cne_certificate_deny.CertificateNumber
				, cne_certificate_deny.Comment
				, cne_certificate_deny.NewCertificateNumber
			from dbo.DeprecatedCommonNationalExamCertificateDeny cne_certificate_deny
			
			drop table dbo.DeprecatedCommonNationalExamCertificateDeny

			alter partition scheme CommonNationalExamCertificateDenyPartitionScheme
			next used [primary]
			
			alter partition function CommonNationalExamCertificateDenyLeftPartition()
			merge range(@partition)

			fetch next from partition_cursor into @partition
		end
		close partition_cursor
		deallocate partition_cursor

		delete from dbo.CommonNationalExamCertificateDenyActivePartition
		where [Year] < @yearFrom
	end

	-- Добавление новых секций запрещений сертификатов.
	if not exists(select 1
			from dbo.CommonNationalExamCertificateDenyActivePartition [partition]
			where [partition].[Year] = @year)
	begin
		alter partition scheme CommonNationalExamCertificateDenyPartitionScheme
		next used [primary]
			
		alter partition function CommonNationalExamCertificateDenyLeftPartition()
		split range (@year)

		insert into dbo.CommonNationalExamCertificateDenyActivePartition
			(
			[Year]
			)
		values 
			(
			@year
			)
	end  
		
	-- Импорт данных запрещений сертификатов.
	if not object_id('dbo.ImportingCommonNationalExamCertificateDeny') is null
		drop table dbo.ImportingCommonNationalExamCertificateDeny

	create table dbo.ImportingCommonNationalExamCertificateDeny
		(
		Id bigint not null
		, CreateDate datetime not null
		, UpdateDate datetime not null
		, UpdateId  uniqueidentifier
		, [Year] int not null
		, CertificateNumber nvarchar(255) not null
		, Comment ntext not null
		, NewCertificateNumber nvarchar(255)
		)

	set @commandText = 
		N'bulk insert dbo.ImportingCommonNationalExamCertificateDeny
		from ''<importFile>'' with (codepage = 1251) ' 

	set @commandText = replace(@commandText, '<importFile>', 
		replace(@importFilePath, '''', '''''')) 

	exec(@commandText) 
	
	set @commandText = replace(
		'alter table dbo.ImportingCommonNationalExamCertificateDeny
		with check
		add constraint ImportingCertificateDenyCK
			check ([Year] = <year>) ', '<year>', @year)

	exec(@commandText) 

	alter table dbo.ImportingCommonNationalExamCertificateDeny
	add constraint ImportingCertificateDenyPK
		primary key clustered ([Year], CertificateNumber)

	if not object_id('dbo.DeprecatedCommonNationalExamCertificateDeny') is null
		drop table dbo.DeprecatedCommonNationalExamCertificateDeny

	create table dbo.DeprecatedCommonNationalExamCertificateDeny
		(
		Id bigint not null
		, CreateDate datetime not null
		, UpdateDate datetime not null
		, UpdateId  uniqueidentifier
		, [Year] int not null
		, CertificateNumber nvarchar(255) not null
		, Comment ntext not null
		, NewCertificateNumber nvarchar(255)
		)

	alter table dbo.DeprecatedCommonNationalExamCertificateDeny
	add constraint DeprecatedCertificatePK
		primary key clustered ([Year], CertificateNumber)

	-- Перенос данных в таблицы.
	alter table dbo.CommonNationalExamCertificateDeny
	switch partition $partition.CommonNationalExamCertificateDenyLeftPartition(@year) 
		to dbo.DeprecatedCommonNationalExamCertificateDeny

	alter table dbo.ImportingCommonNationalExamCertificateDeny
	switch to dbo.CommonNationalExamCertificateDeny
		partition $partition.CommonNationalExamCertificateDenyLeftPartition(@year) 

	drop table dbo.DeprecatedCommonNationalExamCertificateDeny
	drop table dbo.ImportingCommonNationalExamCertificateDeny

	return 0
end
