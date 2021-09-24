namespace Ege.Check.Logic.Services.Dtos.Models
{
    using System;
    using Ege.Check.Logic.Services.Dtos.Metadata;

    /// <summary>
    ///     Предмет
    /// </summary>
    [Serializable]
    [BulkMergeProcedure("LoadSubjects")]
    public class SubjectDto
    {
        /// <summary>
        ///     Код
        /// </summary>
        [PrimaryKeyPart]
        public int Code { get; set; }

        /// <summary>
        ///     Название
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Минимальный балл
        /// </summary>
        public int MinValue { get; set; }

        /// <summary>
        ///     Является ли сочинением/изложением
        /// </summary>
        public bool IsComposition { get; set; }
    }
}
