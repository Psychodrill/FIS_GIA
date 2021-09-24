using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace GVUZ.Util.Services.Parser
{
    public class ApplicationOrderRecordList : IEnumerable<ApplicationOrderRecordDto>, IEnumerator<ApplicationOrderRecordDto>
    {
        private readonly IEnumerable<ImportPackageDto> _packageReader;
        private ImportPackageDto _currentPackage;
        private XmlDocument _currentPackageXml;
        private IEnumerator<XmlElement> _orders;
        private IEnumerator<XmlElement> _applications;

        private static class SchemaNames
        {
            public const string ApplicationNumber = "ApplicationNumber";
            public const string DirectionId = "DirectionID";
            public const string EducationFormId = "EducationFormID";
            public const string EducationLevelId = "EducationLevelID";
            public const string FinanceSourceId = "FinanceSourceID";
            public const string IsBeneficiary = "IsBeneficiary";
            public const string IsForeigner = "IsForeigner";
            public const string Stage = "Stage";
            public const string RegistrationDate = "RegistrationDate";
            public const string Application = "Application";
            public const string OrderOfAdmission = "//OrderOfAdmission";
        }

        public ApplicationOrderRecordList(IEnumerable<ImportPackageDto> packageReader)
        {
            _packageReader = packageReader;
        }
        
        public IEnumerator<ApplicationOrderRecordDto> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            XmlElement applicationElement = GetApplicationElement() ?? GetApplicationElement();

            if (applicationElement != null)
            {
                ApplicationOrderRecordDto current;
                if (TryParse(applicationElement, out current))
                {
                    Current = current;
                    return true;
                }
            }
            
            return false;
        }

        private XmlElement GetApplicationElement()
        {
            if (_applications == null)
            {
                XmlElement orderXml = GetOrderElement() ?? GetOrderElement();

                if (orderXml != null)
                {
                    XmlNodeList applicationElementList = orderXml.SelectNodes(SchemaNames.Application);

                    if (applicationElementList != null)
                    {
                        _applications = applicationElementList.OfType<XmlElement>().GetEnumerator();
                    }
                }
            }

            if (_applications != null)
            {
                if (_applications.MoveNext())
                {
                    return _applications.Current;
                }

                _applications = null;
            }

            return null;
        }

        private XmlElement GetOrderElement()
        {
            if (_orders == null)
            {
                XmlDocument packageXml = GetPackageXml();

                if (packageXml != null)
                {
                    XmlNodeList orderElementList = packageXml.SelectNodes(SchemaNames.OrderOfAdmission);

                    if (orderElementList != null)
                    {
                        _orders = orderElementList.OfType<XmlElement>().GetEnumerator();
                    }
                }
            }

            if (_orders != null)
            {
                if (_orders.MoveNext())
                {
                    return _orders.Current;
                }

                _currentPackageXml = null;
                _orders = null;
            }

            return null;
        }

        private XmlDocument GetPackageXml()
        {
            if (_currentPackageXml == null)
            {
                ImportPackageDto package = GetPackage();

                if (package != null)
                {
                    _currentPackageXml = new XmlDocument();
                    _currentPackageXml.LoadXml(package.PackageData);
                }
            }

            return _currentPackageXml;
        }

        private ImportPackageDto GetPackage()
        {
            if (_packageReader.GetEnumerator().MoveNext())
            {
                _currentPackage = _packageReader.GetEnumerator().Current;
            }
            else
            {
                _currentPackage = null;
            }

            return _currentPackage;
        }

        public void Reset()
        {
            _packageReader.GetEnumerator().Reset();
            _currentPackage = null;
            _currentPackageXml = null;
            _applications = null;
            _orders = null;
            Current = null;
        }

        public ApplicationOrderRecordDto Current { get; private set; }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        private bool TryParse(XmlElement appElement, out ApplicationOrderRecordDto dto)
        {
            if (appElement == null || _orders == null || _orders.Current == null || _currentPackage == null)
            {
                dto = null;
                return false;
            }

            XmlElement orderElement = _orders.Current;

            dto = new ApplicationOrderRecordDto();

            dto.CreatedDate = DateTime.Now;
            dto.ModifiedDate = DateTime.Now;

            // из OrderOfAdmission/Application
            dto.ApplicationNumber = appElement.GetText(SchemaNames.ApplicationNumber);
            dto.RegistrationDate = appElement.GetDate(SchemaNames.RegistrationDate);

            // из OrderOfAdmission
            dto.DirectionId = orderElement.GetInt(SchemaNames.DirectionId);
            dto.EducationFormId = orderElement.GetInt(SchemaNames.EducationFormId);
            dto.EducationLevelId = orderElement.GetInt(SchemaNames.EducationLevelId);
            dto.FinanceSourceId = orderElement.GetInt(SchemaNames.FinanceSourceId);
            dto.IsBeneficiary = orderElement.GetBool(SchemaNames.IsBeneficiary).GetValueOrDefault();
            dto.IsForeigner = orderElement.GetBool(SchemaNames.IsForeigner).GetValueOrDefault();
            dto.Stage = orderElement.GetInt(SchemaNames.Stage);

            // из пакета
            dto.InstitutionId = _currentPackage.InstitutionId;
            dto.PackageCreatedDate = _currentPackage.CreateDate;
            dto.PackageId = _currentPackage.PackageId;
            dto.PackageModifiedDate = _currentPackage.LastDateChanged;

            return true;
        }
    }
}