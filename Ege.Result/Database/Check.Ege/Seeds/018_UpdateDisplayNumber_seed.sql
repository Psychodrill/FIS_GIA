  update b
  set b.DisplayNumber = b.NewDisplayNumber
  from (select bs.DisplayNumber, row_number() over (partition by  SubjectCode order by SubjectCode, Id) as NewDisplayNumber from ap_BallSettings bs) as b 

  update t
  set t.DisplayNumber = t.NewDisplayNumber
  from (select ts.DisplayNumber, row_number() over (partition by  SubjectCode order by SubjectCode, Id) as NewDisplayNumber from ap_TaskSettings ts where ParentTask is null) as t 