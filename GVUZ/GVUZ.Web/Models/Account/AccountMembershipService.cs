using System;
using System.Web.Security;

namespace GVUZ.Web.Models.Account
{
	// The FormsAuthentication type is sealed and contains static members, so it is difficult to
	// unit test code that calls its members. The interface and helper class below demonstrate
	// how to create an abstract wrapper around such a type in order to make the AccountController
	// code unit testable.
	public interface IMembershipService
	{
		int MinPasswordLength { get; }

		bool ValidateUser(string userName, string password);
		bool ApproveUser(string userName);

		MembershipCreateStatus CreateUser(string userName, string password, string email, bool isApproved,
		                                  out object providerUserKey);

		bool ChangePassword(string userName, string oldPassword, string newPassword);
		object GetProviderUserKey(string userName);
		void DeleteUser(string userName);
	}

	public class AccountMembershipService : IMembershipService
	{
		private readonly MembershipProvider _provider;

		public AccountMembershipService()
			: this(null)
		{
		}

		public AccountMembershipService(MembershipProvider provider)
		{
			_provider = provider ?? Membership.Provider;
		}

		public int MinPasswordLength
		{
			get { return _provider.MinRequiredPasswordLength; }
		}

		public bool ValidateUser(string userName, string password)
		{
			if (String.IsNullOrEmpty(userName))
				throw new ArgumentException("Значение должно быть задано.", "userName");
			if (String.IsNullOrEmpty(password))
				throw new ArgumentException("Значение должно быть задано.", "password");

			return _provider.ValidateUser(userName, password);
		}

		public void DeleteUser(string userName)
		{
			_provider.DeleteUser(userName, true);
		}

		public object GetProviderUserKey(string userName)
		{
			MembershipUser user = _provider.GetUser(userName, false);
			return user == null ? null : user.ProviderUserKey;
		}

		public bool ApproveUser(string userName)
		{
			MembershipUser user = _provider.GetUser(userName, false);
			if (user != null)
			{
				user.IsApproved = true;
				_provider.UpdateUser(user);
				return true;
			}
			return false;
		}

		public MembershipCreateStatus CreateUser(string userName, string password, string email,
		                                         bool isApproved, out object providerUserKey)
		{
			if (String.IsNullOrEmpty(userName))
				throw new ArgumentException("Значение должно быть задано.", "userName");
			if (String.IsNullOrEmpty(password))
				throw new ArgumentException("Значение должно быть задано.", "password");
			if (String.IsNullOrEmpty(email)) throw new ArgumentException("Значение должно быть задано.", "email");

			MembershipCreateStatus status;
			MembershipUser user = _provider.CreateUser(userName, password, email, null, null, isApproved, null,
			                                           out status);
			providerUserKey = user == null ? null : user.ProviderUserKey;
			return status;
		}

		public bool ChangePassword(string userName, string oldPassword, string newPassword)
		{
			if (String.IsNullOrEmpty(userName))
				throw new ArgumentException("Значение должно быть задано.", "userName");
			if (String.IsNullOrEmpty(oldPassword))
				throw new ArgumentException("Значение должно быть задано.", "oldPassword");
			if (String.IsNullOrEmpty(newPassword))
				throw new ArgumentException("Значение должно быть задано.", "newPassword");

			// The underlying ChangePassword() will throw an exception rather
			// than return false in certain failure scenarios.
			try
			{
				MembershipUser currentUser = _provider.GetUser(userName, true /* userIsOnline */);
				return currentUser.ChangePassword(oldPassword, newPassword);
			}
			catch (ArgumentException)
			{
				return false;
			}
			catch (MembershipPasswordException)
			{
				return false;
			}
		}
	}
}