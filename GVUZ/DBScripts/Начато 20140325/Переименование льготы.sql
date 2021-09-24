if exists (select * from Benefit where BenefitID = 4 and [Name] = 'Вне конкурса')
begin
	update Benefit
	set 
		[Name] = 'По квоте приёма лиц, имеющих особое право',
		ShortName = 'Квота приёма'
	where BenefitID = 4
end