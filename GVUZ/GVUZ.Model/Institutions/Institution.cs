using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using GVUZ.Model.Helpers;

namespace GVUZ.Model.Institutions
{
    public partial class Institution : IPersonalDataAccessLogger
    {
        public enum FileTypes
        {
            License = 1,
            Accreditation = 2,
            HostelCondition = 3,
        }

        public enum InstitutionFileType
        {
        }

        /// <summary>
        ///     Не использовать в LINQ!
        /// </summary>
        public InstitutionType Type
        {
            get { return (InstitutionType) InstitutionTypeID; }
            set { InstitutionTypeID = (short) value; }
        }


        public IPersonalDataAccessLog CreatePersonalDataAccessLog()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Удаление институтских файлов
        /// </summary>
        public void DeleteFile(InstitutionsEntities context, FileTypes fileType)
        {
            //файлы теперь физически не удаляем
            switch (fileType)
            {
                case FileTypes.Accreditation:
                    InstitutionAccreditation instAccr = InstitutionAccreditation.First();
                    if (instAccr != null)
                    {
                        //context.Attachment.DeleteObject(instAccr.Attachment);
                        instAccr.Attachment = null;
                    }
                    break;
                case FileTypes.HostelCondition:
                    if (HostelAttachmentID != null)
                    {
                        //context.Attachment.DeleteObject(HostelAttachment);
                        HostelAttachment = null;
                    }
                    break;
                case FileTypes.License:
                    InstitutionLicense licAccr = InstitutionLicense.First();
                    if (licAccr != null)
                    {
                        //context.Attachment.DeleteObject(licAccr.Attachment);
                        licAccr.Attachment = null;
                    }
                    break;
            }
        }
    }

    public partial class InstitutionLicense : IDataErrorInfo
    {
        private readonly Dictionary<string, string> _errors = new Dictionary<string, string>();

        //partial void OnLicenseDocumentChanging(byte[] value)
        //{
        //    if(value == null || value.Length == 0)
        //        _errors.Add("LicenseDocument", "License document required.");
        //}

        public string this[string columnName]
        {
            get
            {
                if (_errors.ContainsKey(columnName))
                    return _errors[columnName];
                return string.Empty;
            }
        }

        public string Error
        {
            get { return string.Empty; }
        }
    }
}