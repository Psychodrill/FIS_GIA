IF NOT EXISTS (SELECT id FROM MigrationName WHERE FileName='067_2012_07_11_ReportOrgErrorRequests.sql')
 BEGIN
	BEGIN TRANSACTION;
		BEGIN TRY
			CREATE FUNCTION [dbo].[ReportOrgErrorRequests]
			(	

			)
			RETURNS TABLE 
			AS
			RETURN 
			(
				-- Add the SELECT statement with parameter references here
				SELECT 'Организация' as Name,1000 as Requests
			)
		-- =========================================================================
		-- Запись информации о текущей миграции в лог
		insert into Migrations(MigrationVersion, MigrationName) values (67, '067_2012_07_11_ReportOrgErrorRequests.sql')
		-- =========================================================================
		END TRY
		BEGIN CATCH
			IF @@TRANCOUNT > 0
				ROLLBACK TRANSACTION;
			DECLARE @ErrorMessage NVARCHAR(4000);
			DECLARE @ErrorSeverity INT;
			DECLARE @ErrorState INT;

			SELECT 
				@ErrorMessage = ERROR_MESSAGE(),
				@ErrorSeverity = ERROR_SEVERITY(),
				@ErrorState = ERROR_STATE();
			RAISERROR(@ErrorMessage,@ErrorSeverity,@ErrorState)
		END CATCH;
	IF @@TRANCOUNT > 0
		COMMIT TRANSACTION;
END 