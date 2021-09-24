
-- exec dbo.CheckCommonNationalExamCertificateForm
-- ================================================
-- Проверить бланки свидетельств ЕГЭ.
-- v.1.0: Created by Fomin Dmitriy 08.06.2008
-- v.1.1: Modified by Fomin Dmitriy 16.06.2008
-- Проверки проводятся в момент сохранения формы.
-- ================================================
CREATE procedure dbo.CheckCommonNationalExamCertificateForm
	@regionCode nvarchar(50)
as
begin
	declare
		@commandText nvarchar(4000)
		, @declareCommandText nvarchar(4000)
		, @chooseDbText nvarchar(4000)

	set @chooseDbText = ''
	set @declareCommandText = ''

	set @chooseDbText = 'use <databaseName> '
	set @chooseDbText = replace(@chooseDbText, '<databaseName>', dbo.GetCheckDataDbName())	

	set @declareCommandText = 
		'declare
			@regionId int
			, @year int
			, @partition bigint '

	set @commandText = replace(
		'
		select @regionId = region.Id 
		from dbo.Region region with (nolock)
		where region.Code = ''<region_code>''

		set @year = Year(GetDate())
		set @partition = dbo.GetCommonNationalExamCertificateFormPartition(@regionId, @year)

		select
			form.Number
			, form.CertificateNumber
			, form.IsValid
			, form.IsCertificateExist
			, form.IsCertificateDeny
		from dbo.CommonNationalExamCertificateForm form
		where form.Partition = @partition
			and form.IsDeny = 0 ', '<region_code>', replace(@regionCode, '''', ''''''))

	exec (@chooseDbText + @declareCommandText + @commandText)

	return 0
end
