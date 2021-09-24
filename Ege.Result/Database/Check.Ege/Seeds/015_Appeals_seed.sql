--insert into ap_Appeals (ParticipantExamId ,AppealType, Station, CreateDate, ReviewType, Phone, Mail, AgentType) values (1, 0, null, '2015-03-02', null, null, null, null)
--insert into ap_Appeals (ParticipantExamId ,AppealType, Station, CreateDate, ReviewType, Phone, Mail, AgentType) values (2, 0, null, '2015-03-05', null, null, null, null)
--insert into ap_Appeals (ParticipantExamId ,AppealType, Station, CreateDate, ReviewType, Phone, Mail, AgentType) values (3, 0, null, '2015-03-12', null, null, null, null)

--insert into ap_Appeals (ParticipantExamId ,AppealType, Station, CreateDate, ReviewType, Phone, Mail, AgentType) values (6, 0, null, '2015-03-03', null, null, null, null)
--insert into ap_Appeals (ParticipantExamId ,AppealType, Station, CreateDate, ReviewType, Phone, Mail, AgentType) values (7, 0, null, '2015-03-04', null, null, null, null)
--insert into ap_Appeals (ParticipantExamId ,AppealType, Station, CreateDate, ReviewType, Phone, Mail, AgentType) values (8, 0, null, '2015-03-11', null, null, null, null)

--insert into ap_AppealsHistory(AppealId, AppealStatus, CreateDate, SignDocument, AgentType) values (1, 0, '2015-03-02', 1, null)
--insert into ap_AppealsHistory(AppealId, AppealStatus, CreateDate, SignDocument, AgentType) values (1, 2, '2015-03-03', 1, null)
--
--insert into ap_AppealsHistory(AppealId, AppealStatus, CreateDate, SignDocument, AgentType) values (2, 1, '2015-03-05', 1, null)
--insert into ap_AppealsHistory(AppealId, AppealStatus, CreateDate, SignDocument, AgentType) values (2, 3, '2015-03-06', 1, null)

--insert into ap_AppealsHistory(AppealId, AppealStatus, CreateDate, SignDocument, AgentType) values (3, 1, '2015-03-12', 1, null)
--insert into ap_AppealsHistory(AppealId, AppealStatus, CreateDate, SignDocument, AgentType) values (3, 2, '2015-03-13', 1, null)

--insert into ap_AppealsHistory(AppealId, AppealStatus, CreateDate, SignDocument, AgentType) values (4, 3, '2015-03-03', 1, null)
--insert into ap_AppealsHistory(AppealId, AppealStatus, CreateDate, SignDocument, AgentType) values (4, 4, '2015-03-04', 1, null)

--insert into ap_AppealsHistory(AppealId, AppealStatus, CreateDate, SignDocument, AgentType) values (5, 1, '2015-03-04', 1, null)
--insert into ap_AppealsHistory(AppealId, AppealStatus, CreateDate, SignDocument, AgentType) values (5, 2, '2015-03-05', 1, null)

--insert into ap_AppealsHistory(AppealId, AppealStatus, CreateDate, SignDocument, AgentType) values (6, 1, '2015-03-11', 1, null)
--insert into ap_AppealsHistory(AppealId, AppealStatus, CreateDate, SignDocument, AgentType) values (6, 2, '2015-03-13', 1, null)


SET IDENTITY_INSERT [dbo].[ap_Appeals] ON 
INSERT [dbo].[ap_Appeals] ([Id], [ParticipantExamId], [Status], [CreateDate]) VALUES (1, 1, 0, CAST(0x0000A44F00000000 AS DateTime))
INSERT [dbo].[ap_Appeals] ([Id], [ParticipantExamId], [Status], [CreateDate]) VALUES (2, 1, 2, CAST(0x0000A45000000000 AS DateTime))
INSERT [dbo].[ap_Appeals] ([Id], [ParticipantExamId], [Status], [CreateDate]) VALUES (3, 2, 1, CAST(0x0000A45200000000 AS DateTime))
INSERT [dbo].[ap_Appeals] ([Id], [ParticipantExamId], [Status], [CreateDate]) VALUES (4, 2, 3, CAST(0x0000A45300000000 AS DateTime))
INSERT [dbo].[ap_Appeals] ([Id], [ParticipantExamId], [Status], [CreateDate]) VALUES (5, 3, 1, CAST(0x0000A45900000000 AS DateTime))
INSERT [dbo].[ap_Appeals] ([Id], [ParticipantExamId], [Status], [CreateDate]) VALUES (6, 3, 2, CAST(0x0000A45A00000000 AS DateTime))
INSERT [dbo].[ap_Appeals] ([Id], [ParticipantExamId], [Status], [CreateDate]) VALUES (7, 6, 3, CAST(0x0000A45000000000 AS DateTime))
INSERT [dbo].[ap_Appeals] ([Id], [ParticipantExamId], [Status], [CreateDate]) VALUES (8, 6, 4, CAST(0x0000A45100000000 AS DateTime))
INSERT [dbo].[ap_Appeals] ([Id], [ParticipantExamId], [Status], [CreateDate]) VALUES (9, 7, 1, CAST(0x0000A45100000000 AS DateTime))
INSERT [dbo].[ap_Appeals] ([Id], [ParticipantExamId], [Status], [CreateDate]) VALUES (10, 7, 2, CAST(0x0000A45200000000 AS DateTime))
INSERT [dbo].[ap_Appeals] ([Id], [ParticipantExamId], [Status], [CreateDate]) VALUES (11, 8, 1, CAST(0x0000A45800000000 AS DateTime))
INSERT [dbo].[ap_Appeals] ([Id], [ParticipantExamId], [Status], [CreateDate]) VALUES (12, 8, 2, CAST(0x0000A45A00000000 AS DateTime))
SET IDENTITY_INSERT [dbo].[ap_Appeals] OFF