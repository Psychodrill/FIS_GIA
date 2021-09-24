using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Objects;
using System.Linq;
using System.Web.Script.Serialization;
using FogSoft.Helpers;
using FogSoft.Web.Mvc;

namespace GVUZ.Model.Entrants.Documents
{
    

	public class IdentityDocumentViewModel : BaseDocumentViewModel
	{
		[DisplayName("Вид документа")]
		[LocalRequired]
		public int IdentityDocumentTypeID { get; set; }

		[ScriptIgnore]
		public string IdentityDocumentTypeName { get; set; }

		[ScriptIgnore]
		public object[] IdentityDocumentList { get; set; }

		[DisplayName("Пол")]
		[LocalRequired]
		public int GenderTypeID { get; set; }

		[ScriptIgnore]
		public string GenderTypeName { get; set; }

		[ScriptIgnore]
		public object[] GenderList { get; set; }

		[DisplayName("Гражданство")]
		public int NationalityTypeID { get; set; }

		[ScriptIgnore]
		public string NationalityTypeName { get; set; }

		[ScriptIgnore]
		public object[] NationalityList { get; set; }

		[DisplayName("Дата рождения")]
		[DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
		[Date("<today-14y")]
		[Date(">today-100y")]
		[LocalRequired]
		public DateTime BirthDate { get; set; }

		[DisplayName("Место рождения")]
		//[LocalRequired]
		[StringLength(200)]
		public string BirthPlace { get; set; }

		//[LocalRequired]
		//public new string DocumentOrganization
		//{
		//    get { return base.DocumentOrganization; }
		//    set { base.DocumentOrganization = value; }
		//}

		//[LocalRequired]
		//public new DateTime? DocumentDate
		//{
		//    get { return base.DocumentDate; }
		//    set { base.DocumentDate = value; }
		//}

		[LocalRequired]
		[StringLength(50)]
		public new string DocumentNumber
		{
			get { return base.DocumentNumber; }
			set { base.DocumentNumber = value; }
		}

		[StringLength(20)]
		//[RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Серия документа содержит недопустимые символы")]
		public new string DocumentSeries
		{
			get { return base.DocumentSeries; }
			set { base.DocumentSeries = value; }
		}

		[DisplayName("Дата выдачи")]
		[DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
		[Date(">today-100y")]
		[Date("<=today")]
		[LocalRequired]
		public new DateTime? DocumentDate
		{
			get { return base.DocumentDate; }
			set { base.DocumentDate = value; }
		}

		//[LocalRequired]
		[DisplayName("Код подразделения")]
		[StringLength(200)]
		public string SubdivisionCode { get; set; }

		[ScriptIgnore]
		public int[] RussianDocs { get; set; }

		public override void FillDataImportLoadSave(EntrantsEntities dbContext)
		{
			//ничего тут не делаем
		}

        public override void FillData(EntrantsEntities dbContext, bool isView, int? competitiveGroupId, int? subjectId)
		{
			if (!isView)
			{
// ReSharper disable CoVariantArrayConversion
			    GenderList = new[] {new
			                            {
			                                ID = GenderType.Male,
                                            Name = typeof(GenderType).GetEnumDescription(GenderType.Male),
			                            },
                                        new
			                            {
			                                ID = GenderType.Female,
                                            Name = typeof(GenderType).GetEnumDescription(GenderType.Female),
			                            }
                };

                NationalityList =
                    dbContext.CountryType.OrderBy(x => x.Name).Select(x => new { ID = x.CountryID, x.Name }).ToArray();
                IdentityDocumentList =
					dbContext.IdentityDocumentType.OrderBy(x => x.IdentityDocumentTypeID).Select(x => new { ID = x.IdentityDocumentTypeID, x.Name }).ToArray();
				RussianDocs =
					dbContext.IdentityDocumentType.Where(x => x.IsRussianNationality).Select(x => x.IdentityDocumentTypeID).ToArray();
// ReSharper restore CoVariantArrayConversion
			}
			else
			{
			    GenderTypeName = GenderType.GetName(GenderTypeID);
				
                NationalityTypeName =
					dbContext.CountryType.Where(x => x.CountryID == NationalityTypeID).Select(x => x.Name).FirstOrDefault();
				IdentityDocumentTypeName =
					dbContext.IdentityDocumentType
						.Where(x => x.IdentityDocumentTypeID == IdentityDocumentTypeID).Select(x => x.Name).FirstOrDefault();
			}
		}

		public IdentityDocumentViewModel()
		{
			DocumentTypeID = 1;
		}

		public override void SaveToAdditionalTable(ObjectContext dbContext)
		{
			dbContext.ExecuteStoreCommand(@"
	DELETE FROM EntrantDocumentIdentity WHERE EntrantDocumentID={0}
	INSERT INTO EntrantDocumentIdentity(EntrantDocumentID, IdentityDocumentTypeID, GenderTypeID, NationalityTypeID, BirthDate, BirthPlace, SubdivisionCode)
	VALUES({0}, {1}, {2}, {3}, {4}, {5}, {6})",
				EntrantDocumentID,
				IdentityDocumentTypeID,
				GenderTypeID,
				NationalityTypeID == 0 ? 237 : NationalityTypeID,
				BirthDate == DateTime.MinValue ? (DateTime?)null : BirthDate,
				BirthPlace,
				SubdivisionCode);
		}

		public static bool IsSeriesRequired(int documentTypeID)
		{
			return new[] { 1, 2, 4, 5, 6, 7 }.Contains(documentTypeID);
		}

		public override void Validate(System.Web.Mvc.ModelStateDictionary modelStateContainer, int institutionID)
		{
			base.Validate(modelStateContainer, institutionID);
			if (IdentityDocumentTypeID == 1) //российский паспорт
			{
				if ((DocumentSeries == null || DocumentSeries.Length != 4 || !DocumentSeries.All(Char.IsDigit))
					|| (DocumentNumber == null || DocumentNumber.Length != 6 || !DocumentNumber.All(Char.IsDigit)))
				{
					modelStateContainer.AddModelError("DocumentNumber", "Неверные серия и номер у Паспорта РФ");
					modelStateContainer.AddModelError("DocumentSeries", "");
				}
			}
			else
			{
				if (IsSeriesRequired(IdentityDocumentTypeID) && String.IsNullOrEmpty(DocumentSeries))
				{
					modelStateContainer.AddModelError("DocumentNumber", "Неверная серия у документа");
					modelStateContainer.AddModelError("DocumentSeries", "");
				}
			}
		}

		~IdentityDocumentViewModel()
		{
			DocumentDate = DateTime.MaxValue;
			IdentityDocumentTypeID = 0;
			EntrantDocumentID = 0;
			//строки немутабельные ничего не можем сделать
		}
	}
}