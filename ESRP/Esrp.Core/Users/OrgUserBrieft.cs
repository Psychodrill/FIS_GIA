using System;
using System.Collections.Generic;
using Esrp.Core.Systems;

namespace Esrp.Core.Users
{
	public class OrgUserBrief:IDisposable
	{
		public string Login;
		public string LastName;
		public string FirstName;
		public string MiddleName;
		public string Phone;
		public string AdminComment;
		public string Email { get { return Login; } }
		// текущий статус пользователя
		public UserAccount.UserAccountStatusEnum Status;

	    /// <summary>
	    /// ИС в которых зарегистрирован пользователь
	    /// </summary>
	    public List<string> FullSystemNameList = new List<string>();

	    public bool HasAccessToFbs;
		public bool HasAccessToFbd;

		public string GetFullName()
		{
			return string.Join(" ", new string[] { LastName, FirstName, MiddleName });
		}

		public UserRegistrationType GetRegistrationType()
		{
			UserRegistrationType regType = UserRegistrationType.None;
			if(HasAccessToFbs) regType |= UserRegistrationType.FbsUser;
			if(HasAccessToFbd) regType |= UserRegistrationType.FbdUser;
			return regType;
		}

		public string GetSystemsName()
		{
			List<string> systems = new List<string>();
			if (HasAccessToFbd)
				systems.Add(GeneralSystemManager.GetSystemName(3));
			if (HasAccessToFbs)
				systems.Add(GeneralSystemManager.GetSystemName(2));
			return String.Join(", ", systems.ToArray());
		}

		public bool HasRegDocument;

        public void Dispose()
        {
            this.AdminComment = "";
            
            this.FirstName = "";
            this.FullSystemNameList = null;
            this.HasAccessToFbd = false;
            this.HasAccessToFbs = false;
            this.HasRegDocument = false;
            this.LastName = "";
            this.Login = "";
            this.MiddleName = "";
            this.Phone = "";
            this.Status = UserAccount.UserAccountStatusEnum.None;
        }
    }
}
