namespace Fbs.Core.Users
{
    using System.Data;
    using System.Data.SqlClient;

    /// <summary>
    /// Класс доступа к пользователям
    /// </summary>
    public static class OrgUserDataAccessor
    {
        #region Public Methods and Operators

        /// <summary>
        /// The get.
        /// </summary>
        /// <param name="userLogin">
        /// The user login.
        /// </param>
        /// <returns>
        /// </returns>
        public static OrgUser Get(string userLogin)
        {
            OrgUser Result = null;
            using (var Conn = new SqlConnection(DBSettings.ConnectionString))
            {
                Conn.Open();
                SqlCommand Cmd = Conn.CreateCommand();
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.CommandText = "dbo.GetUserAccount";

                Cmd.Parameters.Add(new SqlParameter("@login", SqlDbType.NVarChar, 255));
                Cmd.Parameters["@login"].Value = userLogin;

                using (SqlDataReader Reader = Cmd.ExecuteReader(CommandBehavior.SingleRow))
                {
                    if (Reader.Read())
                    {
                        Result = new OrgUser(Reader);
                    }

                    Reader.Close();
                }

                Conn.Close();
            }

            return Result;
        }

        /// <summary>
        /// Получение пользователя по заявленной им организации
        /// </summary>
        /// <param name="OrgId">
        /// </param>
        /// <returns>
        /// </returns>
        public static DataTable GetByOrgAsTable(int OrgId)
        {
            var Result = new DataTable();
            using (var Conn = new SqlConnection(DBSettings.ConnectionString))
            {
                SqlCommand Cmd = Conn.CreateCommand();
                Cmd.CommandText =
                    @"SELECT A.[Login], 
                        A.LastName+' '+A.FirstName+' '+A.PatronymicName FIO, A.[Email], A.[Status]
                        FROM dbo.Account A
                        INNER JOIN dbo.OrganizationRequest2010 OReq ON A.OrganizationId=OReq.Id
                        WHERE OReq.OrganizationId=@OrganizationId";

                Cmd.Parameters.AddWithValue("OrganizationId", OrgId);
                Conn.Open();
                Cmd.Connection = Conn;
                Result.Load(Cmd.ExecuteReader());
                Conn.Close();
            }

            return Result;
        }

