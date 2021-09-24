using System;
using System.Collections.Generic;
using Esrp.Core.Organizations;

namespace Esrp.Core.Users
{
	public class OrgRequest
	{
		public int RequestID;
		// статус заявки
		public UserAccount.UserAccountStatusEnum Status;
		public OrgBrief Organization;		

		// пользователи привязанные к заявке
		public List<OrgUserBrief> LinkedUsers = new List<OrgUserBrief>();

		// пользователи привязанные к заявке по своей оргID
		public List<OrgUserBrief> LinkedUsersOrg = new List<OrgUserBrief>();
		
		// заявление подано на активацию уполномоченного сотрудника
		public bool IsForActivation;

		/// <summary>
		/// Проверка может ли заявка быть активирована.
		/// </summary>
		/// <returns></returns>
		public bool CanBeActivated()
		{
			switch (Status)
			{
				case UserAccount.UserAccountStatusEnum.Consideration:
				case UserAccount.UserAccountStatusEnum.Revision:
				case UserAccount.UserAccountStatusEnum.Deactivated:
					return true;
			}

			return false;
		}

		/// <summary>
		/// Проверка может ли заявка быть деактивирована.
		/// </summary>
		/// <returns></returns>
		public bool CanBeDeactivated()
		{
			switch (Status)
			{
				case UserAccount.UserAccountStatusEnum.Activated:
				case UserAccount.UserAccountStatusEnum.Consideration:
				case UserAccount.UserAccountStatusEnum.Registration:
				case UserAccount.UserAccountStatusEnum.Revision:
					return true;
			}

			return false;
		}

		/// <summary>
		/// Проверка может ли заявка быть деактивирована.
		/// </summary>
		/// <returns></returns>
		public bool CanBeSentOnRevision()
		{
			switch (Status)
			{
				case UserAccount.UserAccountStatusEnum.Activated:
				case UserAccount.UserAccountStatusEnum.Consideration:
				case UserAccount.UserAccountStatusEnum.Registration:
				case UserAccount.UserAccountStatusEnum.Revision:
				case UserAccount.UserAccountStatusEnum.Deactivated:
					return true;
			}

			return false;
		}
	}
}
