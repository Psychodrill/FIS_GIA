CREATE PROCEDURE [dbo].[SearchVUZ] 
(@orgNamePrefix varchar(256))
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @mask varchar(260)
	SET @mask='%'+replace(replace(@orgNamePrefix,'%','[%]'),'*','%')+'%'
	
	SELECT OrgName 
	FROM dbo.OrgEtalon 
	WHERE OrgName LIKE @mask
	ORDER BY OrgName
END