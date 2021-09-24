using GVUZ.DAL.Dapper.Model.AllowedDirections;
using System;

namespace GVUZ.DAL.Dto
{
    /// <summary>
    /// Сведения о заявке на добавление или исключение направления из списка разрешенных
    /// или списка направлений с профильными испытаниями
    /// </summary>
    public class InstitutionDirectionRequestDto : IDirectionDescription
    {
        /// <summary>
        /// Id запроса
        /// </summary>
        public int RequestId { get; set; }

        /// <summary>
        /// Id ОО
        /// </summary>
        public int InstitutionId { get; set; }

        /// <summary>
        /// Дата запроса
        /// </summary>
        public DateTime RequestDate { get; set; }

        /// <summary>
        /// Признак наличия комментария к заявке
        /// </summary>
        public bool HasComment { get; set; }

        /// <summary>
        /// Комментарий к заявке
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Вид заявки <see cref="InstitutionDirectionRequestType"/>
        /// </summary>
        public InstitutionDirectionRequestType RequestType { get; set; }

        /// <summary>
        /// Признак отклоненной заявки
        /// </summary>
        public bool IsDenied { get; set; }

        /// <summary>
        /// Id направления
        /// </summary>
        public int DirectionId { get; set; }

        /// <summary>
        /// Код направления
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Новый код направления
        /// </summary>
        public string NewCode { get; set; }

        /// <summary>
        /// Код квалификации
        /// </summary>
        public string QualificationCode { get; set; }

        /// <summary>
        /// Наименование направления
        /// </summary>
        public string DirectionName { get; set; }

        /// <summary>
        /// Наименование квалификации
        /// </summary>
        public string QualificationName { get; set; }

        /// <summary>
        /// Id уровня образования
        /// </summary>
        public int EducationLevelId { get; set; }

        /// <summary>
        /// Наименование уровня образования
        /// </summary>
        public string EducationLevelName { get; set; }
    }
}
