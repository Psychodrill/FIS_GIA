using System.Data.SqlClient;
using GVUZ.ServiceModel.Import.WebService;
using GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders.Base;

namespace GVUZ.ServiceModel.SQL.Dictionaries.DataLoaders
{
    /// <summary>
    /// Загрузка данных справочника "Направления подготовки" (код 10)
    /// </summary>
    public class DirectionDictionaryDataLoader : DictionaryDataLoaderBase<DirectionDictionaryItemDto>
    {
        private const string Query = @"
            --declare @institutionID int = 587;
            select
                dir.DirectionID,
                dir.Name,
                dir.Code,
                dir.NewCode,
                dir.QualificationCode,
                dir.Period,
                par.Code as UGSCode,
                par.Name as UGSName,
                par.ParentDirectionID
            from Direction dir
                left join ParentDirection par on dir.ParentID = par.ParentDirectionID
	            inner join AllowedDirections ad on ad.DirectionID = dir.DirectionID and ad.InstitutionID = @institutionID and AllowedDirectionStatusID in (2,3)
            --Where dir.IsVisible = 1
            order by DirectionID
        ";

        private int institutionID;
        public DirectionDictionaryDataLoader(int institutionID)
        {
            this.institutionID = institutionID;
        }

        protected override DirectionDictionaryItemDto MapDtoFromReader(SqlDataReader reader)
        {
            return new DirectionDictionaryItemDto
                {
                    DirectionID = reader.IsDBNull(0) ? null : reader[0].ToString().Trim(),
                    Name = reader.IsDBNull(1) ? null : reader[1].ToString().Trim(),
                    Code = reader.IsDBNull(2) ? null : reader[2].ToString().Trim(),
                    NewCode = reader.IsDBNull(3) ? null : reader[3].ToString().Trim(),
                    QualificationCode = reader.IsDBNull(4) ? null : reader[4].ToString().Trim(),
                    Period = reader.IsDBNull(5) ? null : reader[5].ToString().Trim(),
                    UGSCode = reader.IsDBNull(6) ? null : reader[6].ToString().Trim(),
                    UGSName = reader.IsDBNull(7) ? null : reader[7].ToString().Trim(),
                    ParentDirectionID = reader.IsDBNull(8) ? null : reader[8].ToString().Trim()
            };
        }

        protected override string GetQueryText()
        {
            return Query.Replace("@institutionID", this.institutionID.ToString());
        }
    }
}