using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Admin.Models.DBContext
{
    public class InstitutionAttachment
    {
        public InstitutionAttachment()
        {
            //InstitutionLicense = new HashSet<InstitutionLicense>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AttachmentId { get; set; }
        public string MimeType { get; set; }
        public string Name { get; set; }
        public byte[] Body { get; set; }
        public Guid FileId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string DisplayName { get; set; }
        public long? ContentLength { get; set; }

        [ForeignKey("AttachmentId")]
        public virtual InstitutionAccreditation InstitutionAccreditation { get; set; }
        
        [ForeignKey("AttachmentId")]
        public virtual InstitutionLicense InstitutionLicense { get; set; }

    }
}
