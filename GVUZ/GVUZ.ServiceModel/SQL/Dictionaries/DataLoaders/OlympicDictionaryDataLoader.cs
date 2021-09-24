using System.Collections.Generic;
using System.Data.SqlClient;
using GVUZ.ServiceModel.Import.WebService;
using GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders.Base;
using System.Linq;

namespace GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders
{
    /// <summary>
    /// Загрузка данных справочника "Олимпиады" (код 19)
    /// </summary>
    public class OlympicDictionaryDataLoader : SqlDictionaryDataLoaderBase<OlympicDictionaryItemDto>
    {
        private const string SelectOlympicsQuery = @"
            select 
                ol.OlympicID,
                ol.Name,
                --ol.OlympicLevelID,
                ol.OlympicNumber,
                ol.OlympicYear
                from [OlympicType] ol";

        private const string SelectSubjectsQuery = @"
            select
	            otp.OlympicTypeID
	            ,os.SubjectID
	            --,isnull(ol.Name, '') as LevelName
                --,otp.OlympicLevelID
                ,case when otp.OlympicLevelID = 1 then 255 
					when otp.OlympicLevelID = 2 then 1
					when otp.OlympicLevelID = 3 then 2
					when otp.OlympicLevelID = 4 then 4
					else null end as 'OlympicLevelID'
                ,otp.OlympicProfileID
            from 
                OlympicTypeProfile otp
                left join OlympicType ot on otp.OlympicTypeID = ot.OlympicID
	            --left join OlympicLevel ol on ol.OlympicLevelID = otp.OlympicLevelID
	            left join OlympicSubject os on os.OlympicTypeProfileID = otp.OlympicTypeProfileID
            ";

        public override OlympicDictionaryItemDto[] Load()
        {
            List<OlympicDictionaryItemDto> dto = new List<OlympicDictionaryItemDto>();

            using (SqlCommand cmd = new SqlCommand(SelectOlympicsQuery))
            {
                cmd.Connection = GetConnection();
                cmd.Transaction = GetTransaction();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dto.Add(MapOlympicFromReader(reader));
                    }
                }
            }

            //Dictionary<string, List<SubjectLevel>> subjectMap = new Dictionary<string, List<SubjectLevel>>(dto.Count);

            using (SqlCommand cmd = new SqlCommand(SelectSubjectsQuery))
            {
                cmd.Connection = GetConnection();
                cmd.Transaction = GetTransaction();

                using (
                    SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var olympicId = reader[0].ToString();
                        var ot = dto.FirstOrDefault(t => t.OlympicID == olympicId);

                        var sID = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                        var lID = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
                        var pID = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);

                        var profile = new Profile()
                        {
                            ProfileID = pID,
                            LevelID = lID,
                            Subjects = new OlympicSubject[] { }
                        };
                        var subject = new OlympicSubject() { SubjectID = sID };

                        if (ot.Profiles == null)
                            ot.Profiles = new Profile[] { };
                        var otp = ot.Profiles.FirstOrDefault(t => t.ProfileID == profile.ProfileID);
                        if (otp == null)
                        {
                            var otProfiles = ot.Profiles.ToList();
                            otProfiles.Add(profile);
                            ot.Profiles = otProfiles.ToArray();
                            otp = profile;
                        }

                        var otSubjects = otp.Subjects.ToList();
                        otSubjects.Add(subject);
                        otp.Subjects = otSubjects.ToArray();


                        //List<SubjectLevel> container;

                        //if (subjectMap.TryGetValue(olympicId, out container))
                        //{
                        //    container.Add(subject);
                        //}
                        //else
                        //{
                        //    subjectMap.Add(olympicId, new List<SubjectLevel> {subject});
                        //}
                    }
                }
            }

            //for (int i = 0; i < dto.Count; i++)
            //{
            //    var item = dto[i];

            //    List<SubjectLevel> subjects;

            //    if (subjectMap.TryGetValue(item.OlympicID, out subjects))
            //    {
            //        item.Subjects = subjects.ToArray();
            //    }
            //}

            return dto.ToArray();
        }

        private static OlympicDictionaryItemDto MapOlympicFromReader(SqlDataReader reader)
        {
            return new OlympicDictionaryItemDto
                {
                    OlympicID = reader.IsDBNull(0) ? null : reader[0].ToString().Trim(),
                    OlympicName = reader.IsDBNull(1) ? null : reader[1].ToString().Trim(),
                    //OlympicLevelID = reader.IsDBNull(2) ? null : reader[2].ToString().Trim(),
                    OlympicNumber = reader.IsDBNull(2) ? null : reader[2].ToString().Trim(),
                    Year = reader.IsDBNull(3) ? null : reader[3].ToString().Trim()
                };
        }

        //private static OlympicSubject MapSubjectFromReader(SqlDataReader reader)
        //{
        //    return new OlympicSubject
        //    {
        //        SubjectID = reader.IsDBNull(1) ? 0 : reader.GetInt32(1), // reader.GetInt32(1),
        //                                                                 //LevelID = reader.IsDBNull(2) ? null : reader[2].ToString()
        //    };
        //}
    }
}