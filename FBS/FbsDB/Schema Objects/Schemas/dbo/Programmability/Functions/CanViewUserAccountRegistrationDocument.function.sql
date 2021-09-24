--------------------------------------------------
-- Можно ли просматривать документ регистрации.
-- v1.0: Created by Fomin Dmitriy 07.04.2008
--------------------------------------------------
CREATE function dbo.CanViewUserAccountRegistrationDocument
	(
	@confirmYear int
	)
returns bit
as
begin
	return case
			when @confirmYear = year(getdate()) then 1
			else 0
		end
end
