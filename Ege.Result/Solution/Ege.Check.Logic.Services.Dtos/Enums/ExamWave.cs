namespace Ege.Check.Logic.Services.Dtos.Enums
{
    using System;
    using System.ComponentModel;
    using System.Xml.Serialization;

    [Serializable]
    public enum ExamWave
    {
        [Description("Досрочный февральский этап")] [XmlEnum("0")] February = 0,
        [Description("Досрочный этап")] [XmlEnum("1")] Early = 1,
        [Description("Основной этап")] [XmlEnum("2")] Main = 2,
        [Description("Дополнительный этап")] [XmlEnum("3")] Additional = 3,
        [Description("Сочинение(изложение)")] [XmlEnum("1024")] Composition = 1024,
    }
}
