--declare @CompetitiveGroupTargetID int = 0;
--declare @CreatedDate datetime = null;
--declare @ModifiedDate datetime = null;
--declare @UID varchar(200) = '777';
--declare @Name varchar(200) = '777';
--declare @InstitutionID int = 587;

declare @CompetitiveGroupTargetIDs identifiers;

MERGE CompetitiveGroupTarget AS TARGET
USING (SELECT @CompetitiveGroupTargetID as CompetitiveGroupTargetID) AS SOURCE
ON TARGET.CompetitiveGroupTargetID = SOURCE.CompetitiveGroupTargetID
WHEN MATCHED THEN
	UPDATE 
	SET 
		
		UID = @UID,
		ModifiedDate = GETDATE(),
		InstitutionID = @InstitutionID,
		ContractOrganizationName = @ContractOrganizationName,
		HaveContract = @HaveContract,
		ContractDate = @ContractDate,
		ContractNumber = @ContractNumber,
		ContractOrganizationOGRN = @ContractOrganizationOGRN,
		ContractOrganizationKPP = @ContractOrganizationKPP,
		EmployerOrganizationName = @EmployerOrganizationName,
		EmployerOrganizationOGRN = @EmployerOrganizationOGRN,
		EmployerOrganizationKPP = @EmployerOrganizationKPP,
		LocationEmployerOrganizations =@LocationEmployerOrganizations
WHEN NOT MATCHED BY TARGET THEN 
	INSERT (
		
		[UID],
		[CreatedDate],
		[InstitutionID],
		[ContractOrganizationName],
		[HaveContract],
		[ContractDate],
		[ContractNumber],
		[ContractOrganizationOGRN],
		[ContractOrganizationKPP],
		[EmployerOrganizationName],
		[EmployerOrganizationOGRN],
		[EmployerOrganizationKPP],
		[LocationEmployerOrganizations]
	)
	VALUES(
		@UID,
		GETDATE(),
		@InstitutionID,
		@ContractOrganizationName,
		@HaveContract,
		@ContractDate,
		@ContractNumber,
		@ContractOrganizationOGRN,
		@ContractOrganizationKPP,
		@EmployerOrganizationName,
		@EmployerOrganizationOGRN,
		@EmployerOrganizationKPP,
		@LocationEmployerOrganizations
	)
	OUTPUT INSERTED.CompetitiveGroupTargetID INTO @CompetitiveGroupTargetIDs;

SELECT cgt.*, 
case when cgti.ID is null then 1 else 0 end  as CanRemove 
FROM CompetitiveGroupTarget AS cgt  (NOLOCK)
outer apply 
(select top(1) cgti.CompetitiveGroupTargetItemID as ID
	From CompetitiveGroupTargetItem cgti  (NOLOCK)
	Where cgti.CompetitiveGroupTargetID = cgt.CompetitiveGroupTargetID) as cgti
WHERE cgt.CompetitiveGroupTargetID = (select ID From @CompetitiveGroupTargetIDs);