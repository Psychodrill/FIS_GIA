using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace GVUZ.DAL.Dto
{
    public class DistributedPlanAdmissionVolumeSaveDto
    {
        public int FinanceSourceId { get; set; }
        public int EducationFormId { get; set; }
        public int BudgetId { get; set; }
        public int Number { get; set; }
    }
}
