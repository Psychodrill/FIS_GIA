SELECT s.*, isnull(semv.MinValue, 0) as MinValue
  FROM [Subject] s
  left join SubjectEgeMinValue semv on semv.SubjectID = s.SubjectId;