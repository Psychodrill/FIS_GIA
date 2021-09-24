/*
 * Обфускация персональных данных в БД
 */

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[NewIDForFunction]'))
DROP VIEW [dbo].[NewIDForFunction]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RandomLetter]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].RandomLetter
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RandomDigit]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].RandomDigit
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Obfuscate]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].Obfuscate
GO

CREATE VIEW NewIDForFunction AS
SELECT NEWID() idNew
GO

CREATE FUNCTION RandomLetter() RETURNS CHAR(1) BEGIN
DECLARE @cReturn CHAR
SELECT @cReturn=CHAR(ABS(CHECKSUM(idNew))%26+192) FROM NewIDForFunction
RETURN @cReturn
END
GO

CREATE FUNCTION RandomDigit() RETURNS CHAR(1) BEGIN
DECLARE @cReturn CHAR
SELECT @cReturn=CAST(ABS(CHECKSUM(idNew))%10 AS CHAR(1)) FROM NewIDForFunction
RETURN @cReturn
END
GO

CREATE FUNCTION Obfuscate(@sIn VARCHAR(MAX))
RETURNS VARCHAR(MAX) AS
BEGIN

IF (@sIn IS NULL)
RETURN NULL;

DECLARE @cbIn INT
DECLARE @sResult VARCHAR(MAX)
DECLARE @cbProcessed INT
DECLARE @sNextChar CHAR(1)
DECLARE @nNextChar INT

SET @cbProcessed = 1
SET @sResult = ''
SELECT @cbIn=LEN(@sIn)

WHILE @cbProcessed <= @cbIn BEGIN
	SET @sNextChar = SUBSTRING(@sIn,@cbProcessed,1)
	SET @nNextChar = ASCII(@sNextChar)
	
	SELECT @sResult = @sResult +
	CASE
		WHEN @nNextChar = 32 THEN ' '
		WHEN @nNextChar = 34 THEN '"'
		WHEN @nNextChar = 40 THEN '('
		WHEN @nNextChar = 41 THEN ')'		
		WHEN @nNextChar = 45 THEN '-'
		WHEN @nNextChar = 46 THEN '.'
		WHEN @nNextChar = 64 THEN '@'
		WHEN @nNextChar = 185 THEN '№'	
		
		WHEN @nNextChar BETWEEN 48 AND 57 THEN dbo.RandomDigit()
		WHEN @nNextChar IN (89,121) THEN @sNextChar
		WHEN @nNextChar BETWEEN 192 AND 223 THEN UPPER(dbo.RandomLetter())
		WHEN @nNextChar BETWEEN 224 AND 255 THEN LOWER(dbo.RandomLetter())				
		
		ELSE dbo.RandomLetter()
	END
	SET @cbProcessed = @cbProcessed + 1
END
RETURN @sResult
END
GO

--------------------------------------------
-- Entrant
--------------------------------------------
UPDATE Entrant
SET
	LastName = dbo.Obfuscate(LastName),
	FirstName = dbo.Obfuscate(FirstName),
	MiddleName = dbo.Obfuscate(MiddleName)
GO
PRINT 'Entrant done..'

--------------------------------------------
-- EntrantDocument
--------------------------------------------
DECLARE @tmp TABLE (
	EntrantDocumentID INT NOT NULL PRIMARY KEY,
	DocumentSeries VARCHAR(10),
	DocumentNumber VARCHAR(50),	
	ObfuscatedDocumentSeries VARCHAR(10),
	ObfuscatedDocumentNumber VARCHAR(50))

INSERT INTO @tmp
SELECT 
	EntrantDocumentID,		
	DocumentSeries,
	DocumentNumber,
	dbo.Obfuscate(DocumentSeries),
	dbo.Obfuscate(DocumentNumber)
FROM dbo.EntrantDocument	
WHERE DocumentSeries IS NOT NULL OR DocumentNumber IS NOT NULL
PRINT 'Tmp done..'

UPDATE EntrantDocument
SET
	DocumentSpecificData = 
	REPLACE(REPLACE(DocumentSpecificData, ISNULL(t.DocumentNumber, ''), ISNULL(t.ObfuscatedDocumentNumber, '')),
		ISNULL(t.DocumentSeries, ''), ISNULL(t.ObfuscatedDocumentSeries, '')),
	DocumentNumber = t.ObfuscatedDocumentNumber,
	DocumentSeries = t.ObfuscatedDocumentSeries
FROM @tmp t
WHERE t.EntrantDocumentID = EntrantDocument.EntrantDocumentID
GO