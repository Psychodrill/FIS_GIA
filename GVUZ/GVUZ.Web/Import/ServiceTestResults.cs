using GVUZ.ServiceModel.Import.Package;
using GVUZ.ServiceModel.Import.WebService.Dto;
using GVUZ.ServiceModel.Import.WebService.Dto.Result;
using GVUZ.ServiceModel.Import.WebService.Dto.Result.Import;

namespace GVUZ.Web.Import
{
	public class ServiceTestResults
	{
		/// <summary>
		/// Тестовый ответ удаления пакета
		/// </summary>
		public static DeleteResultPackage CreateDeleteResultPackage()
		{
			return new DeleteResultPackage
			{
				Log = new LogDto
				{
					Successful = new SuccessfulImportStatisticsDto
					{
						AdmissionVolumes = "0",
						Applications = "0",
						CompetitiveGroupItems = "0",
						Orders = "0",
                        ApplicationsInOrders = "0"
                        //ConsideredApplications = "0",
                        //RecommendedApplications = "0"
					},
					Failed = new FailedImportInfoDto
					{
						CompetitiveGroupItems = new[]
						{
							new CompetitiveGroupItemFailDetailsDto
							{
								CompetitiveGroupName = "Отделение \"Математика\" (Бакалавр)",
								DirectionCode = "010101",
								DirectionName = "Математика",
								ErrorInfo = new ErrorInfoImportDto
								{
									ConflictItemsInfo = new ConflictsResultDto
									{
										CompetitiveGroupItems = new[] { "4" }
									},
									ErrorCode = "11",
									Message = "Указанное направление отсутствует.",
								}
							}
						}
					}
				},
				Conflicts = new ConflictsResultDto
				{
					CompetitiveGroupItems = new[] { "4" },
					Applications = new[] { new ApplicationShortRef() }
				}
			};
		}
	}
}