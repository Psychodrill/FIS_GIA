namespace GVUZ.DAL.Dto
{
    /// <summary>
    /// Данные о документе, прикрепленным к сведениям об ОО <see cref="InstitutionInfoDto"/>
    /// </summary>
    public class InstitutionInfoDocumentDto : AttachmentDto
    {
        /// <summary>
        /// Идентификатор ОО
        /// </summary>
        public int InstitutionId { get; set; }
    }


}
