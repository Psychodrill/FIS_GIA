SELECT * FROM AdmissionVolume AS av
	INNER JOIN DistributedAdmissionVolume AS dav ON dav.AdmissionVolumeID = av.AdmissionVolumeID
WHERE av.InstitutionID = @InstitutionID