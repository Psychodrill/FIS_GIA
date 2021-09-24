using GVUZ.DAL.Dapper.Repository.Interfaces.Institution;
using GVUZ.DAL.Dto;
using Dapper;
using System.Linq;
using System;
using System.Collections.Generic;

namespace GVUZ.DAL.Dapper.Repository.Model.Institution
{
    public class InstitutionRepository : GvuzRepository, IInstitutionRepository
    {
        public InstitutionInfoDto GetInstitutionInfoDto(int institutionId)
        {
            return WithTransaction(tx =>
            {
                var institutionIdParam = new { institutionId };

                var dto = tx.Query<InstitutionInfoDto>(SQLQuery.GetInstitutionInfoDto, institutionIdParam).Single();

                if (dto.LicenseId.HasValue)
                {
                    dto.LicenseDocument = tx.Query<InstitutionInfoDocumentDto>(SQLQuery.GetInstitutionLicenseDocumentDto, new { institutionId, licenseId = dto.LicenseId.Value }).SingleOrDefault();
                }

                if (dto.AccreditationId.HasValue)
                {
                    dto.AccreditationDocument = tx.Query<InstitutionInfoDocumentDto>(SQLQuery.GetInstitutionAccreditationDocumentDto, new { institutionId, accreditationId = dto.AccreditationId.Value }).SingleOrDefault();
                }

                dto.HostelDocument = tx.Query<InstitutionInfoDocumentDto>(SQLQuery.GetInstitutionHostelDocumentDto, institutionIdParam).SingleOrDefault();

                dto.Documents = tx.Query<InstitutionInfoYearDocumentDto>(SQLQuery.GetInstitutionDocuments, institutionIdParam).AsList();

                return dto;
            });
        }

        public void UpdateInstitutionInfo(int institutionId, InstitutionInfoUpdateDto dto)
        {         
            WithTransaction(tx =>
            {
                var update = new
                {
                    institutionId,
                    dto.HasHostel,
                    dto.HasHostelForEntrants,
                    dto.HostelCapacity,
                    dto.HasMilitaryDepartment,
                    dto.Phone,
                    dto.Fax,
                    dto.Site,
                    dto.AccreditationNumber,
                    dto.HasDisabilityEntrance
                };

                tx.Execute(SQLQuery.UpdateInstitutionInfo, update);

                var updateAccreditation = new
                {
                    dto.AccreditationNumber,
                    institutionId
                };

                tx.Execute(SQLQuery.UpdateInstitutionAccreditationNumber, updateAccreditation);

                var institutionIdParam = new { institutionId };

                if (dto.IsLicenseFileDeleted)
                {
                    tx.Execute(SQLQuery.DeleteInstitutionLicenseFile, institutionIdParam);
                }

                if (dto.UploadedLicenseFile != null)
                {
                    int attachmentId = tx.InsertAttachment(dto.UploadedLicenseFile);
                    tx.Execute(SQLQuery.UpdateInstitutionLicenseFile, new { institutionId, attachmentId });
                }

                if (dto.IsAccreditationFileDeleted)
                {
                    tx.Execute(SQLQuery.DeleteInstitutionAccreditationFile, institutionIdParam);
                }

                if (dto.UploadedAccreditationFile != null)
                {
                    int attachmentId = tx.InsertAttachment(dto.UploadedAccreditationFile);
                    tx.Execute(SQLQuery.UpdateInstitutionAccreditationFile, new { institutionId, attachmentId });
                }

                if (dto.IsHostelFileDeleted)
                {
                    tx.Execute(SQLQuery.DeleteInstitutionHostelFile, institutionIdParam);
                }

                if (dto.UploadedHostelFile != null)
                {
                    int attachmentId = tx.InsertAttachment(dto.UploadedHostelFile);
                    tx.Execute(SQLQuery.UpdateInstitutionHostelFile, new { institutionId, attachmentId });
                }
            });
        }

        public void DeleteInstitutionDocument(int institutionId, int attachmentId)
        {
            WithTransaction(tx => tx.Execute(SQLQuery.DeleteInstitutionDocument, new { institutionId, attachmentId }));
        }

        public void AddInstitutionDocument(int institutionId, AttachmentCreateDto dto, int year)
        {
            WithTransaction(tx =>
            {
                int attachmentId = tx.InsertAttachment(dto);
                tx.Execute(SQLQuery.UpdateInstitutionDocument, new { institutionId, attachmentId, year });
            });
        }

        public List<InstitutionInfoYearDocumentDto> GetInstitutionDocumentsList(int institutionId)
        {
            return WithTransaction(tx => tx.Query<InstitutionInfoYearDocumentDto>(SQLQuery.GetInstitutionDocuments, new { institutionId }).AsList());
        }

        /// <summary>
        /// Получение названия ОО по идентификатору
        /// </summary>
        /// <param name="institutionId">Идентификатор ОО</param>
        /// <returns>Название и идентификатор</returns>
        public SimpleDto GetInstitutionName(int institutionId)
        {
            return WithTransaction(tx => tx.Query<SimpleDto>(SQLQuery.GetInstitutionSimpleDto, new { institutionId }).Single());
        }
    }
}
