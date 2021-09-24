using GVUZ.DAL.Dapper.Model.Dictionary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.DAL.Dapper.ViewModel.Dictionary
{
    public class SubjectView : Subject
    {
        public int MinValue { get; set; }
    }
}
