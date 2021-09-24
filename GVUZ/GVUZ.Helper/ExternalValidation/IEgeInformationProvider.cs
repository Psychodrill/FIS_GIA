namespace GVUZ.Helper.ExternalValidation
{
    public interface IEgeInformationProvider
    {
        /// <summary>
        ///     Возвращает ответ от сервиса проверки ФБС.
        /// </summary>
        EgeResultAndStatus GetEgeInformation(EgePacket packet);
    }
}