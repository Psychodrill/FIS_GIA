--------------------------------------------------
-- Имеет ли учетная запись пользователя комментарий администратора.
-- v1.0: Created by Makarev Andrey 04.04.2008
--------------------------------------------------
CREATE function dbo.HasUserAccountAdminComment
	(
	@status nvarchar(255)
	)
returns bit 
as  
begin
	return case
			when @status in (N'deactivated', N'revision') then 1
			else 0
		end
end

