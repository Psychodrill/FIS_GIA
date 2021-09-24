using System;
using System.ComponentModel;
using GVUZ.Model.Entrants.Documents;
using GVUZ.ServiceModel.Import.WebService.Dto.Documents.Base;

namespace GVUZ.ServiceModel.Import.WebService.Dto.Documents
{
    [Description("Документ, удостоверяющий личность")]
    public class IdentityDocumentDto : BaseDocumentDto, IEquatable<IdentityDocumentDto>
    {
        public override EntrantDocumentType EntrantDocumentType
        {
            get { return EntrantDocumentType.IdentityDocument; }
        }

        public string DocumentSeries;
        public string IdentityDocumentTypeID;

        /// <summary>
        ///     Заполняется из энтранта. Оставлено для сохранения существующей структуры и нормального маппинга
        /// </summary>
#warning Это тут лишнее!!!        
        public string GenderTypeID;

        public string NationalityTypeID;
        public string BirthDate;
        public string BirthPlace;
        public string SubdivisionCode;

        #region IEquatable<ApplicationBulkEntity> Members

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as IdentityDocumentDto);
        }

        public bool Equals(IdentityDocumentDto other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            bool result = true;
            if (DocumentSeries != null) result &= other.DocumentSeries.Equals(DocumentSeries);
            if (IdentityDocumentTypeID != null) result &= other.IdentityDocumentTypeID.Equals(IdentityDocumentTypeID);
            if (NationalityTypeID != null) result &= other.NationalityTypeID.Equals(NationalityTypeID);
            if (BirthDate != null) result &= other.BirthDate.Equals(BirthDate);
            if (BirthPlace != null) result &= other.BirthPlace.Equals(BirthPlace);
            if (SubdivisionCode != null) result &= other.SubdivisionCode.Equals(SubdivisionCode);
            return result;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = 17;
                result = result*37 + (DocumentSeries != null ? DocumentSeries.GetHashCode() : 0);
                result = result*37 + (IdentityDocumentTypeID != null ? IdentityDocumentTypeID.GetHashCode() : 0);
                result = result*37 + (NationalityTypeID != null ? NationalityTypeID.GetHashCode() : 0);
                result = result*37 + (BirthDate != null ? BirthDate.GetHashCode() : 0);
                result = result*37 + (BirthPlace != null ? BirthPlace.GetHashCode() : 0);
                result = result*37 + (SubdivisionCode != null ? SubdivisionCode.GetHashCode() : 0);
                return result;
            }
        }

        #endregion
    }
}