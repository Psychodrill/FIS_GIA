namespace Ege.Check.Common.Http
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using JetBrains.Annotations;

    public interface IHttpFileLoader
    {
        [NotNull]
        Task<T> Load<T>([NotNull]string url, [NotNull]Func<Stream, T> parser, [NotNull]string errorMessage);
    }
}
