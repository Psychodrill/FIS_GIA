-- Для отладки
-- Declare @CompetitiveGroupID int = 8;


declare @count int;
declare @deletedCG Identifiers;

select @count = count(*) From ApplicationCompetitiveGroupItem (NOLOCK) Where CompetitiveGroupId = @CompetitiveGroupID;
IF (@count > 0)
	Select 1 as Code, 'Удаление невозможно, имеются заявления по данному конкурсу!' as Message;
ELSE
	BEGIN 
		delete from CompetitiveGroup 
		output deleted.CompetitiveGroupID into @deletedCG
		Where CompetitiveGroupID = @CompetitiveGroupID;

		select @count = count(*) From @deletedCG;
		if (@count = 1)
			Select 0 as Code, 'Конкурс успешно удален.' as Message;
		else
			Select -1 as Code, 'Конкурс не найден, удаление невозможно!' as Message; 
	END;