IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Direction_ParentDirection]') AND parent_object_id = OBJECT_ID(N'[dbo].[Direction]'))
ALTER TABLE [dbo].[Direction] DROP CONSTRAINT [FK_Direction_ParentDirection]
GO

set identity_insert ParentDirection on

delete from ParentDirection

insert into ParentDirection
(ParentDirectionID, Code, Name)
select 56,	'00.00.00', 	'������ ���'
union
select 1,	'01.00.00',  	'���������� � ��������'
union
select 2,	'02.00.00',  	'������������ � �������������� �����'
union
select 3,	'03.00.00',  	'������ � ����������'
union
select 4,	'04.00.00',  	'�����'
union
select 5,	'05.00.00',  	'����� � �����'
union
select 6,	'06.00.00',  	'������������� �����'
union
select 7,	'07.00.00',  	'�����������'
union
select 8,	'08.00.00',  	'������� � ���������� �������������'
union
select 9,	'09.00.00',  	'����������� � �������������� �������'
union
select 10,	'10.00.00',  	'�������������� ������������'
union
select 11,	'11.00.00',  	'�����������, ������������ � ������� �����'
union
select 12,	'12.00.00',  	'��������, ���������������, ���������� � �������������� ������� � ����������'
union
select 13,	'13.00.00',  	'�������- � ���������������'
union
select 14,	'14.00.00',  	'������� ���������� � ����������'
union
select 15,	'15.00.00',  	'��������������'
union
select 16,	'16.00.00',  	'������-����������� ����� � ����������'
union
select 17,	'17.00.00',  	'������ � ������� ����������'
union
select 18,	'18.00.00',  	'���������� ����������'
union
select 19,	'19.00.00',  	'������������ �������� � �������������'
union
select 20,	'20.00.00',  	'������������ ������������ � �������������������'
union
select 21,	'21.00.00',  	'���������� ��������, ������ ����, ������������ ���� � ��������'
union
select 22,	'22.00.00',  	'���������� ����������'
union
select 23,	'23.00.00',  	'������� � ���������� ��������� ����������'
union
select 24,	'24.00.00',  	'����������� � �������-����������� �������'
union
select 25,	'25.00.00',  	'������������� � ������������ ����������� � �������-����������� �������'
union
select 26,	'26.00.00',  	'������� � ���������� ��������������� � ������� ����������'
union
select 27,	'27.00.00',  	'���������� � ����������� ��������'
union
select 28,	'28.00.00',  	'�������������� � �������������'
union
select 29,	'29.00.00',  	'���������� ������ ��������������'
union
select 30,	'30.00.00',  	'��������������� ��������'
union
select 31,	'31.00.00',  	'����������� ��������'
union
select 32,	'32.00.00',  	'����� � �������� � ���������������� ��������'
union
select 33,	'33.00.00',  	'��������'
union
select 34,	'34.00.00',  	'����������� ����'
union
select 35,	'35.00.00',  	'��������, ������ � ������ ���������'
union
select 36,	'36.00.00',  	'����������� � ���������'
union
select 37,	'37.00.00',  	'��������������� �����'
union
select 38,	'38.00.00',  	'��������� � ����������'
union
select 39,	'39.00.00',  	'���������� � ���������� ������'
union
select 40,	'40.00.00',  	'�������������'
union
select 41,	'41.00.00',  	'������������ ����� � ��������������'
union
select 42,	'42.00.00',  	'�������� �������� ���������� � �������������-������������ ����'
union
select 43,	'43.00.00',  	'������ � ������'
union
select 44,	'44.00.00',  	'����������� � �������������� �����'
union
select 45,	'45.00.00',  	'����������� � �����������������'
union
select 46,	'46.00.00',  	'������� � ����������'
union
select 47,	'47.00.00',  	'���������, ����� � ��������������'
union
select 48,	'48.00.00',  	'��������'
union
select 49,	'49.00.00',  	'���������� �������� � �����'
union
select 50,	'50.00.00',  	'���������������'
union
select 51,	'51.00.00',  	'��������������� � ��������������� �������'
union
select 52,	'52.00.00',  	'����������� ��������� � ������������ ����������'
union
select 53,	'53.00.00',  	'����������� ���������'
union
select 54,	'54.00.00',  	'��������������� � ���������� ���� ��������'
union
select 55,	'55.00.00',  	'�������� ���������'

set Identity_insert ParentDirection on

DBCC CheckIdent(ParentDirection, reseed, 57) 

ALTER TABLE [dbo].[Direction]  WITH CHECK ADD  CONSTRAINT [FK_Direction_ParentDirection] FOREIGN KEY([ParentID])
REFERENCES [dbo].[ParentDirection] ([ParentDirectionID])
GO

ALTER TABLE [dbo].[Direction] CHECK CONSTRAINT [FK_Direction_ParentDirection]
GO

