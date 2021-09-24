using GVUZ.DAL.Dapper.Model.AllowedDirections;
using GVUZ.DAL.Dto;
using System.Collections.Generic;

namespace GVUZ.DAL.Dapper.Repository.Interfaces.AllowedDirections
{
    /// <summary>
    /// Репозиторий для работы с заявками на управление списком разрешенных направлений в т.ч. и с профильными специальностями
    /// </summary>
    public interface IAllowedDirectionsRepository
    {
        /// <summary>
        /// Получение списка заявок для управления списком по типам заявок
        /// </summary>
        /// <param name="institutionId">Id ОО</param>
        /// <param name="requestTypes">Список требуемых типов</param>
        /// <returns>Список заявок требуемого типа</returns>
        List<InstitutionDirectionRequestDto> GetDirectionRequestsByTypes(int institutionId, IEnumerable<InstitutionDirectionRequestType> requestTypes);

        /// <summary>
        /// Поиск направлений для включения в список
        /// </summary>
        /// <param name="institutionId">Id ОО</param>
        /// <param name="dto">Критерии поиска</param>
        /// <returns>Список найденных направлений</returns>
        List<DirectionDto> FindDirections(int institutionId, InstitutionDirectionSearchCommand dto);

        /// <summary>
        /// Подтверждение заявки на добавление или удаление разрешенных направлений, а также на включение разрешенных направлений в список с профильными ВИ
        /// </summary>
        /// <param name="institutionId">Id ОО</param>
        /// <param name="requestId">Id заявки</param>
        void ApproveDirectionRequest(int institutionId, int requestId);

        /// <summary>
        /// Отклонение заявки на добавление или удаление разрешенных направлений, а также на включение разрешенных направлений в список с профильными ВИ
        /// </summary>
        /// <param name="institutionId">Id ОО</param>
        /// <param name="requestId">Id заявки</param>
        /// <param name="denialComment">Комментарий к отклоняемой заявке</param>
        void DenyDirectionRequest(int institutionId, int requestId, string denialComment);

        /// <summary>
        /// Удаление отклоненных заявок на добавление или удаление разрешенных направлений или на включение разрешенных направлений в список с профильными ВИ
        /// </summary>
        /// <param name="institutionId">Id ОО</param>
        /// <param name="forProfTests">Признак для удаления отклоненных заявок на включение в список с профильными ВИ</param>
        void RemoveDeniedRequests(int institutionId, bool forProfTests);

        /// <summary>
        /// Получение сведений о заявке на добавление или удаление разрешенных направлений или на включение разрешенных направлений в список с профильными ВИ
        /// </summary>
        /// <param name="institutionId">Id ОО</param>
        /// <param name="requestId">Id заявки</param>
        /// <param name="loadComments">Загружать текст комментария</param>
        /// <returns>Сведения о заявке</returns>
        InstitutionDirectionRequestDto GetDirectionRequestById(int institutionId, int requestId, bool loadComments = false);

        /// <summary>
        /// Удаление нерассмотренной заявки на добавление или удаление разрешенных направлений или на включение направлений в список с профильными ВИ
        /// </summary>
        /// <param name="institutionId">Id ОО</param>
        /// <param name="requestId">Id нерассмотренной заявки</param>
        void DeleteDirectionRequest(int institutionId, int requestId);

        /// <summary>
        /// Формирование заявок на добавление или исключение из списка разрешенных направлений или из списка с профильными ВИ
        /// </summary>
        /// <param name="institutionID">Id ОО</param>
        /// <param name="submits">Сведения о формируемых заявках</param>
        void SubmitDirectionRequests(int institutionID, IEnumerable<SubmitDirectionRequestDto> submits);

        /// <summary>
        /// Добавление выбранных администратором направлений в список разрешенных
        /// </summary>
        /// <param name="institutionID">ID ОО</param>
        /// <param name="items">Сведения о добавляемых направлениях</param>
        void AddAllowedDirections(int institutionID, IEnumerable<AllowedDirectionCreateDto> items);

        /// <summary>
        /// Получение сведений о нерассмотренных заявках для всех ОО
        /// </summary>
        List<InstitutionDirectionRequestSummaryDto> GetDirectionsRequestsPaged(IPageable pageable, ISortable sortable);

        /// <summary>
        /// Получение списка запросов на добавление или удаление направлений из списка разрешенных или списка с профильными ВИ
        /// </summary>
        /// <param name="institutionId">Id ОО</param>
        /// <returns>Список запросов на добавление направлений подготовки в качестве разрешенных для указанной ОО или разрешенных направлений с профильными ВИ</returns>
        List<InstitutionDirectionRequestDto> GetDirectionsRequestDetails(int institutionId);
    }
}
