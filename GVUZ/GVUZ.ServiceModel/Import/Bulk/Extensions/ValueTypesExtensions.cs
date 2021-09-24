using System;
using System.Globalization;
using FogSoft.Helpers;

namespace GVUZ.ServiceModel.Import.Bulk.Extensions
{
    public static class ValueTypesExtensions
    {
        public static bool ToBool(this string value, bool defaults)
        {
            if (string.IsNullOrEmpty(value)) return defaults;

            if (value == "1") return true;
            if (value == "0") return false;

            bool parsed;
            return !Boolean.TryParse(value, out parsed) ? defaults : parsed;
        }

        public static int ToInt(this string value, int defaults)
        {
            if (string.IsNullOrEmpty(value)) return defaults;
            int parsed;
            return !Int32.TryParse(value, out parsed) ? defaults : parsed;
        }

        public static int? ToIntNullable(this string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            int parsed;
            return !Int32.TryParse(value, out parsed) ? (int?) null : parsed;
        }

        public static decimal? ToDecimalNullable(this string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            return value.To((decimal?) null, provider: CultureInfo.InvariantCulture);
        }

        public static DateTime? ToDateTimeNullable(this string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            DateTime parsed;
            return !DateTime.TryParse(value, out parsed) ? null : (DateTime?) parsed;
        }
    }
}