-- =============================================
-- author:	Чувашов И.Н.
-- create date: 19.07.2012
-- description: общественная проверка корректности приказов о зачислении
-- param:
--		@EventId - номер операции
-- output:
--		-
-- =============================================

/*
*/

/*
select * from CheckByMarkSumlog
*/

create proc [dbo].[CheckCNEByMarkSum] 
	@EventId int
as
set nocount on
begin try
	begin tran	
		declare @table table (pkid int primary key identity(1,1), idgroup int, mark int,SubjectId bigint,CertificateId bigint  )
		declare @ListSubject nvarchar(1000),@SumMark int,@cod int
		select @ListSubject=l.Subjects,@SumMark=MarkSum from CheckByMarkSumlog l where @EventId=l.id

		insert @table	  
		select 1,cast(a.mark as int),a.SubjectId,CertificateId
		from CommonNationalExamCertificateSubject a
			join CommonNationalExamCertificate b on a.CertificateId=b.id
			join CheckByMarkSumlog l on l.FirstName=b.FirstName and l.LastName=b.LastName and 
										case when isnull(l.GivenName,'')='' then '2jmj7l5rSw0yVb/vlWAYkK/YBwk' else l.GivenName end=b.PatronymicName
			join ufn_ut_SplitFromString(@ListSubject, ',') c on c.nam=a.SubjectId
		where @EventId=l.id

		if not exists(select * from @table)
		begin
			set @cod=4
			goto metka
		end

		declare @group int,@cnt int,@i int,@k int,@list nvarchar(99), @summa int, @flag bit

		set @cnt=(select count(distinct SubjectId) from @table)
		if @cnt<(select count(nam) from ufn_ut_SplitFromString(@ListSubject, ','))
		begin
			set @cod=3
			goto metka
		end
		  
		update a set idgroup=rn from (
			select b.*,a.rn from 
				(select *,row_number() over (order by a.CertificateId asc) rn from 
					(select distinct CertificateId from @table) a
				) a
				join @table b on a.CertificateId=b.CertificateId
			) a

		select @i=0,@group =max(idgroup),@flag=0 from @table

		declare @table_r table (pkid int primary key identity(1,1), idgroup int, mark int,SubjectId bigint, idbit int)
		declare @tbl table(pkid int primary key,SubjectId int, idgroup int)	
		declare @s int
		
		if @group=1 
			set @s=1
		else
			set @s=power(2,@cnt-1)*@group	
		
		while (@i<@s) and (@flag=0)
		begin								
			insert @tbl
			select id,nam,1 from ufn_ut_SplitFromString(@ListSubject, ',')		
		
			select @list = dbo.ConvSyst(@i,@group)
		
			update a set idgroup=cast(b.nam as int)+1 from
				@tbl a join ufn_ut_SplitFromString(REVERSE(@list), ',') b on a.pkid=b.id			
	
			insert @table_r
			select @i, (select mark from @table c where c.idgroup=b.idgroup and c.SubjectId=a.SubjectId),a.SubjectId, b.idgroup
			from @table a join @tbl b on a.SubjectId=b.SubjectId 
			where 1= case when not exists(select * from @table cc where cc.SubjectId=a.SubjectId and idgroup=1) then 1 
						  when a.idgroup=1 then 1
						  else 0 end

			select @summa=sum(mark) from @table_r where idgroup=@i
	
			if @summa=@SumMark
				set @flag=1		
			
			delete @tbl
			set @i=@i+1
		end			

		if @flag = 0 
		begin
			set @cod=2
			goto metka
		end
		else
			set @cod=1
		metka:		
		select @cod
		update CheckByMarkSumlog set Result=@cod where @EventId=Id
		
	if @@trancount > 0
		commit tran

end try
begin catch
	if @@trancount > 0
		rollback tran 
	declare @er nvarchar(4000)
	set @er=error_message()
	raiserror(@er,16,1) 
	return -1
end catch

