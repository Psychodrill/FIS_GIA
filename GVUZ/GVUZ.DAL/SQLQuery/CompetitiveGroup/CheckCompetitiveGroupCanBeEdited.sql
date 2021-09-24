--DECLARE @CompetitiveGroupID INT = 11; 

Select 
-- Заявление со статусом 8 - нельзя редактировать, кроме количества мест
count(a8.ApplicationID) as ApplicationState8
-- есть заявления, но нет со статусом 8 - можно нехотя редактировать
,count(a9.ApplicationID) as ApplicationStateNot8
From competitiveGroup cg  (NOLOCK)
left join [ApplicationCompetitiveGroupItem] acgi  with (nolock) on acgi.CompetitiveGroupId = cg.CompetitiveGroupID
left join application a8  with (nolock) on a8.ApplicationID = acgi.ApplicationId and a8.StatusID=8
left join application a9  with (nolock) on a9.ApplicationID = acgi.ApplicationId and a9.StatusID!=8
Where cg.CompetitiveGroupID = @CompetitiveGroupID; 