using GVUZ.ImportService2016.Core.Main.Dictionaries;
using GVUZ.ImportService2016.Core.Dto.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GVUZ.ImportService2016.Core.Main.Extensions;
using GVUZ.ImportService2016.Core.Main.Repositories;

namespace GVUZ.ImportService2016.Core.Dto.DataReaders
{

    public class BulkEntrantDocumentReader : BulkReaderBase<BulkEntrantDocumentDto>
    {
        public BulkEntrantDocumentSubjectReader BulkEntrantDocumentSubjectReader { get; set; } 

        public BulkEntrantDocumentReader(PackageData packageData, VocabularyStorage vocabularyStorage)
        {
            BulkEntrantDocumentSubjectReader = new BulkEntrantDocumentSubjectReader(packageData, vocabularyStorage);

            foreach(var app in packageData.ApplicationsToImport())
            {
                _records.AddRange(BulkEntrantDocumentDto.GetItems(app, vocabularyStorage, BulkEntrantDocumentSubjectReader));

            }
            //BulkEntrantocumentDto.CorrectDubles(ref _records);
            _records.CorrectDubles();

            AddGetter("Id", dto => dto.GUID); // 
            AddGetter("ParentId", dto => dto.ApplicationGuid);
            AddGetter("UID", dto => dto.UID);
            AddGetter("ImportPackageId", dto => packageData.ImportPackageId);
            AddGetter("InstitutionId", dto => packageData.InstitutionId);

            AddGetter("InstitutionName", dto => vocabularyStorage.InstitutionName);

            AddGetter("OriginalReceived", dto => dto.OriginalReceived);
            AddGetter("OriginalReceivedDate", dto => GetDateOrNull(dto.OriginalReceivedDate));
            AddGetter("DocumentNumber", dto => GetStringOrNull(dto.DocumentNumber));
            AddGetter("DocumentDate", dto => GetDateOrNull(dto.DocumentDate));
            AddGetter("DocumentOrganization", dto => GetStringOrNull(dto.DocumentOrganization));
            AddGetter("DocumentSeries", dto => GetStringOrNull(dto.DocumentSeries));
            AddGetter("DocumentTypeId", dto => dto.DocumentTypeId);

            AddGetter("FacultyName", dto => GetStringOrNull(dto.FacultyName));
            AddGetter("InstitutionAddress", dto => GetStringOrNull(dto.InstitutionAddress));
            AddGetter("BeginDate", dto => GetDateOrNull(dto.BeginDate));
            AddGetter("EndDate", dto => GetDateOrNull(dto.EndDate));
            AddGetter("EducationFormId", dto => GetIntOrNull(dto.EducationFormId));

            AddGetter("DocumentSpecificData", dto => dto.DocumentSpecificData);

            AddGetter("IdentityDocumentTypeId", dto => GetIntOrNull(dto.IdentityDocumentTypeId));
            AddGetter("NationalityTypeId", dto => GetIntOrNull(dto.NationalityTypeId));
            AddGetter("BirthDate", dto => GetDateOrNull(dto.BirthDate));
            AddGetter("EndYear", dto => GetIntOrNull(dto.EndYear));
            AddGetter("BirthPlace", dto => GetStringOrNull(dto.BirthPlace));
            AddGetter("SubdivisionCode", dto => GetStringOrNull(dto.SubdivisionCode));

            AddGetter("RegistrationNumber", dto => GetStringOrNull(dto.RegistrationNumber));
            AddGetter("QualificationName", dto => GetStringOrNull(dto.QualificationName));
            AddGetter("SpecialityName", dto => GetStringOrNull(dto.SpecialityName));
            AddGetter("SpecializationId", dto => GetIntOrNull(dto.SpecializationId));
            AddGetter("ProfessionId", dto => GetIntOrNull(dto.ProfessionId));
            AddGetter("DocumentTypeNameText", dto => GetStringOrNull(dto.DocumentTypeNameText));

            AddGetter("AdditionalInfo", dto => GetStringOrNull(dto.AdditionalInfo));
            AddGetter("DisabilityTypeId", dto => GetIntOrNull(dto.DisabilityTypeId));

            AddGetter("DiplomaTypeId", dto => GetIntOrNull(dto.DiplomaTypeId));
            AddGetter("OlympicId", dto => GetIntOrNull(dto.OlympicId));
            AddGetter("OlympicPlace", dto => GetStringOrNull(dto.OlympicPlace));
            AddGetter("OlympicDate", dto => GetDateOrNull(dto.OlympicDate));

            AddGetter("ProfileId", dto => GetIntOrNull(dto.ProfileId));
            AddGetter("ClassNumber", dto => GetIntOrNull(dto.ClassNumber));
            AddGetter("OlympicName", dto => GetStringOrNull(dto.OlympicName));
            AddGetter("OlympicProfile", dto => GetStringOrNull(dto.OlympicProfile));

            AddGetter("SportCategoryId", dto => GetIntOrNull(dto.SportCategoryId));
            AddGetter("OrphanCategoryId", dto => GetIntOrNull(dto.OrphanCategoryId));
            AddGetter("CompatriotCategoryId", dto => GetIntOrNull(dto.CompatriotCategoryId));
            AddGetter("CompatriotStatus", dto => GetStringOrNull(dto.CompatriotStatus));
            AddGetter("CountryID", dto => GetIntOrNull(dto.CountryID));

            AddGetter("EntranceTestResultId", dto => dto.EntranceTestResultId.HasValue? dto.EntranceTestResultId.Value : (object)DBNull.Value);
            AddGetter("GPA", dto => dto.GPA.HasValue && dto.GPA != 0 ? dto.GPA : (object)DBNull.Value );
            //AddGetter("IgnoreIdentityImport", dto => GetDateOrNull(dto.IgnoreIdentityImport));

            AddGetter("EntrantDocumentID", dto => dto.ID);

            AddGetter("VeteranCategoryID", dto => GetIntOrNull(dto.VeteranCategoryID));

            AddGetter("LastName", dto => GetStringOrNull(dto.LastName));
            AddGetter("FirstName", dto => GetStringOrNull(dto.FirstName));
            AddGetter("MiddleName", dto => GetStringOrNull(dto.MiddleName));

            AddGetter("ProfileSubjectID", dto => GetIntOrNull(dto.ProfileSubjectID));
            AddGetter("EgeSubjectID", dto => GetIntOrNull(dto.EgeSubjectID));

            //2017
            AddGetter("ParentsLostCategoryID", dto => GetIntOrNull(dto.ParentsLostCategoryID));
            AddGetter("StateEmployeeCategoryID", dto => GetIntOrNull(dto.StateEmployeeCategoryID));
            AddGetter("RadiationWorkCategoryID", dto => GetIntOrNull(dto.RadiationWorkCategoryID));

            AddGetter("StateServicePreparation", dto => dto.StateServicePreparation);
            AddGetter("IsNostrificated", dto => dto.IsNostrificated);

            AddGetter("ReleaseCountryID", dto => dto.ReleaseCountryID == 0 ? (object)DBNull.Value : dto.ReleaseCountryID);
            AddGetter("ReleasePlace", dto => GetStringOrNull(dto.ReleasePlace));
        }
    }

