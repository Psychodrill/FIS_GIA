
--------------------------------------------------
-- Разбивает исходную строку на части, разделенные 
-- запятыми и знаками =.
-- v.1.0: Created by Makarev Andrey 23.05.2008
-- v.1.1: Modified by Fomin Dmitriy 27.05.2008
-- Приведение к стандарту.
-- v.1.2: Rewritten by Valeev Denis 20.05.2009
-- Переписал через xml для оптимизации
-- v.1.3: Rewritten by Yusupov Kirill 1.07.2010
-- Переписал через цикл для оптимизации
--------------------------------------------------
CREATE function [dbo].[GetSubjectMarks]
	(
	@subjectMarks nvarchar(4000)
	)
returns @SubjectMark table (SubjectId int, Mark numeric(5,1))
--returns @SubjectMark table (SubjectId NVARCHAR(20), Mark NVARCHAR(20))
as
begin
	DECLARE @RawMark NVARCHAR(20)
	DECLARE @EQIndex INT
	WHILE (CHARINDEX(',',@subjectMarks)>0)
	BEGIN
		SET @RawMark= SUBSTRING(@subjectMarks,1,CHARINDEX(',',@subjectMarks)-1)

		SET @EQIndex=CHARINDEX('=',@RawMark)

		INSERT INTO @SubjectMark (SubjectId,Mark)
		SELECT 
			SUBSTRING(@RawMark,1,@EQIndex-1),SUBSTRING(@RawMark,@EQIndex+1,LEN(@RawMark)-@EQIndex+1)

		SET @subjectMarks = SUBSTRING(@subjectMarks,CHARINDEX(',',@subjectMarks)+1,LEN(@subjectMarks))
	END
	IF (LEN(@subjectMarks)>0)
	BEGIN
		SET @RawMark= @subjectMarks

		SET @EQIndex=CHARINDEX('=',@RawMark)

		INSERT INTO @SubjectMark (SubjectId,Mark)
		SELECT 
			SUBSTRING(@RawMark,1,@EQIndex-1),SUBSTRING(@RawMark,@EQIndex+1,LEN(@RawMark)-@EQIndex+1)
	END
	RETURN
end
