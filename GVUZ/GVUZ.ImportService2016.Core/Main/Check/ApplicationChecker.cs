using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GVUZ.ImportService2016.Core.Dto.Import;
using System.Xml;
using GVUZ.ImportService2016.Core.Main.Repositories;
using GVUZ.ImportService2016.Core.Main.Log;

namespace GVUZ.ImportService2016.Core.Main.Check
{
    public class ApplicationChecker
    {
        private PackageData _packageData;

        //private IEnumerable<CheckableApplication> _applications;
        List<int> _applications;

        private bool _hasApplications = true;
        private bool _anyApplicationFound = false;
        private bool _isSingle;
        private string _userLogin;

        private XmlDocument _checkResult;

        public string CheckResultString
        {
            get
            {
                if (_checkResult == null)
                    return null;
                return _checkResult.OuterXml;
            }
        }

        public ApplicationChecker(PackageData packageData, List<int> applicationIDs, string userLogin)
        {
            _isSingle = (packageData != null && packageData.Application != null) || (packageData == null);
            _userLogin = userLogin;
            _packageData = packageData;

            _applications = applicationIDs;
            //foreach (int applicationId in applicationIDs)
            //{
            //    //    var application = _packageData.GetApplications.FirstOrDefault(x => x.ID == applicationId);
            //}
        }