        /// <summary>
        /// The update or create.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        public static void UpdateOrCreate(OrgUser user)
        {
            using (var Conn = new SqlConnection(DBSettings.ConnectionString))
            {
                Conn.Open();
                SqlCommand Cmd = Conn.CreateCommand();
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.CommandText = "dbo.UpdateUserAccount";

                Cmd.Parameters.Add(new SqlParameter("@login", SqlDbType.NVarChar, 255)).Direction =
                    ParameterDirection.InputOutput;
                Cmd.Parameters.Add(new SqlParameter("@lastName", SqlDbType.NVarChar, 255));
                Cmd.Parameters.Add(new SqlParameter("@firstName", SqlDbType.NVarChar, 255));
                Cmd.Parameters.Add(new SqlParameter("@patronymicName", SqlDbType.NVarChar, 255));
                Cmd.Parameters.Add(new SqlParameter("@phone", SqlDbType.NVarChar, 255));
                Cmd.Parameters.Add(new SqlParameter("@email", SqlDbType.NVarChar, 255));
                Cmd.Parameters.Add(new SqlParameter("@ipAddresses", SqlDbType.NVarChar, 4000));
                Cmd.Parameters.Add(new SqlParameter("@status", SqlDbType.NVarChar, 255)).Direction =
                    ParameterDirection.InputOutput;
                Cmd.Parameters.Add(new SqlParameter("@registrationDocument", SqlDbType.Image));
                Cmd.Parameters.Add(new SqlParameter("@registrationDocumentContentType", SqlDbType.NVarChar, 255));
                Cmd.Parameters.Add(new SqlParameter("@editorLogin", SqlDbType.NVarChar, 255));
                Cmd.Parameters.Add(new SqlParameter("@editorIp", SqlDbType.NVarChar, 255));
                Cmd.Parameters.Add(new SqlParameter("@password", SqlDbType.NVarChar, 255));
                Cmd.Parameters.Add(new SqlParameter("@passwordHash", SqlDbType.NVarChar, 255));
                Cmd.Parameters.Add(new SqlParameter("@hasFixedIp", SqlDbType.Bit));
                Cmd.Parameters.Add(new SqlParameter("@hasCrocEgeIntegration", SqlDbType.Bit));

                Cmd.Parameters.Add(new SqlParameter("@organizationRegionId", SqlDbType.Int));
                Cmd.Parameters.Add(new SqlParameter("@organizationFullName", SqlDbType.NVarChar, 2000));
                Cmd.Parameters.Add(new SqlParameter("@organizationShortName", SqlDbType.NVarChar, 2000));
                Cmd.Parameters.Add(new SqlParameter("@organizationINN", SqlDbType.NVarChar, 10));
                Cmd.Parameters.Add(new SqlParameter("@organizationOGRN", SqlDbType.NVarChar, 13));
                Cmd.Parameters.Add(new SqlParameter("@organizationFounderName", SqlDbType.NVarChar, 2000));
                Cmd.Parameters.Add(new SqlParameter("@organizationFactAddress", SqlDbType.NVarChar, 500));
                Cmd.Parameters.Add(new SqlParameter("@organizationLawAddress", SqlDbType.NVarChar, 500));
                Cmd.Parameters.Add(new SqlParameter("@organizationDirName", SqlDbType.NVarChar, 255));
                Cmd.Parameters.Add(new SqlParameter("@organizationDirPosition", SqlDbType.NVarChar, 500));
                Cmd.Parameters.Add(new SqlParameter("@organizationPhoneCode", SqlDbType.NVarChar, 255));
                Cmd.Parameters.Add(new SqlParameter("@organizationFax", SqlDbType.NVarChar, 255));
                Cmd.Parameters.Add(new SqlParameter("@organizationIsAccred", SqlDbType.Bit));
                Cmd.Parameters.Add(new SqlParameter("@organizationIsPrivate", SqlDbType.Bit));
                Cmd.Parameters.Add(new SqlParameter("@organizationIsFilial", SqlDbType.Bit));
                Cmd.Parameters.Add(new SqlParameter("@organizationAccredSert", SqlDbType.NVarChar, 255));
                Cmd.Parameters.Add(new SqlParameter("@organizationEMail", SqlDbType.NVarChar, 255));
                Cmd.Parameters.Add(new SqlParameter("@organizationSite", SqlDbType.NVarChar, 255));
                Cmd.Parameters.Add(new SqlParameter("@organizationPhone", SqlDbType.NVarChar, 255));
                Cmd.Parameters.Add(new SqlParameter("@organizationTypeId", SqlDbType.Int));
                Cmd.Parameters.Add(new SqlParameter("@organizationKindId", SqlDbType.Int));
                Cmd.Parameters.Add(new SqlParameter("@ExistingOrgId", SqlDbType.Int));

                // Cmd.Parameters.Add(new SqlParameter("@LOG", System.Data.SqlDbType.NVarChar,2000)).Direction=ParameterDirection.InputOutput ;
                Cmd.Parameters["@login"].Value = user.login;
                Cmd.Parameters["@lastName"].Value = user.lastName;
                Cmd.Parameters["@firstName"].Value = user.firstName;
                Cmd.Parameters["@patronymicName"].Value = user.patronymicName;
                Cmd.Parameters["@phone"].Value = user.phone;
                Cmd.Parameters["@email"].Value = user.email;
                Cmd.Parameters["@ipAddresses"].Value = user.ipAddresses;
                Cmd.Parameters["@registrationDocument"].Value = user.registrationDocument;
                Cmd.Parameters["@registrationDocumentContentType"].Value = user.registrationDocumentContentType;
                Cmd.Parameters["@editorLogin"].Value = user.editorLogin;
                Cmd.Parameters["@editorIp"].Value = user.editorIp;
                Cmd.Parameters["@password"].Value = user.password;
                Cmd.Parameters["@passwordHash"].Value = user.passwordHash;
                Cmd.Parameters["@hasFixedIp"].Value = user.hasFixedIp;
                Cmd.Parameters["@hasCrocEgeIntegration"].Value = user.hasCrocEgeIntegration;
                Cmd.Parameters["@status"].Value = user.statusCode;

                Cmd.Parameters["@organizationTypeId"].Value = user.RequestedOrganization.OrgType.Id;
                Cmd.Parameters["@organizationKindId"].Value = user.RequestedOrganization.Kind.Id;
                Cmd.Parameters["@organizationRegionId"].Value = user.RequestedOrganization.Region.Id;
                Cmd.Parameters["@organizationFullName"].Value = user.RequestedOrganization.FullName;
                Cmd.Parameters["@organizationShortName"].Value = user.RequestedOrganization.ShortName;
                Cmd.Parameters["@organizationINN"].Value = user.RequestedOrganization.INN;
                Cmd.Parameters["@organizationOGRN"].Value = user.RequestedOrganization.OGRN;
                Cmd.Parameters["@organizationFounderName"].Value = user.RequestedOrganization.OwnerDepartment;
                Cmd.Parameters["@organizationFactAddress"].Value = user.RequestedOrganization.FactAddress;
                Cmd.Parameters["@organizationLawAddress"].Value = user.RequestedOrganization.LawAddress;
                Cmd.Parameters["@organizationDirName"].Value = user.RequestedOrganization.DirectorFullName;
                Cmd.Parameters["@organizationDirPosition"].Value = user.RequestedOrganization.DirectorPosition;
                Cmd.Parameters["@organizationPhoneCode"].Value = user.RequestedOrganization.PhoneCityCode;
                Cmd.Parameters["@organizationFax"].Value = user.RequestedOrganization.Fax;
                Cmd.Parameters["@organizationIsAccred"].Value =
                    !string.IsNullOrEmpty(user.RequestedOrganization.AccreditationSertificate);
                Cmd.Parameters["@organizationIsPrivate"].Value = user.RequestedOrganization.IsPrivate;
                Cmd.Parameters["@organizationIsFilial"].Value = user.RequestedOrganization.IsFilial;
                Cmd.Parameters["@organizationAccredSert"].Value = user.RequestedOrganization.AccreditationSertificate;
                Cmd.Parameters["@organizationEMail"].Value = user.RequestedOrganization.EMail;
                Cmd.Parameters["@organizationSite"].Value = user.RequestedOrganization.Site;
                Cmd.Parameters["@organizationPhone"].Value = user.RequestedOrganization.Phone;
                Cmd.Parameters["@ExistingOrgId"].Value = user.RequestedOrganization.OrganizationId;

                Cmd.ExecuteNonQuery();

                // string LOG = Cmd.Parameters["@LOG"].Value.ToString();
                user.login = Cmd.Parameters["@login"].Value.ToString();
                user.status = UserAccount.ConvertStatusCode(Cmd.Parameters["@status"].Value.ToString());

                Conn.Close();
            }
        }

        #endregion
    }
}