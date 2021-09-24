using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FbsService.FbsLoader
{
    partial class CNEDenyLoadingTask
    {
        static public CNEDenyLoadingTask GetProcessTask()
        {
            LoaderContext.BeginLock();
            try
            {
                return LoaderContext.Instance().GetProcessCommonNationalExamCertificateDenyLoadingTask().
                        SingleOrDefault();
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
                LoaderContext.Instance().ExecuteCommonNationalExamCertificateDenyLoading(id);
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
                LoaderContext.Instance().PrepareCommonNationalExamCertificateDenyLoading();
            }
            finally
            {
                LoaderContext.EndLock();
            }
        }

        static public void RefreshTasks(string[] batchUrls)
        {
            LoaderContext.RefreshDenyLoadingTasks(batchUrls);
        }

        static public void AddError(long taskId, int? rowIndex, string error)
        {
            LoaderContext.BeginLock();
            try
            {
                LoaderContext.Instance().UpdateCommonNationalExamCertificateDenyLoadingTaskError(taskId, rowIndex, error);
            }
            finally
            {
                LoaderContext.EndLock();
            }
        }

        static public void AddCertificateDeny(string certificateNumber, string comment, string newCertificateNumber, 
                Guid? updateId, long? editorAccountId, string editorIp)
        {
            LoaderContext.BeginLock();
            try
            {
                LoaderContext.Instance().UpdateLoadingCommonNationalExamCertificateDeny(certificateNumber, comment,
                        newCertificateNumber, updateId, editorAccountId, editorIp);
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
                LoaderContext.Instance().ActivateCommonNationalExamDenyLoadingTask(editorAccountId, editorIp);
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
                LoaderContext.Instance().StopCommonNationalExamCertificateDenyLoadingTask(id);
            }
            finally
            {
                LoaderContext.EndLock();
            }
        }

        static public void ImportCertificateDenyes(string fileName)
        {
            LoaderContext.BeginLock();
            try
            {
                LoaderContext.Instance().ImportCommonNationalExamDenyCertificates(fileName);
            }
            finally
            {
                LoaderContext.EndLock();
            }
        }
    }
}
