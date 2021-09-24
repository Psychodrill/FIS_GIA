using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace FogSoft.Web.Mvc
{
	public enum CompareOperation
	{
		EqualTo,
		LessThan,
		GreaterThan
	}

	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class CompareToAttribute : ValidationAttribute
	{
		// TODO: в будущем добавить клиентскую валидацию
		private readonly CompareOperation _operation;
		public string OtherProperty { get; private set; }
		public string OtherPropertyName { get; private set; }
		/// <summary>
		/// Если задано, считает что одно из свойств может быть пустым.</summary>
		public bool IgnoreNullValues { get; set; }

		public CompareToAttribute(CompareOperation operation, string otherProperty)
			: base(Errors.CompareToAttribute_ErrorMessage)
		{
			if (!Enum.IsDefined(typeof (CompareOperation), operation)) throw new ArgumentOutOfRangeException("operation");
			if (otherProperty == null) throw new ArgumentNullException("otherProperty");

			_operation = operation;
			OtherPropertyName = OtherProperty = otherProperty;
		}

		public override string FormatErrorMessage(string name)
		{
			return string.Format(ErrorMessageString, name,
				Errors.ResourceManager.GetString("CompareToAttribute_" + _operation.ToString(), Errors.Culture), OtherPropertyName);
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			PropertyInfo otherProperty = validationContext.ObjectType.GetProperty(OtherProperty);
			if (otherProperty == null)
				return new ValidationResult(string.Format(Errors.PropertyNotFound, OtherProperty));
			OtherPropertyName = otherProperty.GetDisplayName();
			
			object otherValue = otherProperty.GetValue(validationContext.ObjectInstance, null);
			if (value == null && otherValue == null)
				return null;
			if (value == null || otherValue == null)
				return IgnoreNullValues ? null : new ValidationResult(Errors.CompareToAttribute_OneNull);
			
			IComparable comparable1 = value as IComparable;
			IComparable comparable2 = otherValue as IComparable;

			if (comparable1 == null || comparable2 == null)
				return new ValidationResult(Errors.CompareToAttribute_MustBeComparable);

			var result = comparable1.CompareTo(comparable2);

			if (CheckResult(result))
				return null;
			return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
		}

		private bool CheckResult(int result)
		{
			switch (_operation)
			{
				case CompareOperation.LessThan:
					return result < 0;
				case CompareOperation.EqualTo:
					return result == 0;
				case CompareOperation.GreaterThan:
					return result > 0;
				default:
					throw new InvalidOperationException();
			}
		}
	}
}
