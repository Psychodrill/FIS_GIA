namespace FbsService.FbsCheck
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Xml.Serialization;

    using FbsService.Properties;

    /// <summary>
    /// The cne check batch task.
    /// </summary>
    public class CNECheckBatchTask : Task
    {
        #region Fields

        public long BatchId;

        private CNECheckBatch mBatch;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets Batch.
        /// </summary>
        [XmlIgnore]
        public CNECheckBatch Batch
        {
            get
            {
                if (this.mBatch == null)
                {
                    this.mBatch = CNECheckBatch.GetBatch(this.BatchId);
                }

                return this.mBatch;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The get status.
        /// </summary>
        /// <param name="code">
        /// The code.
        /// </param>
        /// <returns>
        /// </returns>
        protected internal override TaskStatus GetStatus(string code)
        {
            if (code == ExecuteCheck.StatusCode)
            {
                return new ExecuteCheck();
            }
            
            return new ParseData();
        }

        /// <summary>
        /// The get task code.
        /// </summary>
        /// <returns>
        /// The get task code.
        /// </returns>
        protected override string GetTaskCode()
        {
            return "CommonNationalExamCertificateBatchCheck";
        }

        #endregion

        private class ExecuteCheck : TaskStatus
        {
            #region Constants

            /// <summary>
            /// The status code.
            /// </summary>
            public const string StatusCode = "check";

            private const string CheckSPName = "dbo.ExecuteCommonNationalExamCertificateCheck";

            #endregion

            #region Properties

            private CNECheckBatch Batch
            {
                get
                {
                    return ((CNECheckBatchTask)this.Task).Batch;
                }
            }

            #endregion

            #region Methods

            /// <summary>
            /// The execute.
            /// </summary>
            protected internal override void Execute()
            {
                using (var connection = new SqlConnection(Settings.Default.FbsCheckConnectionString))
                {
                    connection.Open();
                    this.CheckData(connection);
                }

                this.Batch.Executing = false;
                this.Batch.Update();
                this.FinishTask();
            }

            /// <summary>
            /// The get status code.
            /// </summary>
            /// <returns>
            /// The get status code.
            /// </returns>
            protected internal override string GetStatusCode()
            {
                return StatusCode;
            }

            private void CheckData(SqlConnection connection)
            {
                using (SqlCommand cmdCheck = connection.CreateCommand())
                {
                    var checkParsingParameters = new[] { new SqlParameter("@batchId", SqlDbType.BigInt), };

                    cmdCheck.CommandType = CommandType.StoredProcedure;
                    cmdCheck.CommandText = CheckSPName;
                    cmdCheck.CommandTimeout = 0;
                    cmdCheck.Parameters.AddRange(checkParsingParameters);
                    cmdCheck.Parameters["@batchId"].Value = this.Batch.Id;
                    try
                    {
                        cmdCheck.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        TaskManager.Instance().Logger.Error(string.Format("ошибка при обновлении статуса для пакета с id = {0}", this.Batch.Id), ex);
                        throw;
                    }
                }
            }

            #endregion
        }

        private class ParseData : TaskStatus
        {
            #region Constants

            /// <summary>
            /// The status code.
            /// </summary>
            public const string StatusCode = "parse";

            private const string SPStartCheckBatchName = "dbo.usp_cne_StartCheckBatch";

            #endregion

            #region Properties

            private CNECheckBatch Batch
            {
                get
                {
                    return ((CNECheckBatchTask)this.Task).Batch;
                }
            }

            #endregion

            #region Methods

            /// <summary>
            /// The execute.
            /// </summary>
            protected internal override void Execute()
            {
                if (this.Batch == null)
                {
                    return;
                }

                using (var connection = new SqlConnection(Settings.Default.FbsCheckConnectionString))
                {
                    connection.Open();

                    try
                    {
                        this.StartCheckBatch(connection);
                    }
                    catch (Exception ex)
                    {
                        TaskManager.Instance().Logger.Error("ошибка в пакетных проверках", ex);
                        this.SetStatus(ExecuteCheck.StatusCode);
                        connection.Close();
                        throw;
                    }
                }

                try
                {
                    this.SetStatus(ExecuteCheck.StatusCode);
                }
                catch (Exception ex)
                {
                    TaskManager.Instance().Logger.Error("ошибка в пакетных проверках", ex);
                    throw;
                }
            }

            /// <summary>
            /// The get status code.
            /// </summary>
            /// <returns>
            /// The get status code.
            /// </returns>
            protected internal override string GetStatusCode()
            {
                return StatusCode;
            }

            private void StartCheckBatch(SqlConnection connection)
            {
                using (SqlCommand cmdCommit = connection.CreateCommand())
                {
                    var commitParsingParameters = new[] { new SqlParameter("@batchId", SqlDbType.BigInt), };
                    TaskManager.Instance().Logger.Debug("DATABASE IS " + connection.Database);
                    cmdCommit.CommandType = CommandType.StoredProcedure;
                    cmdCommit.CommandText = SPStartCheckBatchName;
                    cmdCommit.CommandTimeout = 0;
                    cmdCommit.Parameters.AddRange(commitParsingParameters);
                    cmdCommit.Parameters["@batchId"].Value = this.Batch.Id;
                    cmdCommit.ExecuteNonQuery();
                }
            }

            #endregion
        }
    }
}