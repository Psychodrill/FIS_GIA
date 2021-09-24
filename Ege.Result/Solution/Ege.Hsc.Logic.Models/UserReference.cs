namespace Ege.Hsc.Logic.Models
{
    public class UserReference
    {
        /// <summary>
        /// Идентификатор оператора ФЦТ
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// Логин вуза для ЕСРП
        /// </summary>
        public string EsrpLogin { get; set; }
    }
}