    public static class BulkEntrantDocumentHelper
    {
        public static List<BulkEntrantDocumentDto> CorrectDubles(this List<BulkEntrantDocumentDto> items)
        {
            // Новый код имени Романа Букина, который должен навсегда побороть ошибки 
            // в ImportApplication с Merge EntrantDocument
            Dictionary<int, BulkEntrantDocumentDto> correctDubles = new Dictionary<int, BulkEntrantDocumentDto>();
            int i = items.Count - 1;
            while (i >= 0)
            {
                BulkEntrantDocumentDto dto = items[i];
                if (dto.ID == 0)
                {
                    i--;
                    continue;
                }


                if (correctDubles.ContainsKey(dto.ID))
                {
                    BulkEntrantDocumentDto parent = correctDubles[dto.ID];

                    dto.AdditionalInfo = parent.AdditionalInfo;
                    dto.BirthDate = parent.BirthDate;
                    dto.BirthPlace = parent.BirthPlace;
                    dto.DiplomaTypeId = parent.DiplomaTypeId;
                    dto.DisabilityTypeId = parent.DisabilityTypeId;

                    dto.DocumentDate = parent.DocumentDate;
                    dto.DocumentNumber = parent.DocumentNumber;
                    dto.DocumentOrganization = parent.DocumentOrganization;
                    dto.DocumentSeries = parent.DocumentSeries;
                    dto.DocumentTypeId = parent.DocumentTypeId;
                    dto.DocumentTypeNameText = parent.DocumentTypeNameText;
                    dto.DocumentSpecificData = parent.DocumentSpecificData;

                    dto.OriginalReceived = parent.OriginalReceived;
                    dto.OriginalReceivedDate = parent.OriginalReceivedDate;
                    items[i] = dto;
                }
                else
                {
                    correctDubles.Add(dto.ID, dto);

                }
                i--;
            }

            return items;
        }
    }

    public class BulkEntrantDocumentDto : ImportBase
    {
        public BulkEntrantDocumentDto() { }

        public BulkEntrantDocumentDto(PackageDataApplication application)
        {
            ApplicationGuid = application.GUID;
        }

