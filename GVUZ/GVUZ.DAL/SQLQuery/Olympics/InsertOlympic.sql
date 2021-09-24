--DECLARE @OlympicTypeProfileID INT

insert into OlympicTypeProfile 
(
	OlympicTypeID,
	OlympicProfileID,
	OlympicLevelID,
	OrganizerName,
	FirstName,
	LastName,
	MiddleName,
	Email,
	Position,
	OrganizerAddress,
	PhoneNumber,
	OrganizerConnected,
	CoOrganizerConnected,
	OrganizerID,
	CoOrganizerID,
	OrgOlympicEnterID
)
values
(
	@OlympicTypeID,
	@OlympicProfileID,
	@OlympicLevelID,
	@OrganizerName,
	@FirstName,
	@LastName,
	@MiddleName,
	@Email,
	@Position,
	@OrganizerAddress,
	@PhoneNumber,
	@OrganizerConnected,
	@CoOrganizerConnected,
	@OrganizerID,
	@CoOrganizerID,
	@OrgOlympicEnterID
);

select SCOPE_IDENTITY();