using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Admin.DBContext;
using Admin.Models;
using Admin.Models.DBContext;
using System.IO;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Http;

namespace Admin.Data
{
    public class InstitutionRepository
    {

        //Информация об ОО

        public static Institution GetInstitutesDetails(int InstitutionId, ApplicationContext db)
        {
            Institution details = new Institution();
            try
            {
                details = db.Institution
                    .Where(p => p.InstitutionId == InstitutionId)
                    .FirstOrDefault();
            }
            catch (Exception e)
            {
                string error = e.Message.ToString();
            }
            return details;
        }

        //Получение лицензии
        public static InstitutionLicense GetLicense(int InstitutionId, ApplicationContext db)
        {
            InstitutionLicense license = new InstitutionLicense();
            try
            {
                license = db.InstitutionLicense
                    .Where(p => p.InstitutionId == InstitutionId)
                    //.Include(p => p.InstitutionAttachment)
                    .FirstOrDefault();

                if (license != null)
                {
                InstitutionAttachment attachment = db.InstitutionAttachment
                    .Where(p => p.AttachmentId == license.AttachmentId)
                    .FirstOrDefault();

                license.InstitutionAttachment = attachment;
                }
            }
            catch (Exception e)
            {
                string error = e.Message.ToString();
            }
            return license;
        }

        //Получение аккредитации
        public static InstitutionAccreditation GetAccreditation(int InstitutionId, ApplicationContext db)
        {
            InstitutionAccreditation accreditation = new InstitutionAccreditation();
            try
            {
                accreditation = db.InstitutionAccreditation
                    .Where(p => p.InstitutionId == InstitutionId)
                   // .Include(p => p.InstitutionAttachment)
                    .FirstOrDefault();

                if (accreditation != null)
                {
                InstitutionAttachment attachment = db.InstitutionAttachment
                .Where(p => p.AttachmentId == accreditation.AttachmentId)
                .FirstOrDefault();

                accreditation.InstitutionAttachment = attachment;
                }
            }
            catch (Exception e)
            {
                string error = e.Message.ToString();
            }
            return accreditation;
        }

        //Обновление лицензии
        public static string SaveLicense(int institution_id, string license_number, DateTime license_date, ApplicationContext db)
        {
            try
            {
                var license = db.InstitutionLicense
                        .Where(p => p.InstitutionId == institution_id)
                        .FirstOrDefault();
                if (license != null)
                {
                    license.LicenseNumber = license_number;
                    license.LicenseDate = license_date;
                }
                else 
                {
                    InstitutionLicense il = new InstitutionLicense 
                    { 
                        InstitutionId = institution_id, 
                        LicenseNumber = license_number, 
                        LicenseDate = license_date, 
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now
                    };
                    db.InstitutionLicense.Add(il);
                }
                db.SaveChanges();
            }
            catch (Exception e)
            {
                string error = e.Message.ToString();
                return "<span id='result' style='color: red; '>Ошибка при сохранении лицензии</span>";
            }
            return "<span id='result' style='color: green; '>Информация о лицензии сохранена</span>";
        }

        //Обновление аккредитации
        public static string SaveAccreditation(int institution_id, string accreditation_number, ApplicationContext db)
        {
            try
            {
                InstitutionAccreditation accreditation = db.InstitutionAccreditation
                        .Where(p => p.InstitutionId == institution_id)
                        .FirstOrDefault();
                if (accreditation != null)
                {
                    accreditation.Accreditation = accreditation_number;
                }
                else
                {
                    InstitutionAccreditation ia = new InstitutionAccreditation
                    {
                        InstitutionId = institution_id, 
                        Accreditation = accreditation_number,
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now
                    };
                    db.InstitutionAccreditation.Add(ia);
                }
                db.SaveChanges();
            }
            catch (Exception e)
            {
                string error = e.Message.ToString();
                return "<span id='result' style='color: red; '>Ошибка при сохранении аккредитации</span>";
            }
            return "<span id='result' style='color: green; '>Информация об аккредитации сохранена</span>";
        }

        public static InstitutionAttachment GetAttachment (IFormFile upload)
        {
            string fileName = System.IO.Path.GetFileName(upload.FileName);
            string contentType;
            new FileExtensionContentTypeProvider().TryGetContentType(fileName, out contentType);

            var attachment = new InstitutionAttachment()
            {
                Name = fileName,
                MimeType = contentType
            };
            using (var ms = new MemoryStream())
            {
                upload.CopyTo(ms);
                attachment.Body = ms.ToArray();
            }
            return attachment;
        }


        /*
        public static List<string> GetYears (int institution_id, ApplicationContext db)
        {
            List<string> list = new List<string>();
            try
            {
                var years = db.InstitutionDocuments
                        .Where(p => p.InstitutionId == institution_id)
                        .Select(p => new
                        {
                            Year = p.Year
                        })
                        .Distinct()
                        .OrderBy(p => p.Year);
                if (years != null)
                {
                    foreach(var item  in years)
                    {
                        list.Add(item.Year.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                string error = e.Message.ToString();
            }
            return list;
        }
        */

        //TODO: переделать под Attachment
        public static List<InstitutionDocs> GetDocuments(int institution_id, int year, ApplicationContext db)
        {
            List<InstitutionDocs> list = new List<InstitutionDocs>();
            try
            {                
                var doc =
                        from a in db.InstitutionDocuments
                        join b in db.Attachment
                        on a.AttachmentId equals
                        b.AttachmentId
                        where a.Year == year && a.InstitutionId == institution_id
                        select new
                        {
                            AttachmentId = a.AttachmentId,
                            DocumentName = b.Name,
                            DisplayName = b.DisplayName
                        };

                if (doc != null)
                {
                    foreach (var item in doc)
                    {
                        InstitutionDocs document = new InstitutionDocs();
                        document.AttachmentId = (int)item.AttachmentId;
                        document.DocumentName = item.DocumentName;
                        document.DisplayName = item.DisplayName;
                        if (document != null) { list.Add(document); }
                    }
                }
                
            }
            catch (Exception e)
            {
                string error = e.Message.ToString();
            }
            return list;
        }


    }
}
