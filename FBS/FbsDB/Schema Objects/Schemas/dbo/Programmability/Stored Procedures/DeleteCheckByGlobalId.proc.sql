-- =============================================
-- Удаление проверки из лога
-- v.1.0: Created by Makarev Andrey 21.04.2008
-- =============================================
CREATE PROCEDURE dbo.DeleteCheckFromCommonNationalExamCertificateCheckBatchById
	@id bigint
as
begin
	declare @internalId bigint
	set @internalId = dbo.GetInternalId(@id)

	DELETE FROM [dbo].[CommonNationalExamCertificateCheckBatch]
      WHERE [Id]=@internalId
end

GO

CREATE PROCEDURE dbo.DeleteCheckFromCommonNationalExamCertificateRequestBatchById
	@id bigint
as
begin
	declare @internalId bigint
	set @internalId = dbo.GetInternalId(@id)

	DELETE FROM [dbo].[CommonNationalExamCertificateRequestBatch]
      WHERE [Id]=@internalId
end
