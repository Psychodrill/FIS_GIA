using System;
using System.Collections.Generic;
using System.Linq;
using GVUZ.Model.Institutions;

namespace GVUZ.Web.Helpers
{
    public static class EducationFormsHelper
    {
        public static Dictionary<short, string> EducationFormNames = new Dictionary<short, string>
            {
                {EDFormsConst.O, "Очная форма"},
                {EDFormsConst.OZ, "Очно-заочная форма"},
                {EDFormsConst.Z, "Заочная форма"}

            };

        public static Dictionary<short, string> EducationSourceNames = new Dictionary<short, string>
            {
                { EDSourceConst.Budget, "Бюджет"},
                { EDSourceConst.Paid, "Платные места"},
                { EDSourceConst.Target, "Целевой прием"},
                { EDSourceConst.Quota, "Квота лиц, имеющих особое право"}
            };


        public static string GetDisplayForm(short formId)
        {
            string formName;

            if (EducationFormNames.TryGetValue(formId, out formName))
            {
                return string.Format("{0}", formName);
            }

            return null;
        }

        public static string GetDisplaySource(short sourceId)
        {
            string sourceName;

            if (EducationSourceNames.TryGetValue(sourceId, out sourceName))
            {
                return string.Format("{0}", sourceName);
            }

            return null;
        }

        public static short GetFormIdByName(string formName)
        {
            return EducationFormNames.Single(x => x.Value.Equals(formName, StringComparison.OrdinalIgnoreCase)).Key;
        }

        public static short GetSourceIdByName(string sourceName)
        {
            return EducationSourceNames.Single(x => x.Value.Equals(sourceName, StringComparison.OrdinalIgnoreCase)).Key;
        }
    }
}