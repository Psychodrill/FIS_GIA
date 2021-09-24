using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FogSoft.Web.Mvc;
using GVUZ.Helper;
using GVUZ.Helper.MVC;
using GVUZ.Model.Institutions;
using GVUZ.Web.ContextExtensions;

namespace GVUZ.Web.ViewModels
{
	public class InstituteCommonInfoViewModel : BaseEditViewModel
	{
		public InstituteCommonInfoViewModel()
		{
		}

		public void LoadDictionaries()
		{
			using (var context = new InstitutionsEntities())
			{
				FormOfLawList = context.FormOfLaw.ToList();
				RegionList = context.RegionType.ToList();
			}
		}

		public InstituteCommonInfoViewModel(Institution inst, bool isEdit) : base(isEdit)
		{
			InstitutionLicense instLicense = inst.InstitutionLicense.FirstOrDefault();
			InstitutionAccreditation accreditation = inst.InstitutionAccreditation.FirstOrDefault();

			InstitutionID = inst.InstitutionID;
			FullName = inst.FullName;
			BriefName = inst.BriefName;
			FormOfLawID = inst.FormOfLawID;
			if (/*!isEdit && */FormOfLawID.HasValue)
			{				
				FormOfLawText = inst.FormOfLaw.Name;
			}
			Site = inst.Site;
			RegionID = inst.RegionID;
			if (/*!isEdit && */RegionID.HasValue)
			{
				RegionText = inst.RegionType.Name;
			}
			Address = inst.Address;
			Phone = inst.Phone;
			Fax = inst.Fax;

			if (instLicense != null)
			{
				LicenseNumber = instLicense.LicenseNumber;
				LicenseDate = instLicense.LicenseDate;
				if (instLicense.Attachment != null)
				{
					LicenseDocumentID = instLicense.Attachment.FileID;
					LicenseDocumentName = instLicense.Attachment.Name;
				}
			}

			if (accreditation != null)
			{
				Accreditation = accreditation.Accreditation;
				if (accreditation.Attachment != null)
				{
					AccreditationDocumentID = accreditation.Attachment.FileID;
					AccreditationDocumentName = accreditation.Attachment.Name;
				}
			}

			HasMilitaryDepartment = inst.HasMilitaryDepartment;
			HasHostel = inst.HasHostel;
			HostelCapacity = inst.HostelCapacity;
			HasHostelForEntrants = inst.HasHostelForEntrants;

			if (inst.HostelAttachmentID != null)
			{				
				HostelFecDocumentID = inst.HostelAttachment.FileID;
				HostelFecDocumentName = inst.HostelAttachment.Name;
			}

			if(isEdit)
				LoadDictionaries();
		}

		public InstituteCommonInfoViewModel(InstitutionHistory inst, bool isEdit)
			: base(isEdit)
		{
			InstitutionID = inst.InstitutionID;
			FullName = inst.FullName;
			BriefName = inst.BriefName;
			FormOfLawID = inst.FormOfLawID;
			if (/*!isEdit && */FormOfLawID.HasValue)
			{
				FormOfLawText = inst.FormOfLaw.Name;
			}
			Site = inst.Site;
			RegionID = inst.RegionID;
			if (/*!isEdit && */RegionID.HasValue)
			{
				RegionText = inst.RegionType.Name;
			}
			Address = inst.Address;
			Phone = inst.Phone;
			Fax = inst.Fax;

			LicenseNumber = inst.LicenseNumber;
			LicenseDate = inst.LicenseDate ?? DateTime.MinValue;
			if (inst.LicenseAttachmentID != null)
			{
				LicenseDocumentID = inst.LicenseAttachment.FileID;
				LicenseDocumentName = inst.LicenseAttachment.Name;
			}

			Accreditation = inst.Accreditation;
			if (inst.AccreditationAttachmentID != null)
			{
				AccreditationDocumentID = inst.AccreditationAttachment.FileID;
				AccreditationDocumentName = inst.AccreditationAttachment.Name;
			}

			HasMilitaryDepartment = inst.HasMilitaryDepartment;
			HasHostel = inst.HasHostel;
			HostelCapacity = inst.HostelCapacity;
			HasHostelForEntrants = inst.HasHostelForEntrants;

			if (inst.HostelAttachmentID != null)
			{
				HostelFecDocumentID = inst.HostelAttachment.FileID;
				HostelFecDocumentName = inst.HostelAttachment.Name;
			}

			if (isEdit)
				LoadDictionaries();
		}

