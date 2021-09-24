select 
	e.ExamGlobalId, 
	e.ExamDate,
	s.SubjectName, 
	s.IsBasicMath, 
	s.IsForeignLanguage, 
	s.IsComposition, 
	s.IsCompositionWithLoadableBlanks, 
	s.IsOral,
	answerInExams.answerscount
from dat_Exams e
join dat_Subjects s on s.SubjectCode = e.SubjectCode
left join 
(
	select 
		COUNT(1) as answerscount, 
		pe.ExamGlobalId 
	from 
		ap_ParticipantExams pe
	join 
		ap_Answers a on a.ParticipantExamId = pe.Id
	group by 
		pe.ExamGlobalId
) as answerInExams on answerInExams.ExamGlobalId = e.ExamGlobalId
where 
	answerInExams.answerscount is not null
order by 
	--answerInExams.answerscount, 
	e.ExamDate desc