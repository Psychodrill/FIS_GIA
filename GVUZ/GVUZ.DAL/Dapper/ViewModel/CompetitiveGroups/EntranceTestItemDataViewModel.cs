using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.DAL.Dapper.ViewModel.CompetitiveGroups
{
    public class EntranceTestItemDataViewModel
    {
        public class EntranceTestType
        {
            public static readonly int MainType = 1;
            public static readonly int CreativeType = 2;
            public static readonly int ProfileType = 3;
            //public static readonly int AttestationType = 5;
        }


        public int ItemID { get; set; }
        public int TestType { get; set; }
        public string TestName { get; set; }
        public decimal? Value { get; set; }
        public int? EntranceTestPriority { get; set; }

        /// <summary>
        /// Для возврата, чтобы правильно количество пересчитывать в дереве
        /// </summary>
        public bool CanRemove { get; set; }

        [StringLength(200)]
        public string UID { get; set; }

        public bool IsForSPOandVO { get; set; }
        public int ReplacedEntranceTestItemID { get; set; }
        public string ReplacedEntranceTestItemName { get; set; }

        public bool IsFirst { get; set; }
        public bool IsSecond { get; set; }

        //public bool IsChanged { get; set; }

        public IEnumerable<BenefitItemViewModel> BenefitItems { get; set; }
    }
}
