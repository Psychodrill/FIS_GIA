using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using FogSoft.Helpers;
using GVUZ.Helper;
using GVUZ.Model.Entrants;
using GVUZ.Web.ViewModels;
using PersonalDataAccessLog = GVUZ.Model.Entrants.PersonalDataAccessLog;

namespace GVUZ.Web.ContextExtensions
{
	/// <summary>
	/// Журнал логгирование доступа к ПД
	/// </summary>
	public static class AccessLogExtensions
	{
		private static readonly int ListPageSize = AppSettings.Get("Search.PageSize", 25);

		/// <summary>
		/// Загрузка фильтров для страницы
		/// </summary>
		public static AccessListViewModel InitialFillAccessListViewModel(this EntrantsEntities dbContext)
		{
			var model = new AccessListViewModel();
			var inst = dbContext.Institution.Select(x => new { ID = x.InstitutionID, Name = x.BriefName }).ToList();
			model.Institutions = inst.ToArray();

			var types = GetTypes();
			model.Types = types.Select(x => new { ID = x.Item1, Name = x.Item2 }).ToArray();

			return model;
		}

		private static List<Tuple<int, string>> GetTypes()
		{
			List<Tuple<int, string>> types = new List<Tuple<int, string>>();
			types.Add(new Tuple<int, string>(0, "[Любой]"));
			types.Add(new Tuple<int, string>(1, "Создание"));
			types.Add(new Tuple<int, string>(2, "Чтение"));
			types.Add(new Tuple<int, string>(3, "Обновление"));
			types.Add(new Tuple<int, string>(4, "Удаление"));
			return types;
		}

		/// <summary>
		/// Возвращает набор данных с выбранной фильтрацией/сортировкой
		/// </summary>
		public static AjaxResultModel GetAccessListModel(this EntrantsEntities dbContext, AccessListViewModel model, bool usePaging = true)
		{
			IQueryable<PersonalDataAccessLog> tq = dbContext.PersonalDataAccessLog;
			model.TotalItemCount = tq.Count();

			if (!String.IsNullOrEmpty(model.SelectedInstitution))
				tq = tq.Where(x => dbContext.Institution.Any(y => y.BriefName.Contains(model.SelectedInstitution)));
			if (!String.IsNullOrEmpty(model.SelectedLogin))
				tq = tq.Where(x => x.UserLogin.Contains(model.SelectedLogin));

			var dbTypeCodes = new[] { "C", "R", "U", "D" };

			if (model.SelectedType != 0)
			{
				var type = dbTypeCodes[model.SelectedType - 1];
				tq = tq.Where(x => x.Method == type);
			}
				
			if (model.DateBegin.HasValue)
				tq = tq.Where(x => x.AccessDate >= model.DateBegin);
			if (model.DateEnd.HasValue)
			{
				var endDateAdj = model.DateEnd.Value.Date.AddDays(1).AddSeconds(-1);
				tq = tq.Where(x => x.AccessDate <= endDateAdj);
			}

			model.TotalItemFilteredCount = tq.Count();
			model.TotalPageCount = ((Math.Max(model.TotalItemFilteredCount, 1) - 1) / ListPageSize) + 1;

			if (!model.SortID.HasValue)
				model.SortID = -3;

			if (model.SortID == 1) tq = tq.OrderBy(x => x.UserLogin);
			if (model.SortID == -1) tq = tq.OrderByDescending(x => x.UserLogin);
			if (model.SortID == 2) tq = tq.OrderBy(x => x.InstitutionID);
			if (model.SortID == -2) tq = tq.OrderByDescending(x => x.InstitutionID);
			if (model.SortID == 3) tq = tq.OrderBy(x => x.AccessDate);
			if (model.SortID == -3) tq = tq.OrderByDescending(x => x.AccessDate);
			if (model.SortID == 4) tq = tq.OrderByDescending(x => x.Method).ThenBy(x => x.AccessDate);
			if (model.SortID == -4) tq = tq.OrderByDescending(x => x.Method).ThenByDescending(x => x.AccessDate);
			
			if (usePaging)
				tq = tq.Skip((model.PageNumber ?? 0) * ListPageSize).Take(ListPageSize);
			var res = tq.Select(x => new
			                         	{
			                         		x.ID,
											x.InstitutionID,
			                         		InstitutionName = dbContext.Institution.Where(y => y.InstitutionID == x.InstitutionID).Select(y => y.BriefName).FirstOrDefault(),
			                         		x.AccessDate,
			                         		x.Method,
			                         		x.AccessMethod,
			                         		x.ObjectType,
											x.OldData,
											x.NewData,
											x.UserLogin
			                         	}).ToArray();

			var stringTypes = GetTypes();
			
			Dictionary<string, string> dbTypesToString = new Dictionary<string, string>();
			for (var i = 1; i <= dbTypeCodes.Length; i++)
			{
				dbTypesToString[dbTypeCodes[i - 1]] = stringTypes.Where(x => x.Item1 == i).Select(x => x.Item2).Single();
			}

			Dictionary<string, string> objectsToString = new Dictionary<string, string>
			{
				{ "Application", "Заявление" },
				{ "Entrant", "Абитуриент" },
				{ "IdentityDocument", "Документ" },
				{ "Person", "Абитуриент" },
				{ "UserPolicy", "Логин" },
			};

			model.AccessLogs = res.Select(x => new AccessListViewModel.LogData
			                                       	{
			                                       		ID = x.ID,
														InstitutionID = x.InstitutionID ?? 0,
			                                       		InstitutionName = x.InstitutionName,
			                                       		DateCreated = x.AccessDate.ToString("dd.MM.yyy HH:mm:ss"),
														Content = GetContent(x.OldData) ?? GetContent(x.NewData),
														Type = dbTypesToString[x.Method],
														ObjectMethod = x.AccessMethod,
														ObjectName = objectsToString[x.ObjectType],
														Login = x.UserLogin
			                                       	}).ToArray();
			return new AjaxResultModel { Data = model };
		}

