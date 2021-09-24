using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Fbs.Core.Loggers;

namespace FbsHashedCNEsReplicator
{
   public  class DeniedCNEDataAccessor
    {
        private SqlConnection Conn_ = new SqlConnection();

        public DeniedCNEDataAccessor(string connectionString)
        {
            Conn_.ConnectionString = connectionString;
        }

        public void OpenConnection()
        {
            Conn_.Open();
        }

        public void CloseConnection()
        {
            Conn_.Close();
        }

        public void CreateTempTables()
        {
            SqlCommand Cmd = Conn_.CreateCommand();
            Cmd.CommandText = @"CREATE TABLE #DeniedCNE(
	                    [Id] [bigint] NOT NULL,
	                    [CreateDate] [datetime] NOT NULL,
	                    [UpdateDate] [datetime] NOT NULL,
	                    [UpdateId] [uniqueidentifier] NULL,
	                    [Year] [int] NOT NULL,
	                    [CertificateNumber] [nvarchar](255) COLLATE Cyrillic_General_CI_AS NOT NULL,
	                    [Comment] [ntext] COLLATE Cyrillic_General_CI_AS NOT NULL,
	                    [NewCertificateNumber] [nvarchar](255) COLLATE Cyrillic_General_CI_AS NULL)";
            Cmd.ExecuteNonQuery();
        }

        public void DropTempTables()
        {
            SqlCommand Cmd = Conn_.CreateCommand();
            Cmd.CommandText = "DROP TABLE #DeniedCNE ";
            Cmd.ExecuteNonQuery();
        }

        public void ReplicateTables(ILogger logger)
        {
            SqlCommand Cmd = Conn_.CreateCommand();
            Cmd.CommandTimeout = 10000000;
            Cmd.CommandText = @"UPDATE DeniedCNE 
                            SET DeniedCNE.[Comment] = ToUpdate.Comment,
		                    DeniedCNE.[NewCertificateNumber] = ToUpdate.NewCertificateNumber,
		                    DeniedCNE.UpdateDate = GETDATE()
	                        FROM [CommonNationalExamCertificateDeny] AS DeniedCNE
	                        INNER JOIN
                                (SELECT [Year],
		                        [CertificateNumber],
		                        CAST([Comment] AS NVARCHAR(MAX)) AS Comment,
		                        [NewCertificateNumber]
	                            FROM #DeniedCNE
	                            EXCEPT 
	                            SELECT [Year],
		                        [CertificateNumber],
		                        CAST([Comment] AS NVARCHAR(MAX)) AS Comment,
		                        [NewCertificateNumber]
	                        FROM CommonNationalExamCertificateDeny) ToUpdate 
	                        ON DeniedCNE.[CertificateNumber] = ToUpdate.[CertificateNumber] and DeniedCNE.[Year] = ToUpdate.[Year]";
            Cmd.ExecuteNonQuery();
            logger.WriteMessage("Existing denied CNEs updated " + DateTime.Now.ToLongTimeString());

            Cmd.CommandText = @"DECLARE @MinYear INT, @MaxYear INT
	                        SELECT @MinYear = (SELECT MIN([Year]) FROM #DeniedCNE NewDeniedCNE)
		                    ,@MaxYear = (SELECT MAX([Year]) FROM #DeniedCNE NewDeniedCNE)
                            IF ( @MinYear = @MaxYear )
                            BEGIN
	                            DELETE DeniedCNE
	                            FROM CommonNationalExamCertificateDeny DeniedCNE
	                            LEFT JOIN #DeniedCNE NewDeniedCNE
	                            ON NewDeniedCNE.CertificateNumber = DeniedCNE.CertificateNumber 
	                            AND NewDeniedCNE.Year = DeniedCNE.Year 
	                            WHERE NewDeniedCNE.CertificateNumber IS NULL and DeniedCNE.Year = @MaxYear
                            END ";
            Cmd.ExecuteNonQuery();
            logger.WriteMessage("Obsolete denied CNEs deleted " + DateTime.Now.ToLongTimeString());


            Cmd.CommandText = @"INSERT INTO [CommonNationalExamCertificateDeny]
	                        SELECT NewDeniedCNE.* 
	                        FROM #DeniedCNE NewDeniedCNE
	                        LEFT JOIN CommonNationalExamCertificateDeny DeniedCNE
	                        ON NewDeniedCNE.CertificateNumber = DeniedCNE.CertificateNumber 
	                        AND NewDeniedCNE.Year = DeniedCNE.Year 
	                        WHERE DeniedCNE.CertificateNumber IS NULL";
            Cmd.ExecuteNonQuery();
            logger.WriteMessage("New denied CNEs inserted " + DateTime.Now.ToLongTimeString());
          
            logger.WriteMessage("DeniedCNEs replication succeed");
        }

        private SqlCommand GetInsertDeniedCNECmd()
        {
            SqlCommand Cmd = Conn_.CreateCommand();
            Cmd.CommandText = @"INSERT INTO  #DeniedCNE
                           ([Id]
                           ,[CreateDate]
                           ,[UpdateDate]
                           ,[UpdateId]
                           ,[Year]
                           ,[CertificateNumber]
                           ,[Comment]
                           ,[NewCertificateNumber])
                     VALUES
                           (@Id
                           ,@CreateDate 
                           ,@UpdateDate 
                           ,@UpdateId 
                           ,@Year 
                           ,@CertificateNumber 
                           ,@Comment 
                           ,@NewCertificateNumber)";

            Cmd.Parameters.AddWithValue("Id", null);
            Cmd.Parameters.AddWithValue("CreateDate", DateTime.Now);
            Cmd.Parameters.AddWithValue("UpdateDate", DateTime.Now);
            Cmd.Parameters.AddWithValue("UpdateId", Guid.NewGuid().ToString());

            Cmd.Parameters.AddWithValue("Year", null);
            Cmd.Parameters.AddWithValue("CertificateNumber", null);
            Cmd.Parameters.AddWithValue("Comment", null);
            Cmd.Parameters.AddWithValue("NewCertificateNumber", null);

            return Cmd;
        }


        private SqlCommand InsertDeniedCNECommand_;
        private SqlCommand InsertDeniedCNECommand
        {
            get
            {
                if (InsertDeniedCNECommand_ == null)
                {
                    InsertDeniedCNECommand_ = GetInsertDeniedCNECmd();
                }
                return InsertDeniedCNECommand_;
            }
            set { InsertDeniedCNECommand_ = value; }
        }



        public void AddToDB(DeniedCNE DeniedCNEToAdd)
        {
            InsertDeniedCNECommand.Parameters["Id"].Value = DeniedCNEToAdd.Id;

            InsertDeniedCNECommand.Parameters["Year"].Value = DeniedCNEToAdd.Year;
            InsertDeniedCNECommand.Parameters["CertificateNumber"].Value = DeniedCNEToAdd.CertificateNumber;
            InsertDeniedCNECommand.Parameters["Comment"].Value = DeniedCNEToAdd.Comment;
            InsertDeniedCNECommand.Parameters["NewCertificateNumber"].Value = DeniedCNEToAdd.NewCNENumber;

            InsertDeniedCNECommand.ExecuteNonQuery();
        }
    }
}
