update OlympicTypeProfile

set 
	OrganizerName = @OrganizerName, OrganizerAddress = @OrganizerAddress, 
	FirstName = @FirstName, LastName = @LastName, MiddleName = @MiddleName,
	Email = @Email, Position = @Position, PhoneNumber = @PhoneNumber,
	OlympicLevelID = @OlympicLevelID, 
	OlympicProfileID = @OlympicProfileID,
	OlympicTypeID = @OlympicTypeID,
    OrganizerConnected = @OrganizerConnected,
    CoOrganizerConnected = @CoOrganizerConnected,
    OrganizerID = @OrganizerID,
    CoOrganizerID = @CoOrganizerID,
	OrgOlympicEnterID = @OrgOlympicEnterID

where OlympicTypeProfileID = @OlympicTypeProfileID
