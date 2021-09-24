using System.Linq;
namespace FbsService
{
    partial class FbsContext
    {
    }
}

namespace FbsService.Fbs
{
    partial class FbsContext
    {
        static private ThreadInstanceManager<FbsContext> mInstanceManager =
        new ThreadInstanceManager<FbsContext>(CreateInstance);

        static private FbsContext CreateInstance()
        {
            FbsContext instance = new FbsContext();
            instance.ObjectTrackingEnabled = false;
            instance.CommandTimeout = 0;
            return instance;
        }

        static internal FbsContext Instance()
        {
            return mInstanceManager.Instance();
        }

        static internal void BeginLock()
        {
            mInstanceManager.BeginLock();
        }

        static internal void EndLock()
        {
            mInstanceManager.EndLock();
        }

        static public void ImportCertificates(string certificateFileName, string certificateSubjectFileName)
        {
            BeginLock();
            try
            {
                Instance().ImportCommonNationalExamCertificate(certificateFileName, certificateSubjectFileName);
            }
            finally
            {
                EndLock();
            }
        }

        static public void ImportCertificateDeny(string certificateDenyFileName)
        {
            BeginLock();
            try
            {
                Instance().ImportCommonNationalExamCertificateDeny(certificateDenyFileName);
            }
            finally
            {
                EndLock();
            }
        }
    }
}
