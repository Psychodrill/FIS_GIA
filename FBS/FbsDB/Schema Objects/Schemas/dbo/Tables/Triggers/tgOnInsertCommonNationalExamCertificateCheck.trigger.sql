-- =============================================
-- Тригер на добавление записи в 
-- dbo.CommonNationalExamCertificateCheck
-- v.1.0: Created by Sedov Anton 22.07.2008
-- v.1.1: Modified by Fomin Dmitriy 11.09.2008
-- Ускорение.
-- =============================================
CREATE trigger dbo.tgOnInsertCommonNationalExamCertificateCheck
on dbo.CommonNationalExamCertificateCheck
for insert 
as
begin
	insert into dbo.CheckCommonNationalExamCertificateLog
		(
		Date
		, AccountId
		, CertificateNumber
		, IsOriginal
		, IsBatch
		, IsExist
		)
	select		
		cne_check_batch.CreateDate
		, cne_check_batch.OwnerAccountId AccountId
		, inserted_cne_check.CertificateNumber CertificateNumber
		, inserted_cne_check.IsOriginal
        , 1 IsBatch
		, case
			when not inserted_cne_check.CertificateNumber is null then 1
			else 0
		end
	from 
		Inserted inserted_cne_check
			inner join dbo.CommonNationalExamCertificateCheckBatch cne_check_batch
				on inserted_cne_check.BatchId = cne_check_batch.Id
	where exists(select * from Account a join Organization2010 b on a.OrganizationId=b.id and b.DisableLog=0 and a.id=cne_check_batch.OwnerAccountId)
end
