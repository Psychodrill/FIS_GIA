using System.Globalization;
using FogSoft.Helpers;

namespace GVUZ.ServiceModel.Import.WebService.Dto
{
    public class SubjectDataDto
    {
        public string SubjectID;
        public string Value;

        public decimal? ValueDecimal
        {
            get
            {
                return string.IsNullOrEmpty(Value)
                           ? -1
                           : Value.To<decimal>(provider: CultureInfo.InvariantCulture);
            }
        }
    }
}