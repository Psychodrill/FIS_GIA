using System;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Fbs.Core.CatalogElements
{
    /// <summary>
    /// Класс, предоставляющий обобщенный доступ к каталогам (типы организаций, регионы..)
    /// </summary>
    public static  class CatalogDataAcessor  
    {
        /// <summary>
        /// Справочник допустимых каталогов
        /// </summary>
        public sealed class Catalogs
        {
            public const string Regions = "Region";
            public const string OrganizationKinds = "OrganizationKind";
            public const string OrganizationTypes= "OrganizationType2010";
            public const string UserGroups = "[Group]";
        }

        static readonly StringCollection AllowedCatalogs = new StringCollection();
        static CatalogDataAcessor()
        {
            AllowedCatalogs.AddRange(new string[] { "Region", "OrganizationKind", "OrganizationType2010","[Group]" });
        }
        public static DataTable GetAll(string catalogName)
        {
            if (!AllowedCatalogs.Contains(catalogName))
                throw new Exception("Данное имя справочника [" + catalogName + "] недопустимо");

            DataTable Result = new DataTable();
            using (SqlConnection Conn = new SqlConnection(DBSettings.ConnectionString))
            {
                Conn.Open();
                SqlCommand Cmd = Conn.CreateCommand();
                Cmd.CommandText = "SELECT * FROM " + catalogName;
                if (catalogName == Catalogs.OrganizationTypes)
                {
                    Cmd.CommandText += " ORDER BY SortOrder";
                }
                Result.Load(Cmd.ExecuteReader());
                Conn.Close();
            }
            return Result;
        }
    }

    /// <summary>
    /// Класс доступа к каталогу регионов
    /// </summary>
    public static class RegionDataAcessor 
    {
        /// <summary>
        /// Регионы, в которых могут располагаться организации из справочника организаций
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllInEtalon(string zeroItem)
        {
            DataTable Result = new DataTable();
            using (SqlConnection Conn = new SqlConnection(DBSettings.ConnectionString))
            {
                Conn.Open();
                SqlCommand Cmd = Conn.CreateCommand();
                if (!string.IsNullOrEmpty(zeroItem))
                {
                    Cmd.Parameters.AddWithValue("@zeroItem", zeroItem);
                    Cmd.CommandText = 
                        @"
                        select 
                            0 as Id,
                            @zeroItem as Name
                        union all
                        select
                            Id, 
                            Name
                        from 
                            Region 
                        where
                            InOrganizationEtalon = 1 
                        order by 
                            Name
                        ";
                }
                else
                {
                    Cmd.CommandText = "SELECT * FROM Region WHERE InOrganizationEtalon=1 ORDER BY Name";
                }
                Result.Load(Cmd.ExecuteReader());
                Conn.Close();
            }
            return Result;
        }

        public static DataTable GetAllInOrganization()
        {
            DataTable Result = new DataTable();
            using (SqlConnection Conn = new SqlConnection(DBSettings.ConnectionString))
            {
                Conn.Open();
                SqlCommand Cmd = Conn.CreateCommand();
                Cmd.CommandText = "SELECT * FROM Region WHERE InOrganization=1 ORDER BY Name";
                Result.Load(Cmd.ExecuteReader());
                Conn.Close();
            }
            return Result;
        }
    }

   /// <summary>
   /// Класс доступа к типам организаций (ВУЗ, ССУЗ и т.п.)
   /// </summary>
    public static class OrgTypeDataAccessor
    {
        /// <summary>
        /// Типы, доступные для регистрации пользователей
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllForAddNew()
        {
            DataTable Result = new DataTable();
            using (SqlConnection Conn = new SqlConnection(DBSettings.ConnectionString))
            {
                Conn.Open();
                SqlCommand Cmd = Conn.CreateCommand();
                Cmd.CommandText = "SELECT * FROM OrganizationType2010 WHERE Name !='РЦОИ' AND Name!='Орган управления образованием' AND Name!='Другое'";
                Result.Load(Cmd.ExecuteReader());
                Conn.Close();
            }
            return Result;
        }

        public static string GetName(string Id)
        {
            string Result = "";
            using (SqlConnection Conn = new SqlConnection(DBSettings.ConnectionString))
            {
                Conn.Open();
                SqlCommand Cmd = Conn.CreateCommand();
                Cmd.CommandText = "SELECT Name FROM OrganizationType2010 WHERE Id=@Id";
                Cmd.Parameters.AddWithValue("Id", Id);
                Result = Cmd.ExecuteScalar().ToString();
                Conn.Close();
            }
            return Result;
        }

        public static string GetName(int Id)
        {
            return GetName(Id.ToString());
        }
    }
}
