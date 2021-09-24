

CREATE PROCEDURE [dbo].[UpdateGroupUserEsrp]
	@login nvarchar(255),
	@groupIdEsrp int,
	@groupsEsrp nvarchar(255)
AS
BEGIN
	declare @accountId int
	select @accountId = A.Id
	from Account A
	where A.[Login] = @login
	
	declare @groupId int
	select @groupId = G.Id
	from [Group] G
	where G.GroupIdEsrp = @groupIdEsrp	
	
	if (@groupsEsrp is not null)	
	begin
		declare @sql nvarchar(1000)
		set @sql =
		'delete from GroupAccount
		where 
			GroupAccount.GroupId in (select G.Id
									 from [Group] G
									 where
										G.GroupIdEsrp not in (' + @groupsEsrp + ') 
									 ) '+
			'and GroupAccount.AccountId = ' + cast(@accountId as nvarchar(255))
		exec sp_executesql @sql
	end
	
	insert into GroupAccount (GroupId, AccountId)
	select @groupId, @accountId 
	where not exists(select *
			   from GroupAccount GA
			   where GA.AccountId = @accountId
			   and GA.GroupId = @groupId)	
	
	exec RefreshRoleActivity @accountId, null
	
END

