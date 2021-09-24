using System;
using System.Runtime.Serialization;

namespace GVUZ.ServiceModel.Import.Package
{
    public enum PackageStatus
    {
        None = 0,
        PlacedInQueue = 1,
        Processing = 2,
        Processed = 3
    }

    /// <summary>
    ///     Информация о пакете о его статусе, дате последнего изменения статуса.
    ///     Используется веб-сервисом для информирования клиента о состоянии пакета.
    /// </summary>
    [DataContract]
    public class PackageInfo
    {
        [DataMember] public DateTime PackageAdded;
        [DataMember] public PackageStatus PackageStatus;
        [DataMember] public DateTime PackageStatusChangeDate;
    }
}