		/// <summary>
		/// Генерация строкового представления по сериализованному контенту
		/// </summary>
		private static string GetContent(string serializedContent)
		{
			if (String.IsNullOrEmpty(serializedContent) || serializedContent == "[]") return "";
			var deserializeObject = new JavaScriptSerializer().DeserializeObject(serializedContent);
			var des = deserializeObject as Dictionary<string, object>;
			var arr = deserializeObject as IEnumerable<object>;
			if (des == null && arr == null) return serializedContent;
			
			StringBuilder b = new StringBuilder();
			
			if (arr == null)
				AddContentRow(des, b);
			else
			{
				foreach (var obj in arr)
				{
					var des2 = obj as Dictionary<string, object>;
					if (des2 != null) AddContentRow(des2, b);
					else b.AppendLine((obj ?? "").ToString());
					b.AppendLine();
				}
			}

			return b.ToString();
		}

		private static readonly List<string> _hiddenFields = new List<string>
		{
			"DocumentAttachmentID",
			"RussianDocs"
		};

		private static readonly Dictionary<string, string> _translation = new Dictionary<string, string>
		{
			{ "ApplicationNumber", "Номер заявления" },
			{ "ApplicationDate", "Дата регистрации" },
			{ "ApplicationUID", "UID заявления" },
			{ "EntrantUID", "UID абитуриента" },
		};

		/// <summary>
		/// Добавление конкретной локализованной строки
		/// </summary>
		private static void AddContentRow(Dictionary<string, object> des, StringBuilder b)
		{
			Func<string, string> translate = (x) => _translation.ContainsKey(x) ? _translation[x] : x;
			foreach (var kv in des)
			{
				if (_hiddenFields.Contains(kv.Key)) continue;
				if (kv.Value == null || kv.Value.ToString() == "") continue;
				if (kv.Value is DateTime)
				{
					var dateTime = ((DateTime)kv.Value);
					if (dateTime == DateTime.MinValue) continue;
					b.AppendLine(translate(kv.Key) + ": " + dateTime.ToLocalTime().ToString("dd.MM.yyyy HH:mm"));
				}
				else
				{
					if (kv.Value is int && (int)kv.Value == 0) continue;
					b.AppendLine(translate(kv.Key) + ": " + kv.Value);
				}
			}
		}
	}
}	