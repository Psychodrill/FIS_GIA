using System;
using System.Collections.Generic;

namespace Admin.Models.DBContext
{
    public partial class EntranceTestProfileDirection
    {
        public int DirectionId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual Direction Direction { get; set; }
    }
}