        public BulkEntrantDocumentDto(PackageDataApplication application, TCustomDocument doc, VocabularyStorage vocStorage)
            : this(application)
        {
            FillBase(doc, application);
            AdditionalInfo = doc.AdditionalInfo;
            DocumentTypeNameText = doc.DocumentName;
        }


        public BulkEntrantDocumentDto(PackageDataApplication application, IEduDocument eduDocument, VocabularyStorage vocStorage)
            : this(application)
        {
            FillEduDocument(eduDocument, application, vocStorage);
        }

        public BulkEntrantDocumentDto(PackageDataApplication application, TSchoolCertificateDocument eduDocument, VocabularyStorage vocStorage, Model.Entrants.Documents.EntrantDocumentType documentType, BulkEntrantDocumentSubjectReader bulkEntrantDocumentSubjectReader)
            : this(application, eduDocument, vocStorage)
        {
            DocumentTypeId = (int)documentType;

            StateServicePreparation = eduDocument.StateServicePreparation;

            if (eduDocument.Subjects != null)
                foreach (var subject in eduDocument.Subjects)
                    bulkEntrantDocumentSubjectReader.Add(new BulkEntrantDocumentSubjectDto(subject, this.GUID, vocStorage));
        }

        public BulkEntrantDocumentDto(PackageDataApplication application, TMiddleEduDiplomaDocument eduDocument, VocabularyStorage vocStorage)
            : this(application)
        {
            FillEduDocument(eduDocument, application, vocStorage);
            StateServicePreparation = eduDocument.StateServicePreparation;
        }

        public BulkEntrantDocumentDto(PackageDataApplication application, TBasicDiplomaDocument eduDocument, VocabularyStorage vocStorage)
            : this(application)
        {
            FillEduDocument(eduDocument, application, vocStorage);
            StateServicePreparation = eduDocument.StateServicePreparation;
        }

        public BulkEntrantDocumentDto(PackageDataApplication application, PackageDataApplicationApplicationDocumentsEgeDocument doc, VocabularyStorage vocStorage, BulkEntrantDocumentSubjectReader bulkEntrantDocumentSubjectReader)
            : this(application)
        {
            FillBase(doc, application);
            // В этом нет никакой специальной логики, просто год может быть не задан - тогда null
            DocumentDate = doc.DocumentYear > 1977 ? new DateTime(doc.DocumentYear.To(0), 1, 1) : (DateTime?)null;

            if (doc.Subjects != null)
                foreach (var subject in doc.Subjects)
                    bulkEntrantDocumentSubjectReader.Add(new BulkEntrantDocumentSubjectDto(subject, this.GUID, vocStorage));
        }

        public BulkEntrantDocumentDto(PackageDataApplication application, PackageDataApplicationApplicationDocumentsGiaDocument doc, VocabularyStorage vocStorage, BulkEntrantDocumentSubjectReader bulkEntrantDocumentSubjectReader)
            : this(application)
        {
            FillBase(doc, application);

            if (doc.Subjects != null)
                foreach (var subject in doc.Subjects)
                    bulkEntrantDocumentSubjectReader.Add(new BulkEntrantDocumentSubjectDto(subject, this.GUID, vocStorage));
        }

        public BulkEntrantDocumentDto(PackageDataApplication application, IIdentityDocument doc, VocabularyStorage vocStorage, bool primary)
            : this(application)
        {
            FillBase(doc, application);

            SubdivisionCode = doc.SubdivisionCode;
            IdentityDocumentTypeId = doc.IdentityDocumentTypeID.To(0);
            NationalityTypeId = doc.NationalityTypeID.To(0);
            BirthDate = doc.BirthDate;
            BirthPlace = doc.BirthPlace;

            LastName = doc.LastName;
            FirstName = doc.FirstName;
            MiddleName = doc.MiddleName;

            ReleaseCountryID = application.ApplicationDocuments.IdentityDocument.ReleaseCountryID;
            ReleasePlace = application.ApplicationDocuments.IdentityDocument.ReleasePlace;

            SpecializationId = primary ? 1 : 0; // для отличия основного документа от остальных
        }

        public BulkEntrantDocumentDto(PackageDataApplication application, PackageDataApplicationApplicationDocumentsMilitaryCardDocument doc, VocabularyStorage vocStorage)
            : this(application)
        {
            FillBase(doc, application);
        }

        public BulkEntrantDocumentDto(PackageDataApplication application, PackageDataApplicationApplicationDocumentsStudentDocument doc, VocabularyStorage vocStorage)
            : this(application)
        {
            FillBase(doc, application);

            FacultyName = doc.FacultyName;
            InstitutionAddress = doc.InstitutionAddress;
            BeginDate = doc.BeginDate;
            EndDate = doc.EndDate;
            EducationFormId = doc.EducationFormID;
            SpecialityName = doc.SpecialityName;
            QualificationName = doc.QualificationName;
            CountryID = doc.CountryID;

        }

