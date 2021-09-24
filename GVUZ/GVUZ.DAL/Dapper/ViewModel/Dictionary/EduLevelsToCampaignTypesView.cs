using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GVUZ.DAL.Dapper.ViewModel.Dictionary
{
    public partial class EduLevelsToCampaignTypesView
    {
        public int ItemTypeID { get; set; }
        public string Name { get; set; }
        public short CampaignTypeID { get; set; }
    }
}
