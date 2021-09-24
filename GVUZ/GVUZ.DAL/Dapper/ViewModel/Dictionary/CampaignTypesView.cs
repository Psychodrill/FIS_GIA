using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GVUZ.DAL.Dapper.ViewModel.Dictionary
{
    public partial class CampaignTypesView
    {
        public int CampaignTypeID { get; set; }
        public int ID { get { return CampaignTypeID; } }

        [StringLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Прием на обучение на бакалавриат/специалитет
        /// </summary>
        public const int BachelorAndSpeciality = 1;
        /// <summary>
        /// Прием на обучение в магистратуру
        /// </summary>
        public const int Magistracy = 2;
        /// <summary>
        /// Прием на обучение на СПО
        /// </summary>
        public const int SPO = 3;
        /// <summary>
        /// Прием на подготовку кадров высшей квалификации
        /// </summary>
        public const int HighQualification = 4;
        /// <summary>
        /// Прием иностранцев по направлениям Минобрнауки
        /// </summary>
        public const int Foreigners = 5;
    }
}
