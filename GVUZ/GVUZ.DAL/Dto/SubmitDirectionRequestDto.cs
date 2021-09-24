using GVUZ.DAL.Dapper.Model.AllowedDirections;

namespace GVUZ.DAL.Dto
{
    /// <summary>
    /// Данные для формирования заявки на включение или исключение направления из списка разрешенных или из списка с профильными ВИ
    /// </summary>
    public class SubmitDirectionRequestDto
    {
        /// <summary>
        /// Id направления
        /// </summary>
        public int DirectionId { get; set; }

        /// <summary>
        /// Тип создаваемой заявки
        /// </summary>
        public InstitutionDirectionRequestType RequestType { get; set; }

        /// <summary>
        /// Комментарий автора заявки
        /// </summary>
        public string Comment { get; set; }
    }
}
