using GVUZ.Web.Auth;
using System.Web.Mvc;
using System.Xml.Linq;
using System.Linq;
using System;
using System.Xml;
using System.Xml.Schema;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using GVUZ.Web.Models.Account;
using Newtonsoft.Json;
using upload.Models;

namespace upload.Controllers
{
    public class XMLController : Controller
    {
        public IMembershipService MembershipService { get; set; }

        [HttpPost, ValidateInput(false)]
        public ActionResult UploadXML(string inputXML, int debug = 0)
        {

            try
            {
                var filesXElement = XElement.Parse(inputXML);

                if (ValidateSchema(inputXML))
                {
                    var authData = filesXElement.Elements().Where(element => element.Name == "AuthData").Select(element => new
                    {
                        Login = element.Element("Login")?.Value.Trim(),
                        Password = element.Element("Pass")?.Value.Trim(),
                        InstitutionID = Convert.ToInt32(element.Element("InstitutionID")?.Value),
                        CampageUID = element.Element("CampageUID")?.Value.Trim(),
                        xmlGuid = Guid.NewGuid()
                    }).Single();

                    var xmlApplications = filesXElement.Elements().First(element => element.Name == "Enrollees");

                    if (!string.IsNullOrEmpty(authData.Login) && !string.IsNullOrEmpty(authData.Password))
                    {



                        var countEnrol = filesXElement
                            .Elements().Where(element => element.Name == "Enrollees")
                            .Elements().Count(el => el.Name == "Enrollee");

                        if (countEnrol == 0 || countEnrol > 20)
                        {
                            var colrow = debug == 10203099 ? " colrow:" + countEnrol : "";

                            return Json(new AnswerJson
                            {
                                SUCCESS = false,
                                GUIDS = new List<AnswerUIDS> { new AnswerUIDS { UID = "NULL", GUID = "NULL" } },
                                Message = "Не найдены анкеты абитуриентов или количество анкет абитуриентов превышает 20" + colrow
                            });
                        }


                        if (MembershipService == null)
                        {
                            MembershipService = new AccountMembershipService();
                        }

                        var checkResult = CheckEsrpAuth.CheckUserAccess(authData.Login, authData.Password);


                        //var checkResult = 0;

                        if (checkResult == 1)
                        {

                            List<AnswerUIDS> enr = new List<AnswerUIDS>();

                            using (var fileDb = new File_storageEntities())
                            {
                                foreach (XElement Enrollee in filesXElement.Element("Enrollees").Elements("Enrollee").OrderBy(x => x.Element("UID").Value.Trim()))
                                {
                                    Guid idRow = Guid.NewGuid();

                                    var enrUID = Enrollee.Element("UID").Value.Trim();


                                    enr.Add(new AnswerUIDS { UID = enrUID, GUID = idRow.ToString() });


                                    fileDb.Storage.Add(new Storage
                                    {
                                        id = idRow,
                                        UserLogin = authData.Login,
                                        InstitutionID = authData.InstitutionID,
                                        UID = enrUID,
                                        CreateDate = DateTime.Now,
                                        Body = Enrollee.ToString(),
                                        CampageUID = authData.CampageUID
                                    });

                                }


                                fileDb.SaveChanges();
                            }

                            return Json(new AnswerJson
                            {
                                SUCCESS = true,
                                GUIDS = enr,
                                Message = "Загрузка XML Успешна"
                            });
                        }
                        else
                        {
                            return Json(new AnswerJson
                            {
                                SUCCESS = false,
                                GUIDS = new List<AnswerUIDS> { new AnswerUIDS { UID = "NULL", GUID = "NULL" } },
                                Message = "В пакете содержаться не верные данные для авторизации"
                            });
                        }
                    }
                    else
                    {
                        return Json(new AnswerJson
                        {
                            SUCCESS = false,
                            GUIDS = new List<AnswerUIDS> { new AnswerUIDS { UID = "NULL", GUID = "NULL" } },
                            Message = "В пакете не содержаться данные для авторизации"
                        });
                    }
                }
                else
                {
                    return Json(new AnswerJson
                    {
                        SUCCESS = false,
                        GUIDS = new List<AnswerUIDS> { new AnswerUIDS { UID = "NULL", GUID = "NULL" } },
                        Message = "Неверная Схема XML или поле FileBytes не Base64"
                    });
                }
            }
            catch (Exception ex)
            {

                if (debug != 10203099)
                {
                    return Json(new AnswerJson
                    {
                        SUCCESS = false,
                        GUIDS = new List<AnswerUIDS> { new AnswerUIDS { UID = "NULL", GUID = "NULL" } },
                        Message = "Непредвиденная ошибка"
                    });
                }
                else
                {
                    return Json($"Exception : {ex.ToString()}");
                }
            }
        }

        public bool ValidateSchema(string inputXML)
        {
            XmlDocument xml = new XmlDocument();

            xml.LoadXml(inputXML);

            xml.Schemas.Add(null, ControllerContext.HttpContext.Server.MapPath("~/xml.xsd"));

            try
            {
                xml.Validate(null);
            }
            catch (XmlSchemaValidationException)
            {
                return false;
            }
            return true;
        }

        public class AnswerUIDS
        {
            public string UID { get; set; }
            public string GUID { get; set; }
        }

        public class AnswerJson
        {
            public bool SUCCESS { get; set; }
            public List<AnswerUIDS> GUIDS { get; set; }
            public string Message { get; set; }
        }


        public class AnswerFiles
        {
            public string Body;
            public string Message;
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult FilesFromXML(string idRow)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                string AnswBody = "";
                using (var fileDb = new File_storageEntities())
                {
                    Storage StorXML = fileDb.Storage.Where(x => x.id == new Guid(idRow)).First();

                    doc.LoadXml(StorXML.Body);

                    JsonResult preJson = Json(JsonConvert.SerializeXmlNode(doc).ToString());
                    preJson.MaxJsonLength = 90000000;
                    //return preJson;



                    List<FilesXML> filesFromDB = new List<FilesXML> { };

                    XElement files = XElement.Parse(StorXML.Body);
                    foreach (XElement xmlFile in files.Element("Files").Elements("File"))
                    {
                        FilesXML fileFromDB = new FilesXML();

                        fileFromDB.FileName = xmlFile.Element("FileName").Value.Trim();
                        fileFromDB.FileExtension = xmlFile.Element("FileExtension").Value.Trim();
                        fileFromDB.FileMimeType = xmlFile.Element("FileMimeType").Value.Trim();
                        fileFromDB.FileBody = xmlFile.Element("FileBytes").Value.Trim().Trim();

                        filesFromDB.Add(fileFromDB);
                    }

                    ViewBag.filesDB = filesFromDB;

                    ViewBag.Message = "Files From XML";
                    return View();
                }
            }
            catch (Exception Ex)
            {
                return Json(Ex.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
    }
}
