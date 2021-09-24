using System;
using System.Windows.Forms;
using Core;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace FBSLoader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            cmbYear.SelectedIndex = 0;
            txtBxSource2010.Text = Config.Source2010ConnectionString();
            txtBxSource2011.Text = Config.Source2011ConnectionString();
            txtBxEsrp.Text = Config.EsrpConnectionString();
            txtBxDest.Text = Config.DestConnectionString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ProcessManager manager = new ProcessManager(Convert.ToInt32(cmbYear.Text));
            manager.Do();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

            /*
            Config config = new Config(2011);

            Dictionary<string, string> source = new Dictionary<string, string>();
            HashSet<string> dest = new HashSet<string>();
            Dictionary<string, string> destCert = new Dictionary<string, string>();

            Dictionary<Int64, string> result = new Dictionary<Int64, string>();


            using (SqlConnection sourceConnection = new SqlConnection(config.SourceConnectionString()))
            using (SqlConnection destConnection = new SqlConnection(Config.DestConnectionString()))
            {
                sourceConnection.Open();
                destConnection.Open();
                 
                SqlCommand cmd1 = sourceConnection.CreateCommand();
                cmd1.CommandTimeout = 240;
                cmd1.CommandText =
                @"
                select
                    replace(
	                cast(EC.Id as nvarchar(100)) + '|' +
                    cast(month(EC.CreateDate) as nvarchar(100)) + cast(day(EC.CreateDate) as nvarchar(100)) + '|' +
                    cast(month(EC.UpdateDate) as nvarchar(100)) + cast(day(EC.UpdateDate) as nvarchar(100)) + '|' +
                    cast(EC.Number as nvarchar(100)) + '|' +
                    cast(coalesce(EC.TypographicNumber, '') as nvarchar(100)) + '|' +
                    cast(coalesce(EC.LastName, '') as nvarchar(100)) + '|' +
                    cast(coalesce(EC.FirstName, '') as nvarchar(100)) +  '|' +
                    cast(coalesce(EC.PatronymicName, '') as nvarchar(100)) + '|' +
                    cast(coalesce(EC.PassportSeria, '') as nvarchar(100)) +  '|' +
                    cast(coalesce(EC.PassportNumber, '') as nvarchar(100)) +  '|' +
                    cast(coalesce(EC.RegionId, 0) as nvarchar(100)) + '|' +
                    cast(cast(coalesce(A.Russian, -1) as numeric(18,0)) as nvarchar(100)) + '|' +
	                cast(cast(coalesce(A.Mathematics, -1) as numeric(18,0)) as nvarchar(100)) + '|' +
	                cast(cast(coalesce(A.Physics, -1) as numeric(18,0)) as nvarchar(100)) + '|' +
	                cast(cast(coalesce(A.Chemistry, -1) as numeric(18,0)) as nvarchar(100)) + '|' +
	                cast(cast(coalesce(A.Biology, -1) as numeric(18,0)) as nvarchar(100)) + '|' +
	                cast(cast(coalesce(A.RussiaHistory, -1) as numeric(18,0)) as nvarchar(100)) + '|' +
	                cast(cast(coalesce(A.Geography, -1) as numeric(18,0)) as nvarchar(100)) + '|' +
	                cast(cast(coalesce(A.English, -1) as numeric(18,0)) as nvarchar(100)) + '|' +
	                cast(cast(coalesce(A.German, -1) as numeric(18,0)) as nvarchar(100)) + '|' +
	                cast(cast(coalesce(A.Franch, -1) as numeric(18,0)) as nvarchar(100)) + '|' +
	                cast(cast(coalesce(A.SocialScience, -1) as numeric(18,0)) as nvarchar(100)) + '|' +
	                cast(cast(coalesce(A.Literature, -1) as numeric(18,0)) as nvarchar(100)) + '|' +
	                cast(cast(coalesce(A.Spanish, -1) as numeric(18,0)) as nvarchar(100)) + '|' +
	                cast(cast(coalesce(A.InformationScience, -1) as numeric(18,0)) as nvarchar(100)),
                    ',',
                    '.'),
                    EC.Id
                from 
	                CommonNationalExamCertificate EC
                    left join Region RG on RG.Id = EC.RegionId
                    left join 
	                (
	                select
		                CS.CertificateId,
		                max(case when CS.SubjectId = 1 then CS.Mark else null end) Russian,
		                max(case when CS.SubjectId = 2 then CS.Mark else null end) Mathematics,
		                max(case when CS.SubjectId = 3 then CS.Mark else null end) Physics,
		                max(case when CS.SubjectId = 4 then CS.Mark else null end) Chemistry,
		                max(case when CS.SubjectId = 5 then CS.Mark else null end) Biology,
		                max(case when CS.SubjectId = 6 then CS.Mark else null end) RussiaHistory,
		                max(case when CS.SubjectId = 7 then CS.Mark else null end) Geography,
		                max(case when CS.SubjectId = 8 then CS.Mark else null end) English,
		                max(case when CS.SubjectId = 9 then CS.Mark else null end) German,
		                max(case when CS.SubjectId = 10 then CS.Mark else null end) Franch,
		                max(case when CS.SubjectId = 11 then CS.Mark else null end) SocialScience,
		                max(case when CS.SubjectId = 12 then CS.Mark else null end) Literature,
		                max(case when CS.SubjectId = 13 then CS.Mark else null end) Spanish,
		                max(case when CS.SubjectId = 14 then CS.Mark else null end) InformationScience
	                from
		                CommonNationalExamCertificateSubject CS
	                group by
		                CS.CertificateId
                    ) A on A.CertificateId = EC.Id
                where
                    EC.Year = 2011
                ";
                SqlDataReader reader = cmd1.ExecuteReader();
                while (reader.Read())
                {
                    source.Add(reader[0].ToString(), reader[1].ToString());
                }
                reader.Close();

                SqlCommand cmd = destConnection.CreateCommand();
                cmd.CommandText =
                @"
                select
                    replace(
	                cast(C.CertId as nvarchar(100)) + '|' +
                    cast(month(C.CreateDate) as nvarchar(100)) + cast(day(C.CreateDate) as nvarchar(100)) + '|' +
                    cast(month(C.UpdateDate) as nvarchar(100)) + cast(day(C.UpdateDate) as nvarchar(100)) + '|' +
                    cast(C.CertNumber as nvarchar(100)) + '|' +
                    cast(coalesce(C.TypoNumber, '') as nvarchar(100)) + '|' +
                    cast(coalesce(C.LastName, '') as nvarchar(100)) + '|' +
                    cast(coalesce(C.FirstName, '') as nvarchar(100)) +  '|' +
                    cast(coalesce(C.PatronymicName, '') as nvarchar(100)) + '|' +
                    cast(coalesce(C.PassportSeria, '') as nvarchar(100)) +  '|' +
                    cast(coalesce(C.PassportNumber, '') as nvarchar(100)) +  '|' +
                    cast(coalesce(C.RegionId, 0) as nvarchar(100)) + '|' +
                    cast(coalesce(C.Russian, -1) as nvarchar(100)) + '|' +
	                cast(coalesce(C.Mathematics, -1) as nvarchar(100)) + '|' +
	                cast(coalesce(C.Physics, -1) as nvarchar(100)) + '|' +
	                cast(coalesce(C.Chemistry, -1) as nvarchar(100)) + '|' +
	                cast(coalesce(C.Biology, -1) as nvarchar(100)) + '|' +
	                cast(coalesce(C.RussiaHistory, -1) as nvarchar(100)) + '|' +
	                cast(coalesce(C.Geography, -1) as nvarchar(100)) + '|' +
	                cast(coalesce(C.English, -1) as nvarchar(100)) + '|' +
	                cast(coalesce(C.German, -1) as nvarchar(100)) + '|' +
	                cast(coalesce(C.Franch, -1) as nvarchar(100)) + '|' +
	                cast(coalesce(C.SocialScience, -1) as nvarchar(100)) + '|' +
	                cast(coalesce(C.Literature, -1) as nvarchar(100)) + '|' +
	                cast(coalesce(C.Spanish, -1) as nvarchar(100)) + '|' +
	                cast(coalesce(C.InformationScience, -1) as nvarchar(100)),
                    ',',
                    '.'),
                    C.CertId
                from 
	                Certificates C
	            where
					C.CertYear = 2011
                ";
                SqlDataReader reader1 = cmd.ExecuteReader();
                while (reader1.Read())
                {
                    dest.Add(reader1[0].ToString());
                    destCert.Add(reader1[1].ToString(), reader1[0].ToString());
                }
                reader1.Close();
            }


            int i = 0;
            int j = 0;
            foreach (KeyValuePair<string, string> pair in source)
            {
                if (!dest.Contains(pair.Key))
                {
                    if (destCert.ContainsKey(pair.Value))
                    {
                        i++;
                        //Logger.WriteLog(pair.Key);
                        //Logger.WriteLog(destCert[pair.Value]);
                        //Logger.WriteLog("");
                        //if (i > 1000)
                        //{
                        //    break;
                        //}
                    }
                }
            }

             */
        }
    }
}