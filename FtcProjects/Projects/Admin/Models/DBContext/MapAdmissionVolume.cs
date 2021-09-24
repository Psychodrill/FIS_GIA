using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class MapAdmissionVolume
    {
        public int NewAdmissionVolumeId { get; set; }
        public int? OldAdmissionVolumeId { get; set; }
        public string NewUid { get; set; }
        public string OldUid { get; set; }
    }
}
