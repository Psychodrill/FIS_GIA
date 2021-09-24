namespace Ege.Check.Logic.LoadServices.Processing
{
    using System.Data;
    using JetBrains.Annotations;

    public interface IDatatableCollector<in TDto>
    {
        [NotNull]
        DataTable Create();

        void AddRow(TDto dto, [NotNull] DataTable table);
    }
}