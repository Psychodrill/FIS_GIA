-- exec dbo.GetAskedQuestion

-- =============================================
-- Получение вопроса.
-- Если @isViewCount = 1, то ViewCount увеличить на 1 для показываемой записи.
-- v.1.0: Created by Makarev Andrey 22.04.2008
-- =============================================
CREATE proc dbo.GetAskedQuestion
	@id bigint
	, @isViewCount bit = 1
as
begin
	declare 
		@internalId bigint
	
	set @internalId = dbo.GetInternalId(@id)

	select
		@id [Id]
		, asked_question.Name [Name]
		, asked_question.Question Question
		, asked_question.Answer Answer
		, asked_question.IsActive IsActive
		, asked_question.ContextCodes ContextCodes
		, asked_question.Popularity Popularity
		, asked_question.ViewCount ViewCount
	from 
		dbo.AskedQuestion asked_question with (nolock, fastfirstrow)
	where
		asked_question.[Id] = @internalId

	if @isViewCount = 1
		update asked_question
		set 
			ViewCount = ViewCount + 1
		from 
			dbo.AskedQuestion asked_question with (rowlock)
		where
			asked_question.[Id] = @internalId

	return 0
end
