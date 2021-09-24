-- отсортированный список предметов по которым проводились профильные олимпиады

select
	p.OlympicTypeProfileID,
	(select s.Name from Subject as s where s.SubjectID = j.SubjectID) SubjectName,
	j.SubjectID

from
	OlympicTypeProfile as p
	left join OlympicSubject as j on p.OlympicTypeProfileID = j.OlympicTypeProfileID

order by SubjectName