UPDATE [dbo].[Application]
SET [ApplicationNumber] = LTRIM(RTRIM([ApplicationNumber]))
WHERE ([ApplicationNumber] like ' %') or ([ApplicationNumber] like '% ') or ([ApplicationNumber] like ' % ')