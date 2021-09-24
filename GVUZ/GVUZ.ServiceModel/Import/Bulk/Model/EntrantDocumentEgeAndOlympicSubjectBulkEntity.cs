using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GVUZ.ServiceModel.Import.Bulk.Attributes;
using GVUZ.ServiceModel.Import.Bulk.Model.Base;

namespace GVUZ.ServiceModel.Import.Bulk.Model
{
    [DestinationTableName("blk_EntrantDocumentEgeAndOlympicSubject")]
    public class EntrantDocumentEgeAndOlympicSubjectBulkEntity : BulkEntityBase
    {
        public int? SubjectId { get; set; }
        public decimal? Value { get; set; }
    }
}
