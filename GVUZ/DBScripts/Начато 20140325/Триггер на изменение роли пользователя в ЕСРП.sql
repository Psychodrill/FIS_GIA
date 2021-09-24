/****** Object:  Trigger [GroupAccountUpdateDateTrigger]    Script Date: 03/25/2014 12:27:26 ******/
IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[GroupAccountUpdateDateTrigger]'))
DROP TRIGGER [dbo].[GroupAccountUpdateDateTrigger]
GO

USE [esrp]
GO

/****** Object:  Trigger [dbo].[GroupAccountUpdateDateTrigger]    Script Date: 03/25/2014 12:27:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE TRIGGER [dbo].[GroupAccountUpdateDateTrigger] 
   ON  [dbo].[GroupAccount] 
   AFTER UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	update Account
	set UpdateDate = GETDATE()
	where Id in (select AccountId from inserted)
END

GO


