namespace Ege.Check.Logic.Services.Dtos.Models
{
    using System;
    using Ege.Check.Logic.Services.Dtos.Enums;
    using Ege.Check.Logic.Services.Dtos.Metadata;

    /// <summary>
    ///     Экзамен
    /// </summary>
    [Serializable]
    [BulkMergeProcedure("LoadExams")]
    public class ExamDto
    {
        /// <summary>
        ///     Идентификатор экзамена
        /// </summary>
        [PrimaryKeyPart]
        public int Id { get; set; }

        /// <summary>
        ///     Дата экзамена
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        ///     Волна
        /// </summary>
        public ExamWave WaveCode { get; set; }

        /// <summary>
        ///     Код предмета
        /// </summary>
        public int SubjectCode { get; set; }
    }
}