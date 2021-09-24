using System.ComponentModel;
using System.Net;
using System.Text;
using System.Web;

namespace GVUZ.Web.ViewModels.CompositionResults
{
    public class CompositionResultRecordViewModel
    {
        public int ApplicationId { get; set; }

        [DisplayName("Номер заявления")]
        public string ApplicationNumber { get; set; }

        [DisplayName("Фамилия")]
        public string LastName { get; set; }

        [DisplayName("Имя")]
        public string FirstName { get; set; }

        [DisplayName("Отчество")]
        public string MiddleName { get; set; }

        //[DisplayName("Серия документа, удостоверяющего личность")]
        [DisplayName("Серия документа")]
        public string IdentityDocumentSeries { get; set; }

        //[DisplayName("Номер документа, удостоверяющего личность")]
        [DisplayName("Номер документа")]
        public string IdentityDocumentNumber { get; set; }

        [DisplayName("Код темы сочинения")]
        public string CompositionCode { get; set; }

        [DisplayName("Тема сочинения")]
        public string CompositionTitle { get; set; }

        [DisplayName("Результат")]
        public bool? CompositionResult { get; set; }

        [DisplayName("Бланки сочинений")]
        public string BlankUrl
        {
            get
            {
                StringBuilder sb = new StringBuilder("http://vuz.ege.edu.ru/blanks?");
                sb.AppendFormat("Surname={0}", HttpUtility.UrlEncode(LastName));
                sb.AppendFormat("&FirstName={0}", HttpUtility.UrlEncode(FirstName));
                sb.AppendFormat("&Patronymic={0}", HttpUtility.UrlEncode(MiddleName));
                sb.AppendFormat("&Document={0}", HttpUtility.UrlEncode(IdentityDocumentNumber));

                return sb.ToString();
            }
        }

        public string BlankUrlText
        {
            get
            {
                StringBuilder sb = new StringBuilder("http://vuz.ege.edu.ru/blanks?");
                sb.AppendFormat("Surname={0}", LastName);
                sb.AppendFormat("&FirstName={0}", FirstName);
                sb.AppendFormat("&Patronymic={0}", MiddleName);
                sb.AppendFormat("&Document={0}", IdentityDocumentNumber);

                return sb.ToString();
            }
        }

        [DisplayName("Дата регистрации")]
        public string RegistrationDate { get; set; }

        [DisplayName("Результат")]
        public string CompositionResultText
        {
            get
            {
                if (CompositionResult.HasValue)
                {
                    return CompositionResult.GetValueOrDefault() ? "Зачет" : "Незачет";
                }

                return null;
            }
        }
    }
}