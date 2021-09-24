using System.Collections.Generic;
using System.Linq;

namespace GVUZ.Helper.Rdms
{
    public class DictionaryNames
    {
        private static readonly Dictionary<Dictionary, string> Names;

        static DictionaryNames()
        {
            Names = new Dictionary<Dictionary, string>
                {
                    {Dictionary.FormOfLaw, "Организационно-правовая форма ОУ"},
                    {Dictionary.Country, "Государства"},
                    {Dictionary.Profession, "Профессии НПО"},
                    {Dictionary.RussianFederationSubject, "Субъекты Российской Федерации"}
                };
        }

        public static string Get(Dictionary dictionary)
        {
            return Names[dictionary];
        }

        public static Dictionary<Dictionary, string> GetSortedByName()
        {
            return Names.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        }
    }
}