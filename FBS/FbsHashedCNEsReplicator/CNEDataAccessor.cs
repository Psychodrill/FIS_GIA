using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Fbs.Core.Loggers;

namespace FbsHashedCNEsReplicator
{
    public class CNEDataAccessor
    {
        private SqlConnection Conn_ = new SqlConnection();

        public CNEDataAccessor(string connectionString)
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

        public void DisableIndexes()
        {
            SqlCommand Cmd = Conn_.CreateCommand();
            Cmd.CommandText = @"ALTER TABLE [dbo].[CommonNationalExamCertificateSubject] DROP CONSTRAINT [CertificateSubjectPK]
                             
                            ALTER TABLE [dbo].[CommonNationalExamCertificate] DROP CONSTRAINT [CertificatePK]
                             
                            DROP INDEX [CNE_Id] ON [dbo].[CommonNationalExamCertificate] WITH ( ONLINE = OFF )
                             ";
            Cmd.ExecuteNonQuery();
        }

        public void EnableIndexes()
        {
            SqlCommand Cmd = Conn_.CreateCommand();
            Cmd.CommandText = @"ALTER TABLE [dbo].[CommonNationalExamCertificate] ADD  CONSTRAINT [CertificatePK] PRIMARY KEY CLUSTERED 
                            (
	                            [Year] ASC,
	                            [Id] ASC
                            )WITH (PAD_INDEX  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, FILLFACTOR = 90, ONLINE = OFF) ON [PRIMARY]
 
                            CREATE NONCLUSTERED INDEX [CNE_Id] ON [dbo].[CommonNationalExamCertificate] 
                            (
	                            [Id] ASC
                            )WITH (PAD_INDEX  = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF) ON [PRIMARY]

                            ALTER TABLE [dbo].[CommonNationalExamCertificateSubject] ADD  CONSTRAINT [CertificateSubjectPK] PRIMARY KEY CLUSTERED 
                            (
	                            [Year] ASC,
	                            [CertificateId] ASC,
	                            [SubjectId] ASC
                            )WITH (PAD_INDEX  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, FILLFACTOR = 90, ONLINE = OFF) ON [PRIMARY]
                            ";
            Cmd.ExecuteNonQuery();
        }

