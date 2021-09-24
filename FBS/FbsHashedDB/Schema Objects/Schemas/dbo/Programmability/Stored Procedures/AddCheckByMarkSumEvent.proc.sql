-- =============================================
-- Author:		<Lybenko,,Alexander>
-- Create date: <20.07.2012,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[AddCheckByMarkSumEvent]
	 @LastName nvarchar(1000), @FirstName nvarchar(1000), @PatronymicName nvarchar(1000)=null,@ListSubject nvarchar(1000), @SumMark int,@OrgId int
as
BEGIN
	INSERT INTO CheckByMarkSumLog (LastName,FirstName,GivenName,OrgId,MarkSum,Subjects) values(@LastName,@FirstName,@PatronymicName,@OrgId,@SumMark,@ListSubject)
	select 	@@IDENTITY
END

GO