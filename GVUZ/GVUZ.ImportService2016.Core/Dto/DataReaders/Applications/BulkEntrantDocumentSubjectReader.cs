using GVUZ.ImportService2016.Core.Main.Dictionaries;
using GVUZ.ImportService2016.Core.Dto.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GVUZ.ImportService2016.Core.Main.Extensions;

namespace GVUZ.ImportService2016.Core.Dto.DataReaders
{
    public class BulkEntrantDocumentSubjectReader : BulkReaderBase<BulkEntrantDocumentSubjectDto>
    {
        public BulkEntrantDocumentSubjectReader(PackageData packageData, VocabularyStorage vocabularyStorage)
        {

            AddGetter("Id", dto => dto.GUID ); // 
            //AddGetter("ParentId", dto => dto.ApplicationGuid);
            AddGetter("ParentId", dto => dto.ParentEntrantDocumentGuid);
            AddGetter("UID", dto => dto.UID);
            AddGetter("ImportPackageId", dto => packageData.ImportPackageId);
            AddGetter("InstitutionId", dto => packageData.InstitutionId);

            AddGetter("SubjectID", dto => dto.SubjectID);
            AddGetter("Value", dto => dto.Value);

        }
    }

    public class BulkEntrantDocumentSubjectDto : ImportBase
    {
        public BulkEntrantDocumentSubjectDto() { }

        public BulkEntrantDocumentSubjectDto(int subjectId, Guid parentGuid, VocabularyStorage vocStorage)
        {
            ParentEntrantDocumentGuid = parentGuid;
            SubjectID = subjectId;
        }

        public BulkEntrantDocumentSubjectDto(ISubject subject, Guid parentGuid, VocabularyStorage vocStorage)
        {
            ParentEntrantDocumentGuid = parentGuid; 
            SubjectID = subject.SubjectID.To(0);
            Value = subject.Value.To(0);
        }

        public string ApplicationUID { get; set; }
        public Guid ApplicationGuid { get; set; }
        public string UID { get; set; }

        public Guid ParentEntrantDocumentGuid { get; set; }
        public int SubjectID { get; set; }
        public int Value { get; set; }
    }
}