        public BulkEntrantDocumentDto(PackageDataApplication application, TOlympicDocument doc, VocabularyStorage vocStorage)
            : this(application)
        {
            FillBase(doc, application);
            DiplomaTypeId = doc.DiplomaTypeID.To(0);
            OlympicId = doc.OlympicID.To(0);
            //ProfileId = doc.ProfileID.To(0);
            ProfileId = doc.OlympicTypeProfileID;
            ClassNumber = doc.ClassNumber.To(0);
            // Нет поля в БД! LevelID = doc.LevelID

            ProfileSubjectID = doc.OlympicSubjectID.To(0);
            EgeSubjectID = doc.EgeSubjectID.To(0);

        }

        public BulkEntrantDocumentDto(PackageDataApplication application, TOlympicTotalDocument doc, VocabularyStorage vocStorage, BulkEntrantDocumentSubjectReader bulkEntrantDocumentSubjectReader)
            : this(application)
        {
            FillBase(doc, application);
            ClassNumber = doc.ClassNumber.To(0);
            OlympicId = doc.OlympicID.To(0);
            DiplomaTypeId = doc.DiplomaTypeID.To(0);

            ProfileId = doc.OlympicTypeProfileID;

            //if (doc.Subjects != null)
            //    foreach (var subject in doc.Subjects)
            //        bulkEntrantDocumentSubjectReader.Add(new BulkEntrantDocumentSubjectDto(subject, this.GUID, vocStorage));

            //if (doc.OlympicTypeProfileID != null)
            //    foreach (var otp in doc.OlympicTypeProfileID)
            //        bulkEntrantDocumentSubjectReader.Add(new BulkEntrantDocumentSubjectDto(otp, this.GUID, vocStorage));

        }

        public BulkEntrantDocumentDto(PackageDataApplication application, TAllowEducationDocument doc, VocabularyStorage vocStorage)
            : this(application)
        {
            FillBase(doc, application);
        }

        public BulkEntrantDocumentDto(PackageDataApplication application, TDisabilityDocument doc, VocabularyStorage vocStorage)
            : this(application)
        {
            FillBase(doc, application);
            DisabilityTypeId = doc.DisabilityTypeID.To(0);
        }

        public BulkEntrantDocumentDto(PackageDataApplication application, TMedicalDocument doc, VocabularyStorage vocStorage)
            : this(application)
        {
            FillBase(doc, application);
        }

        // 2016 //
        public BulkEntrantDocumentDto(PackageDataApplication application, TInternationalOlympic doc, VocabularyStorage vocStorage) : this(application)
        {
            FillBase(doc, application);
            CountryID = doc.CountryID.To(0);
            OlympicDate = doc.OlympicDateSpecified ? (DateTime?)doc.OlympicDate : null;
            DocumentTypeNameText = doc.OlympicName;
            OlympicPlace = doc.OlympicPlace;
            OlympicProfile = doc.OlympicProfile;
        }

        public BulkEntrantDocumentDto(PackageDataApplication application, TOrphanDocument doc, VocabularyStorage vocStorage) : this(application)
        {
            FillBase(doc, application);
            OrphanCategoryId = doc.OrphanCategoryID.To(0);
            //AdditionalInfo = doc.AdditionalInfo;
            DocumentTypeNameText = doc.DocumentName;
        }

        public BulkEntrantDocumentDto(PackageDataApplication application, TSportDocument doc, VocabularyStorage vocStorage) : this(application)
        {
            FillBase(doc, application);
            SportCategoryId = doc.SportCategoryID.To(0);
            AdditionalInfo = doc.AdditionalInfo;
            DocumentTypeNameText = doc.DocumentName;
        }

        public BulkEntrantDocumentDto(PackageDataApplication application, TUkraineOlympic doc, VocabularyStorage vocStorage) : this(application)
        {
            FillBase(doc, application);
            DiplomaTypeId = doc.DiplomaTypeID.To(0);
            OlympicDate = doc.OlympicDateSpecified ? (DateTime?)doc.OlympicDate : null;
            DocumentTypeNameText = doc.OlympicName;
            OlympicPlace = doc.OlympicPlace;
            OlympicProfile = doc.OlympicProfile;
        }
        public BulkEntrantDocumentDto(PackageDataApplication application, TVeteranDocument doc, VocabularyStorage vocStorage) : this(application)
        {
            FillBase(doc, application);
            VeteranCategoryID = doc.VeteranCategoryID.To(0);
            DocumentTypeNameText = doc.DocumentName;
        }

