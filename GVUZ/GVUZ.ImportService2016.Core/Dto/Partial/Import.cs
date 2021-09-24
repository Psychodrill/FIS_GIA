using GVUZ.ImportService2016.Core.Main.Dictionaries;
using GVUZ.ImportService2016.Core.Main.Dictionaries.EntranceTest;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GVUZ.ImportService2016.Core.Main.Extensions;
using System.Xml.Serialization;
using GVUZ.Model.Entrants.Documents;
using GVUZ.ImportService2016.Core.Main.Dictionaries.Olympic;

namespace GVUZ.ImportService2016.Core.Dto.Import
{

    class Import
    {
    }


    public class ImportBase
    {
        private Guid guid;
        /// <summary>
        /// Уникальный GUID записи. Если исходно не было, то в первый раз генериться новый
        /// </summary>
        public Guid GUID
        {
            get
            {
                if (guid == null || guid.Equals(Guid.Empty)) guid = Guid.NewGuid();
                return guid;
            }
            set { guid = value; }
        }

        public virtual int ID { get; set; }
    }

    public partial class PackageData
    {
        public int InstitutionId { get; set; }
        public int ImportPackageId { get; set; }

        /// <summary>
        /// Тип пакета, GVUZ.ServiceModel.Import.Core.Packages.PackageType
        /// </summary>
        public int PackageType { get; set; }

        /// <summary>
        /// Заявления, прошедшие все проверки. Во всех DataReader'ах импользовать это вместо PackageData.Applications!
        /// </summary>
        public List<PackageDataApplication> ApplicationsToImport()
        {
            if (GetApplications != null)
                return GetApplications.Where(t => !t.IsBroken && !t.ShortUpdate).ToList();
            else
                return new List<PackageDataApplication>();
        }

        public List<PackageDataApplication> ApplicationsToShortImport()
        {
            if (GetApplications != null)
                return GetApplications.Where(t => !t.IsBroken && t.ShortUpdate).ToList();
            else
                return new List<PackageDataApplication>();
        }

        public List<PackageDataCampaignInfoCampaign> CampaignsToImport()
        {
            if (CampaignInfo != null && CampaignInfo.Campaigns != null)
                return CampaignInfo.Campaigns.Where(t => !t.IsBroken).ToList();
            else return new List<PackageDataCampaignInfoCampaign>();
        }

        public List<PackageDataInstitutionAchievement> InstitutionAchievementsToImport()
        {
            if (InstitutionAchievements != null)
                return InstitutionAchievements.Where(t => !t.IsBroken).ToList();
            //return InstitutionAchievements.ToList();
            else return new List<PackageDataInstitutionAchievement>();
        }

        public List<PackageDataAdmissionInfoItem> AdmissionVolumesToImport()
        {
            if (AdmissionInfo == null || AdmissionInfo.AdmissionVolume == null)
                return new List<PackageDataAdmissionInfoItem>();
            else
                return AdmissionInfo.AdmissionVolume.Where(t => !t.IsBroken).ToList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IsPlan">true - план, false - факт</param>
        /// <returns></returns>
        public List<PackageDataAdmissionInfoItem1> DistributedAdmissionVolumesToImport() 
        {
            if (AdmissionInfo == null || AdmissionInfo.DistributedAdmissionVolume == null)
                return new List<PackageDataAdmissionInfoItem1>();
            else
                return AdmissionInfo.DistributedAdmissionVolume.Where(t => !t.IsBroken).ToList(); // t.IsPlan == IsPlan &&
        }

        public List<PackageDataAdmissionInfoCompetitiveGroup> CompetitiveGroupsToImport()
        {
            if (AdmissionInfo == null || AdmissionInfo.CompetitiveGroups == null)
                return new List<PackageDataAdmissionInfoCompetitiveGroup>();
            else
                return AdmissionInfo.CompetitiveGroups.Where(t => !t.IsBroken).ToList();
        }

        public List<PackageDataOrdersOrderOfAdmission> OrderOfAdmissionsToImport()
        {
            if ((Orders != null) && (Orders.OrdersOfAdmission != null))
                return Orders.OrdersOfAdmission.Where(t => !t.IsBroken).ToList();
            else
                return new List<PackageDataOrdersOrderOfAdmission>();
        }
        public List<PackageDataOrdersOrderOfException> OrderOfExceptionToImport()
        {
            if ((Orders != null) && (Orders.OrdersOfException != null))
                return Orders.OrdersOfException.Where(t => !t.IsBroken).ToList();
            else
                return new List<PackageDataOrdersOrderOfException>();
        }

        public List<PackageDataOrdersApplication> OrderApplicationsToImport()
        {
            if ((Orders != null) && (Orders.Applications != null))
                return Orders.Applications.Where(t => !t.IsBroken).ToList();
            else
                return new List<PackageDataOrdersApplication>();
        }

        public List<PackageDataTargetOrganization> TargetOrganizationsToImport()
        {
            if (TargetOrganizations != null)
                return TargetOrganizations.Where(t => !t.IsBroken).ToList();
            else
                return new List<PackageDataTargetOrganization>();
        }

        public List<PackageDataInstitutionProgram> InstitutionProgramToImport()
        {
            if (InstitutionPrograms != null)
                return InstitutionPrograms.Where(t => !t.IsBroken).ToList();
            else
                return new List<PackageDataInstitutionProgram>();
        }

        #region Для импорта 1 заявления!
        private PackageDataApplication applicationField;

        /// <remarks/>
        public PackageDataApplication Application
        {
            get
            {
                return this.applicationField;
            }
            set
            {
                this.applicationField = value;
            }
        }

        /// <summary>
        /// Получение списка всех заявлений (для импорта как одного, так и многих заявлений
        /// (Использовать везде вместо PackageData.Applications!
        /// </summary>
        /// <returns></returns>
        public PackageDataApplication[] GetApplications
        {
            get
            {
                if (this.Applications != null)
                    return this.Applications;
                else if (this.applicationField != null)
                    return new PackageDataApplication[] { this.applicationField };
                else
                    return null;
            }
        }

        #endregion
    }

