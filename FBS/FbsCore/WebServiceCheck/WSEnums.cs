namespace Fbs.Core.WebServiceCheck
{
    /// <summary>
    /// The ws check type.
    /// </summary>
    public enum WSCheckType
    {
        /// <summary>
        /// The ws unknown.
        /// </summary>
        wsUnknown, 

        /// <summary>
        /// одиночная проверка свидетельства
        /// </summary>
        wsSingleCheck,

        /// <summary>
        /// пакетная проверка свидетельств
        /// </summary>
        wsBatchCheck
    }

    /// <summary>
    /// The ws search type.
    /// </summary>
    public enum WSSearchType
    {
        /// <summary>
        /// The ws unknown.
        /// </summary>
        wsUnknown, 

        /// <summary>
        /// ФИО + Номер свидетельства
        /// </summary>
        wsCertNumber,

        /// <summary>
        /// ФИО + Серия паспорта + Номер паспорта
        /// </summary>
        wsPassport,

        /// <summary>
        /// пакетная проверка свидетельств
        /// </summary>
        wsTypoNumber
    }
}