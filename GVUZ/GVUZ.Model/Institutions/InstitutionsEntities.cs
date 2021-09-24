using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;

namespace GVUZ.Model.Institutions
{
    public partial class InstitutionsEntities //: IAttachmentFactory
    {
        partial void OnContextCreated()
        {
            this.InitCommandTimeout();
        }

        /// <summary>
        ///     Создает ОУ и связывает его с учетной записью администратора.
        /// </summary>
        public Institution CreateInstitution
            (string fullName, string inn, string ogrn, string userName, string administratorName,
             Guid administratorID, InstitutionType institutionType)
        {
            var institution = new Institution
                {
                    FullName = fullName,
                    INN = inn,
                    OGRN = ogrn,
                    Type = institutionType
                };
            var policy = new UserPolicy
                {
                    FullName = administratorName,
                    Institution = institution,
                    IsMainAdmin = true,
                    UserID = administratorID,
                    UserName = userName
                };

            UserPolicy.AddObject(policy);

            this.AddRootStructureItemsAndSave(institution, "Нет");

            SaveChanges();

            return institution;
        }

        /// <summary>
        ///     Загружает ОУ по идентификатору.
        /// </summary>
        public Institution LoadInstitution(int instituionID)
        {
            return Institution
                .Include(x => x.InstitutionLicense)
                .Include(x => x.InstitutionLicense.Select(a => a.Attachment))
                .Include(x => x.InstitutionAccreditation)
                .Include(x => x.InstitutionAccreditation.Select(a => a.Attachment))
                .Include(x => x.HostelAttachment)
                .Include(x => x.FormOfLaw)
                .Include(x => x.RegionType).First(x => x.InstitutionID == instituionID);
        }

        /// <summary>
        ///     Загружает исторический ОУ по идентификатору.
        /// </summary>
        public InstitutionHistory LoadInstitutionHistory(int instituionID, int historyID)
        {
            return InstitutionHistory
                .Include(x => x.HostelAttachment)
                .Include(x => x.AccreditationAttachment)
                .Include(x => x.LicenseAttachment)
                .Include(x => x.FormOfLaw)
                .Include(x => x.RegionType)
                .First(x => x.InstitutionID == instituionID && x.InstitutionHistoryID == historyID);
        }

        /// <summary>
        ///     Ищет ОУ по переданным <see cref="InstitutionSearchHierarchyParameters" />.
        /// </summary>
        //public TResult SearchInstitutions<TResult>
        //    (InstitutionSearchParameters<TResult> parameters, out int? totalPageCount)
        //{
        //    ValidateParameters(parameters);
        //    ObjectParameter pageCount = new ObjectParameter("TotalPageCount", typeof (int));
        //    var items = PerformSearch(parameters.IsVUZ, parameters.IsSSUZ, parameters.NamePart,
        //                              parameters.DirectionName, parameters.DirectionCode, parameters.RegionName,
        //                              parameters.FormOfLawID,
        //                              parameters.EducationLevelID, parameters.StudyID, parameters.AdmissionTypeID,
        //                              parameters.HasMilitaryDepartment, parameters.HasPreparatoryCourses,
        //                              parameters.HasOlympics,
        //                              parameters.DepthLimit, parameters.ParentStructureID,
        //                              parameters.PageSize, parameters.PageNumber, parameters.Snils,
        //                              pageCount);
        //    var results = parameters.Convert(items, parameters);
        //    totalPageCount = parameters.ParentStructureID.HasValue ? null : pageCount.Value.To<int?>();
        //    return results;
        //}
        private static void ValidateParameters<T>(InstitutionSearchParameters<T> parameters)
        {
            if (!parameters.IsVUZ && !parameters.IsSSUZ)
                throw new ValidationException("Выберите хотя бы один из типов - ВУЗ или ССУЗ.");
            if (parameters.Convert == null)
                throw new InvalidOperationException("Не задан метод конвертации.");
        }

        public Attachment AddAttachment()
        {
            var attachment = new Attachment();
            Attachment.AddObject(attachment);
            return attachment;
        }
    }
}