        public BulkEntrantDocumentDto(PackageDataApplication application, TCompatriotDocument doc, VocabularyStorage vocStorage) : this(application)
        {
            FillBase(doc, application);
            CompatriotCategoryId = doc.CompariotCategoryID.To(0);
            //AdditionalInfo = doc.AdditionalInfo;
            DocumentTypeNameText = doc.DocumentName;
            CompatriotStatus = doc.CompatriotStatus;
        }

        // 2017

        public BulkEntrantDocumentDto(PackageDataApplication application, TPauperDocument doc, VocabularyStorage vocStorage) : this(application)
        {
            FillBase(doc, application);
            DocumentTypeNameText = doc.DocumentName;
        }

        public BulkEntrantDocumentDto(PackageDataApplication application, TParentsLostDocument doc, VocabularyStorage vocStorage) : this(application)
        {
            FillBase(doc, application);
            ParentsLostCategoryID = doc.ParentsLostCategoryID.To(0);
            DocumentTypeNameText = doc.DocumentName;
        }

        public BulkEntrantDocumentDto(PackageDataApplication application, TStateEmployeeDocument doc, VocabularyStorage vocStorage) : this(application)
        {
            FillBase(doc, application);
            StateEmployeeCategoryID = doc.StateEmployeeCategoryID.To(0);
            DocumentTypeNameText = doc.DocumentName;
        }

        public BulkEntrantDocumentDto(PackageDataApplication application, TRadiationWorkDocument doc, VocabularyStorage vocStorage) : this(application)
        {
            FillBase(doc, application);
            RadiationWorkCategoryID = doc.RadiationWorkCategoryID.To(0);
            DocumentTypeNameText = doc.DocumentName;
        }

        public void FillBase(IBaseDocument doc, PackageDataApplication application)
        {
            UID = doc.UID;
            ID = doc.ID;
            GUID = doc.GUID;
            DocumentTypeId = (int)doc.EntrantDocumentType;
            OriginalReceived = doc.OriginalReceivedDateSpecified;
            OriginalReceivedDate = doc.OriginalReceivedDate;
            DocumentSeries = doc.DocumentSeries;
            DocumentNumber = doc.DocumentNumber;
            DocumentDate = doc.DocumentDate;
            DocumentOrganization = doc.DocumentOrganization;

            if (ID == 0)
            {
                var res = ADOApplicationRepository.GetEntrantDocumentByData(application.EntrantID, DocumentTypeId, DocumentSeries, DocumentNumber, doc.DocumentDate, DocumentOrganization, UID);
                if (res.Item1 != 0)
                {
                    ID = res.Item1;
                    if (res.Item2 != Guid.Empty)
                    {
                        doc.GUID = res.Item2;
                        GUID = res.Item2;
                    }
                }
            }

            DocumentSpecificData = "";  //new JavaScriptSerializer().Serialize(doc);
        }

        protected void FillEduDocument(IEduDocument doc, PackageDataApplication application, VocabularyStorage vocStorage)
        {
            FillBase(doc, application);

            RegistrationNumber = doc.RegistrationNumber;

            //QualificationName = vocStorage.DirectionVoc.GetQualificationName(doc.QualificationTypeID.ToString());
            //SpecialityName = vocStorage.DirectionVoc.GetSpecialityName(doc.SpecialityID.ToString());
            QualificationName = VocabularyStatic.DirectionVoc.GetQualificationName(doc.SpecialityID.To(0));
            SpecialityName = VocabularyStatic.DirectionVoc.GetSpecialityName(doc.SpecialityID.To(0));
            // Проверить, точно ли это так? раньше искали по коду, а теперь по ID?

            GPA = doc.GPA;

            SpecializationId = null; // (doc.SpecializationID.To(0));
            EndYear = doc.EndYear.To(0);
            IsNostrificated = doc.IsNostrificated;
            FacultyName = doc.FacultyName;
            InstitutionAddress = doc.InstitutionAddress;
            BeginDate = doc.BeginDate;
            EndDate = doc.EndDate;
            EducationFormId = doc.EducationFormID;
            SpecialityName = doc.SpecialityName;
            QualificationName = doc.QualificationName;
            CountryID = doc.CountryID;

        }

