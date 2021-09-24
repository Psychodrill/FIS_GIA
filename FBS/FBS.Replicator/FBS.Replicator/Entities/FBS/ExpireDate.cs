using System;
using System.Data;

namespace FBS.Replicator.Entities.FBS
{
    public class FBSExpireDate
    {
        public FBSExpireDate(FastDataReader reader)
        {
            Year = DataHelper.GetShort(reader, "Year").Value;
            ExpireDate = DataHelper.GetDateTime(reader, "ExpireDate").Value;
        }

        public readonly short Year;
        public readonly DateTime ExpireDate;
    }
}
