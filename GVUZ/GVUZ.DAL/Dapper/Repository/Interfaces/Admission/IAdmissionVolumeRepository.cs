using GVUZ.DAL.Dapper.ViewModel.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GVUZ.Web.ViewModels.KcpDistribution;



namespace GVUZ.DAL.Dapper.Repository.Interfaces.Admission
{
    public interface IAdmissionVolumeRepository
    {
        AdmissionVolumeViewModel FillAdmissionVolumeViewModel(AdmissionVolumeViewModel model, bool forEdit);

        AdmissionVolumeViewModel UpdateAdmissionVolume(KcpUpdateViewModel model);
        AdmissionVolumeViewModel SaveAdmissionVolume(AdmissionVolumeViewModel model);

        AdmissionVolumeViewModel.RowData CheckKCP(int campaignID, int educationLevelID, int? directionID, int? parentDirectionID);
        AdmissionVolumeViewModel.RowData CheckDistributionKCP(int AdmissionVolumeId, int BudgetLevelId);

    }
}
