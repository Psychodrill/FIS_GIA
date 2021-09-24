using GVUZ.ImportService2016.Core.Dto.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GVUZ.ImportService2016.Core.Dto.DataReaders.AdmissionInfo
{
    public class BulkBenefitItemDataReader : BulkReaderBase<BulkBenefitItemData>
    {
        public BulkBenefitItemDataReader(PackageData packageData)
        {

            AddGetter("GUID", dto => dto.GUID);
            AddGetter("ParentID", dto => dto.ParentID);
            AddGetter("InstitutionID", dto => packageData.InstitutionId);
            AddGetter("ImportPackageId", dto => packageData.ImportPackageId);

            AddGetter("OlympicTypeID", dto => GetIntOrNull(dto.OlympicTypeID));
            AddGetter("SubjectId", dto => GetIntOrNull(dto.SubjectId));
            AddGetter("EgeMinValue", dto => GetIntOrNull(dto.EgeMinValue));

            AddGetter("OlympicProfileID", dto => dto.ProfileID);
            AddGetter("OlympicLevelFlags", dto => dto.LevelFlags);
            AddGetter("ClassFlags", dto => dto.ClassFlags);
        }
    }

    public class BulkBenefitItemData : ImportBase
    {
        public BulkBenefitItemData() { }

        public Guid ParentID { get; set; }

        public int? OlympicTypeID { get; set; }
        public int? SubjectId { get; set; }
        public int? EgeMinValue { get; set; }

        public int? ProfileID { get; set; }
        public int? LevelFlags { get; set; }
        public int? ClassFlags { get; set; }
    }
}
