using System;
using System.Linq;

namespace FbsService.FbsLoader
{
    partial class CNELoadingTask
    {
        static public CNELoadingTask GetProcessTask()
        {
            LoaderContext.BeginLock();
            try
            {
                return LoaderContext.Instance().GetProcessCommonNationalExamCertificateLoadingTask().SingleOrDefault();
            }
            finally
            {
                LoaderContext.EndLock();
            }
        }

        static public void ExecuteTask(long id)
        {
            LoaderContext.BeginLock();
            try
            {
                LoaderContext.Instance().ExecuteCommonNationalExamCertificateLoading(id);
            }
            finally
            {
                LoaderContext.EndLock();
            }
        }

        static public void PrepareLoading()
        {
            LoaderContext.BeginLock();
            try
            {
                LoaderContext.Instance().PrepareCommonNationalExamCertificateLoading();
            }
            finally
            {
                LoaderContext.EndLock();
            }
        }

        static public void RefreshTasks(string[] batchUrls)
        {
            LoaderContext.RefreshLoadingTasks(batchUrls);
        }

        static public void AddError(long taskId, int? rowIndex, string error)
        {
            LoaderContext.BeginLock();
            try
            {
                LoaderContext.Instance().UpdateCommonNationalExamCertificateLoadingTaskError(taskId, rowIndex, error);
            }
            finally
            {
                LoaderContext.EndLock();
            }
        }

        static public void AddCertificate(out long? id, string number, string educationInstitutionCode, string lastName,
                string firstName, string patronymicName, bool sex, string @class, string passportSeria,
                string passportNumber, string entrantNumber, int regionId, Guid? updateId,
                long? editorAccountId, string editorIp, string typographicNumber)
        {
            id = null;
            LoaderContext.BeginLock();
            try
            {
                LoaderContext.Instance().UpdateLoadingCommonNationalExamCertificate(ref id, number, 
                        educationInstitutionCode, lastName, firstName, patronymicName, sex, @class,
                        passportSeria, passportNumber, entrantNumber, regionId, updateId, editorAccountId, editorIp, typographicNumber);
            }
            finally
            {
                LoaderContext.EndLock();
            }
        }

        static public void AddCertificateSubject(long certificateId, int regionId, int subjectId, 
                float mark, bool hasAppeal)
        {
            LoaderContext.BeginLock();
            try
            {
                LoaderContext.Instance().UpdateLoadingCommonNationalExamCertificateSubject(certificateId, 
                        regionId, subjectId, mark, hasAppeal);
            }
            finally
            {
                LoaderContext.EndLock();
            }
        }

        static public void ActivateLoadingTaskAuto(long editorAccountId, string editorIp)
        {
            LoaderContext.BeginLock();
            try
            {
                LoaderContext.Instance().ActivateCommonNationalExamLoadingTask(editorAccountId, editorIp);
            }
            finally
            {
                LoaderContext.EndLock();
            }
        }

        static public void DeactivateTask(long id)
        {
            LoaderContext.BeginLock();
            try
            {
                LoaderContext.Instance().StopCommonNationalExamCertificateLoadingTask(id);
            }
            finally
            {
                LoaderContext.EndLock();
            }
        }

        static public void ImportCertificates(string fileName)
        {
            LoaderContext.BeginLock();
            try
            {
                LoaderContext.Instance().ImportCommonNationalExamCertificates(fileName);
            }
            finally
            {
                LoaderContext.EndLock();
            }
        }

        static public void ImportCertificateSubjects(string fileName)
        {
            LoaderContext.BeginLock();
            try
            {
                LoaderContext.Instance().ImportCommonNationalExamCertificateSubjects(fileName);
            }
            finally
            {
                LoaderContext.EndLock();
            }
        }
    }
}
