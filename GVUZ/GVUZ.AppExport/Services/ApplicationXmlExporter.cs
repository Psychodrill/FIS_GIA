using System;
using System.IO;
using System.Text;
using System.Xml;

namespace GVUZ.AppExport.Services
{
    public class ApplicationXmlExporter
    {
        private readonly IApplicationExportLoader _exportLoader;
        private readonly long _orgId;
        private int? _esrpId;
        private XmlWriter _writer;

        public ApplicationXmlExporter(IApplicationExportLoader exportLoader, long orgId)
        {
            _exportLoader = exportLoader;
            _orgId = orgId;
        }

        public void Export(Stream output)
        {
            try
            {
                InitWriter(output);
                BeginWrite();
                _exportLoader.ApplicationFetched += OnApplicationFetched;
                _exportLoader.Load();
                EndWrite();
            }
            finally
            {
                _exportLoader.ApplicationFetched -= OnApplicationFetched;
                CloseWriter();
                
            }
            
        }

        private void InitWriter(Stream output)
        {
            if (_writer != null)
            {
                _writer.Close();
                _writer = null;
            }

            _writer = XmlWriter.Create(output, new XmlWriterSettings { CloseOutput = false, Indent = true, Encoding = new UTF8Encoding(false) });
        }

        private void CloseWriter()
        {
            if (_writer != null)
            {
                _writer.Flush();
                _writer.Close();
                _writer = null;
            }
        }

        private void BeginWrite()
        {
            _writer.WriteStartElement("Root");
            _writer.WriteStartElement("PackageData");
            _writer.WriteStartElement("Applications");
        }

        private void EndWrite()
        {
            _writer.WriteFullEndElement(); // </Applications>
            _writer.WriteFullEndElement(); // </PackageData>
            _writer.WriteStartElement("EsrpOrgId");
            _writer.WriteValue(GetEsrpId());
            _writer.WriteFullEndElement(); // </EsrpOrgId>
            _writer.WriteFullEndElement(); // </Root>
        }

        private void WriteApplication(ApplicationExportDto data)
        {
            _writer.WriteStartElement("Application");
            _writer.WriteElementString("AppID", XmlConvert.ToString(data.AppId));
            _writer.WriteElementString("RegistrationDate", FormatDateTime(data.RegistrationDate));
            if (data.LastDenyDate.HasValue)
            {
                _writer.WriteElementString("LastDenyDate", FormatDateTime(data.LastDenyDate.Value));
            }
            _writer.WriteElementString("StatusID", XmlConvert.ToString(data.StatusId));
            _writer.WriteStartElement("FinSourceAndEduForms");
            foreach (var item in data.FinSourceAndEduForms)
            {
                WriteFinSourceEduForm(item);
            }
            _writer.WriteFullEndElement();
            _writer.WriteEndElement(); // </Application>
        }

        private void WriteFinSourceEduForm(ApplicationExportFinSourceDto data)
        {
            _writer.WriteStartElement("FinSourceEduForm");
            _writer.WriteElementString("FinanceSourceID", XmlConvert.ToString(data.FinanceSourceId));
            _writer.WriteElementString("EducationFormID", XmlConvert.ToString(data.EducationFormId));
            _writer.WriteElementString("EducationLevelID", XmlConvert.ToString(data.EducationLevelId));
            _writer.WriteElementString("DirectionID", XmlConvert.ToString(data.DirectionId));
            _writer.WriteElementString("Number", data.Number);

            if (data.OrderTypeId.HasValue)
            {
                _writer.WriteElementString("OrderTypeID", XmlConvert.ToString(data.OrderTypeId.Value));

                if (data.IsForBeneficiary.HasValue)
                {
                    _writer.WriteElementString("IsForBeneficiary", XmlConvert.ToString(data.IsForBeneficiary.Value));
                }

                if (data.UseBeneficiarySubject.HasValue)
                {
                    _writer.WriteElementString("UseBeneficiarySubject", XmlConvert.ToString(data.UseBeneficiarySubject.Value));
                }

                if (data.CommonBeneficiaryDocTypeId.HasValue)
                {
                    _writer.WriteElementString("CommonBeneficiaryDocTypeId", XmlConvert.ToString(data.CommonBeneficiaryDocTypeId.Value));
                }

                if (data.EntranceTestResults.Count > 0)
                {
                    _writer.WriteStartElement("EntranceTestResults");

                    foreach (var item in data.EntranceTestResults)
                    {
                        WriteEntranceTestResult(item);
                    }

                    _writer.WriteFullEndElement(); // </EntranceTestResults>
                }
            }

            _writer.WriteFullEndElement(); // </FinSourceEduForm>
        }

        private void WriteEntranceTestResult(ApplicationExportEntranceTestDto data)
        {
            _writer.WriteStartElement("EntranceTestResult");

            _writer.WriteElementString("EntranceTestResultID", XmlConvert.ToString(data.EntranceTestResultId));
            _writer.WriteElementString("ResultValue", XmlConvert.ToString(data.ResultValue));
            _writer.WriteElementString("ResultSourceTypeID", XmlConvert.ToString(data.ResultSourceTypeId));
            _writer.WriteElementString("EntranceTestTypeID", XmlConvert.ToString(data.EntranceTestTypeId));
            
            _writer.WriteFullEndElement(); // </EntranceTestResult>
        }

        private void OnApplicationFetched(object sender, ApplicationExportEventArgs applicationExportEventArgs)
        {
            WriteApplication(applicationExportEventArgs.Data);
        }

        private int GetEsrpId()
        {
            if (!_esrpId.HasValue)
            {
                _esrpId = _exportLoader.GetInstitutionEsrpId(_orgId);
            }

            return _esrpId.Value;
        }

        private static string FormatDateTime(DateTime dt)
        {
            return dt.ToString("yyyy-MM-ddTHH:mm:ss");
        }
    }
}