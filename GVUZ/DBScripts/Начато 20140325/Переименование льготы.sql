if exists (select * from Benefit where BenefitID = 4 and [Name] = '��� ��������')
begin
	update Benefit
	set 
		[Name] = '�� ����� ����� ���, ������� ������ �����',
		ShortName = '����� �����'
	where BenefitID = 4
end