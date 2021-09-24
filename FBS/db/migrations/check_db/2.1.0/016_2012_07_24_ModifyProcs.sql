-- =========================================================================
-- ������ ���������� � ������� �������� � ���
insert into Migrations(MigrationVersion, MigrationName) values (16, '016_2012_07_24_ModifyProcs.sql')
-- =========================================================================
GO 

/****** Object:  StoredProcedure [dbo].[SearchProcessingCommonNationalExamCertificateCheckBatch]    Script Date: 07/24/2012 20:50:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- exec SearchProcessingCommonNationalExamCertificateCheckBatch
-- ============================================================
-- ��������� ������� ������� ��� �������� ������������ ���
-- , ����������� � ���������
-- v.1.0: Created by Sedov Anton 28.05.2008
-- v.1.1: Modified by Fomin Dmitriy 28.05.2008
-- ��������� ����������� �� IsProcess.
-- v.1.2: Modified by Fomin Dmitriy 28.05.2008
-- ������ ���� Batch - ������� ����� �� ������ ������.
-- ============================================================
ALTER procedure [dbo].[SearchProcessingCommonNationalExamCertificateCheckBatch]
as
begin
	declare 
		@chooseDbText nvarchar(4000)
		, @commandText nvarchar(4000)

	set @chooseDbText = replace('use <database>', '<database>', dbo.GetCheckDataDbName())
	
	set @commandText = 
		N'
		select 
			exam_certificate_check_batch.Id Id
			, exam_certificate_check_batch.Executing Executing
		from dbo.CommonNationalExamCertificateCheckBatch exam_certificate_check_batch with(nolock)
		where 
			exam_certificate_check_batch.IsProcess = 1
			and exam_certificate_check_batch.Executing is null '

	set @commandText = @chooseDbText + @commandText
	
	exec sp_executesql @commandText

	return 0
end
