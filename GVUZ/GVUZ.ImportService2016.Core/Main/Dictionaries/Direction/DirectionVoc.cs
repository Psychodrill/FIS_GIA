using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Main.Dictionaries.Direction
{
    public class DirectionVoc : VocabularyBase<DirectionVocDto>
    {
        public DirectionVoc(DataTable dataTable) : base(dataTable) { }

        public string GetSpecialityName(string code)
        {
            var item = items.FirstOrDefault(i => i.Code == code);
            return GetSpecialityName(item);
        }

        public string GetSpecialityName(int id)
        {
            var item = items.FirstOrDefault(i => i.DirectionID == id);
            return GetSpecialityName(item);
        }

        private static string GetSpecialityName(DirectionVocDto item)
        {
            if (item == null) return "";

            string res = item.Code + " " + item.Name;
            if (!string.IsNullOrEmpty(item.NewCode))
                res = item.NewCode + "/" + res;

            return res;
        }

        public string GetQualificationName(string code)
        {
            var item = items.FirstOrDefault(i => i.QUALIFICATIONCODE == code);
            return item != null ? item.QUALIFICATIONNAME : "";
        }
        public string GetQualificationName(int id)
        {
            var item = items.FirstOrDefault(i => i.DirectionID == id);
            return item != null ? item.QUALIFICATIONNAME : "";
        }

    }

    public class DirectionVocDto : VocabularyBaseDto
    {
        public int DirectionID { get; set; }
        public string  Code { get; set; }
        public string  QUALIFICATIONCODE { get; set; }
        public string  QUALIFICATIONNAME { get; set; }
        public string  NewCode { get; set; }
        public int ParentDirectionID { get; set; }

        public override int ID
        {
            get { return DirectionID; }
            set { DirectionID = value; }
        }
    }
}