		public AjaxResultModel SaveToDb()
		{
			using (var dbContext = new InstitutionsEntities())
			{
				Institution inst = dbContext.LoadInstitution(InstitutionID);

				//GVUZ-855 не меняем эти данные
				//inst.FullName = FullName;
				//inst.BriefName = BriefName;
				//inst.FormOfLawID = FormOfLawID;
				//inst.RegionID = RegionID;

				inst.Site = Site;
				inst.Address = Address;
				inst.Phone = Phone;
				inst.Fax = Fax;
				inst.HasMilitaryDepartment = HasMilitaryDepartment;
				inst.HasHostel = HasHostel;
				inst.HostelCapacity = HasHostel ? HostelCapacity : null;
				inst.HasHostelForEntrants = HasHostelForEntrants;

				// сохранение License				
				InstitutionLicense instLicense = !inst.InstitutionLicense.Any()
				                                 	? new InstitutionLicense {InstitutionID = inst.InstitutionID}
				                                 	: inst.InstitutionLicense.First();

				instLicense.LicenseNumber = LicenseNumber;				
				instLicense.LicenseDate = LicenseDate;
				if (LicenseDocumentID != null)
				{
					if (instLicense.AttachmentID != null)
					{
						//теперь пишем в хистори и не удаляем
						//dbContext.Attachment.DeleteObject(instLicense.Attachment);
					}

					instLicense.Attachment = dbContext.Attachment.First(x => x.FileID == LicenseDocumentID);
				}
				else if (LicenseDocumentDeleted)
					instLicense.AttachmentID = null;

				if (!inst.InstitutionLicense.Any())
					dbContext.InstitutionLicense.AddObject(instLicense);

				// сохранение Accreditation				
				InstitutionAccreditation instAccreditation = !inst.InstitutionAccreditation.Any()
				                                             	? new InstitutionAccreditation
				                                             	  	{InstitutionID = inst.InstitutionID}
				                                             	: inst.InstitutionAccreditation.First();

				instAccreditation.Accreditation = Accreditation;
				if (AccreditationDocumentID != null)
				{
					if(instAccreditation.AttachmentID != null)
					{
						//теперь пишем в хистори и не удаляем
						//dbContext.Attachment.DeleteObject(instAccreditation.Attachment);
					}
					instAccreditation.Attachment = dbContext.Attachment.First(x => x.FileID == AccreditationDocumentID);
				}
				else if (AccreditationDocumentDeleted)
					instAccreditation.AttachmentID = null;


				if (!inst.InstitutionAccreditation.Any())
					dbContext.InstitutionAccreditation.AddObject(instAccreditation);
				// сохранение информации по общежитию
				if (HostelFecDocumentID != null)
				{
					if (inst.HostelAttachmentID != null)
					{	
						//теперь пишем в хистори и не удаляем
						//dbContext.Attachment.DeleteObject(inst.HostelAttachment);
					}
					inst.HostelAttachment = dbContext.Attachment.First(x => x.FileID == HostelFecDocumentID);
				}

				dbContext.SaveChanges();
				dbContext.AddChangesToHistory(InstitutionID);
			}
			return new AjaxResultModel();
		}

		public int InstitutionID { get; set; }

		[DisplayName("Полное наименование")]
		//[LocalRequired]
		[StringLength(500)]
		public string FullName { get; set; }

		[DisplayName("Краткое наименование")]
		//[LocalRequired]
		[StringLength(50)]
		public string BriefName { get; set; }

		[DisplayName("Организационно-правовая форма")]
		public int? FormOfLawID { get; set; }

		public string FormOfLawText { get; private set; }

		[DisplayName("Сайт")]
		[Site]
		[StringLength(255)]
		public string Site { get; set; }

		[DisplayName("Регион")]
		public int? RegionID { get; set; }

		public string RegionText { get; private set; }

		[DisplayName("Адрес")]
		[StringLength(500)]
		public string Address { get; set; }

		[DisplayName("Телефон")]
		[StringLength(30)]
		public string Phone { get; set; }

		[DisplayName("Факс")]
		[StringLength(30)]
		public string Fax { get; set; }

		[DisplayName("Лицензия №")]
		[LocalRequired]
		[StringLength(50)]
		public string LicenseNumber { get; set; }

		[DisplayName("от")]
		[LocalRequired]
		[DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
		//[CustomValidation(typeof (CommonValidator), "ValidateSqlDateTimeDiapason")]
		[Date(">today-100y")]
		[Date("<=today")]
		public DateTime LicenseDate { get; set; }

		[DisplayName("Аккредитация")]
		[LocalRequired]
		[StringLength(50)]
		public string Accreditation { get; set; }

		[DisplayName("Наличие воен. кафедры")]
		public bool HasMilitaryDepartment { get; set; }

		[DisplayName("Наличие общежития")]
		public bool HasHostel { get; set; }

		[DisplayName("Количество мест")]
		[LocalRange(1, 99999)]
		public int? HostelCapacity { get; set; }

		[DisplayName("Общежитие абитуриентам")]
		public bool HasHostelForEntrants { get; set; }

		public IEnumerable FormOfLawList;
		public IEnumerable RegionList;

		public Guid? LicenseDocumentID { get; set; }
		public string LicenseDocumentName { get; set; }

		public Guid? AccreditationDocumentID { get; set; }
		public string AccreditationDocumentName { get; set; }

        public Guid? RulesID { get; set; }
        public string RulesDocumentName { get; set; }

		public Guid? HostelFecDocumentID { get; set; }
		[DisplayName("Условие предоставления")]
		public string HostelFecDocumentName { get; set; }
		
		[DisplayName("Просмотр информации на дату")]
		public int? HistoryID { get; set; }

		public bool HostelFecDocumentDeleted { get; set; }
		public bool LicenseDocumentDeleted { get; set; }
		public bool AccreditationDocumentDeleted { get; set; }

        public int TestNumberToAdd { get; set; }

        

		public IEnumerable ChangeHistories { get; set; }

		//public bool IsFilesValid(HttpFileCollectionBase files)
		//{
		//    using (var dbContext = new InstitutionsEntities())
		//    {
		//        Institution inst = dbContext.LoadInstitution(InstitutionID);
		//        HttpPostedFileBase hpf = files["licenseFile"];
		//        if (hpf == null || hpf.ContentLength == 0)
		//        {
		//            inst.InstitutionLicense.Load();
		//            return inst.InstitutionLicense.Count() > 0;
		//        }
		//        hpf = files["accreditationFile"];
		//        if (hpf == null || hpf.ContentLength == 0)
		//        {
		//            inst.InstitutionAccreditation.Load();
		//            return inst.InstitutionAccreditation.Count() > 0;
		//        }
		//        return true;
		//    }
		//}
	}
}