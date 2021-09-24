using System.Data;
using FBS.Replicator.Replication.ERBDToFBS;

namespace FBS.Replicator.Entities.ERBD
{
    public class ERBDCancelledCertificate
    {
        public ERBDCancelledCertificate(FastDataReader reader)
        {
            Id = new CertificateId(reader);

            string reason = DataHelper.GetString(reader, "Reason");
            Reason = DataHelper.StringToBytes(reason);

            HashCode = reason.GetHashCode();
        }

        public ERBDCertificate Certificate;

        public readonly CertificateId Id;

        public readonly int HashCode;

        public readonly byte[] Reason;

        public ERBDToFBSActions Action;

        public string ReasonStr { get { return DataHelper.BytesToString(Reason); } }
    }
}
