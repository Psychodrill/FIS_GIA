using System;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using FogSoft.Helpers;
using GVUZ.Helper.Import;
using GVUZ.Model.Entrants;
using GVUZ.ServiceModel.Import.AppCheckProcessor;
using GVUZ.ServiceModel.Import.Core.Packages;
using GVUZ.ServiceModel.Import.Package;
using GVUZ.ServiceModel.Import.Schemas;
using GVUZ.ServiceModel.Import.WebService.Dto.Result;
using System.Configuration;
using System.Data.SqlClient;
using GVUZ.ImportService2016.Core.Main.Check;
using System.Collections.Generic;

namespace GVUZ.Web.Import.Infractrusture
{
    /// <summary>
    /// Проверка одного заявления
    /// </summary>
    public class DoCheckApplicationSingleService : ImportBaseService
    {
        public DoCheckApplicationSingleService(XElement data) : base(data) { }

        public override XElement ProcessData()
        {
            CheckAuth(false);
            ValidatePackage(XsdManager.XsdName.DoCheckApplicationSingleServiceRequest);

            var packageData = GetElementByNameWithThrowError("CheckApp");
            packageData = TrimPackageStrings(packageData);
            var applicationInfo = new Serializer().Deserialize<CheckApp>(packageData);

            LogHelper.Log.WarnFormat(">>> Проверка одного заявления (DoCheckApplicationSingleService) institutionId={0}", _institutionId);

            int applicationID = 0;
            int statusID = 0;
            if (applicationInfo != null)
            {
                var connectionString = ConfigurationManager.ConnectionStrings["Main"].ConnectionString;
                using (var connection = new SqlConnection(connectionString))
                {
                    var ds = new DataSet();

                    connection.Open();
                    string sql = "Select top(1) ApplicationID, StatusID from Application (NOLOCK) Where ApplicationNumber=@number And  CONVERT(date, RegistrationDate)=CONVERT(date, @regdate) And InstitutionID=@institutionID;";
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 60000; // минуты хватит?

                        cmd.Parameters.Add(new SqlParameter("@number", applicationInfo.ApplicationNumber));
                        cmd.Parameters.Add(new SqlParameter("@regdate", applicationInfo.RegistrationDateString));
                        cmd.Parameters.Add(new SqlParameter("@institutionID", _institutionId));

                        var adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(ds);

                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            applicationID = (int)ds.Tables[0].Rows[0][0];
                            statusID = (int)ds.Tables[0].Rows[0][1];
                        }

                        //var res = cmd.ExecuteScalar();
                        //if (res != null)
                        //    int.TryParse(res.ToString(), out applicationID);
                    }
                }
            }
            //Юсупов Кирилл: изменил интерфейс вызова ApplicationChecker на новую версию.
            //TODO: возможно, нужно передавать признак заявлений из Крыма 
            //и признак необходимости проверки ЕГЭ (для СПО не проверяем);
            //пока передаю их как Крым=false, проверка ЕГЭ=true
            var checker = new GVUZ.ImportService2016.Core.Main.Check.ApplicationChecker(null, new List<int>() {applicationID }, _login);
            checker.DoCheck();
            
            PackageManager.CreateInfoPackage(packageData.ToString(SaveOptions.None), _institutionId, PackageType.CheckApplicationSingle,
           _login, null, checker.CheckResultString);

            XElement results = XElement.Parse(checker.CheckResultString); 
            return results; //.AddInstitutionId(_institutionId).EmbraceToRoot();


            // Старый код!
            /* Проверяем заявление в ФБС */
            //EgeDocumentCheckResultDto crDto; GetEgeDocumentDto edDto;
            //try
            //{
            //    ApplicationChecker.CheckSingleApplication(_institutionId, applicationInfo, _login, out crDto, out edDto);
            //}
            //catch (DataException ex)
            //{
            //    return XmlImportHelper.GenerateErrorElement(ex.Message);
            //}

            //var results = GetXelement(new AppSingleCheckResult
            //{
            //    GetEgeDocuments = edDto,
            //    EgeDocumentCheckResults = crDto
            //});

            //PackageManager.CreateInfoPackage(packageData.ToString(SaveOptions.None), _institutionId, PackageType.CheckApplicationSingle,
            //    _login, null, results.ToString(SaveOptions.None));

            //return results.AddInstitutionId(_institutionId).EmbraceToRoot();
        }
    }
}