        public static List<BulkEntrantDocumentDto> GetItems(PackageDataApplication application, VocabularyStorage vocStorage, BulkEntrantDocumentSubjectReader bulkEntrantDocumentSubjectReader)
        {
            var res = new List<BulkEntrantDocumentDto>();
            // В EntrantDocument попадают:
            // 1. AcademicDiplomaDocument
            if (application.ApplicationDocuments != null)
            {
                if (application.ApplicationDocuments.CustomDocuments != null)
                    foreach (var doc in application.ApplicationDocuments.CustomDocuments)
                    {
                        res.Add(new BulkEntrantDocumentDto(application, doc, vocStorage));
                    }
                if (application.ApplicationDocuments.EduDocuments != null)
                    foreach (var doc in application.ApplicationDocuments.EduDocuments)
                    {
                        switch (doc.ItemElementName)
                        {
                            case ItemChoiceType2.AcademicDiplomaDocument:
                            case ItemChoiceType2.EduCustomDocument:
                            case ItemChoiceType2.HighEduDiplomaDocument:
                            case ItemChoiceType2.IncomplHighEduDiplomaDocument:        
                            case ItemChoiceType2.PhDDiplomaDocument:
                            case ItemChoiceType2.PostGraduateDiplomaDocument:
                                res.Add(new BulkEntrantDocumentDto(application, doc.Item as IEduDocument, vocStorage));
                                break;

                            case ItemChoiceType2.SchoolCertificateBasicDocument:
                                res.Add(new BulkEntrantDocumentDto(application, doc.Item as TSchoolCertificateDocument, vocStorage, Model.Entrants.Documents.EntrantDocumentType.SchoolCertificateBasicDocument, bulkEntrantDocumentSubjectReader));
                                break;
                            case ItemChoiceType2.SchoolCertificateDocument:
                                res.Add(new BulkEntrantDocumentDto(application, doc.Item as TSchoolCertificateDocument, vocStorage, Model.Entrants.Documents.EntrantDocumentType.SchoolCertificateDocument, bulkEntrantDocumentSubjectReader));
                                break;

                            case ItemChoiceType2.MiddleEduDiplomaDocument:
                                res.Add(new BulkEntrantDocumentDto(application, doc.Item as TMiddleEduDiplomaDocument, vocStorage));
                                break;
                            case ItemChoiceType2.BasicDiplomaDocument:
                                res.Add(new BulkEntrantDocumentDto(application, doc.Item as TBasicDiplomaDocument, vocStorage));
                                break;

                            default:
                                throw new Exception("Необратабываемый тип документа!");

                        }
                    }
                if (application.ApplicationDocuments.EgeDocuments != null)
                    foreach (var doc in application.ApplicationDocuments.EgeDocuments)
                    {
                        res.Add(new BulkEntrantDocumentDto(application, doc, vocStorage, bulkEntrantDocumentSubjectReader));
                    }
                if (application.ApplicationDocuments.GiaDocuments != null)
                    foreach (var doc in application.ApplicationDocuments.GiaDocuments)
                    {
                        res.Add(new BulkEntrantDocumentDto(application, doc, vocStorage, bulkEntrantDocumentSubjectReader));
                    }
                if (application.ApplicationDocuments.IdentityDocument != null)
                {
                    res.Add(new BulkEntrantDocumentDto(application, application.ApplicationDocuments.IdentityDocument, vocStorage, true));
                }
                if (application.ApplicationDocuments.OtherIdentityDocuments != null)
                {
                    foreach (var doc in application.ApplicationDocuments.OtherIdentityDocuments)
                    {
                        if (string.IsNullOrWhiteSpace(doc.UID) || doc.UID != application.ApplicationDocuments.IdentityDocument.UID)
                            res.Add(new BulkEntrantDocumentDto(application, doc, vocStorage, false));
                    }
                }
                if (application.ApplicationDocuments.MilitaryCardDocument != null)
                {
                    res.Add(new BulkEntrantDocumentDto(application, application.ApplicationDocuments.MilitaryCardDocument, vocStorage));
                }
                if (application.ApplicationDocuments.StudentDocument != null)
                {
                    res.Add(new BulkEntrantDocumentDto(application, application.ApplicationDocuments.StudentDocument, vocStorage));
                }
                if (application.ApplicationDocuments.CompatriotDocuments != null)
                {
                    foreach (var doc in application.ApplicationDocuments.CompatriotDocuments)
                    {
                        res.Add(new BulkEntrantDocumentDto(application, doc, vocStorage));
                    }
                }
                if (application.ApplicationDocuments.OrphanDocuments != null)
                {
                    foreach (var doc in application.ApplicationDocuments.OrphanDocuments)
                    {
                        res.Add(new BulkEntrantDocumentDto(application, doc, vocStorage));
                    }
                }
                if (application.ApplicationDocuments.VeteranDocuments != null)
                {
                    foreach (var doc in application.ApplicationDocuments.VeteranDocuments)
                    {
                        res.Add(new BulkEntrantDocumentDto(application, doc, vocStorage));
                    }
                }
                if (application.ApplicationDocuments.PauperDocuments != null)
                {
                    foreach (var doc in application.ApplicationDocuments.PauperDocuments)
                    {
                        res.Add(new BulkEntrantDocumentDto(application, doc, vocStorage));
                    }
                }
                if (application.ApplicationDocuments.ParentsLostDocuments != null)
                {
                    foreach (var doc in application.ApplicationDocuments.ParentsLostDocuments)
                    {
                        res.Add(new BulkEntrantDocumentDto(application, doc, vocStorage));
                    }
                }
                if (application.ApplicationDocuments.StateEmployeeDocuments != null)
                {
                    foreach (var doc in application.ApplicationDocuments.StateEmployeeDocuments)
                    {
                        res.Add(new BulkEntrantDocumentDto(application, doc, vocStorage));
                    }
                }
                if (application.ApplicationDocuments.RadiationWorkDocuments != null)
                {
                    foreach (var doc in application.ApplicationDocuments.RadiationWorkDocuments)
                    {
                        res.Add(new BulkEntrantDocumentDto(application, doc, vocStorage));
                    }
                }

            } //Application.ApplicationDocuments

            // 2. ApplicationCommonBenefit(s)
            foreach (var benefit in application.getAllBenefits())
            {
                if (benefit.DocumentReason != null && benefit.DocumentReason.Item != null)
                {
                    if (benefit.DocumentReason.Item is TOlympicDocument)
                    {
                        res.Add(new BulkEntrantDocumentDto(application, benefit.DocumentReason.Item as TOlympicDocument, vocStorage));
                    }
                    if (benefit.DocumentReason.Item is TOlympicTotalDocument)
                    {
                        res.Add(new BulkEntrantDocumentDto(application, benefit.DocumentReason.Item as TOlympicTotalDocument, vocStorage, bulkEntrantDocumentSubjectReader));
                    }
                    if (benefit.DocumentReason.Item is TCustomDocument)
                    {
                        res.Add(new BulkEntrantDocumentDto(application, benefit.DocumentReason.Item as TCustomDocument, vocStorage));
                    }

                    if (benefit.DocumentReason.Item is TInternationalOlympic)
                    {
                        res.Add(new BulkEntrantDocumentDto(application, benefit.DocumentReason.Item as TInternationalOlympic, vocStorage));
                    }
                    if (benefit.DocumentReason.Item is TOrphanDocument)
                    {
                        res.Add(new BulkEntrantDocumentDto(application, benefit.DocumentReason.Item as TOrphanDocument, vocStorage));
                    }
                    if (benefit.DocumentReason.Item is TSportDocument)
                    {
                        res.Add(new BulkEntrantDocumentDto(application, benefit.DocumentReason.Item as TSportDocument, vocStorage));
                    }
                    if (benefit.DocumentReason.Item is TUkraineOlympic)
                    {
                        res.Add(new BulkEntrantDocumentDto(application, benefit.DocumentReason.Item as TUkraineOlympic, vocStorage));
                    }
                    if (benefit.DocumentReason.Item is TVeteranDocument)
                    {
                        res.Add(new BulkEntrantDocumentDto(application, benefit.DocumentReason.Item as TVeteranDocument, vocStorage));
                    }
                    if (benefit.DocumentReason.Item is TPauperDocument)
                    {
                        res.Add(new BulkEntrantDocumentDto(application, benefit.DocumentReason.Item as TPauperDocument, vocStorage));
                    }
                    if (benefit.DocumentReason.Item is TParentsLostDocument)
                    {
                        res.Add(new BulkEntrantDocumentDto(application, benefit.DocumentReason.Item as TParentsLostDocument, vocStorage));
                    }
                    if (benefit.DocumentReason.Item is TStateEmployeeDocument)
                    {
                        res.Add(new BulkEntrantDocumentDto(application, benefit.DocumentReason.Item as TStateEmployeeDocument, vocStorage));
                    }
                    if (benefit.DocumentReason.Item is TRadiationWorkDocument)
                    {
                        res.Add(new BulkEntrantDocumentDto(application, benefit.DocumentReason.Item as TRadiationWorkDocument, vocStorage));
                    }
                    if (benefit.DocumentReason.Item is PackageDataApplicationApplicationCommonBenefitDocumentReasonMedicalDocuments)
                    {
                        var medicalDocuments = benefit.DocumentReason.Item as PackageDataApplicationApplicationCommonBenefitDocumentReasonMedicalDocuments;
                        if (medicalDocuments.AllowEducationDocument != null)
                        {
                            res.Add(new BulkEntrantDocumentDto(application, medicalDocuments.AllowEducationDocument, vocStorage));
                        }
                        if (medicalDocuments.BenefitDocument != null && medicalDocuments.BenefitDocument.Item != null)
                        {
                            if (medicalDocuments.BenefitDocument.Item is TDisabilityDocument)
                            {
                                res.Add(new BulkEntrantDocumentDto(application, medicalDocuments.BenefitDocument.Item as TDisabilityDocument, vocStorage));
                            }
                            if (medicalDocuments.BenefitDocument.Item is TMedicalDocument)
                            {
                                res.Add(new BulkEntrantDocumentDto(application, medicalDocuments.BenefitDocument.Item as TMedicalDocument, vocStorage));
                            }

                        }
                    }
                }
            } //ApplicationCommonBenefit(s)

            // 3. EntranceTestResults
            if (application.EntranceTestResults != null)
            {
                foreach (var etResult in application.EntranceTestResults)
                {
                    if (etResult.ResultDocument != null && etResult.ResultDocument.Item != null)
                    {
                        if (etResult.ResultDocument.Item is TOlympicDocument)
                        {
                            res.Add(new BulkEntrantDocumentDto(application, etResult.ResultDocument.Item as TOlympicDocument, vocStorage));
                        }
                        if (etResult.ResultDocument.Item is TOlympicTotalDocument)
                        {
                            res.Add(new BulkEntrantDocumentDto(application, etResult.ResultDocument.Item as TOlympicTotalDocument, vocStorage, bulkEntrantDocumentSubjectReader));
                        }
                        //FIS - 1768 (30.10.2017 akopylov) не хватает документов для льготы 100 баллов
                        //TSportDocument
                        //TUkraineOlympic
                        //TInternationalOlympic
                        //TCustomDocument
                        if (etResult.ResultDocument.Item is TSportDocument)
                        {
                            res.Add(new BulkEntrantDocumentDto(application, etResult.ResultDocument.Item as TSportDocument, vocStorage));
                        }
                        if (etResult.ResultDocument.Item is TUkraineOlympic)
                        {
                            res.Add(new BulkEntrantDocumentDto(application, etResult.ResultDocument.Item as TUkraineOlympic, vocStorage));
                        }
                        if (etResult.ResultDocument.Item is TInternationalOlympic)
                        {
                            res.Add(new BulkEntrantDocumentDto(application, etResult.ResultDocument.Item as TInternationalOlympic, vocStorage));
                        }
                        if (etResult.ResultDocument.Item is TCustomDocument)
                        {
                            res.Add(new BulkEntrantDocumentDto(application, etResult.ResultDocument.Item as TCustomDocument, vocStorage));
                        }


                    }
                }
            }
            return res;
        }

