-- =========================================================================
-- Запись информации о текущей миграции в лог
insert into Migrations(MigrationVersion, MigrationName) values (44, '044_2012_06_08_GetNEWebUICheckLog')
-- =========================================================================

/****** Object:  StoredProcedure [dbo].[GetNEWebUICheckLog]    Script Date: 06/08/2012 12:20:18 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetNEWebUICheckLog]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GetNEWebUICheckLog]
GO

/****** Object:  StoredProcedure [dbo].[GetNEWebUICheckLog]    Script Date: 06/08/2012 12:20:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE proc [dbo].[GetNEWebUICheckLog]
	@login nvarchar(255), @startRowIndex int = 1,@maxRowCount int = null, @showCount bit = null,   -- если > 0, то выбирается общее кол-во
	@TypeCode nvarchar(255) -- Тип проверки
as
begin
	declare @accountId bigint, @endRowIndex integer

	if isnull(@maxRowCount, -1) = -1 
		set @endRowIndex = 10000000
	else
		set @endRowIndex = @startRowIndex + @maxRowCount

	if exists ( select 1 from [Account] as a2
				join [GroupAccount] ga on ga.[AccountId] = a2.[Id]
				join [Group] as g on ga.[GroupId] = g.[Id] and g.[Code] = 'Administrator'
				where a2.[Login] = @login )
		set @accountId = null
	else 
		set @accountId = isnull(
			(select account.[Id] 
			from dbo.Account account with (nolock, fastfirstrow) 
			where account.[Login] = @login), 0)

	if isnull(@showCount, 0) = 0
	begin	
		if @accountId is null 
				select *
				from 
				(
					select b.Id,b.CNENumber, b.LastName, b.FirstName, b.PatronymicName,b.Marks,b.TypographicNumber,b.PassportSeria,b.PassportNumber,
						2000+cast(substring(b.CNENumber,len(b.CNENumber)-1,2) as int) YearCertificate, case when FoundedCNEId is null then 0 else 1 end CheckCertificate,
						c.[login] [login],EventDate,		
						row_number() over (order by b.EventDate desc) rn 
					from
					(
						select top (@endRowIndex) b.id 
						from dbo.CNEWebUICheckLog b with (nolock) 
							join Account c on b.AccountId=c.id
							join Organization2010 d on d.id=c.OrganizationId
						where @TypeCode=TypeCode and d.DisableLog=0												
						order by b.EventDate desc					
					) a join CNEWebUICheckLog b on a.id=b.id
						join Account c on b.AccountId=c.id								
				) s 
				where s.rn between @startRowIndex and @endRowIndex 
			else
				select *
				from 
				(
					select top (@endRowIndex) b.Id,b.CNENumber, b.LastName, b.FirstName, b.PatronymicName,b.Marks,b.TypographicNumber,b.PassportSeria,b.PassportNumber,
						2000+cast(substring(b.CNENumber,len(b.CNENumber)-1,2) as int) YearCertificate, case when FoundedCNEId is null then 0 else 1 end CheckCertificate,
						c.[login] [login],EventDate,		
						row_number() over (order by b.EventDate desc) rn 
					from
					(
						select top (@endRowIndex) b.id 
						from dbo.CNEWebUICheckLog b with (nolock) 
							join Account c on b.AccountId=c.id
							join Organization2010 d on d.id=c.OrganizationId						
						where b.AccountId = @accountId and @TypeCode=TypeCode and d.DisableLog=0						
						order by b.EventDate desc						
					) a join CNEWebUICheckLog b on a.id=b.id	
						join Account c on b.AccountId=c.id
				) s 
				where s.rn between @startRowIndex and @endRowIndex					
	end
	else
		if @accountId is null 
			select count(*) 
			from dbo.CNEWebUICheckLog b with (nolock) 
				join Account c on b.AccountId=c.id
				join Organization2010 d on d.id=c.OrganizationId	
			where @TypeCode=TypeCode and d.DisableLog=0	
		else
			select count(*) 
			from dbo.CNEWebUICheckLog b with (nolock) 
				join Account c on b.AccountId=c.id
				join Organization2010 d on d.id=c.OrganizationId	
			where b.AccountId = @accountId and @TypeCode=TypeCode and d.DisableLog=0	

	return 0
end

IF @@ERROR<>0 OR @@TRANCOUNT=0 BEGIN IF @@TRANCOUNT>0 ROLLBACK SET NOEXEC ON END

GO


