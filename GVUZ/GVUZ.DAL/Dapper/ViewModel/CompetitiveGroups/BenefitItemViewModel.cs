using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GVUZ.DAL.Dapper.ViewModel.CompetitiveGroups
{
    public class BenefitItemViewModel
    {
        public BenefitItemViewModel()
        {
        }

        public const int OLYMPIC_ALL = 255;
        public const int CLASS_ALL = 255;
        public const int CLASS_ALL_VSOSH = 28; // ВсОШ = только 9-11 классы!
        public const int PROFILE_ALL = 255;

        public int BenefitItemID { get; set; }
        public int EntranceTestItemID {get; set;}
        
        [DisplayName("Минимальный балл ЕГЭ, необходимый для использования льготы")]
        public int EgeMinValue { get; set; }

        [StringLength(200)]
        public string UID { get; set; }

        public short OlympicDiplomTypeID { get; set; }

        [DisplayName("Тип диплома")]
        public string DiplomType { get; set; }

        [DisplayName("Год олимпиады")]
        public int OlympicYear { get; set; }

        [DisplayName("Номер олимпиады из перечня")]
        public bool IsForAllOlympic { get; set; }

        public bool IsProfileSubject { get; set; }

        [DisplayName("Уровень олимпиады")]
        public int OlympicLevelFlags { get; set; }

        [DisplayName("Класс обучения")]
        public int ClassFlags { get; set; }

        public int BenefitID { get; set; }

        [DisplayName("Вид льготы")]
        public string BenefitName { get; set; }

        public bool IsCreative { get; set; }
        public bool IsAthletic { get; set; }

        public IEnumerable<BenefitItemSubjectViewModel> BenefitItemSubjects { get; set; }
        public IEnumerable<BenefitItemOlympicViewModel> BenefitItemOlympics { get; set; }
        public IEnumerable<BenefitItemProfileViewModel> BenefitItemProfiles { get; set; }
    }

    public class BenefitItemSubjectViewModel
    {
        public int ID { get; set; }
        public int SubjectID { get; set; }
        public int EgeMinValue { get; set; }
        public string SubjectName { get; set; }
        public bool IsEge { get; set; }
        public bool IsOlympic { get; set; }
    }

    public class BenefitItemOlympicViewModel
    {
        public int ID { get; set; }
        public int OlympicID { get; set; }
        public int OlympicNumber { get; set; }
        public string Name { get; set; }
        public short OlympicLevelFlags { get; set; }
        public short ClassFlags { get; set; }
        public int OlympicYear { get; set; }
        
        public string ProfileNames { get; set; }

        //public string ProfileNames
        //{
        //    get
        //    {
        //        var res = "";
        //        if (BenefitItemOlympicProfiles != null)
        //            res = string.Join(", ", BenefitItemOlympicProfiles.Select(t => t.ProfileName).ToArray());
        //        return res;
        //    }
        //}

        public IEnumerable<BenefitItemProfileViewModel> BenefitItemOlympicProfiles { get; set; }
    }

    //public class BenefitItemOlympicProfileViewModel
    //{
    //    public int ID { get; set; }
    //    public int OlympicProfileID { get; set; }
    //    public string ProfileName { get; set; }

    //}

    public class BenefitItemProfileViewModel
    {
        public int ID { get; set; }
        public int OlympicProfileID { get; set; }
        public string ProfileName { get; set; }
    }
}
