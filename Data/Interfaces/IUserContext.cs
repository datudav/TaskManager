using TaskManager.Models.Requests;
using Responses = TaskManager.Models.Responses;

namespace TaskManager.Data.Interfaces
{
	public interface IUserContext
	{
		public Responses.User CreateUser(UserAddRequest request);

		public void LoginUser(UserLoginRequest request);

		public Responses.User GetUser(string userName, long userId);

	}
}