        public string ApplicationUID { get; set; }
        public Guid ApplicationGuid { get; set; }
        public string UID { get; set; }


        public bool OriginalReceived { get; set; }
        public DateTime? OriginalReceivedDate { get; set; }
        public string DocumentNumber { get; set; }
        public DateTime? DocumentDate { get; set; }
        public string DocumentOrganization { get; set; }
        public string DocumentSeries { get; set; }
        public int DocumentTypeId { get; set; }

        public int? IdentityDocumentTypeId { get; set; }
        public int? NationalityTypeId { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? EndYear { get; set; }
        public string DocumentSpecificData { get; set; }
        public string BirthPlace { get; set; }
        public string SubdivisionCode { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }

        public string RegistrationNumber { get; set; }
        //public int? QualificationTypeId { get; set; }
        //public int? SpecialityId { get; set; }
        public string QualificationName { get; set; }
        public string SpecialityName { get; set; }

        public int? SpecializationId { get; set; }
        public int? ProfessionId { get; set; }
        public string DocumentTypeNameText { get; set; }
        public string AdditionalInfo { get; set; }
        public int? DisabilityTypeId { get; set; }
        public int? DiplomaTypeId { get; set; }
        public int? OlympicId { get; set; }
        public string OlympicPlace { get; set; }
        public DateTime? OlympicDate { get; set; }

        public int? ProfileId { get; set; }
        public int? ClassNumber { get; set; }
        public string OlympicName { get; set; }
        public string OlympicProfile { get; set; }
        public int? SportCategoryId { get; set; }
        public int? OrphanCategoryId { get; set; }
        public int? CompatriotCategoryId { get; set; }
        public int? VeteranCategoryID { get; set; }
        public int? ParentsLostCategoryID { get; set; }
        public int? StateEmployeeCategoryID { get; set; }
        public int? RadiationWorkCategoryID { get; set; }

        public bool? StateServicePreparation { get; set;}
        public int? CountryID { get; set; }
        public Guid? EntranceTestResultId { get; set; }
        public float? GPA { get; set; }

        public int? ProfileSubjectID { get; set; }
        public int? EgeSubjectID { get; set; }

        public bool? IsNostrificated { get; set; }

        public string FacultyName { get; set; }
        public string InstitutionAddress { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public byte? EducationFormId { get; set; }
        public string CompatriotStatus { get; set; }



        public int? ReleaseCountryID { get; set; }
        public string ReleasePlace { get; set; }

    }
}
