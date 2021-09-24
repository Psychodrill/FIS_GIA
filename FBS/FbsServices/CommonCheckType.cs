namespace FbsServices
{
    /// <summary>
    /// Тип проверки через интерфейс для выбора истории проверок
    /// </summary>
    public enum CommonCheckType
    {   
        /// <summary>
        /// Запрос по регистрационному номеру и ФИО
        /// </summary>
        CertificateNumber = 1,
     
        /// <summary>
        /// Запрос по типографскому номеру и ФИО
        /// </summary>
        TypographicNumber = 2,
        
        /// <summary>
        /// Запрос по ФИО, номеру и серии документа
        /// </summary>
        DocumentNumber = 3,

        /// <summary>
        /// Запрос по ФИО и баллам по предметам
        /// </summary>
        NameAndMarks = 4,

        /// <summary>
        /// Запрос по неполным данным
        /// </summary>
        Wildcard = 5,

        /// <summary>
        /// Запрос по ФИО, номеру и серии документа (старый формат)
        /// </summary>
        DocumentNumberObsolete = 6
    }
}