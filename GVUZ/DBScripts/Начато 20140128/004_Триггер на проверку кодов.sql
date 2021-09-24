/****** Object:  Trigger [dbo].[CheckCode]    Script Date: 02/11/2014 16:01:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[CheckCode] 
   ON  [dbo].[Direction] 
   AFTER INSERT, UPDATE
AS 
BEGIN
	SET NOCOUNT ON;
	
	declare @errors int
	select @errors = Count(DirectionID) from inserted
	where Code is null and NewCode is null
	
	if @errors > 0 
	begin
		raiserror('ѕол€ Code и NewCode не могут быть пустыми одновременно', 16, 1)
		ROLLBACK
	end
END

GO


