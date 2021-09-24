using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using GVUZ.Helper;
using GVUZ.Model;
using GVUZ.Model.Courses;
using GVUZ.Web.ViewModels;

namespace GVUZ.Web.ContextExtensions
{
	/// <summary>
	/// Методы для работы с подготовительными курсами
	/// </summary>
	public static class PreparatoryCourseExtensions
	{
		/// <summary>
		/// Создать подготовительный курс
		/// </summary>
		public static AjaxResultModel CreatePreparatoryCourse(this CoursesEntities dbContext, AddPreparatoryCourseViewModel model)
		{
			bool isEdit = model.CourseID > 0;
			PreparatoryCourse course;
			if (isEdit)
			{
				course = dbContext.PreparatoryCourse
					.Include(c => c.Attachment)
					.Include(c => c.CourseSubject)
					.Where(x => x.PreparatoryCourseID == model.CourseID).FirstOrDefault();
				if (course == null)
					return new AjaxResultModel("Не найден курс подготовки");
			}
			else
				course = new PreparatoryCourse();

			if (String.IsNullOrWhiteSpace(model.CourseName))
			    return new AjaxResultModel("Необходимо ввести название курса");
			course.CourseName = model.CourseName;
			//course.CourseType = dbContext.CourseType.Where(x => x.CourseID == model.CourseTypeID).First();
			course.CourseType = null;
			course.Information = model.Information;
			if (!isEdit)
			{
				course.Institution = dbContext.Institution.Where(x => x.InstitutionID == model.InstitutionID).FirstOrDefault();
				if (course.Institution == null) //странная фатальная ошибка, соответственно обработка соответствующая
					return new AjaxResultModel("Не найден вуз");
			}

			if (isEdit && (model.FileID.HasValue || model.FileDeleted) && course.MoreInformation > 0)
			{				
				dbContext.Attachment.DeleteObject(course.Attachment);
				course.Attachment = null;
			}

			if (model.FileID.HasValue)
				course.Attachment = dbContext.Attachment.Where(x => x.FileID == model.FileID).FirstOrDefault();

			if (isEdit)
			{				
				foreach (var courseSubject in course.CourseSubject.ToArray())
					dbContext.CourseSubject.DeleteObject(courseSubject);
			}

			if (!isEdit)
				dbContext.PreparatoryCourse.AddObject(course);
			//если есть предметы, заполняем их
			if (model.SubjectData != null)
			{
				Subject[] subjects = dbContext.Subject.ToArray();
				foreach (string s in model.SubjectData)
				{
					CourseSubject sbj = new CourseSubject();
					Subject subject = subjects.Where(x => x.Name == s).FirstOrDefault();
					sbj.PreparatoryCourse = course;
					if (subject != null)
						sbj.Subject = subject;
					sbj.SubjectName = s;
					dbContext.CourseSubject.AddObject(sbj);
				}
			}

			try
			{
				dbContext.SaveChanges();
			}
			catch (Exception ex)
			{
				SqlException inner = ex.InnerException as SqlException;
				if (inner != null && inner.Message.Contains("UK_PreparatoryCourse"))
				{
					return new AjaxResultModel("Курс с данным именем уже существует");
				}

				if (inner != null && inner.Message.Contains("UK_CourseSubject"))
				{
					return new AjaxResultModel("Название предметов повторяется");
				}
				//неизвестная ошибка при сохранении - кидаем эксепшен дальше
				throw;
			}

			return new AjaxResultModel
			{
				Data = new PreparatoryCourseViewModel.CourseData
				{
					CourseID = course.PreparatoryCourseID,					
					Name = course.CourseName,
					InformationRaw = course.Information,
					SubjectsList = course.CourseSubject.Select(y => y.SubjectName ?? y.Subject.Name),
					FileID = course.Attachment != null ? course.Attachment.FileID : (Guid?)null,
					FileName = course.Attachment != null ? course.Attachment.Name : null
				}
			};
		}

		/// <summary>
		/// Создать модель для подготовительного курса
		/// </summary>
		public static AddPreparatoryCourseViewModel InitPreparatoryCourse(this CoursesEntities dbContext, AddPreparatoryCourseViewModel model)
		{
            model.PredefinedSubjects = DictionaryCache.GetEntries(DictionaryCache.DictionaryTypeEnum.Subject).OrderBy(x => x.Value).Select(x => x.Value).ToArray();
			model.CourseTypeList = dbContext.CourseType.OrderBy(x => x.Name).Select(x => new { ID = x.CourseID, Name = x.Name }).ToArray();
			return model;
		}

		/// <summary>
		/// Загрузить подготовительный курс
		/// </summary>
		public static AddPreparatoryCourseViewModel LoadPreparatoryCourse(this CoursesEntities dbContext, AddPreparatoryCourseViewModel model)
		{
			model = dbContext.InitPreparatoryCourse(model);
			PreparatoryCourse course = dbContext.PreparatoryCourse
				.Include(c => c.Attachment)
				.Include(c => c.CourseSubject)
				.First(x => x.PreparatoryCourseID == model.CourseID);
			
			if (course.MoreInformation > 0)
			{				
				model.FileID = course.Attachment.FileID;
				model.FileName = course.Attachment.Name;
			}
			//model.CourseTypeID = course.CourseTypeID;
			model.Information = course.Information;
			model.CourseName = course.CourseName;
			model.SubjectData = course.CourseSubject.OrderBy(x => x.SubjectName).Select(x => x.SubjectName ?? x.Subject.Name).ToArray();
			return model;
		}

		/// <summary>
		/// Получить список подготовительных курсов
		/// </summary>
		public static PreparatoryCourseViewModel FillPreparatoryCourses(this CoursesEntities dbContext, PreparatoryCourseViewModel model)
		{
			var q = from x in dbContext.PreparatoryCourse
			        where x.InstitutionID == model.InstitutionID
			        orderby x.CourseName
			        select new PreparatoryCourseViewModel.CourseData
			               {
			               	CourseID = x.PreparatoryCourseID,
			               	Name = x.CourseName,
			               	InformationRaw = x.Information,
							//Subjects = x.CourseSubject.Aggregate("", (a, y) => a + ";" + y.SubjectName)
							SubjectsList = x.CourseSubject.Select(y => y.SubjectName ?? y.Subject.Name),
							FileID = x.Attachment.FileID,
							FileName = x.Attachment.Name
			               };
			model.Courses = q.ToArray();
			return model;
		}

		/// <summary>
		/// Удалить подготовительный курс
		/// </summary>
		public static AjaxResultModel DeletePreparatoryCourse(this CoursesEntities dbContext, int? preparatoryCourseID)
		{
			PreparatoryCourse course = dbContext.PreparatoryCourse.Where(x => x.PreparatoryCourseID == preparatoryCourseID).FirstOrDefault();
			if (course == null)
				return new AjaxResultModel("Не найден курс");
			dbContext.PreparatoryCourse.DeleteObject(course);
			dbContext.SaveChanges();
			return new AjaxResultModel();
		}
	}
}