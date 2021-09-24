using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace GVUZ.ServiceModel.Import.WebService.Dto
{
    public enum ImportStatus
    {
        None = 0,
        Insert = 1,
        Update = 2,
        Delete = 4
    }

    public class BaseDto
    {
        [XmlIgnore] public List<string> ErrorMessages = new List<string>();

        public BaseDto()
        {
            Id = Guid.NewGuid();
        }

        [XmlIgnore]
        public virtual Guid Id { get; set; }

        public virtual string UID { get; set; }

        [XmlIgnore]
        public virtual string ParentUID { get; set; }

        [XmlIgnore]
        public bool IsBroken { get; set; }
    }
}