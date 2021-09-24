namespace Ege.Dal.Common.Helpers
{
    using System;
    using System.Data;
    using JetBrains.Annotations;

    public interface IDbTypeFactory
    {
        DbType Create([NotNull]Type cSharpType);
    }
}