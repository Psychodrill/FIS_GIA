using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace GVUZ.Web.ViewModels
{
    public class BenefitItemSubjectViewModel
    {
        public int Id { get; set; }

        public int BenefitItemId { get; set; }

        public int SubjectId { get; set; }

        [DisplayName("Общеобразовательный предмет")]
        public string SubjectName { get; set; }

        [DisplayName("Минимальный балл")]
        public int EgeMinValue { get; set; }

        [DisplayName("Минимальный балл ЕГЭ")]
        public string MinBall { get { return null; } }

        public override string ToString()
        {
            return SubjectName + " - " + EgeMinValue.ToString();
        }

    }
}