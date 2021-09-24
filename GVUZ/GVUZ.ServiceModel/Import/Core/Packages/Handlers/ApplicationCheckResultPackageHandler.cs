using GVUZ.Model.Import.Package;
using GVUZ.Model.Import.Schemas;
using GVUZ.Model.Import.WebService.Dto;
using GVUZ.Model.Import.WebService.Dto.Result;

namespace GVUZ.Model.Import.Core.Packages.Handlers
{
	/// <summary>
	/// Обработчик проверки заявлений. Тестовый.
	/// </summary>
	public class ApplicationCheckResultPackageHandler : PackageHandler
	{
		private readonly ImportPackage _repositoryPackageData;
		private readonly GetResultCheckApplication _inputPackage;

		public ApplicationCheckResultPackageHandler(ImportPackage repositoryPackageData)
		{
			_repositoryPackageData = repositoryPackageData;
			_inputPackage = PackageHelper.LoadObjectFromXml<GetResultCheckApplication>(_repositoryPackageData.PackageData);
			if (_inputPackage == null)
				throw new ImportException("Incorrect package structure.");
		}

		public override string ValidatePackage(string packageData)
		{
			return ValidatePackage(packageData, XsdFileManager.XsdName.GetResultCheckApplication);
		}

		public override string Process()
		{
			// string packageValidationResult = ValidatePackage(_repositoryPackageData.PackageData);
			// if(!String.IsNullOrEmpty(packageValidationResult)) return packageValidationResult;

			ImportedAppCheckResultPackage resultPackage = new ImportedAppCheckResultPackage
				{
					PackageID = "43",
					StatusCheckCode = "3",
					StatusCheckMessage = "Processed",
					EgeDocumentCheckResults = new[]
					  {
							new EgeDocumentCheckResultDto
								{
									Application = new ApplicationShortRef
									{
										ApplicationNumber = "45245245",
										RegistrationDateString = "2010-08-12"
									},
									EgeDocuments = new[]
									{	
										new EgeDocumentCheckItemDto
										{
											DocumentNumber = "545245234",
											DocumentDate = "2010-06-01",
											StatusCode = "2",
											StatusMessage = "Verified",
											CorrectResults = new[]
											{
												new CorrectResultItemDto
													{
														Score = "4",
														SubjectName = "Математика"
													}
											},
											IncorrectResults = new[]
											{
												new IncorrectResultItemDto
													{
														Score = "5",
														SubjectName = "Русский язык",
														Comment = "В заявлении указана оценка 4."
													}
											}
										}
									}
								}
						},
					GetEgeDocuments = new[]
						{
							new GetEgeDocumentDto
								{
									Application = new ApplicationShortRef
									{
										ApplicationNumber = "3421",
										RegistrationDateString = "2010-07-11"
									},
									EgeDocuments = new[]
									{
										new EgeDocumentDto
											{
												CertificateNumber = "АВ 3434431",
												Status = "Valid",
												TypographicNumber = "А 34134510-КТ",
												Year = "2010",
												Marks = new[]
												{
													new SubjectMarkDto
														{
															SubjectMark = "4",
															SubjectName = "Алгебра"
														},
													new SubjectMarkDto
														{
															SubjectMark = "5",
															SubjectName = "Геометрия"
														}
												}
											}
									}
								},
						}

				};

			return PackageHelper.GenerateXmlPackageIntoString(resultPackage);
		}

		public override void AddExtraInfoToPackage(ImportPackage importPackage)
		{			
		}

		public override void Dispose()
		{
			// на случай использования единого DbContext'а.
		}
	}
}
