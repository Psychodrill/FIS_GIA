namespace Fbs.Core.Organizations
{
    /// <summary>
    /// Статус организации
    /// </summary>
    public enum OrganizationStatus
    {
        /// <summary>
        /// Действующая организация
        /// </summary>
        Operating = 1,

        /// <summary>
        /// Реорганизованная организация
        /// </summary>
        Reorganized = 2,

        /// <summary>
        /// Ликвидированная организация
        /// </summary>>
        Liquidated = 3
    }
}