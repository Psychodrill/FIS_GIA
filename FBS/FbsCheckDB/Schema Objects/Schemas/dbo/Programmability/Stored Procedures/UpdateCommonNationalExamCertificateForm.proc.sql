
/*
   Для работы процедуры требуются следующие 
   временные таблицы:
   create table #CommonNationalExamCertificateFormNumberRange
		(	
		NumberFrom nvarchar(255)
		, NumberTo nvarchar(255)
		)
	create table #CommonNationalExamCertificateForm
		(
		CheckingFormId uniqueidentifier
		, Number nvarchar(255)
		, CertificateNumber nvarchar(255)
		, LastName nvarchar(255)
		, FirstName nvarchar(255)
		, PatronymicName nvarchar(255)
		, PassportSeria nvarchar(255)
		, PassportNumber nvarchar(255)
		, IsBlank bit
		, IsDuplicate bit
		, IsDeny bit
		, IsValid bit
		)
	create table #CommonNationalExamCertificateSubjectForm
		(
		CheckingFormId uniqueidentifier
		, SubjectCode nvarchar(255)
		, Mark numeric(5,1)
		)
*/

-- exec dbo.UpdateCommonNationalExamCertificateForm
-- ================================================
-- Сохранить изменения бланков свидетельств ЕГЭ.
-- v.1.0: Created by Fomin Dmitriy 07.06.2008
-- v.1.1: Modified by Fomin Dmitriy 16.06.2008
-- Добавлено вычисление результатов.
-- v.1.2: Modified by Sedov Anton 16.06.2008
-- Добавлен выбор базы  на кторой  будет 
-- выполняться ХП.
-- ================================================
CREATE procedure [dbo].[UpdateCommonNationalExamCertificateForm]
	@regionCode nvarchar(50)
