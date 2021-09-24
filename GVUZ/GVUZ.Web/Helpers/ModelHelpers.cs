using GVUZ.DAL.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace GVUZ.Web.Helpers
{
    public static class ModelHelpers
    {
        public static Dictionary<string, string> ToKeyValueDictionary(this ModelStateDictionary modelState)
        {
            return modelState.Where(x => !string.IsNullOrEmpty(x.Key))
                             .ToDictionary(k => k.Key,
                                           v => string.Join("\n", v.Value.Errors.Select(x => x.ErrorMessage)));
        }

        public static AttachmentCreateDto AsAttachmentCreateDto(this HttpPostedFileBase uploadedFile)
        {
            return uploadedFile != null ?
                   new AttachmentCreateDto
                   {
                       FileName = uploadedFile.FileName,
                       Content = uploadedFile.InputStream,
                       ContentLength = uploadedFile.ContentLength,
                       ContentType = uploadedFile.ContentType
                   } :
                   null;

        }

        /// <summary>
        /// Возвращает описание направления подготовки в виде строки
        /// </summary>
        /// <param name="dir">Сведения о направлении</param>
        /// <returns>Описание направления</returns>
        public static string DisplayName(this IDirectionDescription dir)
        {
            const int spoCode = DAL.Dapper.ViewModel.Dictionary.EDLevelConst.SPO; // SPO 17; // код СПО
            StringBuilder sb = new StringBuilder();
            //sb.Append(string.IsNullOrWhiteSpace(dir.Code) ? string.Empty : dir.Code.Trim());
            //sb.Append(".");
            //sb.Append(string.IsNullOrWhiteSpace(dir.QualificationCode) ? string.Empty : dir.QualificationCode.Trim());
            //sb.Append("/");
            sb.Append(string.IsNullOrWhiteSpace(dir.NewCode) ? string.Empty : dir.NewCode.Trim());
            
            if (!string.IsNullOrWhiteSpace(dir.DirectionName))
            {
                sb.AppendFormat(" - {0}", dir.DirectionName.Trim());
            }

            if (dir.EducationLevelId == spoCode && !string.IsNullOrWhiteSpace(dir.QualificationName) && dir.QualificationName != "null")
            {
                sb.AppendFormat(" ({0})", dir.QualificationName);
            }

            return sb.ToString().Trim();
        }
    }
}