        public void CreateTempTables()
        {
            SqlCommand Cmd = Conn_.CreateCommand();
            Cmd.CommandText = @"CREATE TABLE #CNE(
	[Id] [bigint] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[UpdateId] [uniqueidentifier] NOT NULL,
	[EditorAccountId] [bigint] NOT NULL,
	[EditorIp] [nvarchar](255) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[Number] [nvarchar](255) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[EducationInstitutionCode] [nvarchar](255) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[Year] [int] NOT NULL,
	[LastName] [nvarchar](255) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[FirstName] [nvarchar](255) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[PatronymicName] [nvarchar](255) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[Sex] [bit] NOT NULL,
	[Class] [nvarchar](255) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[InternalPassportSeria] [nvarchar](255) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[PassportSeria] [nvarchar](255) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[PassportNumber] [nvarchar](255) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[EntrantNumber] [nvarchar](255) COLLATE Cyrillic_General_CI_AS NULL,
	[RegionId] [int] NOT NULL,
	[TypographicNumber] [nvarchar](255) COLLATE Cyrillic_General_CI_AS NULL)

CREATE TABLE #CNEMark(
	[Id] [bigint] NOT NULL,
	[CertificateId] [bigint] NOT NULL,
	[SubjectId] [bigint] NOT NULL,
	[Mark] [numeric](5, 1) NULL,
	[HasAppeal] [bit] NOT NULL,
	[Year] [int] NOT NULL,
	[RegionId] [int] NOT NULL)";
            Cmd.ExecuteNonQuery();
        }

        public void DropTempTables()
        {
            SqlCommand Cmd = Conn_.CreateCommand();
            Cmd.CommandText = @"DROP TABLE #CNE 
                            DROP TABLE #CNEMark";
            Cmd.ExecuteNonQuery();
        }

        public void ReplicateTables(ILogger logger)
        {
            SqlCommand Cmd = Conn_.CreateCommand();
            Cmd.CommandTimeout = 10000000;
            Cmd.CommandText = @"UPDATE CNE SET 
                            CNE.LastName = ToUpdate.LastName,
	                        CNE.FirstName = ToUpdate.FirstName,
	                        CNE.PatronymicName = ToUpdate.PatronymicName,
	                        CNE.Sex = ToUpdate.Sex,
	                        CNE.Class = ToUpdate.Class,
	                        CNE.InternalPassportSeria = ToUpdate.InternalPassportSeria,
	                        CNE.PassportSeria = ToUpdate.PassportSeria,
	                        CNE.PassportNumber = ToUpdate.PassportNumber,
	                        CNE.EntrantNumber = ToUpdate.EntrantNumber,
	                        CNE.RegionId = ToUpdate.RegionId,
	                        CNE.EducationInstitutionCode = ToUpdate.EducationInstitutionCode,
	                        CNE.UpdateDate = GETDATE(),
	                        CNE.TypographicNumber = ToUpdate.TypographicNumber
                            FROM 
                                (SELECT [Id],
	                            [Year],
	                            [Number],
	                            [EducationInstitutionCode],
	                            [LastName] collate Cyrillic_General_CS_AS [LastName],
	                            [FirstName] collate Cyrillic_General_CS_AS [FirstName],
	                            [PatronymicName] collate Cyrillic_General_CS_AS [PatronymicName],
	                            [Sex],
	                            [Class],
	                            [InternalPassportSeria],
	                            [PassportSeria],
	                            [PassportNumber],
	                            [EntrantNumber],
	                            [RegionId],
	                            [TypographicNumber] FROM #CNE  AS TempCNE
	                            EXCEPT
	                            SELECT [Id],
		                        [Year],
		                        [Number], 
		                        [EducationInstitutionCode],
		                        [LastName] collate Cyrillic_General_CS_AS [LastName],
		                        [FirstName] collate Cyrillic_General_CS_AS [FirstName],
		                        [PatronymicName] collate Cyrillic_General_CS_AS [PatronymicName],
		                        [Sex],
		                        [Class],
		                        [InternalPassportSeria],
		                        [PassportSeria],
		                        [PassportNumber],
		                        [EntrantNumber],
		                        [RegionId],
		                        [TypographicNumber] 
	                            FROM [CommonNationalExamCertificate] AS OldCNE) ToUpdate
	                        INNER JOIN CommonNationalExamCertificate CNE on ToUpdate.Id = CNE.Id and ToUpdate.Year = CNE.Year";
            Cmd.ExecuteNonQuery();
            logger.WriteMessage("Existing CNEs updated " + DateTime.Now.ToLongTimeString());

            Cmd.CommandText = @"INSERT INTO CommonNationalExamCertificate 
                            SELECT ToInsert.* FROM #CNE ToInsert
                            LEFT JOIN CommonNationalExamCertificate CNE on CNE.Id = ToInsert.Id and CNE.Year = ToInsert.Year
	                        WHERE CNE.Id IS NULL";
            Cmd.ExecuteNonQuery();
            logger.WriteMessage("New CNEs inserted " + DateTime.Now.ToLongTimeString());
            logger.WriteMessage("CNEs replication succeed");

            Cmd.CommandText = @"UPDATE CNEMark SET 
                            CNEMark.Mark = ToUpdate.Mark,
                            CNEMark.HasAppeal = ToUpdate.HasAppeal,
                            CNEMark.RegionId = ToUpdate.RegionId  
                            FROM CommonNationalExamCertificateSubject CNEMark
                            INNER JOIN 
                                (SELECT [CertificateId],
                                [SubjectId],
                                [Mark],
                                [HasAppeal],
                                [Year],
                                [RegionId] from #CNEMark as TempCNEMarks
                                EXCEPT
                                SELECT
                                [CertificateId],
                                [SubjectId],
                                [Mark],
                                [HasAppeal],
                                [Year],
                                [RegionId]
                            FROM [CommonNationalExamCertificateSubject] AS OldCNEMarks ) AS ToUpdate
                            ON ToUpdate.CertificateId = CNEMark.CertificateId 
                            AND ToUpdate.SubjectId = CNEMark.SubjectId 
                            AND ToUpdate.Year = CNEMark.Year";

            Cmd.ExecuteNonQuery();
            logger.WriteMessage("Existing CNE marks updated " + DateTime.Now.ToLongTimeString());

            Cmd.CommandText = @"INSERT INTO CommonNationalExamCertificateSubject
                            SELECT ToInsert.* 
                            FROM #CNEMark ToInsert 
                            LEFT JOIN CommonNationalExamCertificateSubject CNEMark
                            ON ToInsert.CertificateId = CNEMark.CertificateId 
                            AND ToInsert.SubjectId = CNEMark.SubjectId 
                            AND ToInsert.Year = CNEMark.Year 
                            WHERE CNEMark.CertificateId IS NULL";

            Cmd.ExecuteNonQuery();
            logger.WriteMessage("New CNE marks inserted " + DateTime.Now.ToLongTimeString());

            Cmd.CommandText = @"DELETE CNEMark
                            FROM CommonNationalExamCertificateSubject CNEMark
                            LEFT JOIN #CNEMark ToDelete
                            ON CNEMark.CertificateId = ToDelete.CertificateId
                            AND CNEMark.SubjectId = ToDelete.SubjectId
                            AND CNEMark.Year = ToDelete.Year
                            WHERE ToDelete.CertificateId IS NULL
                            AND EXISTS ( 
                                SELECT TOP 1 1 
                                FROM #CNEMark 
                                WHERE CertificateId = CNEMark.CertificateId and Year = CNEMark.Year)";

            Cmd.ExecuteNonQuery();
            logger.WriteMessage("Obsolete CNE marks deleted " + DateTime.Now.ToLongTimeString());
            logger.WriteMessage("CNEMarks replication succeed");

            Cmd.ExecuteNonQuery();
        }

        private SqlCommand GetInsertCNECmd()
        {
            SqlCommand Cmd = Conn_.CreateCommand();
            Cmd.CommandText = @"INSERT INTO #CNE
           ([Id]
           ,[CreateDate]
           ,[UpdateDate]
           ,[UpdateId]
           ,[EditorAccountId]
           ,[EditorIp]
           ,[Number]
           ,[EducationInstitutionCode]
           ,[Year]
           ,[LastName]
           ,[FirstName]
           ,[PatronymicName]
           ,[Sex]
           ,[Class]
           ,[InternalPassportSeria]
           ,[PassportSeria]
           ,[PassportNumber]
           ,[EntrantNumber]
           ,[RegionId]
           ,[TypographicNumber])
     VALUES
           (@Id 
           ,@CreateDate
           ,@UpdateDate
           ,@UpdateId 
           ,@EditorAccountId 
           ,@EditorIp 
           ,@Number 
           ,@EducationInstitutionCode 
           ,@Year 
           ,@LastName 
           ,@FirstName 
           ,@PatronymicName 
           ,@Sex 
           ,@Class 
           ,@InternalPassportSeria 
           ,@PassportSeria 
           ,@PassportNumber 
           ,@EntrantNumber 
           ,@RegionId 
           ,@TypographicNumber)";

            Cmd.Parameters.AddWithValue("Id", null);
            Cmd.Parameters.AddWithValue("CreateDate", DateTime.Now);
            Cmd.Parameters.AddWithValue("UpdateDate", DateTime.Now);
            Cmd.Parameters.AddWithValue("UpdateId", Guid.NewGuid().ToString());
            Cmd.Parameters.AddWithValue("EditorAccountId", 0);
            Cmd.Parameters.AddWithValue("EditorIp", "127.0.0.1");
            Cmd.Parameters.AddWithValue("Number", null);
            Cmd.Parameters.AddWithValue("EducationInstitutionCode", null);
            Cmd.Parameters.AddWithValue("Year", null);

            Cmd.Parameters.AddWithValue("LastName", null);
            Cmd.Parameters.AddWithValue("FirstName", null);
            Cmd.Parameters.AddWithValue("PatronymicName", null);

            Cmd.Parameters.AddWithValue("Sex", null);
            Cmd.Parameters.AddWithValue("Class", null);
            Cmd.Parameters.AddWithValue("InternalPassportSeria", null);
            Cmd.Parameters.AddWithValue("PassportSeria", null);
            Cmd.Parameters.AddWithValue("PassportNumber", null);
            Cmd.Parameters.AddWithValue("EntrantNumber", null);
            Cmd.Parameters.AddWithValue("RegionId", null);
            Cmd.Parameters.AddWithValue("TypographicNumber", null);

            return Cmd;
        }
   

        private SqlCommand InsertCNECommand_;
        private SqlCommand InsertCNECommand
        {
            get
            {
                if (InsertCNECommand_ == null)
                {
                    InsertCNECommand_ = GetInsertCNECmd();
                }
                return InsertCNECommand_;
            }
            set { InsertCNECommand_ = value; }
        }

        

        public void AddToDB(CNE CNEToAdd)
        {
            InsertCNECommand.Parameters["Id"].Value = CNEToAdd.Id;
         
            InsertCNECommand.Parameters["Number"].Value = CNEToAdd.Number;
            InsertCNECommand.Parameters["EducationInstitutionCode"].Value = CNEToAdd.EducationInstitutionCode;
            InsertCNECommand.Parameters["Year"].Value = CNEToAdd.Year;

            InsertCNECommand.Parameters["LastName"].Value = CNEToAdd.LastName;
            InsertCNECommand.Parameters["FirstName"].Value = CNEToAdd.FirstName;
            InsertCNECommand.Parameters["PatronymicName"].Value = CNEToAdd.PatronymicName;

            InsertCNECommand.Parameters["Sex"].Value = CNEToAdd.Sex;
            InsertCNECommand.Parameters["Class"].Value = CNEToAdd.Class;
            InsertCNECommand.Parameters["InternalPassportSeria"].Value = CNEToAdd.PassportSeria;
            InsertCNECommand.Parameters["PassportSeria"].Value = CNEToAdd.PassportSeria;
            InsertCNECommand.Parameters["PassportNumber"].Value = CNEToAdd.PassportNumber;
            InsertCNECommand.Parameters["EntrantNumber"].Value = CNEToAdd.EntrantNumber;
            InsertCNECommand.Parameters["RegionId"].Value = CNEToAdd.RegionId;
            InsertCNECommand.Parameters["TypographicNumber"].Value = CNEToAdd.TypographicNumber;

            InsertCNECommand.ExecuteNonQuery();

            SqlCommand InsCmd = Conn_.CreateCommand();
            foreach (CNESubjectMark Mark in CNEToAdd.SubjectMarks)
            {
                Mark.AppentToSql(InsCmd, CNEToAdd.Id);
            }
            if (CNEToAdd.SubjectMarks.Count > 0)
                InsCmd.ExecuteNonQuery();
        }
    }
}
