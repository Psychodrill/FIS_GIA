namespace Ege.Check.Logic.Services.Dtos.Enums
{
    using System;
    using System.Xml.Serialization;

    [Serializable]
    public enum TaskType
    {
        [XmlEnum("0")] A = 0,
        [XmlEnum("1")] B = 1,
        [XmlEnum("2")] C = 2,
        [XmlEnum("3")] D = 3,
    }
}