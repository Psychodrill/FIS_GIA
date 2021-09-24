namespace Ege.Check.Logic.Services.Dtos.Models
{
    using System;
    using Ege.Check.Logic.Services.Dtos.Metadata;

    /// <summary>
    ///     Регион
    /// </summary>
    [Serializable]
    [BulkMergeProcedure("LoadRegions")]
    public class RegionDto
    {
        /// <summary>
        ///     Идентификатор
        /// </summary>
        [PrimaryKeyPart]
        public int Id { get; set; }

        /// <summary>
        ///     Имя
        /// </summary>
        public string Name { get; set; }
    }
}