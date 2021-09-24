namespace Ege.Hsc.Logic.Blanks
{
    using System.IO;
    using System.Linq;
    using Ege.Check.Common;
    using Ege.Hsc.Dal.Entities;
    using Ege.Hsc.Logic.Models.Requests;
    using JetBrains.Annotations;
    using TsSoft.Excel.Generators;

    public interface IErrorFileCreator
    {
        Stream CreateErrorFile(BlankRequest request);
    }

    class ErrorFileCreator : IErrorFileCreator
    {
        [NotNull]private readonly IMapper<BlankRequest, ParticipantErrorCollectionExcelModel> _errorMapper;
        [NotNull] private readonly IExcelGenerator<ParticipantErrorCollectionExcelModel> _generator;

        public ErrorFileCreator(
            [NotNull]IMapper<BlankRequest, ParticipantErrorCollectionExcelModel> errorMapper, 
            [NotNull]IExcelGenerator<ParticipantErrorCollectionExcelModel> generator)
        {
            _errorMapper = errorMapper;
            _generator = generator;
        }

        public Stream CreateErrorFile(BlankRequest request)
        {
            var model = _errorMapper.Map(request);
            if (model == null || !model.Errors.Any())
            {
                return null;
            }
            return _generator.Generate(model);
        }
    }
}
