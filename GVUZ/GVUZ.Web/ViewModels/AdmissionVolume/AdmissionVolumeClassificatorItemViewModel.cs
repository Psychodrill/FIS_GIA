using System.Collections.Generic;

namespace GVUZ.Web.ViewModels.AdmissionVolume
{
    public class AdmissionVolumeClassificatorItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class AdmissionVolumeLevelViewModel : AdmissionVolumeClassificatorItemViewModel
    {
        public IEnumerable<AdmissionVolumeDirectionGroupViewModel> DirectionGroups { get; set; }
    }

    public class AdmissionVolumeDirectionGroupViewModel : AdmissionVolumeClassificatorItemViewModel
    {
        public string Code { get; set; }
        public IEnumerable<AdmissionVolumeDirectionViewModel> Directions { get; set; }
        public bool IsForUGS { get; set; }
        public int AvailableForDistribution { get; set; }
        public int TotalDistributed { get; set; }

    }

    public class AdmissionVolumeDirectionViewModel : AdmissionVolumeClassificatorItemViewModel
    {
        public string Code { get; set; }
        public int AvailableForDistribution { get; set; }
        public int TotalDistributed { get; set; }
    }
}