using GVUZ.DAL.Dapper.Repository.Interfaces.Dictionary;
using GVUZ.DAL.Dapper.Repository.Model.Dictionary;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GVUZ.DAL.Dapper.ViewModel.Dictionary
{
    public class AdmissionItemTypeView
    {
        public const int EducationFormType = 7;
        public const int EducationLevelType = 2;
        public const int EducationFinanceSourceType = 8;

        public int ItemTypeID { get; set; }
        public int ID { get { return ItemTypeID; } }

        [StringLength(100)]
        public string Name { get; set; }

        public short ItemLevel { get; set; }

        public bool CanBeSkipped { get; set; }

        public int DisplayOrder { get; set; }
    }

    public class AdmissionItemTypeConstants
    {
        public const short Institution = 0;
        public const short Course = 1;
        public const short Bachelor = 2;
        //public const short BachelorShort = 3;
        public const short Magistracy = 4;
        public const short Speciality = 5;
        public const short SPO = 17;
        public const short Faculty = 6;
        public const short Cathedra = 7;
        public const short PlacesType = 8;
        public const short Direction = 9;
        public const short PostalTuition = 10;
        public const short FullTimeTuition = 11;
        public const short MixedTuition = 12;
        public const short BudgetPlaces = 14;
        public const short PaidPlaces = 15;
        public const short TargetReception = 16;
        public const short HighQualification = 18;
        //public const short AppliedBachelor = 19;
        public const short Quota = 20;

        public const string FacultyName = "Факультет";
        public const string CathedraName = "Кафедра";
    }

    public class EdConst
    {
        IDictionaryRepository dictionaryRepository;
        public EdConst()
        {
            this.dictionaryRepository = new DictionaryRepository();
        }
        public EdConst(IDictionaryRepository dictionaryRepository)
        {
            this.dictionaryRepository = dictionaryRepository;
        }
        public string GetConstDescription(short value)
        {
            return dictionaryRepository.GetConstDescription(value);
        }
    }

    public class EDFormsConst : EdConst
    {
        public const short O = AdmissionItemTypeConstants.FullTimeTuition;
        public const short OZ = AdmissionItemTypeConstants.MixedTuition;
        public const short Z = AdmissionItemTypeConstants.PostalTuition;
    }

    public class EDSourceConst : EdConst
    {
        public const short Paid = AdmissionItemTypeConstants.PaidPlaces;
        public const short Budget = AdmissionItemTypeConstants.BudgetPlaces;
        public const short Target = AdmissionItemTypeConstants.TargetReception;
        public const short Quota = AdmissionItemTypeConstants.Quota;
    }

    public class EDLevelConst : EdConst
    {
        public const short Bachelor = AdmissionItemTypeConstants.Bachelor;
        //public const short BachelorShort = AdmissionItemTypeConstants.BachelorShort;
        public const short Magistracy = AdmissionItemTypeConstants.Magistracy;
        public const short Speciality = AdmissionItemTypeConstants.Speciality;
        public const short SPO = AdmissionItemTypeConstants.SPO;
        public const short HighQualification = AdmissionItemTypeConstants.HighQualification;
        //public const short AppliedBachelor = AdmissionItemTypeConstants.AppliedBachelor;
    }
}
