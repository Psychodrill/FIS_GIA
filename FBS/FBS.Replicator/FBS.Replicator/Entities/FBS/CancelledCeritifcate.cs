using System.Data;
using FBS.Replicator.Replication.ERBDToFBS;

namespace FBS.Replicator.Entities.FBS
{
    public class FBSCancelledCertificate
    {
        public FBSCancelledCertificate(FastDataReader reader)
        {
            Id = new CertificateId(reader);
            string reason = DataHelper.GetString(reader, "Reason");

            HashCode = reason.GetHashCode();
        }

        public FBSCertificate Certificate;

        public readonly CertificateId Id;

        public readonly int HashCode;

        public ERBDToFBSActions Action;
    }
}
