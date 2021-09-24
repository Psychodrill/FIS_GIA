insert into Migrations(MigrationVersion, MigrationName) values (91, '091_2013_06_14_fbs.sql')
go

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procEDURE [dbo].[ExecuteChecksCount]
    @OrganizationId bigint,
    @CertificateId bigint = null,
    @CertificateIdGuid uniqueidentifier =null
AS
BEGIN
  if ((@CertificateId is null and @CertificateIdGuid is null) or @OrganizationId is null or @OrganizationId=0)
    return -1

  -- Выполняла ли данная организация проверку данного сертификата
    declare @isExists bit

  -- Значения инкрементов
  declare @uniqueIHEaFCheck int
  declare @uniqueIHECheck int
  declare @uniqueIHEFCheck int 

  declare @uniqueTSSaFCheck int
  declare @uniqueTSSCheck int
  declare @uniqueTSSFCheck int
    
  declare @uniqueRCOICheck int
  declare @uniqueOUOCheck int
  declare @uniqueFounderCheck int
  declare @uniqueOtherCheck int 
    
    -- Тип организации
    declare @orgType int
    
    -- Является ли организация филлиалом
    declare @isFilial bit

  -- Инициализация переменных
    set @isExists = 0
  set @uniqueIHEaFCheck = 0
  set @uniqueIHECheck = 0
  set @uniqueIHEFCheck = 0
  set @uniqueTSSaFCheck = 0
  set @uniqueTSSCheck = 0
  set @uniqueTSSFCheck = 0
  set @uniqueRCOICheck = 0
  set @uniqueOUOCheck = 0
  set @uniqueFounderCheck = 0
  set @uniqueOtherCheck = 0
    set @orgType = 0
    set @isFilial = 0

  if @CertificateIdGuid is null 
    set @isExists = 
        (
      select count(*) 
      from [dbo].[OrganizationCertificateChecks] OCC 
      where OCC.CertificateId = @CertificateId and OCC.OrganizationId = @OrganizationId
      )
  else  
    set @isExists = 
        (
      select count(*) 
      from [dbo].[OrganizationCertificateChecks] OCC 
      where OCC.CertificateIdGuid = @CertificateIdGuid and OCC.OrganizationId = @OrganizationId
      )

    if (@isExists = 0)
    begin
      insert into [dbo].[OrganizationCertificateChecks] (CertificateId, OrganizationId, CertificateIdGuid)
        values (@CertificateId, @OrganizationId, @CertificateIdGuid)
        
        select
          @orgType = isnull(O.TypeId, 0),
            @isFilial = (case when (O.MainId is null) then 0 else 1 end)
        from
          [dbo].[Organization2010] O
        where
          O.Id = @OrganizationId

        if (@orgType = 0)
          return -1

    if (@orgType = 1)
        begin
          set @uniqueIHEaFCheck = 1
            if (@isFilial = 1)
              set @uniqueIHEFCheck = 1
            else
              set @uniqueIHECheck = 1
        end           
        
    if (@orgType = 2)
        begin
          set @uniqueTSSaFCheck = 1
            if (@isFilial = 1)
              set @uniqueTSSFCheck = 1
            else
              set @uniqueTSSCheck = 1
        end           
        
    if (@orgType = 3)
        begin
          set @uniqueRCOICheck = 1
        end           

    if (@orgType = 4)
        begin
          set @uniqueOUOCheck = 1
        end           

    if (@orgType = 6)
        begin
          set @uniqueFounderCheck = 1
        end           

    if (@orgType = 5)
        begin
          set @uniqueOtherCheck = 1
        end
    
        declare @year int
        if @CertificateId is null
      set @year = 
        (select top 1 C.UseYear
        from [prn].[Certificates] C
        where C.CertificateID = @CertificateIdGuid)
        else
      set @year = 
        (select top 1 C.[Year]
        from CommonNationalExamCertificate C
        where C.Id = @CertificateId)
        
    if not exists 
      (select *
      from [dbo].[ExamCertificateUniqueChecks] EC
      where EC.Id = @CertificateId) 
      and
      not exists (select *
      from [dbo].[ExamCertificateUniqueChecks] EC
      where EC.idGUID = @CertificateIdGuid) 
    begin
      insert into [dbo].[ExamCertificateUniqueChecks] 
        (
        [Year], 
        Id, 
        UniqueChecks,
        UniqueIHEaFCheck,
        UniqueIHECheck,
        UniqueIHEFCheck,
        UniqueTSSaFCheck,
        UniqueTSSCheck,
        UniqueTSSFCheck,
        UniqueRCOICheck,
        UniqueOUOCheck,
        UniqueFounderCheck,
        UniqueOtherCheck,
        idGUID
        )
      values 
        (
        @year, 
        @CertificateId,
        1,
        @uniqueIHEaFCheck,
        @uniqueIHECheck,
        @uniqueIHEFCheck,
        @uniqueTSSaFCheck,
        @uniqueTSSCheck,
        @uniqueTSSFCheck,
        @uniqueRCOICheck,
        @uniqueOUOCheck,
        @uniqueFounderCheck,
        @uniqueOtherCheck,
        @CertificateIdGuid
        )
    end
    else begin
        if @CertificateId is not null  
        update 
              [dbo].[ExamCertificateUniqueChecks]
        set 
          UniqueChecks = UniqueChecks + 1,
          UniqueIHEaFCheck = UniqueIHEaFCheck + @uniqueIHEaFCheck,
          UniqueIHECheck = UniqueIHECheck + @uniqueIHECheck,
          UniqueIHEFCheck = UniqueIHEFCheck + @uniqueIHEFCheck,
          UniqueTSSaFCheck = UniqueTSSaFCheck + @uniqueTSSaFCheck,
          UniqueTSSCheck = UniqueTSSCheck + @uniqueTSSCheck,
          UniqueTSSFCheck = UniqueTSSFCheck + @uniqueTSSFCheck,
          UniqueRCOICheck = UniqueRCOICheck + @uniqueRCOICheck,
          UniqueOUOCheck = UniqueOUOCheck + @uniqueOUOCheck,
          UniqueFounderCheck = UniqueFounderCheck + @uniqueFounderCheck,
          UniqueOtherCheck = UniqueOtherCheck + @uniqueOtherCheck
        where
              id = @CertificateId
              and [Year] = @year
          else
        update 
              [dbo].[ExamCertificateUniqueChecks]
        set 
          UniqueChecks = UniqueChecks + 1,
          UniqueIHEaFCheck = UniqueIHEaFCheck + @uniqueIHEaFCheck,
          UniqueIHECheck = UniqueIHECheck + @uniqueIHECheck,
          UniqueIHEFCheck = UniqueIHEFCheck + @uniqueIHEFCheck,
          UniqueTSSaFCheck = UniqueTSSaFCheck + @uniqueTSSaFCheck,
          UniqueTSSCheck = UniqueTSSCheck + @uniqueTSSCheck,
          UniqueTSSFCheck = UniqueTSSFCheck + @uniqueTSSFCheck,
          UniqueRCOICheck = UniqueRCOICheck + @uniqueRCOICheck,
          UniqueOUOCheck = UniqueOUOCheck + @uniqueOUOCheck,
          UniqueFounderCheck = UniqueFounderCheck + @uniqueFounderCheck,
          UniqueOtherCheck = UniqueOtherCheck + @uniqueOtherCheck
        where
              idGUID = @CertificateIdGuid
              and [Year] = @year          
        end

        return 1
    end
    else begin
      return 0
    end
END
