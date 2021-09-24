
-- =============================================
-- Роли пользователей
-- v.1.0: Created by Makarev Andrey 02.04.2008
-- =============================================
CREATE view [dbo].[AccountRole] with schemabinding
as
select 
	group_account.AccountId AccountId
	, [role].Code RoleCode
	, group_account.GroupId GroupId
	, group_role.RoleId RoleId
	, group_role.IsActiveCondition IsActiveCondition
from
	dbo.GroupAccount group_account
		inner join dbo.GroupRole group_role 
			on group_account.GroupId = group_role.GroupId
		inner join dbo.[Role] [role] 
			on [role].Id = group_role.RoleId
where
	group_role.IsActive = 1

