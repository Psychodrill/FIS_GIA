
--------------------------------------------------
-- Разбивает исходную строку на части, разделенные запятыми.
-- v.1.0: Created by Makarev Andrey 04.04.2008
-- v.1.1: Modified by Makarev Andrey 16.04.2008
-- Измение размера выходного массива.
-- v.1.2: Rewritten by Valeev Denis 20.05.2009
-- Переписал в рамках оптимизации через xml
--------------------------------------------------
CREATE function [dbo].[GetDelimitedValues]
	(
	@ids nvarchar(4000)
	)
returns @Values table ([value] nvarchar(4000))
as
begin
	if len(ltrim(rtrim(@ids))) > 0
	begin
		DECLARE @x xml
		set @x = '<root><v>' + replace(@ids, ',', '</v><v>') + '</v></root>'
		insert into @Values
		SELECT  T.c.value('.','nvarchar(4000)')
		FROM    @x.nodes('/root/v') T ( c )
	end
	return	
end