as
begin
	declare
		@commandText nvarchar(4000)
		, @declareCommandText nvarchar(4000)
		, @chooseDbText nvarchar(4000)
		, @insertCommandText nvarchar(4000)
		, @insertSubjectCommandText nvarchar(4000)
		, @alterCommandText nvarchar(4000)
		, @deleteOldCertificateForm nvarchar(4000)
		, @deleteOldSubjectForm nvarchar(4000)
		, @baseName nvarchar(255)

	set @chooseDbText = ''
	set @declareCommandText = ''
	set @alterCommandText = ''

	set @chooseDbText = 'use <databaseName> '
	set @chooseDbText = replace(@chooseDbText, '<databaseName>', dbo.GetCheckDataDbName())
	set @baseName = dbo.GetDataDbName(1, 1)
		

	set @declareCommandText = 
		N'
		declare
			@regionId int
			, @year int
			, @partition bigint
			, @updateId uniqueidentifier
			, @date datetime
			, @innerCommandText nvarchar(4000)

		set @innerCommandText = ''''
	
		declare @formId table
			(
			FormId bigint
			, Number nvarchar(255)
			)

		declare @certificateForm table
			(
			CheckingFormId uniqueidentifier
			, Number nvarchar(255)
			, CertificateId bigint
			, CertificateYear int
			, CertificateNumber nvarchar(255)
			, LastName nvarchar(255)
			, FirstName nvarchar(255)
			, PatronymicName nvarchar(255)
			, PassportSeria nvarchar(255)
			, PassportNumber nvarchar(255)
			, IsBlank bit
			, IsDeny bit
			, IsDuplicate bit
			, IsValid bit
			, IsCertificateExist bit
			, IsCertificateDeny bit
			)
		'
	
	set @commandText =
		N'
		exec dbo.RefreshCommonNationalExamCertificateFormPartition

		select @regionId = region.Id
		from dbo.Region region with (nolock)
		where region.Code = ''<regionCode>''

		set @year = Year(GetDate())
		set @partition = dbo.GetCommonNationalExamCertificateFormPartition(@regionId, @year)
		set @updateId = NewId()
		set @date = GetDate();

		insert into @certificateForm
		select
			form.CheckingFormId
			, form.Number
			, [certificate].Id
			, [certificate].[Year]
			, form.CertificateNumber
			, form.LastName
			, form.FirstName
			, form.PatronymicName
			, form.PassportSeria
			, form.PassportNumber
			, form.IsBlank
			, form.IsDeny
			, form.IsDuplicate
			, cast(case
				when not [certificate].Id is null
					and form.LastName = certificate.LastName
					and form.FirstName = certificate.FirstName
					and form.PatronymicName = certificate.PatronymicName
					and certificate.InternalPassportSeria = dbo.GetInternalPassportSeria(form.PassportSeria)
					and form.PassportNumber = certificate.PassportNumber
					and certificate_deny.Id is null then 1
				else 0
			end as bit) IsValid
			, cast(case
				when not [certificate].Id is null then 1
				else 0
				end as bit) IsCertificateExist
			, cast(case
				when not certificate_deny.Id is null then 1
				else 0
			end as bit) IsCertificateDeny
		from #CommonNationalExamCertificateForm form
			left outer join <dataDbName>.dbo.CommonNationalExamCertificate [certificate] with (nolock)
				on [certificate].Number = form.CertificateNumber
			left outer join <dataDbName>.dbo.CommonNationalExamCertificateDeny certificate_deny with (nolock)
				on certificate_deny.CertificateNumber = form.CertificateNumber

		if exists(select 1
				from #CommonNationalExamCertificateFormNumberRange new_range
					full outer join dbo.CommonNationalExamCertificateFormNumberRange old_range with (nolock)
						on old_range.[Year] = @year
							and old_range.RegionId = @regionId
							and new_range.NumberFrom = old_range.NumberFrom
							and new_range.NumberTo = old_range.NumberTo
					where 
						new_range.NumberFrom is null
						or old_range.NumberFrom is null
						or new_range.NumberTo is null
						or old_range.NumberTo is null)
			begin
				delete from dbo.CommonNationalExamCertificateFormNumberRange
				where [Year] = @year
					and RegionId = @regionId

				insert into dbo.CommonNationalExamCertificateFormNumberRange
					(
					CreateDate
					, UpdateDate
					, UpdateId
					, [Year]
					, RegionId
					, NumberFrom
					, NumberTo
					)
				select
					@date
					, @date
					, @updateId
					, @year
					, @regionId
					, [range].NumberFrom
					, [range].NumberTo
				from #CommonNationalExamCertificateFormNumberRange [range]
			end
			'

		set @insertCommandText =
			N'
			if not object_id(''dbo.UpdatingCommonNationalExamCertificateForm<regionCode>'') is null
				drop table dbo.UpdatingCommonNationalExamCertificateForm<regionCode>

			create table dbo.UpdatingCommonNationalExamCertificateForm<regionCode>
				(
				Id bigint identity(1,1) not null
				, Number nvarchar(255) not null 
				, CreateDate datetime not null
				, UpdateDate datetime not null
				, UpdateId uniqueidentifier not null
				, [Year] int not null
				, RegionId int not null
				, [Partition] bigint not null
				, CertificateNumber nvarchar(255) not null
				, LastName nvarchar(255) 
				, FirstName nvarchar(255)
				, PatronymicName nvarchar(255)
				, PassportSeria nvarchar(255)
				, PassportNumber nvarchar(255)
				, IsBlank bit not null
				, IsDeny bit not null
				, IsDuplicate bit not null
				, IsValid bit
				, IsCertificateExist bit
				, IsCertificateDeny bit 
				)

			insert dbo.UpdatingCommonNationalExamCertificateForm<regionCode>
				(
				Number
				, CreateDate
				, UpdateDate
				, UpdateId
				, [Year]
				, RegionId
				, [Partition]
				, CertificateNumber
				, LastName
				, FirstName
				, PatronymicName
				, PassportSeria
				, PassportNumber
				, IsBlank
				, IsDeny
				, IsDuplicate
				, IsValid
				, IsCertificateExist
				, IsCertificateDeny
				)
			output inserted.Id, inserted.Number into @formId
			select
				form.Number
				, @date
				, @date
				, @updateId
				, @year
				, @regionId
				, @partition
				, form.CertificateNumber
				, form.LastName
				, form.FirstName
				, form.PatronymicName
				, form.PassportSeria
				, form.PassportNumber
				, form.IsBlank
				, form.IsDeny
				, form.IsDuplicate
				, form.IsValid
				, form.IsCertificateExist
				, form.IsCertificateDeny
			from @certificateForm form
			where form.IsValid = 0

			insert dbo.UpdatingCommonNationalExamCertificateForm<regionCode>
				(
				Number
				, CreateDate
				, UpdateDate
				, UpdateId
				, [Year]
				, RegionId
				, [Partition]
				, CertificateNumber
				, LastName
				, FirstName
				, PatronymicName
				, PassportSeria
				, PassportNumber
				, IsBlank
				, IsDeny
				, IsDuplicate
				, IsValid
				, IsCertificateExist
				, IsCertificateDeny
				)
			output inserted.Id, inserted.Number into @formId
			select
				form.Number
				, @date
				, @date
				, @updateId
				, @year
				, @regionId
				, @partition
				, form.CertificateNumber
				, form.LastName
				, form.FirstName
				, form.PatronymicName
				, form.PassportSeria
				, form.PassportNumber
				, form.IsBlank
				, form.IsDeny
				, form.IsDuplicate
				, cast(case
					when not exists(select 1
							from <dataDbName>.dbo.CommonNationalExamCertificateSubject certificate_subject with (nolock)
								inner join @certificateForm inner_form
									on inner_form.CheckingFormId = form.CheckingFormId
										and certificate_subject.CertificateId = form.CertificateId
										and certificate_subject.[Year] = form.CertificateYear
								inner join dbo.Subject [subject] with (nolock)
									on subject.Id = certificate_subject.SubjectId
								full outer join #CommonNationalExamCertificateSubjectForm form_subject with (nolock)
									on form_subject.SubjectCode = [subject].Code
										and form_subject.Mark = certificate_subject.Mark
							where 
								(form_subject.CheckingFormId is null
									or form_subject.CheckingFormId = form.CheckingFormId)
								and (form_subject.CheckingFormId is null
									or certificate_subject.Id is null)) then 1
					else 0
				end as bit) IsValid
				, form.IsCertificateExist
				, form.IsCertificateDeny
			from @certificateForm form
			where form.IsValid = 1 '

		set @deleteOldCertificateForm = 
			N'
			if not object_id(''dbo.OldCommonNationalExamCertificateForm<regionCode>'') is null
				drop table dbo.OldCommonNationalExamCertificateForm<regionCode>

			create table dbo.OldCommonNationalExamCertificateForm<regionCode>
				(
				Id bigint identity(1,1) not null
				, Number nvarchar(255) not null 
				, CreateDate datetime not null
				, UpdateDate datetime not null
				, UpdateId uniqueidentifier not null
				, [Year] int not null
				, RegionId int not null
				, [Partition] bigint not null
				, CertificateNumber nvarchar(255) not null
				, LastName nvarchar(255) 
				, FirstName nvarchar(255)
				, PatronymicName nvarchar(255)
				, PassportSeria nvarchar(255)
				, PassportNumber nvarchar(255)
				, IsBlank bit not null
				, IsDeny bit not null
				, IsDuplicate bit not null
				, IsValid bit
				, IsCertificateExist bit
				, IsCertificateDeny bit 
				)
			
			set @innerCommandText = 
				N'' 
				alter table dbo.OldCommonNationalExamCertificateForm<regionCode> 
				with check
				add constraint OldCertificateFromCK<regionCode>
					check ([Partition] = <inner_partition>)
				''
			set @innerCommandText = replace(@innerCommandText, ''<inner_partition>'', @partition)

			exec (@innerCommandText)

			alter table dbo.OldCommonNationalExamCertificateForm<regionCode>
			add constraint OldCertificateFormPK<regionCode>
				primary key clustered ([Partition], CertificateNumber)

			insert into dbo.OldCommonNationalExamCertificateForm<regionCode>
			select
				Number 
				, CreateDate 
				, UpdateDate 
				, UpdateId 
				, [Year]
				, RegionId
				, [Partition] 
				, CertificateNumber 
				, LastName 
				, FirstName 
				, PatronymicName 
				, PassportSeria
				, PassportNumber 
				, IsBlank 
				, IsDeny 
				, IsDuplicate 
				, IsValid 
				, IsCertificateExist 
				, IsCertificateDeny 
			from
				dbo.CommonNationalExamCertificateForm  
			where 
				[Partition] = @partition

			delete cne_certificate_form 
			from dbo.CommonNationalExamCertificateForm cne_certificate_form
			where cne_certificate_form.[Partition] = @partition
			'

		set @deleteOldSubjectForm = 
			N'
			if not object_id(''dbo.OldCommonNationalExamCertificateSubjectForm<regionCode>'') is null
				drop table dbo.OldCommonNationalExamCertificateSubjectForm<regionCode>

			create table dbo.OldCommonNationalExamCertificateSubjectForm<regionCode>
				(
				Id bigint identity(1,1) not null
				, [Year] int not null
				, RegionId int not null
				, [Partition] bigint not null
				, FormId bigint
				, SubjectId int not null
				, Mark numeric(5,1)
				)
			
			set @innerCommandText = 
				N''
				alter table dbo.OldCommonNationalExamCertificateSubjectForm<regionCode>
				with check
				add constraint OldCertificateSubjectFormCK<regionCode>
					check ([Partition] = <inner_partition>)
				''
			set @innerCommandText = replace(@innerCommandText, ''<inner_partition>'', @partition)

			exec (@innerCommandText)

			alter table dbo.OldCommonNationalExamCertificateSubjectForm<regionCode>
			with check
			add constraint OldCertificateSubjectFormPK<regionCode>
				primary key clustered ([Partition], SubjectId)	

			insert dbo.OldCommonNationalExamCertificateSubjectForm<regionCode>
			select
				[Year]
				, RegionId
				, [Partition] 
				, FormId 
				, SubjectId 
				, Mark 
			from  dbo.CommonNationalExamCertificateSubjectForm
			where [Partition] = @partition
			
			delete cne_subject_form 
			from dbo.CommonNationalExamCertificateSubjectForm cne_subject_form
			where cne_subject_form.[Partition] = @partition
			'

		
		set @alterCommandText = 
			N'	
			set @innerCommandText = 
				N'' 
				alter table dbo.UpdatingCommonNationalExamCertificateForm<regionCode> 
				with check
				add constraint UpdatingCertificateFromCK<regionCode>
					check ([Partition] = <inner_partition>)
				''
			set @innerCommandText = replace(@innerCommandText, ''<inner_partition>'', @partition)

			exec (@innerCommandText)

			alter table dbo.UpdatingCommonNationalExamCertificateForm<regionCode>
			add constraint UpdatingCertificateFormPK<regionCode>
				primary key clustered ([Partition], CertificateNumber)
			'

		set @insertSubjectCommandText = 
			N'
			if not object_id(''dbo.UpdatingCommonNationalExamCertificateSubjectForm<regionCode>'') is null
				drop table dbo.UpdatingCommonNationalExamCertificateSubjectForm<regionCode>

			create table dbo.UpdatingCommonNationalExamCertificateSubjectForm<regionCode>
				(
				Id bigint identity(1,1) not null
				, Year int not null
				, RegionId int not null
				, [Partition] bigint not null
				, FormId bigint
				, SubjectId int not null
				, Mark numeric(5,1)
				)

			insert dbo.UpdatingCommonNationalExamCertificateSubjectForm<regionCode>
				(
				[Year]
				, RegionId
				, [Partition]
				, FormId
				, SubjectId
				, Mark
				)
			select
				@year
				, @regionId
				, @partition
				, form_id.FormId
				, subject.Id
				, form_subject.Mark
			from #CommonNationalExamCertificateSubjectForm form_subject
				inner join #CommonNationalExamCertificateForm form
					inner join @formId form_id
						on form_id.Number = form.Number
					on form.CheckingFormId = form_subject.CheckingFormId
				left outer join dbo.Subject [subject] with (nolock)
					on subject.Code = form_subject.SubjectCode

			set @innerCommandText = 
				N''
				alter table dbo.UpdatingCommonNationalExamCertificateSubjectForm<regionCode>
				with check
				add constraint UpdatingCertificateSubjectFormCK<regionCode>
					check ([Partition] = <inner_partition>)
				''
			set @innerCommandText = replace(replace(@innerCommandText, ''<inner_partition>'', @partition),
					''<regionCode>'', @regionId)

			exec (@innerCommandText)

			alter table dbo.UpdatingCommonNationalExamCertificateSubjectForm<regionCode>
			with check
			add constraint UpdatingCertificateSubjectFormPK<regionCode>
				primary key clustered ([Partition], SubjectId)

			-- перенос  данных
			alter table dbo.UpdatingCommonNationalExamCertificateForm<regionCode>
			switch to dbo.CommonNationalExamCertificateForm
				partition $partition.CommonNationalExamCertificateFormPartitionFunction(@partition)

			alter table dbo.UpdatingCommonNationalExamCertificateSubjectForm<regionCode>
			switch to dbo.CommonNationalExamCertificateSubjectForm
				partition $partition.CommonNationalExamCertificateSubjectFormPartitionFunction(@partition)
			
			-- очистка
			drop table dbo.OldCommonNationalExamCertificateForm<regionCode>
			drop table dbo.OldCommonNationalExamCertificateSubjectForm<regionCode>

			drop table dbo.UpdatingCommonNationalExamCertificateForm<regionCode>
			drop table dbo.UpdatingCommonNationalExamCertificateSubjectForm<regionCode>		
			'

	set @commandText = replace(replace(@commandText, '<regionCode>', 
			replace(@regionCode, '''', '''''')), '<dataDbName>', @baseName)
	set @insertCommandText = replace(replace(@insertCommandText, '<regionCode>', 
			replace(@regionCode, '''', '''''')), '<dataDbName>', @baseName)
	set @deleteOldCertificateForm = replace(@deleteOldCertificateForm, '<regionCode>', replace(@regionCode, '''', ''''''))	
	set @deleteOldSubjectForm = replace(@deleteOldSubjectForm, '<regionCode>', replace(@regionCode, '''', ''''''))
	set @alterCommandText = replace(@alterCommandText, '<regionCode>', replace(@regionCode, '''', ''''''))
	set @insertSubjectCommandText = replace(@insertSubjectCommandText, '<regionCode>', replace(@regionCode, '''', ''''''))

	exec (@chooseDbText + @declareCommandText + @commandText + @insertCommandText +
			@deleteOldCertificateForm + @deleteOldSubjectForm + @alterCommandText + @insertSubjectCommandText)
	
	return 0
end

