namespace Ege.Check.Logic.Services.Dtos.Metadata
{
    using System;

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class PrimaryKeyPartAttribute : Attribute
    {
    }
}
