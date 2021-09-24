using GVUZ.DAL.Dto;
using System.Collections.Generic;

namespace GVUZ.DAL.Dapper.Repository.Interfaces.Institution
{
    public interface IInstitutionRepository
    {
        /// <summary>
        /// Получение общих сведений об ОО для просмотра или редактирования
        /// </summary>
        /// <param name="institutionId">Id ОО</param>
        /// <returns>Данные об ОО для просмотра или редактировани</returns>
        InstitutionInfoDto GetInstitutionInfoDto(int institutionId);

        /// <summary>
        /// Обновление общих сведений об ОО
        /// </summary>
        /// <param name="institutionID">Id ОО</param>
        /// <param name="dto">Данные обновляемых сведений</param>
        void UpdateInstitutionInfo(int institutionID, InstitutionInfoUpdateDto dto);

        /// <summary>
        /// Удаление файла-документа ОО
        /// </summary>
        /// <param name="institutionId">Id ОО</param>
        /// <param name="attachmentId">Id удаляемого файла-документа</param>
        void DeleteInstitutionDocument(int institutionId, int attachmentId);

        /// <summary>
        /// Добавление нового файла - документа ОО с привязкой к году
        /// </summary>
        /// <param name="institutionId">Id ОО</param>
        /// <param name="dto">Данные добавляемого файла-документа</param>
        /// <param name="year">Год, которому соответствует документ</param>
        void AddInstitutionDocument(int institutionId, AttachmentCreateDto dto, int year);

        /// <summary>
        /// Получение списка документов, прикрепленных к ОО с привязкой по годам
        /// </summary>
        /// <param name="institutionId">Id ОО</param>
        /// <returns>Список документов</returns>
        List<InstitutionInfoYearDocumentDto> GetInstitutionDocumentsList(int institutionId);

        /// <summary>
        /// Получение названия ОО по идентификатору
        /// </summary>
        /// <param name="institutionId">Идентификатор ОО</param>
        /// <returns>Название и идентификатор</returns>
        SimpleDto GetInstitutionName(int institutionId);
    }
}
