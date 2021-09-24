-- выборка данных для отображения на вкладке общей информации об ОО
-- dto: InstitutionInfoDto
--declare @institutionId int = 571

;with lic as (
	select top 1 * from InstitutionLicense where InstitutionId = @institutionId
	order by CreatedDate desc
),
acc as (
	select top 1 * from InstitutionAccreditation where InstitutionId = @institutionId
	order by CreatedDate desc
)
select top 1
	ins.InstitutionID,
	ins.FullName,
	ins.BriefName,
	fl.FormOfLawId,
	fl.Name FormOfLawName,
	ins.Site,
	reg.RegionId,
	reg.Name RegionName,
	ins.City,
	ins.Address,
	ins.Phone,
	ins.Fax,
	-- лицензия
	lic.LicenseId,
	lic.LicenseNumber,
	lic.LicenseDate,
	-- аккредитация
	acc.AccreditationID,
	acc.Accreditation AccreditationNumber,
	
	ins.HasHostel,
	ins.HostelCapacity,
	ins.HasHostelForEntrants,
	ins.HasMilitaryDepartment,

	ins.HasDisabilityEntrance
from 
	Institution ins
	left join FormOfLaw fl on ins.FormOfLawID = fl.FormOfLawID
	left join RegionType reg on ins.RegionID = reg.RegionId
	left join lic on lic.InstitutionID = ins.InstitutionID
	left join acc on acc.InstitutionID = acc.InstitutionID
where 
	ins.InstitutionID = @institutionId