using System;
using System.Linq;
using System.Xml.Linq;
using GVUZ.Helper.Import;
using GVUZ.ServiceModel.Import;
using GVUZ.ServiceModel.Import.Bulk.Attributes;
using GVUZ.ServiceModel.Import.Core.Packages;
using GVUZ.ServiceModel.Import.Package;
using GVUZ.ServiceModel.Import.Schemas;
using GVUZ.Web.Auth;
using GVUZ.Web.Import.Infractrusture.Exceptions;

namespace GVUZ.Web.Import.Infractrusture
{
    public abstract class ImportBaseService : IDisposable
    {
        public XElement _data;
        public int _institutionId;
        public string _login;

        public bool IsIncludeFilials 
        {
            get { return GetElementByName(_data, "InstitutionID", false) == null; }
        }

        public XElement GetXelement<T>(T o)
        {
            return XElement.Parse(PackageHelper.GenerateXmlPackageIntoString(o));
        }

        public XElement GetElementByName(XElement serviceData, string elementName, bool elementIsNotNull)
        {
            var el = XmlImportHelper.GetXmlStringForElement(serviceData, elementName);
            if (el == null && elementIsNotNull) throw new ImportXmlElementNotFoudException(elementName);
            return el;
        }

        public XElement GetElementByNameWithThrowError(string elementName)
        {
            return GetElementByName(_data, elementName, true);
        }

        protected ImportBaseService(XElement data)
        {
            if (data == null)
                throw new ImportXmlValidationException("Пустой XML");
            _data = data;

            //Юсупов: перенес в импорт чтобы не тормозить тут
            //string dataStr = data.ToString();
            //dataStr = dataStr
            //    .Replace("<FinSourceAndEduForm>", "<FinSourceEduForm>")
            //    .Replace("</FinSourceAndEduForm>", "</FinSourceEduForm>")
            //    .Replace("<EducationalFormID>", "<EducationFormID>")
            //    .Replace("</EducationalFormID>", "</EducationFormID>");
            //_data = XElement.Parse(dataStr);
        }

        public virtual XElement ProcessData()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Проверка авторизации
        /// </summary>
        /// <param name="allowNoInstitution"></param>
        public void CheckAuth(bool allowNoInstitution)
        {
            ValidatePackage(XsdManager.XsdName.AuthData);
            
            _login = GetElementByNameWithThrowError("Login").Value;
            var pass = GetElementByNameWithThrowError("Pass").Value;

            /* Необязательный идентификатор вуза */
            var instElement = GetElementByName(_data, "InstitutionID", false);
            if (instElement != null)
                Int32.TryParse(instElement.Value, out _institutionId);

            if (!EsrpAuthHelperV2.ESRPAuthDisabled(_login))
            {
                var checkResult = CheckEsrpAuth.CheckUserAccess(_login, pass);
                if (checkResult == 0)
                    throw new ImportAuthorizationException("Нет доступа к ФИС. Неверный логин или пароль");
                if (checkResult == -1)
                    throw new ImportAuthorizationException("Ошибка доступа к ЕСРП");
            }

            //обновляем права и смотрим на результат
            var helper = new EsrpAuthHelperV2();
            var updateUserDetailsResult = helper.UpdateUserRights(_login, ref _institutionId);
            if (updateUserDetailsResult == UpdateUserDetailsResult.Failed)
                throw new ImportAuthorizationException("Произошла ошибка во время проверки прав доступа");
            if (updateUserDetailsResult == UpdateUserDetailsResult.NoInstution && !allowNoInstitution)
                throw new ImportAuthorizationException("Ошибка доступа. Указанный филиал не найден");
            if (updateUserDetailsResult == UpdateUserDetailsResult.IncorrectInstution)
                throw new ImportAuthorizationException("Ошибка доступа. Нет прав доступа к указанному филиалу");
        }

        /// <summary>
        /// Проверка корректности схемы XML
        /// </summary>
        /// <returns></returns>
        public virtual void ValidatePackage(XsdManager.XsdName xsd)
        {
            var rootElementName = xsd.GetRootTagName();
            var element = GetElementByName(_data, rootElementName, true);
            var error = new XsdManager().ValidateXmlBySheme(element, xsd);
            if (error != null) throw new ImportXmlValidationException(error);            
        }

