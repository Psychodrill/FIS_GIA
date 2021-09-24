using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.DAL.Dapper.ViewModel.CompetitiveGroups
{
    public class DirectionViewModel
    {
        public const int DIRECTION_STATUS_USERADD = 3;

        public int DirectionID { get; set; }
        public int ID { get { return DirectionID; } }

        [DisplayName("Направление")]
        public string DirectionName { get; set; }

        public int ParentID { get; set; }
        public string UGSNAME { get; set; }
        public string UGSCODE { get; set; }

        public string Code { get; set; }
        public string NewCode { get; set; }

        public int EducationLevelID { get; set; }

        public int CreativeDirectionID { get; set; }
        public int ProfileDirectionID { get; set; }

        [DisplayName("Творческое/профессиональное")]
        public bool IsCreative { get { return CreativeDirectionID != 0; } }
        [DisplayName("Профильное")]
        public bool IsProfile { get { return ProfileDirectionID != 0; } }

        public int AllowedDirectionStatusID { get; set; }

        public string Name
        {
            get
            {
                var code = Code;
                if (!string.IsNullOrWhiteSpace(code) && !string.IsNullOrWhiteSpace(NewCode))
                    code += "/";
                code += NewCode;
                if (!string.IsNullOrWhiteSpace(code))
                    code += " ";

                var type = IsCreative && IsProfile ?
                    " (творческое/профессиональное, профильное)" :
                    IsCreative ? " (творческое/профессиональное)" :
                    IsProfile ? " (профильное)" : string.Empty;


                return code + DirectionName + type;
            }
        }
    }
}
