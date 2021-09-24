/*
Отсутствуют сведения об индексе из SQLQuery3.sql - vm-cpdb.fbs (sa (70))
Обработчик запросов считает, что реализация следующего индекса может сократить стоимость запроса на 55.1255%.
*/


USE [fbs]
GO
CREATE NONCLUSTERED INDEX [IX_CommonNationalExamCertificateCheck_LastName_OtherFields2]
ON [dbo].[CommonNationalExamCertificateCheck] ([LastName])
INCLUDE ([Id],[BatchId],[CertificateNumber])
GO

