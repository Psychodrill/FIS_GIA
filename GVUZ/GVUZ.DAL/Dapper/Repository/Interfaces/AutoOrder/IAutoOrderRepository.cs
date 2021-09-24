using GVUZ.DAL.Dapper.ViewModel.AutoOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.DAL.Dapper.Repository.Interfaces.AutoOrder
{
    public interface IAutoOrderRepository
    {
        List<AutoOrderCheckBoxModel> Load(int institutionId);
        void Save(AutoOrderCheckBoxModel model, int institutionId);
    }
}
