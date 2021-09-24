/*Заполняем датами поля для документов, чьи оригиналы предоставлены в неопределённое время (к задаче https://redmine.armd.ru/issues/20324)*/
UPDATE dbo.ApplicationEntrantDocument
SET dbo.ApplicationEntrantDocument.OriginalReceivedDate =
 dbo.ApplicationEntrantDocument.CreatedDate
 FROM dbo.ApplicationEntrantDocument
 join Application a on a.[ApplicationID] = dbo.ApplicationEntrantDocument.[ApplicationID]
WHERE a.OriginalDocumentsReceived = 1 and dbo.ApplicationEntrantDocument.OriginalReceivedDate is NULL