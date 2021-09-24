using System.Linq;

namespace GVUZ.Model.Institutions
{
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
        public static string GetConstDescription(short value)
        {
            using (var dbContext = new InstitutionsEntities())
            {
                var val = dbContext.AdmissionItemType.FirstOrDefault(x => x.ItemTypeID == value);
                return val == null ? string.Empty : val.Name;
            }
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