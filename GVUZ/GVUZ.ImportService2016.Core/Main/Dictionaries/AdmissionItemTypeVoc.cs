using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries
{
    public class AdmissionItemTypeVoc : VocabularyBase<AdmissionItemTypeVocDto>
    {
        public AdmissionItemTypeVoc(DataTable dataTable) : base(dataTable) { }

        public List<AdmissionItemTypeVocDto> GetFinanceSource() { return items.Where(t => t.ItemLevel == 8).ToList();  }
        public List<AdmissionItemTypeVocDto> GetEducationForm() { return items.Where(t => t.ItemLevel == 7).ToList(); }
        public List<AdmissionItemTypeVocDto> GetEducationLevel() { return items.Where(t => t.ItemLevel == 2).ToList(); }
    }

    public class AdmissionItemTypeVocDto : VocabularyBaseDto
    {
        public int ItemTypeID { get; set; }
        public int ItemLevel { get; set; }

        public override int ID
        {
            get { return ItemTypeID; }
            set { ItemTypeID = value; }
        }
    }
}
