namespace FbsServices
{
    /// <summary>
    /// Вид проверки через интерфейс для выбора истории проверок
    /// </summary>
    public enum CertificateCheckMode
    {
        /// <summary>
        /// Интерактивная проверка (1)
        /// </summary>
        Interactive = 1,

        /// <summary>
        /// Пакетная проверка (2)
        /// </summary>
        Batch = 2
    }
}