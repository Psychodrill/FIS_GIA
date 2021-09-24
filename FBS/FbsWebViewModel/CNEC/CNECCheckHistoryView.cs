namespace FbsWebViewModel.CNEC
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// история записей об уникальных проверках сертификатов 
    /// </summary>
    public class CNECCheckHistoryView
    {
        /// <summary>
        /// id сертификата
        /// </summary>
        public Guid CertificateId { get; set; }

        /// <summary>
        /// id организации
        /// </summary>
        public int OrganizationId { get; set; }

        /// <summary>
        /// полное название организации
        /// </summary>
        public string OrganizationFullName { get; set; }

        /// <summary>
        /// Записи о проверках сертификата организацией
        /// </summary>
        public List<CNECCheckHystoryEntryView> CheckEntries { get; set; }
    }
}