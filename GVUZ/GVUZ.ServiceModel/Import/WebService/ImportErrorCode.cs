using System;
using System.Linq.Expressions;

namespace GVUZ.Model.Import.WebService
{
	public enum ImportActionType
	{
		DataLoad = 1
	}

	public enum ImportObjectType
	{
		AdmissionVolume = 1
	}

	public enum ImportErrorType
	{
		// не найдено объекта, на который ссылается импортируемый объект
		CantFindRefObject = 1,
		// свойство объекта не может быть пустым
		PropertyCantBeEmpty = 2,
		// свойство объекта содержит некорректные данные
		IncorrectPropertyValue = 3
	}

	public static class ImportErrorCode
	{
		// тест работы вывода свойства выражения
		// можно выводить атрибут объекта, в котором произошла ошибка.		
		private static string Test<TObject>(Expression<Func<TObject, object>> exp)
		{
			var type = typeof(TObject);
			var objectName = type.Name;
			var member = (MemberExpression)exp.Body;
			return member.Member.Name;			
		}

		public static string GetErrorCode(ImportActionType actionType, ImportObjectType objectType,
			string objectID, ImportErrorType errorType)
		{
			// можно кодифицировать как числами, так и строковыми идентификаторами
			return string.Format("{1,2:00}{2:00}{3:000}-{0}", objectID, ((int)actionType), ((int)objectType), 
				((int)errorType));
		}		
	}
}
