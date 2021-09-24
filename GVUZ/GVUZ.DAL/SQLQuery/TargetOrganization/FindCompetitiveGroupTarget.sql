--DECLARE
--	@CompetitiveGroupTargetID INT,
--	@InstitutionID INT

SELECT
    cgt.CompetitiveGroupTargetID,
    COALESCE(cgt.ContractOrganizationName, cgt.Name) AS ContractOrganizationName,
    cgt.UID,
    cgt.CreatedDate,
    cgt.ModifiedDate,
    cgt.InstitutionID,
    cgt.HaveContract,
    cgt.ContractNumber,
    cgt.ContractDate,
    cgt.ContractOrganizationOGRN,
    cgt.ContractOrganizationKPP,
    cgt.EmployerOrganizationName,
    cgt.EmployerOrganizationOGRN,
    cgt.EmployerOrganizationKPP,
    cgt.LocationEmployerOrganizations
FROM
    CompetitiveGroupTarget AS cgt
WHERE cgt.CompetitiveGroupTargetID=@CompetitiveGroupTargetID
    AND cgt.InstitutionID=@InstitutionID