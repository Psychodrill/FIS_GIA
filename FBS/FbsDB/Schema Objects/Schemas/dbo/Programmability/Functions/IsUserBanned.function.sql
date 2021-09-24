--------------------------------------------------
-- Имеет ли учетная запись пользователя комментарий администратора.
-- v1.0: Created by Makarev Andrey 04.04.2008
--------------------------------------------------
CREATE function dbo.IsUserBanned
	(
	@login nvarchar(255)
	)
returns bit 
as  
begin
declare @result bit
set @result = 0
select top 1 @result = IsBanned from Account where [Login] = @login
return @result
	
end

