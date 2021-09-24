using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace FbsService
{
    public partial class BulkFile
    {
        static public string Directory = ConfigurationManager.AppSettings["BulkFileDirectory"];

        internal void Store()
        {
            TaskContext.BeginLock();
            try
            {
                TaskContext.Instance().InternalUpdateBulkFile(this);
            }
            finally
            {
                TaskContext.EndLock();
            }
        }

        static internal BulkFile[] GetAbsenteeBulkFiles()
        {
            TaskContext.BeginLock();
            try
            {
                return TaskContext.Instance().SearchAbsenteeBulkFile().ToArray();
            }
            finally
            {
                TaskContext.EndLock();
            }
        }

        static internal BulkFile[] GetDeprecatedBulkFiles()
        {
            TaskContext.BeginLock();
            try
            {
                return TaskContext.Instance().SearchDeprecatedBulkFile(true).ToArray();
            }
            finally
            {
                TaskContext.EndLock();
            }
        }

        static internal BulkFile GetBulkFile(string code)
        {
            TaskContext.BeginLock();
            try
            {
                return TaskContext.Instance().GetBulkFile(code).SingleOrDefault();
            }
            finally
            {
                TaskContext.EndLock();
            }
        }

        static public BulkFile GetAbsenteeDbSubscriptionBulkFile(string code, string dbName)
        {
            TaskContext.BeginLock();
            try
            {
                return TaskContext.Instance().GetAbsenteeDbSubscriptionBulkFile(code, dbName).SingleOrDefault();
            }
            finally
            {
                TaskContext.EndLock();
            }
        }

        public void AppendDbSubscription(string dbName)
        {
            TaskContext.BeginLock();
            try
            {
                TaskContext.Instance().UpdateBulkFileDbSubscription(this.Code, this.Date, dbName);
            }
            finally
            {
                TaskContext.EndLock();
            }
        }

        static public void AppendBulkFile(string code, string fileName)
        {
            BulkFile bulk = new BulkFile();
            bulk.Code = code;
            bulk.Date = DateTime.Now;
            bulk.FileName = fileName;
            bulk.Store();
        }
    }

    partial class TaskContext
    {
        internal void InternalUpdateBulkFile(BulkFile bulk)
        {
            this.UpdateBulkFile(bulk);
        }
    }
}
