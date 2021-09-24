using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace FbsHashedCNEsReplicator
{
    public class CNESubjectMark
    {
        int RegionId_;
        public int RegionId
        {
            get { return RegionId_; }
            set { RegionId_ = value; }
        }
        int SubjectId_;
        public int SubjectId
        {
            get { return SubjectId_; }
            set { SubjectId_ = value; }
        }
        float Mark_;
        public float Mark
        {
            get { return Mark_; }
            set { Mark_ = value; }
        }

        bool HasAppeal_;
        public bool HasAppeal
        {
            get { return HasAppeal_; }
            set { HasAppeal_ = value; }
        }
        int Year_;
        public int Year
        {
            get { return Year_; }
            set { Year_ = value; }
        }

        public void AppentToSql(SqlCommand cmd, long  CNEId)
        {
            cmd.CommandText += @" INSERT INTO #CNEMark
           ([Id]
           ,[CertificateId]
           ,[SubjectId]
           ,[Mark]
           ,[HasAppeal]
           ,[Year]
           ,[RegionId])
     VALUES
           (@Id" + SubjectId.ToString() + @" 
           ,@CertificateId" + SubjectId.ToString() + @" 
           ,@SubjectId" + SubjectId.ToString() + @" 
           ,@Mark" + SubjectId.ToString() + @" 
           ,@HasAppeal" + SubjectId.ToString() + @" 
           ,@Year" + SubjectId.ToString() + @" 
           ,@RegionId" + SubjectId.ToString() + @")";

            cmd.Parameters.AddWithValue("Id" + SubjectId.ToString(), 0);
            cmd.Parameters.AddWithValue("CertificateId" + SubjectId.ToString(), CNEId);
            cmd.Parameters.AddWithValue("SubjectId" + SubjectId.ToString(), SubjectId);
            cmd.Parameters.AddWithValue("Mark" + SubjectId.ToString(), Mark);
            cmd.Parameters.AddWithValue("HasAppeal" + SubjectId.ToString(), HasAppeal);
            cmd.Parameters.AddWithValue("Year" + SubjectId.ToString(), Year);
            cmd.Parameters.AddWithValue("RegionId" + SubjectId.ToString(), RegionId);
        }
    }
}
