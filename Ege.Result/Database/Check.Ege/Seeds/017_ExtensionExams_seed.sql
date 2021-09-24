insert into dat_Subjects(SubjectCode, SubjectName, MinValue, IsComposition, IsBasicMath, IsForeignLanguage, IsCompositionWithLoadableBlanks) values (20, N'Сочинение', 1, 1, 0, 0, 1)
insert into dat_Subjects(SubjectCode, SubjectName, MinValue, IsComposition, IsBasicMath, IsForeignLanguage) values (21, N'Изложение', 1, 1, 0, 0)
insert into dat_Subjects(SubjectCode, SubjectName, MinValue, IsComposition, IsBasicMath, IsForeignLanguage) values (22, N'Математика базовая', 3, 0, 1, 0)
insert into dat_Subjects(SubjectCode, SubjectName, MinValue, IsComposition, IsBasicMath, IsForeignLanguage) values (29, N'Английский язык (устный)', 20, 0, 0, 1)
--insert into dat_Subjects(SubjectCode, SubjectName, MinValue, IsComposition, IsBasicMath, IsForeignLanguage) values (30, N'Немецкий язык (устный)', 20, 0, 0, 1)
--insert into dat_Subjects(SubjectCode, SubjectName, MinValue, IsComposition, IsBasicMath, IsForeignLanguage) values (31, N'Французский язык (устный)', 20, 0, 0, 1)
--insert into dat_Subjects(SubjectCode, SubjectName, MinValue, IsComposition, IsBasicMath, IsForeignLanguage) values (33, N'Испанский язык (устный)', 20, 0, 0, 1)

insert into dat_Exams(ExamGlobalId, ExamDate, WaveCode, SubjectCode) values (182, convert(datetime, '2014-11-12T00:00:00.000', 126), 2, 29)
--insert into dat_Exams(ExamGlobalId, ExamDate, WaveCode, SubjectCode) values (183, convert(datetime, '2014-11-12T00:00:00.000', 126), 2, 30)
--insert into dat_Exams(ExamGlobalId, ExamDate, WaveCode, SubjectCode) values (184, convert(datetime, '2014-11-12T00:00:00.000', 126), 2, 31)
--insert into dat_Exams(ExamGlobalId, ExamDate, WaveCode, SubjectCode) values (185, convert(datetime, '2014-11-12T00:00:00.000', 126), 2, 33)
insert into dat_Exams(ExamGlobalId, ExamDate, WaveCode, SubjectCode) values (186, convert(datetime, '2014-11-20T00:00:00.000', 126), 1024, 20)
insert into dat_Exams(ExamGlobalId, ExamDate, WaveCode, SubjectCode) values (187, convert(datetime, '2014-11-20T00:00:00.000', 126), 1024, 21)
insert into dat_Exams(ExamGlobalId, ExamDate, WaveCode, SubjectCode) values (188, convert(datetime, '2014-11-20T00:00:00.000', 126), 2, 22)

set identity_insert ap_ParticipantExams on
insert into ap_ParticipantExams(Id, ParticipantId, ExamGlobalId, PrimaryMark, TestMark, Mark5, ProcessCondition, CreateDate, IsHidden) values (26752888, 1, 182, 33, 52, 5, 6, convert(datetime, '2014-11-13T00:00:00.000', 126), 0)	--1328813,
--insert into ap_ParticipantExams(Id, ParticipantId, ExamGlobalId, PrimaryMark, TestMark, Mark5, ProcessCondition, CreateDate, IsHidden) values (26752889, 1, 183, 15, 39, 5, 6, convert(datetime, '2014-11-13T00:00:00.000', 126), 0)	--1328813,
--insert into ap_ParticipantExams(Id, ParticipantId, ExamGlobalId, PrimaryMark, TestMark, Mark5, ProcessCondition, CreateDate, IsHidden) values (26752890, 1, 184, 38, 57, 5, 6, convert(datetime, '2014-11-13T00:00:00.000', 126), 0)	--1328813,
--insert into ap_ParticipantExams(Id, ParticipantId, ExamGlobalId, PrimaryMark, TestMark, Mark5, ProcessCondition, CreateDate, IsHidden) values (26752891, 1, 185, 57, 82, 5, 6, convert(datetime, '2014-11-13T00:00:00.000', 126), 0)	--1328813,
insert into ap_ParticipantExams(Id, ParticipantId, ExamGlobalId, PrimaryMark, TestMark, Mark5, ProcessCondition, CreateDate, IsHidden) values (26752892, 1, 186, 43, 70, 5, 6, convert(datetime, '2014-11-21T00:00:00.000', 126), 0)	--1328813,
--insert into ap_ParticipantExams(Id, ParticipantId, ExamGlobalId, PrimaryMark, TestMark, Mark5, ProcessCondition, CreateDate, IsHidden) values (26752893, 1, 187, 31, 55, 5, 6, convert(datetime, '2014-11-21T00:00:00.000', 126), 0)	--1328813,
insert into ap_ParticipantExams(Id, ParticipantId, ExamGlobalId, PrimaryMark, TestMark, Mark5, ProcessCondition, CreateDate, IsHidden) values (26752894, 1, 188, 53, 79, 4 ,6, convert(datetime, '2014-11-21T00:00:00.000', 126), 0)	--1328813,
set identity_insert ap_ParticipantExams off

