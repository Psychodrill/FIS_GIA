using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FogSoft.Web.Mvc;
using GVUZ.Helper.MVC;
using GVUZ.Model.Institutions;
using GVUZ.Web.ContextExtensions;

namespace GVUZ.Web.ViewModels
{
	public class AddStructureItemViewModel : BaseEditViewModel
	{
		public int StructureItemID { get; set; }

		public bool IsAdd { get; set; }

		[DisplayName("Полное наименование")]
		[LocalRequired]
		[StringLength(255)]
		public string FullName { get; set; }

		[DisplayName("Краткое наименование")]
		[StringLength(50)]
		public string BriefName { get; set; }

		[DisplayName("Тип")]
		[LocalRequired]
		public short StructureType { get; set; }

		[DisplayName("Сайт")]
		[Site]
		[StringLength(255)]
		public string Site { get; set; }

		public virtual IEnumerable StructureTypeList
		{
			get
			{
				using (var dbContext = new InstitutionsEntities())
				{
					return dbContext.GetAvailableDescendantStructure(StructureItemID)
						.ConvertAll(x => new {ID = (short) x, Description = x.GetName()});
				}
			}
		}

		public IEnumerable Directions
		{
			get
			{
				using (var dbContext = new InstitutionsEntities())
				{
                    return dbContext.GetAllowedInstitutionItemDirections(StructureItemID)
                        .OrderBy(x => x.Code)
                        .Select(x => new 
                        { 
                            Code = x.Code == null ? "/" + x.NewCode.Trim() : (x.NewCode == null ? (x.Code.Trim() + "." + x.QUALIFICATIONCODE.Trim() + "/") : x.Code.Trim() + "." + x.QUALIFICATIONCODE.Trim() + "/" + x.NewCode.Trim()), 
                            Name = x.Code == null ?  "/" + x.NewCode.Trim() + " " + x.Name.Trim() : (x.NewCode == null ? (x.Code.Trim() + "." + x.QUALIFICATIONCODE.Trim()) + "/ " + x.Name.Trim() : (x.Code.Trim() + "." + x.QUALIFICATIONCODE.Trim() + "/" + x.NewCode.Trim() + " " + x.Name)) 
                        })
                        .ToList();
				}
			}
		}

		[DisplayName("Код направления")]
		public string DirectionCode { get; set; }

		public string StructureItemName { get; set; }

		public AddStructureItemViewModel()
		{
			IsAdd = true;
		}

		public AddStructureItemViewModel(int parentStructureItemID) : this()
		{
			StructureItemID = parentStructureItemID;
		}
	}
}