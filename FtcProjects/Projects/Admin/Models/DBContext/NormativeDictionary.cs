using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class NormativeDictionary
    {
        public int DictionaryId { get; set; }
        public string Name { get; set; }
        public int VersionId { get; set; }
        public DateTime? ActivationDate { get; set; }
        public byte VersionStateId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual NormativeVersionState VersionState { get; set; }
    }
}
