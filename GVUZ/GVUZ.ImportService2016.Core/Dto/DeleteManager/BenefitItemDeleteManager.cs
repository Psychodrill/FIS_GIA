using GVUZ.ImportService2016.Core.Main.Dictionaries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Dto.DeleteManager
{
    public class BenefitItemDeleteManager : BaseDeleteManager<BenefitItemCVocDto>
    {
        public BenefitItemDeleteManager(BenefitItemCVocDto dto, VocabularyStorage vocabularyStorage)
            : base(dto, vocabularyStorage)
        {

        }

        /// <summary>
        /// Есть ли зависимости по данной записи?
        /// </summary>
        /// <returns>True - нет зависимостей, False - есть зависимости</returns>
        public override bool CheckDependency()
        {
            return true;
        }
    }
}
