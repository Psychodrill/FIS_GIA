-- =============================================
-- Author:		Yusupov K.I.
-- Create date: 04-06-2010
-- Description:	Пишет событие интерактивной проверки сертификата в журнал
-- =============================================
create PROCEDURE [dbo].[AddCNEWebUICheckEvent]
@AccountLogin NVARCHAR(255),					-- логин проверяющего
	@LastName NVARCHAR(255)=null,				-- фамилия сертифицируемого
	@FirstName NVARCHAR(255)=null,				-- имя сертифицируемого
	@PatronymicName NVARCHAR(255)=null,			-- отчетсво сертифицируемого
	@PassportSeria NVARCHAR(20)=NULL,			-- серия документа сертифицируемого (паспорта)
	@PassportNumber NVARCHAR(20)=NULL,			-- номер документа сертифицируемого (паспорта)
	@CNENumber NVARCHAR(20)=NULL,				-- номер сертификата
	@TypographicNumber NVARCHAR(20)=NULL,		-- типографический номер сертификата 
	@RawMarks NVARCHAR(500)=null,				-- средние оценки по предметам (через запятую, в определенном порядке)
	@IsOpenFbs bit=null,
	@EventId INT output							-- id зарегистрированного события
AS
BEGIN
	IF (SELECT disablelog 
		FROM dbo.Organization2010 
		 WHERE Id IN (SELECT OrganizationId FROM dbo.Account WHERE Login = @AccountLogin)) = 1
	BEGIN
	SELECT @EventId = 0 -- !!!!!!!! стоял NULL, но в этом случае приложение валится !!!!!!!!!!!!
	RETURN
	END
	
	IF 
	(
		@TypographicNumber IS NULL AND
		@CNENumber IS NULL AND
		@PassportNumber IS NULL AND
		@RawMarks IS NULL
	)
	BEGIN
		RAISERROR (N'Не указаны паспортные данные, типографский номер, номер свидетельства и баллы по предметам одновременно',10,1)
		RETURN
	END

	DECLARE @AccountId BIGINT
	SELECT
		@AccountId = Acc.[Id]
	FROM
		dbo.Account Acc WITH (nolock, fastfirstrow)
	WHERE
		Acc.[Login] = @AccountLogin	

	IF (@TypographicNumber IS NOT NULL)
	BEGIN
		INSERT INTO CNEWebUICheckLog 
			(AccountId,TypeCode,FirstName,LastName,PatronymicName,TypographicNumber) 
        VALUES 
			(@AccountId,'Typographic',@FirstName,@LastName,@PatronymicName,@TypographicNumber)
	END
	ELSE IF (@CNENumber IS NOT NULL)
	BEGIN
		declare @logTypeNumber nvarchar(50)
		set @logTypeNumber = 'CNENumber' + case when isnull(@IsOpenFbs,0) = 1 then 'Open' else '' end
		INSERT INTO CNEWebUICheckLog 
			(AccountId,TypeCode,FirstName,LastName,PatronymicName,CNENumber,Marks) 
        VALUES 
			(@AccountId,@logTypeNumber,@FirstName,@LastName,@PatronymicName,@CNENumber,@RawMarks)
	END
	ELSE IF (@PassportNumber IS NOT NULL)
	BEGIN
		declare @logTypePassport nvarchar(50)
		set @logTypePassport = 'Passport' + case when isnull(@IsOpenFbs,0) = 1 then 'Open' else '' end

		INSERT INTO CNEWebUICheckLog 
			(AccountId,TypeCode,FirstName,LastName,PatronymicName,PassportSeria,PassportNumber,Marks) 
        VALUES 
			(@AccountId,@logTypePassport,@FirstName,@LastName,@PatronymicName,@PassportSeria,@PassportNumber,@RawMarks)
	END
	ELSE IF (@RawMarks IS NOT NULL)
	BEGIN
		INSERT INTO CNEWebUICheckLog 
			(AccountId,TypeCode,FirstName,LastName,PatronymicName,Marks) 
        VALUES 
			(@AccountId,'Marks',@FirstName,@LastName,@PatronymicName,@RawMarks)
	END
	  SELECT @EventId = @@Identity
END