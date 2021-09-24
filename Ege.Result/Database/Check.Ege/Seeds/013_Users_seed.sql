insert into [Users](RegionId, [LoginName], [Password], [IsEnabled]) values(null, 'fct', '1000:FrRvThbP/VgqIlPwGDyGJHM2kT0wRqbi:fXGUQITUdXlnjniSECojA2+W6iElSZuP', 1)
insert into [UserRoles](UserId, Role) select UserId, 1 from Users where LoginName = 'fct'

insert into [Users](RegionId, [LoginName], [Password], [IsEnabled]) values(61, 'rcoi', '1000:FrRvThbP/VgqIlPwGDyGJHM2kT0wRqbi:fXGUQITUdXlnjniSECojA2+W6iElSZuP', 1)
insert into [UserRoles](UserId, Role) select UserId, 2 from Users where LoginName = 'rcoi'

-- insert into [Users](RegionId, [LoginName], [Password], [IsEnabled]) values(null, 'hsc', '1000:FrRvThbP/VgqIlPwGDyGJHM2kT0wRqbi:fXGUQITUdXlnjniSECojA2+W6iElSZuP', 1)
-- insert into [UserRoles](UserId, Role) select UserId, 3 from Users where LoginName = 'hsc'