    public interface IBroken : IUid
    {
        bool IsBroken { get; set; }
    }

    #region Application
    [Description("Заявление")]
    public partial class PackageDataApplication : ImportBase, IBroken
    {
        private bool isBroken = false;
        /// <summary>
        /// Запись имеет конфликты, не надо ее импортировать
        /// </summary>
        public bool IsBroken
        {
            get { return isBroken; }
            set { isBroken = value; }
        }

        public string EntrantUID
        {
            get { return this.Entrant != null ? this.Entrant.UID : null; }
        }
        public int EntrantID { get { return this.Entrant != null ? this.Entrant.ID : 0; } }

        public List<PackageDataApplicationApplicationCommonBenefit> getAllBenefits()
        {
            var res = new List<PackageDataApplicationApplicationCommonBenefit>();
            if (ApplicationCommonBenefits != null)
                res.AddRange(ApplicationCommonBenefits);

            return res;
        }

        public List<CompetitiveGroupVocDto> SelectedCompetitiveGroupsFull { get; set; }
        //public List<CompetitiveGroupItemDict> SelectedCompetitiveGroupItemsFull { get; set; }

        public bool ShortUpdate { get; set; }
    }

    //public class BaseDict
    //{
    //    public string UID { get; set; }
    //    public int ID { get; set; }
    //}
    //public class CompetitiveGroupDict : BaseDict
    //{
    //    public CompetitiveGroupVocDto CompetitiveGroup { get; set; }
    //}

    //public class CompetitiveGroupItemDict : BaseDict
    //{
    //    public int CompetitiveGroupID { get; set; }
    //    public CompetitiveGroupItemVocDto CompetitiveGroupItem { get; set; }
    //}

    public partial class PackageDataApplicationEntrant : ImportBase
    {
        public string Email { get; set; }
        public int RegionID { get; set; }
        public int TownTypeID { get; set; }
        public string Address { get; set; }

        public DateTime? BirthDate { get; set; }
    }

    public partial class PackageDataApplicationApplicationDocumentsIdentityDocument : ImportBase, IEquatable<PackageDataApplicationApplicationDocumentsIdentityDocument>, IBaseDocument, IIdentityDocument
    {
        public Model.Entrants.Documents.EntrantDocumentType EntrantDocumentType
        {
            get { return Model.Entrants.Documents.EntrantDocumentType.IdentityDocument; }
        }


        #region IEquatable<ApplicationBulkEntity> Members

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as PackageDataApplicationApplicationDocumentsIdentityDocument);
        }

