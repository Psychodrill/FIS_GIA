update Institution
set
	HasHostel = @HasHostel,
	HasHostelForEntrants = @HasHostelForEntrants,
	HostelCapacity = @HostelCapacity,
	HasMilitaryDepartment = @HasMilitaryDepartment,
	Phone = @Phone,
	Fax = @Fax,
	Site = @Site,
	HasDisabilityEntrance = @HasDisabilityEntrance
where
	InstitutionId = @institutionId