USE [GVUZ]
GO

/****** Object: SqlProcedure [dbo].[FindEntrantOlympic] Script Date: 13.04.2016 22:02:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE procedure FindEntrantOlympic
	@docId int,	@olympicId int, @olympicTypeProfileId int,
	@errorMessage nvarchar(4000) = null output, @violationMessage nvarchar(4000) = null output, @violationId int = 0 output
as begin
	set nocount on;

	declare @olympicNumber int 
	declare @vosh bit = 0
	declare @entrantId int 
	declare @documentSeries varchar(20)
	declare @documentNumber varchar(100)
	declare @cnt int 

	-- определение по номеру типа олимпиады ОШ или ВОШ
	select @olympicNumber = OlympicNumber from OlympicType where OlympicID = @olympicId
	if (@olympicNumber is null)	set @vosh = 1 -- значит ВОШ

	-- выбираем идентификатор абитуриента, номер и серию по документу
	select @entrantId = EntrantID, @documentSeries = DocumentSeries, @documentNumber = DocumentNumber 
	from EntrantDocument (NOLOCK) where EntrantDocumentID = @docId

	-- ищем связь абитуриента с персоной
	declare @entrantPersonId int
	select @entrantPersonId = PersonId from Entrant (NOLOCK) where EntrantID = @entrantId

	-- связи нет, строим ее процедурой SyncEntrant
	if (@entrantPersonId is null)
	begin
		execute dbo.SyncEntrant @entrantId
		select @entrantPersonId = PersonId from Entrant (NOLOCK) where EntrantID = @entrantId		
	end
	
	-- связи все равно нет, уходим
	if (@entrantPersonId is null)
	begin
		set @errorMessage = N'Не удалось определить идентификатор физ. лица (Entrant.PersonId)'
		set @violationId = 11
		set @violationMessage = 'Техническая ошибка при проверке результатов олимпиад'

		update Application 
		set ViolationID = @violationId where ApplicationID = 
			(select top 1 ApplicationID from ApplicationEntrantDocument (NOLOCK) where EntrantDocumentID = @docId)
		
		return 0 	
	end	

	-- ищем в таблице OlympicDiplomant все дипломы, связанные с текущим абитуриентом
	select @cnt = count(distinct OlympicDiplomantID) from OlympicDiplomant 
	where PersonId = @entrantPersonId and OlympicTypeProfileID = @olympicTypeProfileId and DiplomaNumber = @documentNumber

	-- записи о дипломах отсутствуют, уходим, код 8
	if (@cnt = 0)
	begin  
		set @violationMessage = N'Для диплома победителя/призера № ' + isnull(@documentSeries,'') + ' ' + 
			isnull(@documentNumber,'') + ' не было найдено подтверждение результатов олимпиады'
		set @violationID = 8
		
		update EntrantDocument set OlympApproved = 0 where EntrantDocumentID = @docId
				
		update Application 
		set ViolationID = @violationId where ApplicationID = 
			(select top 1 ApplicationID from ApplicationEntrantDocument (NOLOCK) where EntrantDocumentID = @docId)
				
		return 0 	
	end

	-- записи о дипломах есть, уходим, все нормально
	update EntrantDocument set OlympApproved = 1 where EntrantDocumentID = @docId

	return 0 	
end
