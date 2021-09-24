using System.Collections;
using GVUZ.ServiceModel.Import.Core.Operations.Conflicts;

namespace GVUZ.ServiceModel.Import.WebService.Dto.Result.Import
{
    public class ErrorInfoImportDto
    {
        public ConflictsResultDto ConflictItemsInfo;
        public string ErrorCode;
        public string Message;

        public ErrorInfoImportDto()
        {
        }

        public ErrorInfoImportDto(int code) : this(code, null)
        {
        }

        public ErrorInfoImportDto(int code, ConflictsResultDto conflictResultDto)
        {
            ErrorCode = code.ToString();
            Message = ConflictMessages.GetMessage(code);
            ConflictItemsInfo = conflictResultDto;
            if (conflictResultDto != null)
            {
                if (IsNullOrEmpty(conflictResultDto.ApplicationCommonBenefits))
                    if (IsNullOrEmpty(conflictResultDto.Applications))
                        if (IsNullOrEmpty(conflictResultDto.CompetitiveGroupItems))
                            if (IsNullOrEmpty(conflictResultDto.EntranceTestResults))
                                if (IsNullOrEmpty(conflictResultDto.OrdersOfAdmission))
                                    ConflictItemsInfo = null;

                if (IsNullOrEmpty(conflictResultDto.ApplicationCommonBenefits))
                    conflictResultDto.ApplicationCommonBenefits = null;
                if (IsNullOrEmpty(conflictResultDto.Applications))
                    conflictResultDto.Applications = null;
                if (IsNullOrEmpty(conflictResultDto.CompetitiveGroupItems))
                    conflictResultDto.CompetitiveGroupItems = null;
                if (IsNullOrEmpty(conflictResultDto.EntranceTestResults))
                    conflictResultDto.EntranceTestResults = null;
                if (IsNullOrEmpty(conflictResultDto.OrdersOfAdmission))
                    conflictResultDto.OrdersOfAdmission = null;
            }
        }

        private static bool IsNullOrEmpty(ICollection collection)
        {
            return collection == null || collection.Count == 0;
        }
    }
}