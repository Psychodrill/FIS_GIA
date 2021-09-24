namespace Ege.Check.Dal.Entities.Interfaces
{
    using System;

    /// <summary>
    ///     У сущности есть идентификатор Guid
    /// </summary>
    public interface IHasGuidId
    {
        Guid Id { get; }
    }
}