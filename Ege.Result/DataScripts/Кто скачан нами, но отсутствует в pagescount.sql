select 
	hp.RegionId,
	pe.ExamGlobalId,
	Cast(e.ExamDate as date),
	bi.Barcode,
	bi.ProjectBatchId,
	bi.ProjectName,
	bi.PageCount/2 as PageCount
from
	[Hsc].dbo.BlanksDownload bd
join
	[Hsc].dbo.Participants hp on hp.Id = bd.ParticipantId
join
	ap_ParticipantExams pe on pe.Id = hp.ParticipantExamId
join
	dat_Exams e on e.ExamGlobalId = pe.ExamGlobalId
join
	ap_BlankInfo bi on bi.ParticipantExamId = pe.Id
left join
	[Hsc].dbo.PagesCount pc 
		on 
			pc.ExamGlobalId = pe.ExamGlobalId 
			and pc.Barcode = bi.Barcode 
			and pc.ProjectBatchId = bi.ProjectBatchId
			and pc.ProjectName = bi.ProjectName
where pc.Id is null
group by	
	hp.RegionId,
	pe.ExamGlobalId,
	e.ExamDate,
	bi.Barcode,
	bi.ProjectBatchId,
	bi.ProjectName,
	bi.PageCount