set identity_insert ap_Answers on
insert into ap_Answers(Id, RegionId, ParticipantExamId, TaskTypeCode, TaskNumber, AnswerValue, Mark) values (100575589, 61, 26752888, 3, 1, N'Test1', 1)
insert into ap_Answers(Id, RegionId, ParticipantExamId, TaskTypeCode, TaskNumber, AnswerValue, Mark) values (100575590, 61, 26752888, 3, 2, N'Test2', 4)
insert into ap_Answers(Id, RegionId, ParticipantExamId, TaskTypeCode, TaskNumber, AnswerValue, Mark) values (100575591, 61, 26752888, 3, 3, N'Test3', 3)
insert into ap_Answers(Id, RegionId, ParticipantExamId, TaskTypeCode, TaskNumber, AnswerValue, Mark) values (100575592, 61, 26752888, 3, 4, N'Test4', 1)
insert into ap_Answers(Id, RegionId, ParticipantExamId, TaskTypeCode, TaskNumber, AnswerValue, Mark) values (100575593, 61, 26752888, 3, 5, N'Test5', 2)
insert into ap_Answers(Id, RegionId, ParticipantExamId, TaskTypeCode, TaskNumber, AnswerValue, Mark) values (100575594, 61, 26752888, 3, 6, N'Test6', 3)
insert into ap_Answers(Id, RegionId, ParticipantExamId, TaskTypeCode, TaskNumber, AnswerValue, Mark) values (100575595, 61, 26752888, 3, 7, N'Test7', 1)
insert into ap_Answers(Id, RegionId, ParticipantExamId, TaskTypeCode, TaskNumber, AnswerValue, Mark) values (100575596, 61, 26752888, 3, 8, N'Test8', 2)

insert into ap_Answers(Id, RegionId, ParticipantExamId, TaskTypeCode, TaskNumber, AnswerValue, Mark) values (100575597, 61, 26752894, 1, 1, N'Test1', 1)
insert into ap_Answers(Id, RegionId, ParticipantExamId, TaskTypeCode, TaskNumber, AnswerValue, Mark) values (100575598, 61, 26752894, 1, 2, N'Test2', 1)
insert into ap_Answers(Id, RegionId, ParticipantExamId, TaskTypeCode, TaskNumber, AnswerValue, Mark) values (100575599, 61, 26752894, 1, 3, N'Test3', 1)
insert into ap_Answers(Id, RegionId, ParticipantExamId, TaskTypeCode, TaskNumber, AnswerValue, Mark) values (100575600, 61, 26752894, 1, 4, N'Test4', 1)
insert into ap_Answers(Id, RegionId, ParticipantExamId, TaskTypeCode, TaskNumber, AnswerValue, Mark) values (100575601, 61, 26752894, 1, 5, N'Test5', 0)

insert into ap_Answers(Id, RegionId, ParticipantExamId, TaskTypeCode, TaskNumber, AnswerValue, Mark) values (100575602, 61, 26752892, 0, 1, N'Test1', 1)
insert into ap_Answers(Id, RegionId, ParticipantExamId, TaskTypeCode, TaskNumber, AnswerValue, Mark) values (100575603, 61, 26752892, 0, 2, N'Test2', 1)
insert into ap_Answers(Id, RegionId, ParticipantExamId, TaskTypeCode, TaskNumber, AnswerValue, Mark) values (100575604, 61, 26752892, 0, 3, N'Test3', 1)
insert into ap_Answers(Id, RegionId, ParticipantExamId, TaskTypeCode, TaskNumber, AnswerValue, Mark) values (100575605, 61, 26752892, 0, 4, N'Test4', 1)
insert into ap_Answers(Id, RegionId, ParticipantExamId, TaskTypeCode, TaskNumber, AnswerValue, Mark) values (100575606, 61, 26752892, 0, 5, N'Test5', 0)
set identity_insert ap_Answers off

insert into ap_ParticipantExamLinks(ParticipantExamId, OralParticipantExamId) values ((select Id from ap_ParticipantExams where ParticipantId = 1 and ExamGlobalId = 25), 26752888)