        public bool Equals(PackageDataApplicationApplicationDocumentsIdentityDocument other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            bool result = true;
            if (DocumentSeries != null) result &= other.DocumentSeries.Equals(DocumentSeries);
            /*if (IdentityDocumentTypeID != null)*/
            result &= other.IdentityDocumentTypeID.Equals(IdentityDocumentTypeID);
            /*if (NationalityTypeID != null)*/
            result &= other.NationalityTypeID.Equals(NationalityTypeID);
            if (BirthDate != null) result &= other.BirthDate.Equals(BirthDate);
            if (BirthPlace != null) result &= other.BirthPlace.Equals(BirthPlace);
            if (SubdivisionCode != null) result &= other.SubdivisionCode.Equals(SubdivisionCode);
            return result;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = 17;
                result = result * 37 + (DocumentSeries != null ? DocumentSeries.GetHashCode() : 0);
                result = result * 37 + (IdentityDocumentTypeID != 0 ? IdentityDocumentTypeID.GetHashCode() : 0);
                result = result * 37 + (NationalityTypeID != 0 ? NationalityTypeID.GetHashCode() : 0);
                result = result * 37 + (BirthDate != null ? BirthDate.GetHashCode() : 0);
                result = result * 37 + (BirthPlace != null ? BirthPlace.GetHashCode() : 0);
                result = result * 37 + (SubdivisionCode != null ? SubdivisionCode.GetHashCode() : 0);
                return result;
            }
        }

        #endregion
    }

    public partial class PackageDataApplicationApplicationDocumentsIdentityDocument1 : ImportBase, IEquatable<PackageDataApplicationApplicationDocumentsIdentityDocument>, IBaseDocument, IIdentityDocument
    {
        public Model.Entrants.Documents.EntrantDocumentType EntrantDocumentType
        {
            get { return Model.Entrants.Documents.EntrantDocumentType.IdentityDocument; }
        }


        #region IEquatable<ApplicationBulkEntity> Members

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as PackageDataApplicationApplicationDocumentsIdentityDocument);
        }

        public bool Equals(PackageDataApplicationApplicationDocumentsIdentityDocument other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            bool result = true;
            if (DocumentSeries != null) result &= other.DocumentSeries.Equals(DocumentSeries);
            /*if (IdentityDocumentTypeID != null)*/
            result &= other.IdentityDocumentTypeID.Equals(IdentityDocumentTypeID);
            /*if (NationalityTypeID != null)*/
            result &= other.NationalityTypeID.Equals(NationalityTypeID);
            if (BirthDate != null) result &= other.BirthDate.Equals(BirthDate);
            if (BirthPlace != null) result &= other.BirthPlace.Equals(BirthPlace);
            if (SubdivisionCode != null) result &= other.SubdivisionCode.Equals(SubdivisionCode);
            return result;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = 17;
                result = result * 37 + (DocumentSeries != null ? DocumentSeries.GetHashCode() : 0);
                result = result * 37 + (IdentityDocumentTypeID != 0 ? IdentityDocumentTypeID.GetHashCode() : 0);
                result = result * 37 + (NationalityTypeID != 0 ? NationalityTypeID.GetHashCode() : 0);
                result = result * 37 + (BirthDate != null ? BirthDate.GetHashCode() : 0);
                result = result * 37 + (BirthPlace != null ? BirthPlace.GetHashCode() : 0);
                result = result * 37 + (SubdivisionCode != null ? SubdivisionCode.GetHashCode() : 0);
                return result;
            }
        }

        #endregion
    }

    public interface IIdentityDocument : IBaseDocument
    {
        string LastName { get; set; }
        string FirstName { get; set; }
        string MiddleName { get; set; }
        uint GenderID { get; set; }
        bool GenderIDSpecified { get; set; }
        string SubdivisionCode { get; set; }

        uint IdentityDocumentTypeID { get; set; }
        uint NationalityTypeID { get; set; }
        DateTime BirthDate { get; set; }
        string BirthPlace { get; set; }
        //int ReleaseCountryID { get; set; }
        //string ReleasePlace { get; set; }
    }




    #region Documents
    public partial class PackageDataApplicationApplicationDocuments
    {
        public List<IBaseDocument> AllDocuments()
        {
            var res = new List<IBaseDocument>();
            if (this.CompatriotDocuments != null) res.AddRange(this.CompatriotDocuments);
            if (this.CustomDocuments != null) res.AddRange(this.CustomDocuments);
            if (this.EduDocuments != null) res.AddRange(this.EduDocuments.Select(t => t.Item as IBaseDocument).Where(t => t != null));
            if (this.EgeDocuments != null) res.AddRange(this.EgeDocuments);
            if (this.GiaDocuments != null) res.AddRange(this.GiaDocuments);
            if (this.IdentityDocument != null) res.Add(this.IdentityDocument);
            if (this.MilitaryCardDocument != null) res.Add(this.MilitaryCardDocument);
            if (this.OrphanDocuments != null) res.AddRange(this.OrphanDocuments);
            if (this.OtherIdentityDocuments != null) res.AddRange(this.OtherIdentityDocuments);
            if (this.SportDocuments != null) res.AddRange(this.SportDocuments);
            if (this.StudentDocument != null) res.Add(this.StudentDocument);
            if (this.VeteranDocuments != null) res.AddRange(this.VeteranDocuments);
            if (this.PauperDocuments != null) res.AddRange(this.PauperDocuments);
            if (this.ParentsLostDocuments != null) res.AddRange(this.ParentsLostDocuments);
            if (this.StateEmployeeDocuments != null) res.AddRange(this.StateEmployeeDocuments);
            if (this.RadiationWorkDocuments != null) res.AddRange(this.RadiationWorkDocuments);

            return res;
        }
    }

    public partial class TCustomDocument : ImportBase, IBaseDocument
    {
        public Model.Entrants.Documents.EntrantDocumentType EntrantDocumentType
        {
            get { return Model.Entrants.Documents.EntrantDocumentType.CustomDocument; }
        }
    }



    public interface IOlympicDocument
    {
        int OlympicTypeProfileID { get; set; }
        uint DiplomaTypeID { get; set; }
        uint OlympicID { get; set; }
        uint ClassNumber { get; set; }
    }
    public partial class TOlympicDocument : ImportBase, IBaseDocument, IOlympicDocument
    {
        public Model.Entrants.Documents.EntrantDocumentType EntrantDocumentType
        {
            get { return Model.Entrants.Documents.EntrantDocumentType.OlympicDocument; }
        }


        public string DocumentOrganization { get { return ""; } set { } }

        public int OlympicTypeProfileID { get; set; }
    }
    public partial class TOlympicTotalDocument : ImportBase, IBaseDocument, IOlympicDocument
    {
        public Model.Entrants.Documents.EntrantDocumentType EntrantDocumentType
        {
            get { return Model.Entrants.Documents.EntrantDocumentType.OlympicTotalDocument; }
        }

        public DateTime DocumentDate { get { return DateTime.MinValue; } set { } }
        public string DocumentOrganization { get { return ""; } set { } }

        public int OlympicTypeProfileID { get; set; } 
    }

    public partial class TInternationalOlympic : ImportBase, IBaseDocument
    {
        public Model.Entrants.Documents.EntrantDocumentType EntrantDocumentType
        {
            get { return Model.Entrants.Documents.EntrantDocumentType.InternationalOlympic; }
        }

        public DateTime DocumentDate { get { return DateTime.MinValue; } set { } }
        public string DocumentOrganization { get { return ""; } set { } }
    }
    public partial class TOrphanDocument : ImportBase, IBaseDocument
    {
        public Model.Entrants.Documents.EntrantDocumentType EntrantDocumentType
        {
            get { return Model.Entrants.Documents.EntrantDocumentType.OrphanDocument; }
        }
    }

    public partial class TVeteranDocument : ImportBase, IBaseDocument
    {
        public Model.Entrants.Documents.EntrantDocumentType EntrantDocumentType
        {
            get { return Model.Entrants.Documents.EntrantDocumentType.VeteranDocument; }
        }
    }

    public partial class TSportDocument : ImportBase, IBaseDocument
    {
        public Model.Entrants.Documents.EntrantDocumentType EntrantDocumentType
        {
            get { return (Model.Entrants.Documents.EntrantDocumentType)SportCategoryID; }
        }
    }

    public partial class TUkraineOlympic : ImportBase, IBaseDocument
    {
        public Model.Entrants.Documents.EntrantDocumentType EntrantDocumentType
        {
            get { return Model.Entrants.Documents.EntrantDocumentType.UkraineOlympic; }
        }

        public DateTime DocumentDate { get { return DateTime.MinValue; } set { } }
        public string DocumentOrganization { get { return ""; } set { } }
    }

    public partial class TCompatriotDocument : ImportBase, IBaseDocument
    {
        public Model.Entrants.Documents.EntrantDocumentType EntrantDocumentType
        {
            get { return Model.Entrants.Documents.EntrantDocumentType.CompatriotDocument; }
        }

        public uint CompatriotCategoryID
        {
            get
            {
                return CompariotCategoryID;
            }
            set
            {
                CompariotCategoryID = value;
            }
        }

    }

    public partial class TPauperDocument : ImportBase, IBaseDocument
    {
        public Model.Entrants.Documents.EntrantDocumentType EntrantDocumentType
        {
            get { return Model.Entrants.Documents.EntrantDocumentType.PauperDocument; }
        }
    }

    public partial class TParentsLostDocument : ImportBase, IBaseDocument
    {
        public Model.Entrants.Documents.EntrantDocumentType EntrantDocumentType
        {
            get { return Model.Entrants.Documents.EntrantDocumentType.ParentsLostDocument; }
        }
    }

    public partial class TStateEmployeeDocument : ImportBase, IBaseDocument
    {
        public Model.Entrants.Documents.EntrantDocumentType EntrantDocumentType
        {
            get { return Model.Entrants.Documents.EntrantDocumentType.StateEmployeeDocument; }
        }
    }

    public partial class TRadiationWorkDocument : ImportBase, IBaseDocument
    {
        public Model.Entrants.Documents.EntrantDocumentType EntrantDocumentType
        {
            get { return Model.Entrants.Documents.EntrantDocumentType.RadiationWorkDocument; }
        }
    }

    public partial class TAcademicDiplomaDocument : ImportBase, IEduDocument
    {
        public GVUZ.Model.Entrants.Documents.EntrantDocumentType EntrantDocumentType { get { return Model.Entrants.Documents.EntrantDocumentType.AcademicDiplomaDocument; } }
        public uint EndYear { get { return 0; } set { } }
        public float GPA { get { return 0; } set { } }
        ushort IEduDocument.SpecializationID
        {
            get
            {
                return (ushort)this.SpecialityID;
            }

            set
            {
                this.SpecialityID = (uint)value;
            }
        }
    }
    public partial class TBasicDiplomaDocument : ImportBase, IEduDocument
    {
        public GVUZ.Model.Entrants.Documents.EntrantDocumentType EntrantDocumentType { get { return Model.Entrants.Documents.EntrantDocumentType.BasicDiplomaDocument; } }
        //public uint SpecialityID { get { return 0; } set { } }
        public ushort SpecializationID { get { return 0; } set { } }
    }
    public partial class TEduCustomDocument : ImportBase, IEduDocument
    {
        public GVUZ.Model.Entrants.Documents.EntrantDocumentType EntrantDocumentType { get { return Model.Entrants.Documents.EntrantDocumentType.EduCustomDocument; } }

        public uint QualificationTypeID { get { return 0; } set { } }
        public string RegistrationNumber { get { return ""; } set { } }
        public uint SpecialityID { get { return 0; } set { } }
        public ushort SpecializationID { get { return 0; } set { } }
        public uint EndYear { get { return 0; } set { } }
        public float GPA { get { return 0; } set { } }
        public bool? IsNostrificated { get { return false; } set { } }
    }
    public partial class THighEduDiplomaDocument : ImportBase, IEduDocument
    {
        public GVUZ.Model.Entrants.Documents.EntrantDocumentType EntrantDocumentType { get { return Model.Entrants.Documents.EntrantDocumentType.HighEduDiplomaDocument; } }
    }
    public partial class TIncomplHighEduDiplomaDocument : ImportBase, IEduDocument
    {
        public GVUZ.Model.Entrants.Documents.EntrantDocumentType EntrantDocumentType { get { return Model.Entrants.Documents.EntrantDocumentType.IncomplHighEduDiplomaDocument; } }
        public uint EndYear { get { return 0; } set { } }
        public float GPA { get { return 0; } set { } }
    }
    public partial class TMiddleEduDiplomaDocument : ImportBase, IEduDocument
    {
        public GVUZ.Model.Entrants.Documents.EntrantDocumentType EntrantDocumentType { get { return Model.Entrants.Documents.EntrantDocumentType.MiddleEduDiplomaDocument; } }
    }
    public partial class TPhDDiplomaDocument : ImportBase, IEduDocument
    {
        public GVUZ.Model.Entrants.Documents.EntrantDocumentType EntrantDocumentType { get { return Model.Entrants.Documents.EntrantDocumentType.PhDDiplomaDocument; } }
    }
    public partial class TPostGraduateDiplomaDocument : ImportBase, IEduDocument
    {
        public GVUZ.Model.Entrants.Documents.EntrantDocumentType EntrantDocumentType { get { return Model.Entrants.Documents.EntrantDocumentType.PostGraduateDiplomaDocument; } }
    }

    public partial class TSchoolCertificateDocument : ImportBase, IEduDocument
    {
        public GVUZ.Model.Entrants.Documents.EntrantDocumentType EntrantDocumentType
        {
            get { return Model.Entrants.Documents.EntrantDocumentType.SchoolCertificateDocument; }
        }
        public uint QualificationTypeID { get { return 0; } set { } }
        public string RegistrationNumber { get { return ""; } set { } }
        public uint SpecialityID { get { return 0; } set { } }
        public ushort SpecializationID { get { return 0; } set { } }
    }
    public partial class TSchoolCertificateDocumentSubjectData : ISubject { }

    public interface IApplicationDocument : IBaseDocument
    {
        string AdditionalInfo { get; set; }
        string DiplomaTypeID { get; set; }
        string DisabilityTypeID { get; set; }
        //string DocumentSeries { get; set; }
        string DocumentTypeNameText { get; set; }
        string EndYear { get; set; }
        string OlympicDate { get; set; }
        string OlympicID { get; set; }
        string LevelID { get; set; }
        string OlympicPlace { get; set; }
        string ProfessionID { get; set; }
        string QualificationTypeID { get; set; }
        string RegistrationNumber { get; set; }
        string SpecialityID { get; set; }
        string SpecializationID { get; set; }

        //public string DecisionNumber;
        //public string DecisionDate;
        string SubjectID { get; set; }

        string SubjectName { get; set; }

        //[XmlArrayItem(ElementName = "SubjectBriefData")]
        //public SubjectBriefDataDto[] Subjects =
        //    new SubjectBriefDataDto[0];

        string Value { get; set; }
        float? GPA { get; set; }
    }

    public interface IBaseDocument : IUid
    {
        //string UID { get; set; }
        DateTime DocumentDate { get; set; }
        string DocumentNumber { get; set; }
        string DocumentSeries { get; set; }
        string DocumentOrganization { get; set; }
        bool OriginalReceivedDateSpecified { get; set; }
        DateTime OriginalReceivedDate { get; set; }
        GVUZ.Model.Entrants.Documents.EntrantDocumentType EntrantDocumentType { get; }

        Guid GUID { get; set; }
        int ID { get; set; }
    }
    public interface IEduDocument : IBaseDocument
    {
        uint QualificationTypeID { get; set; }
        string RegistrationNumber { get; set; }
        uint SpecialityID { get; set; }
        ushort SpecializationID { get; set; }
        uint EndYear { get; set; }
        float GPA { get; set; } 
        bool? IsNostrificated { get; set; }
        string FacultyName { get; set; }
        string InstitutionAddress { get; set; }
        DateTime BeginDate { get; set; }
        DateTime EndDate { get; set; }
        byte? EducationFormID { get; set; }
        string SpecialityName { get; set; }
        string QualificationName { get; set; }
        int CountryID { get; set; }
    }


    public partial class PackageDataApplicationApplicationDocumentsEgeDocument : ImportBase, IBaseDocument
    {
        public string DocumentSeries { get { return ""; } set { } }

        public string DocumentOrganization { get { return ""; } set { } }

        public Model.Entrants.Documents.EntrantDocumentType EntrantDocumentType
        {
            get { return Model.Entrants.Documents.EntrantDocumentType.EgeDocument; }
        }
    }

    public partial class PackageDataApplicationApplicationDocumentsGiaDocument : ImportBase, IBaseDocument
    {
        public string DocumentSeries { get { return ""; } set { } }

        public Model.Entrants.Documents.EntrantDocumentType EntrantDocumentType
        {
            get { return Model.Entrants.Documents.EntrantDocumentType.GiaDocument; }
        }
    }



    public partial class PackageDataApplicationApplicationDocumentsMilitaryCardDocument : ImportBase, IBaseDocument
    {
        public Model.Entrants.Documents.EntrantDocumentType EntrantDocumentType
        {
            get { return Model.Entrants.Documents.EntrantDocumentType.MilitaryCardDocument; }
        }
    }

    public partial class PackageDataApplicationApplicationDocumentsStudentDocument : ImportBase, IBaseDocument
    {
        public Model.Entrants.Documents.EntrantDocumentType EntrantDocumentType
        {
            get { return Model.Entrants.Documents.EntrantDocumentType.StudentDocument; }
        }

        public string DocumentSeries { get { return ""; } set { } }

        public bool OriginalReceivedDateSpecified { get { return true; } set { } }

        public DateTime OriginalReceivedDate { get { return DateTime.MinValue; } set { } }

    }



    public interface ISubject
    {
        uint SubjectID { get; set; }
        uint Value { get; set; }
    }
    public partial class PackageDataApplicationApplicationDocumentsGiaDocumentSubjectData : ISubject { }
    public partial class PackageDataApplicationApplicationDocumentsEgeDocumentSubjectData : ISubject { }

    //#warning XSD 2016
    //public partial class TOlympicTotalDocumentSubjectBriefData : ISubject
    //{
    //    public uint Value { get { return 0; } set { } }
    //}

    public partial class TAllowEducationDocument : ImportBase, IBaseDocument
    {
        public string DocumentSeries { get { return ""; } set { } }
        public Model.Entrants.Documents.EntrantDocumentType EntrantDocumentType
        {
            get { return Model.Entrants.Documents.EntrantDocumentType.AllowEducationDocument; }
        }
    }

    public partial class TDisabilityDocument : ImportBase, IBaseDocument
    {
        public Model.Entrants.Documents.EntrantDocumentType EntrantDocumentType
        {
            get { return Model.Entrants.Documents.EntrantDocumentType.DisabilityDocument; }
        }
    }
    public partial class TMedicalDocument : ImportBase, IBaseDocument
    {
        public string DocumentSeries { get { return ""; } set { } }
        public Model.Entrants.Documents.EntrantDocumentType EntrantDocumentType
        {
            get { return Model.Entrants.Documents.EntrantDocumentType.MedicalDocument; }
        }
    }
    #endregion

    public interface IUid
    {
        string UID { get; set; }
    }

    public partial class PackageDataApplicationEntranceTestResult : IUid
    {
        public CompetitiveGroupVocDto CompetitiveGroupDict { get; set; }
        public EntranceTestItemCVocDto EntranceTestItemC { get; set; }
    }
    public partial class PackageDataApplicationApplicationCommonBenefit : IUid
    {
        public CompetitiveGroupVocDto CompetitiveGroupDict { get; set; }
    }
    public partial class PackageDataApplicationFinSourceEduForm
    {
        private bool isBroken = false;
        /// <summary>
        /// Запись имеет конфликты, не надо ее импортировать
        /// </summary>
        public bool IsBroken
        {
            get { return isBroken; }
            set { isBroken = value; }
        }

        public int CompetitiveGroupID { get; set; }
        //public CompetitiveGroupDict CompetitiveGroupDict { get; set; }
        //public CompetitiveGroupItemDict CompetitiveGroupItemDict { get; set; }
    }

    public partial class PackageDataApplicationIndividualAchievement
    {
        public int? EntrantDocumentID { get; set; }
        public int? InstitutionAchievementID { get; set; }
    }

    public partial class PackageDataApplicationEntrantCitizenship
    {
        public int? CountryID { get; set; }
    }
    #endregion Application

    #region Campaign


    public interface ICourseEdLevel
    {
        //uint Course { get; set; }
        uint EducationLevelID { get; set; }
    }

    [Description("Приемная кампания")]
    public partial class PackageDataCampaignInfoCampaign : ImportBase, IBroken, IUid
    {
        private bool isBroken = false;
        /// <summary>
        /// Запись имеет конфликты, не надо ее импортировать
        /// </summary>
        public bool IsBroken
        {
            get { return isBroken; }
            set { isBroken = value; }
        }

        public int EducationFormFlag()
        {
            int res = 0;
            if (EducationForms != null)
            {

                if (EducationForms.Contains((uint)GVUZ.Model.Institutions.EDFormsConst.O))
                    res |= 1;
                if (EducationForms.Contains((uint)GVUZ.Model.Institutions.EDFormsConst.OZ))
                    res |= 2;
                if (EducationForms.Contains((uint)GVUZ.Model.Institutions.EDFormsConst.Z))
                    res |= 4;
            }
            return res;
        }
    }

    //public partial class PackageDataCampaignInfoCampaignCampaignDate : ImportBase, IUid, ICourseEdLevel { }

    //public partial class PackageDataCampaignInfoCampaignEducationLevel : ICourseEdLevel { }

    #endregion Campaign

    #region TargetOrganizations
    [Description("Целевая организация")]
    public partial class PackageDataTargetOrganization : ImportBase, IBroken, IUid
    {
        private bool isBroken = false;
        /// <summary>
        /// Запись имеет конфликты, не надо ее импортировать
        /// </summary>
        public bool IsBroken
        {
            get { return isBroken; }
            set { isBroken = value; }
        }
        
    }

    #endregion

    #region InstitutionPrograms
    [Description("Образовательная программа")]
    public partial class PackageDataInstitutionProgram : ImportBase, IBroken, IUid
    {
        private bool isBroken = false;
        /// <summary>
        /// Запись имеет конфликты, не надо ее импортировать
        /// </summary>
        public bool IsBroken
        {
            get { return isBroken; }
            set { isBroken = value; }
        }
    }

    #endregion

    #region AdmissionInfo

    public partial class PackageDataAdmissionInfo
    {

        //public List<Tuple<int, string>> ObjectsToDelete = new List<Tuple<int, string>>();


        //public void AdmissionVolumeDeletion(AdmissionVolumeVocDto dto)
        //{
        //    ObjectsToDelete.Add(new Tuple<int, string>(dto.ID, "AdmissionVolume"));
        //}

        //public void CompetitiveGroupDeletion(CompetitiveGroupVocDto dto)
        //{
        //    ObjectsToDelete.Add(new Tuple<int, string>(dto.ID, "CompetitiveGroup"));
        //}

        //public void DeleteObject(VocabularyBaseDto dto)
        //{
        //    ObjectsToDelete.Add(new Tuple<int, string>(dto.ID, dto.GetType().Name));
        //}
    }

    [Description("Объем приема")]
    public partial class PackageDataAdmissionInfoItem : ImportBase, IBroken, IUid
    {
        private bool isBroken = false;
        /// <summary>
        /// Запись имеет конфликты, не надо ее импортировать
        /// </summary>
        public bool IsBroken
        {
            get { return isBroken; }
            set { isBroken = value; }
        }

        //public int CampaignID { get; set; }
    }

    [Description("Распределенный объем приема")]
    public partial class PackageDataAdmissionInfoItem1 : ImportBase, IBroken //, IUid
    {
        private bool isBroken = false;
        /// <summary>
        /// Запись имеет конфликты, не надо ее импортировать
        /// </summary>
        public bool IsBroken
        {
            get { return isBroken; }
            set { isBroken = value; }
        }

        public string Key() { return AdmissionVolumeUID + "||" + LevelBudget.ToString() + "||" + (IsPlan ? "1" : "0"); }

        // Не используется, просто для корректного наследования. Рефакторинг?
        public string UID { get; set; }

        public int AdmissionVolumeID { get; set; }
        public Guid AdmissionVolumeGUID { get; set; }
    }

    [Description("Конкурс")]
    public partial class PackageDataAdmissionInfoCompetitiveGroup : ImportBase, IBroken, IUid
    {
        private bool isBroken = false;
        /// <summary>
        /// Запись имеет конфликты, не надо ее импортировать
        /// </summary>
        public bool IsBroken
        {
            get { return isBroken; }
            set { isBroken = value; }
        }

        public bool UseAnyDirectionsFilter { get; set; }

        public int CampaignID { get; set; }
        //public int DirectionFilterType { get; set; }
    }

    [Description("Направление конкурса")]
    public partial class PackageDataAdmissionInfoCompetitiveGroupCompetitiveGroupItem : ImportBase //, IUid  
    {
        public Guid ParentID { get; set; }

    }

    [Description("Программа обучения")]
    public partial class PackageDataAdmissionInfoCompetitiveGroupEduProgram : ImportBase, IUid
    {
        public Guid ParentID { get; set; }
        public int InstitutionProgramID { get; set; }
    }

    public partial class PackageDataAdmissionInfoCompetitiveGroupTargetOrganization : ImportBase
    {
        public Guid ParentID { get; set; }
    }

    [Description("Места для целевого приема")]
    public partial class PackageDataAdmissionInfoCompetitiveGroupTargetOrganizationCompetitiveGroupTargetItem : ImportBase //, IUid
    {
        public int TargetID { get; set; }
        public int CompetitiveGroupID { get; set; }
        public Guid CompetitiveGroupGUID { get; set; }
    }

    [Description("Вступительные испытания конкурса")]
    public partial class PackageDataAdmissionInfoCompetitiveGroupEntranceTestItem : ImportBase, IUid
    {
        public Guid ParentID { get; set; }

        public int SubjectID { get; set; }
        public string SubjectName { get; set; }
        public bool IsFirst { get; set; }
        public bool IsSecond { get; set; }

    }


    public interface IBenefitItemOlympic
    {
        uint OlympicID { get; set; }
        uint LevelID { get; set; }
        bool LevelIDSpecified { get; set; }
        uint[] Profiles { get; set; }
        uint ClassID { get; set; }

        int LevelFlag { get; }
    }

    public partial class PackageDataAdmissionInfoCompetitiveGroupCommonBenefitItemOlympic : IBenefitItemOlympic
    {
        public int LevelFlag
        {
            get
            {
                return this.LevelID.To(0);
                // Справочник 3 = справочник 19, спросите у Н.С.
                //switch (this.LevelID)
                //{
                //    case 2: return 1;
                //    case 3: return 2;
                //    case 4: return 4;
                //    default: return OlympicTypeVoc.All_Olympic_Level;
                //}
            }


        }
    }
    public partial class PackageDataAdmissionInfoCompetitiveGroupEntranceTestItemEntranceTestBenefitItemOlympic : IBenefitItemOlympic
    {
        public int LevelFlag
        {
            get
            {
                return this.LevelID.To(0);
                // Справочник 3 = справочник 19, спросите у Н.С.
                //switch (this.LevelID)
                //{
                //    case 2: return 1;
                //    case 3: return 2;
                //    case 4: return 4;
                //    default: return OlympicTypeVoc.All_Olympic_Level;
                //}
            }
        }
    }


    public interface ICommonBenefitItem
    {
        uint[] OlympicDiplomTypes { get; set; }
        bool IsForAllOlympics { get; set; }
        uint BenefitKindID { get; set; }

        int OlympicYear { get; }

        uint LevelForAllOlympics { get; set; }
        bool LevelForAllOlympicsSpecified { get; set; }
        uint ClassForAllOlympics { get; set; }
        bool ClassForAllOlympicsSpecified { get; set; }

        uint[] ProfileForAllOlympics { get; set; }

        uint[] Olympics { get; set; }

        IBenefitItemOlympic[] BenefitItemOlympicsLevels { get; }

        int LevelForAllOlympicsFlag { get; }

        /// <summary>
        /// Содержит только ВсОШ'и
        /// </summary>
        bool IsVsosh { get; }

    }
    public abstract class BenefitItemImportBase : ImportBase
    {
        public Guid ParentCompetitiveGroup { get; set; }
        public Guid ParentEntranceTestItem { get; set; }

        public int OlympicYear { get { return DateTime.Now.Year; } }
        public bool IsProfileSubject { get; set; }

    }

    [Description("Льгота")]
    public partial class PackageDataAdmissionInfoCompetitiveGroupCommonBenefitItem : BenefitItemImportBase, IUid, ICommonBenefitItem
    {
        public int OlympicDiplomTypesParsed
        {
            get { return (OlympicDiplomTypes ?? new uint[0]).Aggregate(0, (i, s) => i | s.To(0)); }
        }

        [XmlIgnore()]
        public IBenefitItemOlympic[] BenefitItemOlympicsLevels
        {
            get
            {
                return this.OlympicsLevels;
            }
        }

        public int LevelForAllOlympicsFlag
        {
            get
            {
                return LevelForAllOlympics.To(0);

                //switch (this.LevelForAllOlympics)
                //{
                //    case 2: return 1;
                //    case 3: return 2;
                //    case 4: return 4;
                //    default: return OlympicTypeVoc.All_Olympic_Level;
                //}
            }
        }

        public bool IsVsosh
        {
            get
            {
                if (this.IsForAllOlympics) return false;
                if (this.Olympics != null)
                {
                    return this.Olympics.All(o => VocabularyStatic.OlympicTypeVoc.Items.Any(t => o == t.OlympicID && t.IsVshosh));
                }
                if (this.OlympicsLevels != null)
                {
                    return this.OlympicsLevels.All(o => VocabularyStatic.OlympicTypeVoc.Items.Any(t => o.OlympicID == t.OlympicID && t.IsVshosh));
                }
                return false;
            }
        }
    }



    [Description("Льгота")]
    public partial class PackageDataAdmissionInfoCompetitiveGroupEntranceTestItemEntranceTestBenefitItem : BenefitItemImportBase, IUid, ICommonBenefitItem
    {
        public int OlympicDiplomTypesParsed
        {
            get { return (OlympicDiplomTypes ?? new uint[0]).Aggregate(0, (i, s) => i | s.To(0)); }
        }

        [XmlIgnore()]
        public IBenefitItemOlympic[] BenefitItemOlympicsLevels
        {
            get
            {
                return this.OlympicsLevels;
            }
        }

        public int LevelForAllOlympicsFlag
        {
            get
            {
                return LevelForAllOlympics.To(0);
                //switch (this.LevelForAllOlympics)
                //{
                //    case 2: return 1;
                //    case 3: return 2;
                //    case 4: return 4;
                //    default: return OlympicTypeVoc.All_Olympic_Level;
                //}
            }
        }

        public bool IsVsosh
        {
            get
            {
                if (this.IsForAllOlympics) return false;
                if (this.Olympics != null)
                {
                    return this.Olympics.All(o => VocabularyStatic.OlympicTypeVoc.Items.Any(t => o == t.OlympicID && t.IsVshosh));
                }
                if (this.OlympicsLevels != null)
                {
                    return this.OlympicsLevels.All(o => VocabularyStatic.OlympicTypeVoc.Items.Any(t => o.OlympicID == t.OlympicID && t.IsVshosh));
                }
                return false;
            }
        }
    }

    public class CompetitiveGroupItemCustom
    {
        public PackageDataAdmissionInfoCompetitiveGroupCompetitiveGroupItem CompetitiveGroupItem { get; set; }
        public PackageDataAdmissionInfoCompetitiveGroup Parent { get; set; }
    }
    public class CompetitiveGroupTargetItemCustom
    {
        public PackageDataAdmissionInfoCompetitiveGroupTargetOrganizationCompetitiveGroupTargetItem CompetitiveGroupTargetItem { get; set; }
        public PackageDataAdmissionInfoCompetitiveGroup Parent { get; set; }
    }
    #endregion AdmissionInfo

    #region InstitutionArchievements
    [Description("Индивидуальные достижения, учитываемые ОО")]
    public partial class PackageDataInstitutionAchievement : ImportBase, IUid, IBroken
    {
        private bool isBroken = false;
        /// <summary>
        /// Запись имеет конфликты, не надо ее импортировать
        /// </summary>
        public bool IsBroken
        {
            get { return isBroken; }
            set { isBroken = value; }
        }

        public string Key() { return InstitutionAchievementUID + "||" + CampaignUID; }
        public int CampaignID { get; set; }

        public string UID
        {
            get { return InstitutionAchievementUID; }
            set { InstitutionAchievementUID = value; }
        }
    }
    #endregion

    #region OrderOfAdmission

    public abstract class PackageDataOrdersOrderBase : ImportBase, IBroken
    {
        private bool isBroken = false;
        /// <summary>
        /// Запись имеет конфликты, не надо ее импортировать
        /// </summary>
        public bool IsBroken
        {
            get { return isBroken; }
            set { isBroken = value; }
        }

        public int CampaignID { get; set; }

        //public int? Course { get; set; }

        //public int ApplicationCompetitiveGroupItemID { get; set; }

        public int OrderStatus { get; set; }
        public abstract string UID { get; set; }
    }

    public interface IOrder : IBroken
    {
        int ID { get; set; }
        Guid GUID { get; set; }
        string CampaignUID { get; set; }
        int CampaignID { get; set; }
        string OrderName { get; set; }
        string OrderNumber { get; set; }
        DateTime OrderDate { get; set; }
        bool OrderDateSpecified { get; set; }
        DateTime OrderDatePublished { get; set; }
        bool OrderDatePublishedSpecified { get; set; }
        uint EducationFormID { get; set; }
        bool EducationFormIDSpecified { get; set; }
        uint FinanceSourceID { get; set; }
        bool FinanceSourceIDSpecified { get; set; }
        uint EducationLevelID { get; set; }
        bool EducationLevelIDSpecified { get; set; }
        uint Stage { get; set; }
        bool StageSpecified { get; set; }
        int OrderStatus { get; set; }
    }

    [Description("Приказ о зачислении")]
    public partial class PackageDataOrdersOrderOfAdmission : PackageDataOrdersOrderBase, IOrder
    {
        public override string UID
        {
            get { return OrderOfAdmissionUID; }
            set { OrderOfAdmissionUID = value; }
        }
    }

    [Description("Приказ об исключении")]
    public partial class PackageDataOrdersOrderOfException : PackageDataOrdersOrderBase, IOrder
    {
        public override string UID
        {
            get { return OrderOfExceptionUID; }
            set { OrderOfExceptionUID = value; }
        }
    }

    [Description("Включение заявления в приказ")]
    public partial class PackageDataOrdersApplication : ImportBase, IBroken
    {
        private bool isBroken = false;
        /// <summary>
        /// Запись имеет конфликты, не надо ее импортировать
        /// </summary>
        public bool IsBroken
        {
            get { return isBroken; }
            set { isBroken = value; }
        }

        public string UID
        {
            get { return ApplicationUID; }
            set { ApplicationUID = value; }
        }

        public int OrderID { get; set; }
        public int ApplicationCompetitiveGroupItemID { get; set; }
    }

    //public partial class PackageDataOrderOfAdmissionApplication
    //{
    //    public int ApplicationID { get; set; }
    //}
    #endregion
}
