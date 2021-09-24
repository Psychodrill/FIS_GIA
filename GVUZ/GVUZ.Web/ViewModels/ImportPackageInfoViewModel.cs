using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Web.Mvc;
using GVUZ.Helper;
using GVUZ.Web.Controllers;

namespace GVUZ.Web.ViewModels
{
    public class ImportPackageInfoViewModel
    {
        [DisplayName(@"ID")]
        public int PackageID { get; set; }

        [DisplayName(@"Имя учётной записи отправителя")]
        public string Login { get; set; }

        [DisplayName("Наименование ОО")]
        public string InstitutionName { get; set; }
        [DisplayName("Дата отправки")]
        public string DateSent { get; set; }
        [DisplayName("Дата обработки")]
        public string DateProcessing { get; set; }
        [DisplayName("Тип")]
        public string Type { get; set; }
        [DisplayName("Статус")]
        public string Status { get; set; }

        [DisplayName("Исходный пакет")]
        public string GetPackageString { get { return null; } }

        [DisplayName("Обработанных объектов")]
        public int CountProcessed { get; set; }
        [DisplayName("Не удалось обработать объектов")]
        public int CountNotProcessed { get; set; }
        [DisplayName("Ошибка")]
        public string ResultError { get; set; }

        [DisplayName("Комментарий")]
        public string Comment { get; set; }

        public class ErrorObjectsData
        {
            [DisplayName("Тип объекта")]
            public string ObjectType { get; set; }

            [DisplayName("Данные")]
            public string ObjectDetails { get; set; }

            [DisplayName("Код ошибки")]
            public string ErrorCode { get; set; }

            [DisplayName("Текст ошибки")]
            public string ErrorText { get; set; }
        }

        public ErrorObjectsData ErrorObjectsDataDescr
        {
            get { return null; }
        }

        public ErrorObjectsData[] ErrorObjects { get; set; }

        public class SuccessObjectsData
        {
            [DisplayName("Тип объекта")]
            public string ObjectType { get; set; }

            [DisplayName("Данные")]
            public string ObjectDetails { get; set; }

            [DisplayName("Ссылка")]
            public string LinkString { get { return null; } }

            public int InstitutionID { get; set; }


            private Func<UrlHelper, string> _link;


            public void SetLink<T>(Expression<Action<T>> link) where T : Controller
            {
                _link = (helper) => helper.Generate(link);
            }

            public string GetLink(UrlHelper helper)
            {
                var link = _link(helper);
                return link;
                //helper.Generate<InstitutionAdminController>(x => x.SwitchToInstitutionRedirect(InstitutionID, link));
                //http://zubrus.srvdev.ru/browse/FIS-837
            }
            public bool HasLink
            {
                get { return _link != null; }
            }
        }

        public SuccessObjectsData SuccessObjectsDataDescr
        {
            get { return null; }
        }

        public SuccessObjectsData[] SuccessObjects { get; set; }

        public int? SortID  {get; set;}
}
}