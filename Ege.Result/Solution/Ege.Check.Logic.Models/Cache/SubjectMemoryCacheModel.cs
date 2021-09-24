namespace Ege.Check.Logic.Models.Cache
{
    public class SubjectMemoryCacheModel
    {
        public int SubjectCode { get; set; }

        public string SubjectName { get; set; }

        public int MinValue { get; set; }

        public bool IsComposition { get; set; }

        public bool IsBasicMath { get; set; }

        public bool IsForeignLanguage { get; set; }

        public bool IsOral { get; set; }

        public string SubjectDisplayName { get; set; }

        public int? WrittenSubjectCode { get; set; }

        public override string ToString()
        {
            return
                string.Format(
                    "SubjectCode: {0}, SubjectName: {1}, MinValue: {2}, IsComposition: {3}, IsBasicMath: {4}, IsForeignLanguage: {5}, IsOral: {6}, SubjectDisplayName: {7}, WrittenSubjectCode: {8}",
                    SubjectCode, SubjectName, MinValue, IsComposition, IsBasicMath, IsForeignLanguage, IsOral, SubjectDisplayName, WrittenSubjectCode);
        }
    }
}