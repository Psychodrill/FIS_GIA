namespace Ege.Hsc.Logic.Mappers
{
    using System;
    using Ege.Check.Common;
    using Ege.Hsc.Logic.Models.Servers;
    using TsSoft.Commons.Collections;

    class BlankServerErrorToExcelModelMapper : IMapper<BlankServerError, BlankServerErrorExcelModel>
    {
        public BlankServerErrorExcelModel Map(BlankServerError @from)
        {
            if (from == null)
            {
                throw new ArgumentNullException("from");
            }
            return new BlankServerErrorExcelModel
            {
                Code = from.Code,
                ExamDate = from.ExamDate,
                RbdId = from.RbdId,
                Error = Enums.GetEnumDescription(from.Error),
            };
        }
    }
}
