using GdNet.Integrations.DropzoneMvc.Controllers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;
using upload.Models;

namespace upload.Controllers
{
    public class UploadController : Controller
    {
        // GET: Upload
        public ActionResult Index()
        {
            return View();
        }

        // POST: Commit
        [HttpPost]
        public ActionResult Commit(string campageUID, string enrUID)
        {
            try
            {
                campageUID = campageUID.Trim();
                enrUID = enrUID.Trim();

                string login = "";
                string password = "";

                int institutionID = 0;

                // Check permissions
                var checkResult = CheckEsrpAuth.CheckUserAccess(login, password);

                if (checkResult)
                {
                    string tempFilesFolder = Server.MapPath(ConfigurationManager.AppSettings["TempFilesRoot"]);
                    var files = new DropzoneMonitor(Request).GetActiveUploadedFiles(); // file will be the full path minis the TempFilesRoot

                    if (files == null || !files.Any())
                    {
                        ViewBag.Error = "Отсутствуют файлы для загрузки.";
                        return View("Error");
                    }

                    string currentFolder = new FileInfo(files.First()).Directory.FullName;

                    EnrolleeXML enrollee = new EnrolleeXML()
                    {
                        UID = enrUID,
                        Files = files.Select(x => new FilesXML()
                        {
                            FileName = Path.GetFileNameWithoutExtension(x),
                            FileMimeType = GetFileMimeType(x),
                            FileExtension = GetFileExtension(x),
                            FileBody = FileToBase64(x)
                        })
                        .ToArray()
                    };

                    using (var fileDb = new File_storageEntities())
                    {
                        foreach (var file in files)
                        {
                            Guid idRow = Guid.NewGuid();

                            //var enrUID = Enrollee.Element("UID").Value.Trim();

                            fileDb.Storage.Add(new Storage
                            {
                                id = idRow,
                                UserLogin = login,
                                InstitutionID = institutionID,
                                UID = enrUID,
                                CreateDate = DateTime.Now,
                                Body = SerializeToString(enrollee),
                                CampageUID = campageUID
                            });
                        }
                    }

                    DirectoryInfo di = new DirectoryInfo(currentFolder);
                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                    di.Delete();

                    return View("Success");
                }

                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.ToString();
                return View("Error");
            }
        }

        private string FileToBase64(string path)
        {
            Byte[] bytes = System.IO.File.ReadAllBytes(path);
            return Convert.ToBase64String(bytes);
        }

        private string GetFileMimeType(string path)
        {
            return MimeTypes.GetMimeType(path);
        }

        private string GetFileExtension(string path)
        {
            return Path.GetFileName(path).Split(new[] { '.' }).Last();
        }

        // To Clean XML
        private string SerializeToString<T>(T value)
        {
            var emptyNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var serializer = new XmlSerializer(value.GetType());
            var settings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = true
            };

            using (var stream = new StringWriter())
            using (var writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, value, emptyNamespaces);
                return stream.ToString();
            }
        }
    }
}