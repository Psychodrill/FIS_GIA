using System;
using System.Collections.Generic;

namespace GVUZ.Helper.ExternalValidation
{
    public static class LanguageSubjects
    {
        public const string ForeignLanguage = "Иностранный язык";
        public const string ForeignLanguagePrefix = "Иностранный язык - ";

        public static readonly Dictionary<string, string> EntranceTestBySubject =
            new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase)
                {
                    {"английский язык", "Иностранный язык - английский"},
                    {"немецкий язык", "Иностранный язык - немецкий"},
                    {"французский язык", "Иностранный язык - французский"},
                    {"испанский язык", "Иностранный язык - испанский"}
                };

        public static readonly Dictionary<string, string> SubjectByEntranceTest;

        public static readonly List<string> EntranceTestSubjects = new List<string>
            {
                "Иностранный язык - английский",
                "Иностранный язык - немецкий",
                "Иностранный язык - французский",
                "Иностранный язык - испанский"
            };

        static LanguageSubjects()
        {
            SubjectByEntranceTest = new Dictionary<string, string>();
            foreach (var keyValuePair in EntranceTestBySubject)
                SubjectByEntranceTest.Add(keyValuePair.Value, keyValuePair.Key);
        }
    }
}