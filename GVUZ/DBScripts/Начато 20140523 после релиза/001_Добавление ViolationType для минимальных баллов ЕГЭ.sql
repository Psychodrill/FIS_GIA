  if not exists (select * from ViolationType where ViolationID = 7 and BriefName = 'Общая льгота - минимум ЕГЭ')
  insert into ViolationType Values(7, 'Общая льгота - минимум ЕГЭ', 'Результаты ЕГЭ ниже количества баллов, необходимых для использования особого права', GETDATE(), GETDATE())
  