set identity_insert ap_TaskSettings on
insert into ap_TaskSettings(Id, ParentTask, SubjectCode, TaskTypeCode, TaskNumber, MaxValue,  CriteriaName) values (4152, null, 9, 3, 1, 1, null)
insert into ap_TaskSettings(Id, ParentTask, SubjectCode, TaskTypeCode, TaskNumber, MaxValue,  CriteriaName) values (4153, null, 9, 3, 2, 5, null)
insert into ap_TaskSettings(Id, ParentTask, SubjectCode, TaskTypeCode, TaskNumber, MaxValue,  CriteriaName) values (4154, null, 9, 3, null, null ,null)
insert into ap_TaskSettings(Id, ParentTask, SubjectCode, TaskTypeCode, TaskNumber, MaxValue,  CriteriaName) values (4155, 4154, 9, 3, 3, 3, N'Решение коммуникативной задачи (содержание)')
insert into ap_TaskSettings(Id, ParentTask, SubjectCode, TaskTypeCode, TaskNumber, MaxValue,  CriteriaName) values (4156, 4154, 9, 3, 4, 2, N'Организация высказывания')
insert into ap_TaskSettings(Id, ParentTask, SubjectCode, TaskTypeCode, TaskNumber, MaxValue,  CriteriaName) values (4157, 4154, 9, 3, 5, 2, N'Языковое оформление высказывания')
insert into ap_TaskSettings(Id, ParentTask, SubjectCode, TaskTypeCode, TaskNumber, MaxValue,  CriteriaName) values (4158, null, 9, 3, null, null ,null)
insert into ap_TaskSettings(Id, ParentTask, SubjectCode, TaskTypeCode, TaskNumber, MaxValue,  CriteriaName) values (4159, 4158, 9, 3, 6, 3 ,N'Решение коммуникативной задачи (содержание)')
insert into ap_TaskSettings(Id, ParentTask, SubjectCode, TaskTypeCode, TaskNumber, MaxValue,  CriteriaName) values (4160, 4158, 9, 3, 7, 2 ,N'Организация высказывания')
insert into ap_TaskSettings(Id, ParentTask, SubjectCode, TaskTypeCode, TaskNumber, MaxValue,  CriteriaName) values (4161, 4158, 9, 3, 8, 2 ,N'Языковое оформление высказывания')

insert into ap_TaskSettings(Id, ParentTask, SubjectCode, TaskTypeCode, TaskNumber, MaxValue, CriteriaName) values (1581, null, 20, 0, 1, 1, null)
insert into ap_TaskSettings(Id, ParentTask, SubjectCode, TaskTypeCode, TaskNumber, MaxValue, CriteriaName) values (1576, 1581, 20, 0, 1, 1, N'Первый критерий сочинения')
insert into ap_TaskSettings(Id, ParentTask, SubjectCode, TaskTypeCode, TaskNumber, MaxValue, CriteriaName) values (1577, 1581, 20, 0, 2, 1, N'Второй критерий сочинения')
insert into ap_TaskSettings(Id, ParentTask, SubjectCode, TaskTypeCode, TaskNumber, MaxValue, CriteriaName) values (1578, 1581, 20, 0, 3, 1, N'Третий критерий сочинения')
insert into ap_TaskSettings(Id, ParentTask, SubjectCode, TaskTypeCode, TaskNumber, MaxValue, CriteriaName) values (1579, 1581, 20, 0, 4, 1, N'Четвёртый критерий сочинения')
insert into ap_TaskSettings(Id, ParentTask, SubjectCode, TaskTypeCode, TaskNumber, MaxValue, CriteriaName) values (1580, 1581, 20, 0, 5, 1, N'Пятый критерий сочинения')
set identity_insert ap_TaskSettings off

set identity_insert ap_BallSettings on
insert into ap_BallSettings(Id, SubjectCode, TaskTypeCode, TaskNumber, MaxValue, LegalSymbols) values (1571, 22, 1, 1, 1 ,N'кириллица, пробелы, дефисы')
insert into ap_BallSettings(Id, SubjectCode, TaskTypeCode, TaskNumber, MaxValue, LegalSymbols) values (1572, 22, 1, 2, 1 ,N'кириллица, пробелы, дефисы')
insert into ap_BallSettings(Id, SubjectCode, TaskTypeCode, TaskNumber, MaxValue, LegalSymbols) values (1573, 22, 1, 3, 1 ,N'кириллица, пробелы, дефисы')
insert into ap_BallSettings(Id, SubjectCode, TaskTypeCode, TaskNumber, MaxValue, LegalSymbols) values (1574, 22, 1, 4, 1 ,N'кириллица, пробелы, дефисы')
insert into ap_BallSettings(Id, SubjectCode, TaskTypeCode, TaskNumber, MaxValue, LegalSymbols) values (1575, 22, 1, 5, 1 ,N'кириллица, пробелы, дефисы')
set identity_insert ap_BallSettings off

insert into ap_BlankInfo(ParticipantExamId, BlankType, Answer, PrimaryMark, Barcode, PageCount, CreateDate, ProjectName, ProjectBatchId) values (26752892, 0, null, 50, 'fake_barcode', 3, convert(datetime, '2015-03-04', 126), 'essay', 666);
