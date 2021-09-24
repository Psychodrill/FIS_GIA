/*
Отсутствуют сведения об индексе из SQLQuery3.sql - vm-cpdb.fbs (sa (70))
Обработчик запросов считает, что реализация следующего индекса может сократить стоимость запроса на 44.5233%.
*/

CREATE NONCLUSTERED INDEX [IX_CommonNationalExamCertificateRequest_LastName_OtherFields]
ON [dbo].[CommonNationalExamCertificateRequest] ([LastName])
INCLUDE ([Id],[BatchId],[SourceCertificateId])
GO

