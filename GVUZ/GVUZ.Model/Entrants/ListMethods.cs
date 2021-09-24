using System;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using GVUZ.DAL.Dapper.ViewModel.CompetitiveGroups;
using GVUZ.DAL.Dapper.ViewModel;
using GVUZ.DAL.Dapper;
using GVUZ.DAL;
using System.Configuration;
using System.Data.SqlClient;

namespace GVUZ.Model.Entrants
{
    public partial class EntrantsEntities
    {
        public List<DictionaryItem> GetCountries()
        {
            return (from c in CountryType
                    orderby c.DisplayOrder, c.Name
                    select new DictionaryItem { ID = c.CountryID, Name = c.Name, HasChild = c.HasRegions }).ToList();
        }

        public List<DictionaryItem> GetRegions(int countryID)
        {
            return (from r in RegionType
                    where r.CountryID == countryID
                    orderby r.DisplayOrder, r.Name
                    select new DictionaryItem { ID = r.RegionId, Name = r.Name }).ToList();
        }

        public List<string> GetCities(int regionID, string namePart)
        {
            //if (string.IsNullOrEmpty(namePart)) throw new ArgumentNullException("namePart");
            if (string.IsNullOrEmpty(namePart)) namePart = null;

            return (from c in CityType
                    where c.RegionID == regionID && (namePart == null || c.Name.Contains(namePart))
                    group c by c.Name into distinct
                    orderby distinct.Key
                    select distinct.Key).ToList();
        }

        public List<CompetitiveGroupProperty> GetCGProperties(int competitiveGroupId)
        {

            List<CompetitiveGroupProperty> innerList = new List<CompetitiveGroupProperty>();

            string commandText = string.Format(@"SELECT PropertyTypeCode, 
                                                        PropertyValue
                                                        FROM dbo.CompetitiveGroupProperties
                                                        Where CompetitiveGroupID = {0}", competitiveGroupId);

            SqlParameter[] parameters = new SqlParameter[1];
            parameters[0] = new SqlParameter("CompetitiveGroupID", competitiveGroupId);
            var _connectionString = ConfigurationManager.ConnectionStrings["Main"].ConnectionString;
            using (var connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(commandText, connection);
                command.Parameters.Add(new SqlParameter("CompetitiveGroupID", competitiveGroupId));

                try
                {
                    connection.Open();
                    SqlDataReader r = command.ExecuteReader();
                    CompetitiveGroupProperty cgp;
                    while (r.Read())
                    {
                        cgp = new CompetitiveGroupProperty();

                        cgp.PropertyTypeCode = (int)r["PropertyTypeCode"];
                        cgp.PropertyValue = r["PropertyValue"] as string;

                        innerList.Add(cgp);
                    }
                    r.Close();
                    connection.Close();
                }
                catch (SqlException e)
                {
                    throw e;
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    if (connection.State != ConnectionState.Closed) { connection.Close(); }
                }


            }
            return innerList;

        }


    }
}
