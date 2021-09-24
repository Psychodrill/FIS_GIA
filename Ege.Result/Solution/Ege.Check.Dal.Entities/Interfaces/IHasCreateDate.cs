namespace Ege.Check.Dal.Entities.Interfaces
{
    using System;

    /// <summary>
    ///     У сущности есть дата создания
    /// </summary>
    public interface IHasCreateDate
    {
        DateTime CreateDate { get; set; }
    }
}