        public void Dispose()
        {
            _data = null;
            GC.SuppressFinalize(this);

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        public XElement ValidateProcessPackageResults(ImportPackage importPackage, int institutionID, PackageType packageType)
        {
            if (importPackage == null)
                return XmlImportHelper.GenerateErrorElement("Пакет не найден");

            if (importPackage.InstitutionID != institutionID)
                return XmlImportHelper.GenerateErrorElement("Пакет принадлежит другому институту");

            if (importPackage.TypeID != (int)packageType)
                return XmlImportHelper.GenerateErrorElement("Пакет указанного типа не найден");

            if (importPackage.StatusID != (int)PackageStatus.Processed)
                return XmlImportHelper.GenerateErrorElement("Пакет еще не обработан");

            if (importPackage.StatusID == (int)PackageStatus.Processed && importPackage.ProcessResultInfo == null)
                return XmlImportHelper.GenerateErrorElement("Произошла ошибка во время обработки пакета");

            return null;
        }
        /// <summary>
        /// Содержит список элементов строкового типа, которые могут встречаться в пакетах и чувствительны к пробелам в начале и конце
        /// </summary>
        private readonly string[] _stringElements =
            {
                "ApplicationNumber",
                "CompetitiveGroupItemID"
                /*"UID" 
                "SubjectName",
                "FirstName",
                "LastName",
                "MiddleName",
                "CampaignUID",
                "Name",
                "TargetOrganisationName",
                "DocumentSeries",
                "DocumentNumber",
                "DocumentOrganisation",
                "AdditionalInfo",*/
            };
        /// <summary>
        /// Структура для определения стандартных вступительных испытаний
        /// </summary>
        private struct Subject
        {
            public readonly int SubjectID; 
            public readonly string SubjectName;
            public readonly bool IsEge;

            private Subject (int id, string name, bool ege)
            {
                SubjectID = id;
                SubjectName = name;
                IsEge = ege;
            }

            /// <summary>
            /// Список стандартных вступительных испытаний
            /// </summary>
            public static readonly Subject[] Subjects =
                {
                    new Subject(1, "Русский язык", true),
                    new Subject(2, "Математика", true),
                    new Subject(3, "Информатика и ИКТ", true),
                    new Subject(4, "Биология", true),
                    new Subject(5, "География", true),
                    new Subject(6, "Иностранный язык", true),
                    new Subject(7, "История", true),
                    new Subject(8, "Литература", true),
                    new Subject(9, "Обществознание", true),
                    new Subject(10, "Физика", true),
                    new Subject(11, "Химия", true),
                    new Subject(12, "Иностранный язык - немецкий", true),
                    new Subject(13, "Иностранный язык - французский", true),
                    new Subject(14, "Иностранный язык - испанский", true),
                    new Subject(15, "Астрономия", false),
                    new Subject(16, "Искусство", false),
                    new Subject(17, "Лингвистика", false),
                    new Subject(18, "Основы безопасности жизнедеятельности", false),
                    new Subject(19, "Право", false),
                    new Subject(20, "Технология", false),
                    new Subject(21, "Физическая культура", false),
                    new Subject(22, "Экология", false),
                    new Subject(23, "Экономика", false),
                    new Subject(24, "Иностранный язык - английский", true),
                };
        }



        /// <summary>
        /// Метод, возвращающий пакет, с тримом необходимых строк
        /// </summary>
        /// <param name="importPackage">Входной пакет</param>
        /// <returns>Результат</returns>
        public XElement TrimPackageStrings(XElement importPackage)
        {
            var elements = importPackage.Descendants().Where(element => _stringElements.Contains(element.Name.LocalName));
            //var elements = importPackage.Descendants().Where(element => element.Descendants().Count() == 0);
            if (elements.Count() > 0)
                foreach (var element in elements)
                {
                    element.SetValue(element.Value.Trim());
                }
            return importPackage;
        }

        /// <summary>
        /// Заменяем все узлы SubjectName узлами SubjectID c правильно заменёнными значениями
        /// </summary>
        /// <param name="importPackage">Входной пакет</param>
        /// <returns>Результат</returns>
        public XElement ReplaceEntranceTestNameWithSubjectIfPossible(XElement importPackage)
        {
            var elements = importPackage.Descendants().Where(element => element.Name.LocalName == "SubjectName");
            if (elements.Count() > 0)
                foreach (var element in elements)
                {
                    string elementValue = element.Value;
                    if (!Subject.Subjects.Select(x => x.SubjectName).Contains(elementValue)) continue;
                    element.Name = "SubjectID";
                    element.Value = Subject.Subjects.First(x => x.SubjectName == elementValue).SubjectID.ToString();
                }
            return importPackage;
        }
    }
}