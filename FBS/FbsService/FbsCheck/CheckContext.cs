using System.Data;

namespace FbsService.FbsCheck
{

    internal partial class CheckContext
    {
        static private ThreadInstanceManager<CheckContext> mInstanceManager =
                new ThreadInstanceManager<CheckContext>(CreateInstance);

        static private CheckContext CreateInstance()
        {
            CheckContext instance = new CheckContext();
            instance.ObjectTrackingEnabled = false;
            instance.CommandTimeout = 0;
            return instance;
        }

        static internal CheckContext Instance()
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
