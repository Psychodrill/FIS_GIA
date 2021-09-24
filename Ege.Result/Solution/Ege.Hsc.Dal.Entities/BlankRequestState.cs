namespace Ege.Hsc.Dal.Entities
{
    using System.ComponentModel;

    public enum BlankRequestState
    {
        [Description("В очереди")]
        Queued = 0,
        [Description("Формируется архив")]
        Zipping = 1,
        [Description("Архив сформирован")]
        Zipped = 2,
        [Description("Архив удалён")]
        ZipDeleted = 3,
    }
}