        public void DoCheck()
        {
            if (!_applications.Any())
            {
                _hasApplications = false;
                //if (_packageData != null) Log.LogHelper.Log.DebugFormat("№ {0} Нет заявлений для проверки", _packageData.ImportPackageId);
            }

            StringBuilder resultBuilder = new StringBuilder();

            StringBuilder egeDocumentCheckResultsBuilder = new StringBuilder();
            StringBuilder olympicDocumentCheckResultsBuilder = new StringBuilder();
            StringBuilder getEgeDocumentsBuilder = new StringBuilder();

            //StringBuilder getOlympicDocumentsBuilder = new StringBuilder();
            StringBuilder getCompositionResultsBuilder = new StringBuilder();

            foreach (var applicationID in _applications)
            {
                //if (_packageData != null) Log.LogHelper.Log.DebugFormat("№ {0} Начало по {1}", _packageData.ImportPackageId, applicationID);
                using (ADOApplicationChecksRepository checksRepository = new ADOApplicationChecksRepository(applicationID, _userLogin, _isSingle))
                {
                    if (_packageData != null)
                    {
                        var packageApplication = _packageData.GetApplications.FirstOrDefault(t => t.UID == checksRepository.UID);
                        if (packageApplication == null || packageApplication.StatusID != GVUZ.ServiceModel.Import.ApplicationStatusType.Accepted) // && !_isSingle)
                        {
                            //LogHelper.Log.DebugFormat("№ {0} packageApplication = null or Status!=4 {1}", _packageData.ImportPackageId, applicationID);
                            continue;
                        }
                    }

                    string applicationXml = checksRepository.GetApplicationXML();


                    //if (_packageData != null) LogHelper.Log.DebugFormat("№ {0} {1} CheckEntrantApplicationsCount() ", _packageData.ImportPackageId, applicationID);
                    ApplicationCheckResult entrantApplicationsCountCheckResult = checksRepository.CheckEntrantApplicationsCount();
                    if (entrantApplicationsCountCheckResult != null)
                    {
                        //if (_packageData != null) LogHelper.Log.DebugFormat("№ {0} {1} CheckEntrantApplicationsCount() has Result ", _packageData.ImportPackageId, applicationID);
                        if (entrantApplicationsCountCheckResult.Exception != null)
                        {
                            AppendXml(getEgeDocumentsBuilder, "GetEgeDocuments", "GetEgeDocument", string.Format("<Error>Произошла техническая ошибка</Error>"));
                            LogHelper.Log.Error(entrantApplicationsCountCheckResult.Exception.Message, entrantApplicationsCountCheckResult.Exception);
                        }
                        else if (entrantApplicationsCountCheckResult.ViolationCode != 0)
                        {
                            AppendXml(getEgeDocumentsBuilder, "GetEgeDocuments", "GetEgeDocument", string.Format("<Error>{0}</Error>", entrantApplicationsCountCheckResult.ViolationMessage));
                        }
                    }
                    //if (_packageData != null) LogHelper.Log.DebugFormat("№ {0} {1} CheckEgeMarks()++ ", _packageData.ImportPackageId, applicationID);
                    ApplicationCheckResult egeMarksCheckResult = checksRepository.CheckEgeMarks();
                    ApplicationCheckResult olympiadMarksCheckResult = checksRepository.CheckOlympiadMarks(applicationXml);
                    ApplicationCheckResult compositionMarksCheckResult = checksRepository.CheckCompositionMarks();

                    var hasAnyViolations
                        = (entrantApplicationsCountCheckResult != null && entrantApplicationsCountCheckResult.ViolationCode != 0)
                        || ((egeMarksCheckResult != null) && (egeMarksCheckResult.ViolationCode.GetValueOrDefault() != 0))
                        || ((olympiadMarksCheckResult != null) && (olympiadMarksCheckResult.ViolationCode.GetValueOrDefault() != 0))
                        || ((compositionMarksCheckResult!= null) && (compositionMarksCheckResult.ViolationCode.GetValueOrDefault() != 0));

                    //if (_packageData != null) LogHelper.Log.DebugFormat("№ {0} {1} CheckEgeMarks()++ Violations: {2} ", _packageData.ImportPackageId, applicationID, hasAnyViolations);

                    //Если Single, подразумевается всего одно заявление, если его не нашли - можно выходить
                    if ((_isSingle) && (
                            (egeMarksCheckResult != null && egeMarksCheckResult.ApplicationNotFound) 
                            || (olympiadMarksCheckResult != null && olympiadMarksCheckResult.ApplicationNotFound)
                            || (compositionMarksCheckResult != null && compositionMarksCheckResult.ApplicationNotFound)
                        ))
                    {
                        _anyApplicationFound = false;
                        //if (_packageData != null) LogHelper.Log.DebugFormat("№ {0} _anyApplicationFound = false {1}", _packageData.ImportPackageId, applicationID);
                        break;
                    }

                    if (entrantApplicationsCountCheckResult != null)
                    {
                        _anyApplicationFound = true;
                    }

                    if (egeMarksCheckResult != null)
                    {
                        ProcessCheckResult(getEgeDocumentsBuilder, egeDocumentCheckResultsBuilder, egeMarksCheckResult, "GetEgeDocuments", "GetEgeDocument", "EgeDocumentCheckResults", "EgeDocumentCheckResult");
                    }
                    else
                    {
                        AppendXml(getEgeDocumentsBuilder, "GetEgeDocuments", "GetEgeDocument", applicationXml);
                    }

                    //ProcessCheckResult(olympicDocumentCheckResultsBuilder, null, olympiadMarksCheckResult, "GetOlympicDocuments", "GetOlympicDocument", "OlympicDocumentCheckResults", "OlympicDocumentCheckResult");
                    if (olympiadMarksCheckResult != null)
                        ProcessCheckResult(olympicDocumentCheckResultsBuilder, null, olympiadMarksCheckResult, "OlympicDocumentCheckResults", "OlympicDocumentCheckResult", null, null);
                    if (compositionMarksCheckResult != null)
                        ProcessCheckResult(getCompositionResultsBuilder, null, compositionMarksCheckResult, "GetCompositionResults", "GetCompositionResult", null, null);

                    if ((hasAnyViolations)
                        // TODO: && (!applicationIsFromCrimea) 
                        )
                    {
                        checksRepository.SetApplicationStatus3();
                    }
                    else
                    {
                        checksRepository.SetApplicationStatus4();
                    }
                }
            }

            int statusCheckCode = _hasApplications && _anyApplicationFound ? 0 : 1;
            string statusCheckMessage = _hasApplications && _anyApplicationFound ? "Найдены заявления для проверки" : "Не найдены заявления для проверки";

            if (_isSingle)
            {
                resultBuilder.Append("<AppSingleCheckResult xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">").AppendLine();
                if ((!_hasApplications) || (!_anyApplicationFound))
                {
                    resultBuilder.AppendFormat("<Error><ErrorCode>{0}</ErrorCode><ErrorText>{1}</ErrorText></Error>", statusCheckCode, statusCheckMessage).AppendLine();
                }
            }
            else if (_packageData != null)
            {
                resultBuilder.Append("<ImportedAppCheckResultPackage xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">").AppendLine();
                resultBuilder.AppendFormat("    <PackageID>{0}</PackageID>", _packageData.ImportPackageId).AppendLine();
                resultBuilder.AppendFormat("    <StatusCheckCode>{0}</StatusCheckCode>", statusCheckCode.ToString()).AppendLine();
                resultBuilder.AppendFormat("    <StatusCheckMessage>{0}</StatusCheckMessage>", statusCheckMessage).AppendLine();
            }

            if (_hasApplications && _anyApplicationFound)
            {
                CloseXml(egeDocumentCheckResultsBuilder, "EgeDocumentCheckResults");
                CloseXml(olympicDocumentCheckResultsBuilder, "OlympicDocumentCheckResults");
                CloseXml(getEgeDocumentsBuilder, "GetEgeDocuments");
                CloseXml(getCompositionResultsBuilder, "GetCompositionResults");

                resultBuilder.Append(egeDocumentCheckResultsBuilder.ToString());
                resultBuilder.Append(olympicDocumentCheckResultsBuilder.ToString());
                resultBuilder.Append(getEgeDocumentsBuilder.ToString());
                resultBuilder.Append(getCompositionResultsBuilder.ToString());
            }

            resultBuilder.Append(_isSingle ? "</AppSingleCheckResult>" : "</ImportedAppCheckResultPackage>");

            _checkResult = new XmlDocument();
            try
            {
                _checkResult.LoadXml(resultBuilder.ToString());
            }
            catch (Exception ex)
            {
                _checkResult.LoadXml(String.Format("<Error><ErrorCode>{0}</ErrorCode><ErrorText>{1}</ErrorText></Error>", 1, "Произошла техническая ошибка"));
                LogHelper.Log.Error(ex.Message, ex);
                LogHelper.Log.Error(resultBuilder.ToString(), ex);
            }
        }

        private void ProcessCheckResult(StringBuilder getXmlPartBuilder, StringBuilder checkXmlPartBuilder, ApplicationCheckResult checkResult, string getResultRootTag, string getResultMultipleContainerTag, string checkResultRootTag, string checkResultMultipleContainerTag)
        {
            if (checkResult.ApplicationNotFound)
            {
                AppendXml(getXmlPartBuilder, getResultRootTag, getResultMultipleContainerTag, "<Error>Заявление не найдено</Error>");
            }
            else
            {
                _anyApplicationFound = true;
                if (checkResult.Exception == null)
                {
                    if (checkXmlPartBuilder != null)
                    {
                        AppendXml(checkXmlPartBuilder, checkResultRootTag, checkResultMultipleContainerTag, checkResult.XmlResult_Check);
                    }
                    AppendXml(getXmlPartBuilder, getResultRootTag, getResultMultipleContainerTag, checkResult.XmlResult_Get);
                }
                else
                {
                    AppendXml(getXmlPartBuilder, getResultRootTag, getResultMultipleContainerTag, "<Error>Произошла техническая ошибка</Error>");
                    LogHelper.Log.Error(checkResult.Exception.Message, checkResult.Exception);
                }
            }
        }

        private void AppendXml(StringBuilder xmlPartBuilder, string rootTag, string multipleContainerTag, string xmlContent)
        {
            if (string.IsNullOrEmpty(xmlContent))
                return;

            //Пустые теги не нужны, например:
            //<root />, <root></root>
            //Но тег <Error /> допускается
            if ((xmlContent.Split('/').Length <= 2) && (!xmlContent.ToUpper().Contains("ERROR")))
                return;

            if (xmlPartBuilder.Length == 0)
            {
                xmlPartBuilder.AppendFormat("<{0}>", rootTag).AppendLine();
            }
            if (!_isSingle)
            {
                xmlPartBuilder.AppendFormat("   <{1}>{0}</{1}>", xmlContent, multipleContainerTag).AppendLine();
            }
            else
            {
                xmlPartBuilder.AppendFormat("    {0}", xmlContent).AppendLine();
            }
        }

        private void CloseXml(StringBuilder xmlPartBuilder, string rootTag)
        {
            if (xmlPartBuilder.Length > 0)
            {
                xmlPartBuilder.AppendFormat("</{0}>", rootTag).AppendLine();
            }
        }
    }

    public class CheckableApplication
    {
        public CheckableApplication(int id, string comment, bool isFromCrimea, bool checkEge, int statusId)
        {
            Id = id;
            Comment = comment;
            IsFromCrimea = isFromCrimea;
            CheckEge = checkEge;
            StatusId = statusId;
        }
        public int Id { get; private set; }
        public string Comment { get; private set; }
        public bool IsFromCrimea { get; private set; }
        public bool CheckEge { get; private set; }

        public int StatusId { get; private set; }
    }
}
