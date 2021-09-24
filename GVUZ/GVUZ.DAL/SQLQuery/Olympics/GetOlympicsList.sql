--1.1.2.1	Перечень олимпиад 
--таблица с перечнем олимпиад школьников (из OlympicTypeProfile, связанной с OlympicType), в составе столбцов:
--OlympicType.OlympicNumber 	связь OlympicType и OlympicTypeProfile по OlympicType.OlympicID = OlympicTypeProfile.OlympicTypeID
--OlympicType.Name связь OlympicType и OlympicTypeProfile по OlympicType.OlympicID = OlympicTypeProfile.OlympicTypeID
--OlympicProfile.ProfileName связь OlympicTypeProfile и OlympicProfile по OlympicProfileID
--OlympicTypeProfile.OrganizerName
--OlympicType.OlympicYear

select
	o.OlympicID,
	o.Name,
	o.OlympicNumber,
	o.OlympicYear,
	(select n.ProfileName from OlympicProfile as n where n.OlympicProfileID = p.OlympicProfileID) ProfileName,
	(select l.Name from OlympicLevel as l where l.OlympicLevelID = p.OlympicLevelID) LevelName,
	p.OrganizerName,
	p.OlympicProfileID,
	p.OlympicLevelID,
	p.OlympicTypeProfileID

from
	OlympicTypeProfile as p
	left join OlympicType as o on p.OlympicTypeID = o.OlympicID 


	--left join OlympicSubject as j on p.OlympicTypeProfileID = j.OlympicTypeProfileID 
	--(select s.Name from Subject as s where s.SubjectID = j.SubjectID) SubjectName,
	--,j.SubjectID

--DECLARE @PageNumber INT = 1
--DECLARE @PageSize   INT = 10

--ORDER BY 2 DESC
--OFFSET @PageSize * (@PageNumber - 1) ROWS FETCH NEXT @PageSize ROWS ONLY;
