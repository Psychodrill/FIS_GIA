namespace Ege.Check.Logic.Services.Inspectors
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using JetBrains.Annotations;

    interface IRequestLogWriter
    {
        [NotNull]
        Task Log(MemoryStream stream, [NotNull]Type dto, Guid responseId);
    }
}
