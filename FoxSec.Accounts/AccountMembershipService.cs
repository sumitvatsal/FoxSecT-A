using System;
using System.Web.Security;
using FoxSec.Common.Helpers;
using FoxSec.DomainModel.DomainObjects;
using FoxSec.Infrastructure.EF.Repositories;

namespace FoxSec.Accounts
{
	public class AccountMembershipService : IMembershipService
	{
		private readonly IUserRepository _userRepository;
		public AccountMembershipService(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}
		public int MinPasswordLength
		{
			get { throw new NotImplementedException(); }
		}
		public bool ValidateUser(string loginName, string password, out User user)
		{
			user = _userRepository.FindByLoginName(loginName);
            return user != null && user.Password == EncodePassword.ToMD5(password);
		}

		public MembershipCreateStatus CreateUser(string userName, string password, string email)
		{
			throw new NotImplementedException();
		}

		public bool ChangePassword(string userName, string oldPassword, string newPassword)
		{
			throw new NotImplementedException();
		}
	}
}
