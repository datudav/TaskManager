using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TaskManager.Models.Requests;
using Responses = TaskManager.Models.Responses;

namespace TaskManager.Managers.Interfaces
{
	public interface IUserManager
	{
		public string Authenticate(UserLoginRequest request);

		public Responses.User CreateUser(UserAddRequest request);

		public Responses.User GetUser(string userName, long userId = 0);
	}
}
