using System;

namespace GVUZ.Model.Applications
{
	/// <summary>
	/// Ошибка валидации
	/// </summary>
    public class ApplicationValidationErrorInfo : IEquatable<ApplicationValidationErrorInfo>
	{
		public int Code;
		public string Message;
		public ApplicationValidationErrorInfo(int code, string message)
		{
			Code = code;
			Message = message;
		}

        #region IEquatable<ApplicationValidationErrorInfo> Members
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals(obj as ApplicationValidationErrorInfo);
        }

        public bool Equals(ApplicationValidationErrorInfo other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            bool result = true;
            result &= other.Code.Equals(Code);
            result &= other.Message.Equals(Message);
            return result;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = 17;
                result = result * 37 + Code.GetHashCode();
                result = result * 37 + Message.GetHashCode();
                return result;
            }
        }
        #endregion

    }
}