using System;

namespace GVUZ.ServiceModel.Import.WebService.Dto
{
    [Serializable]
    public class AllowedDirectionsDto
    {
        public int DirectionId { get; set; }
        public string Name { get; set; }
        public short? AdmissionItemTypeID { get; set; }
    }
}