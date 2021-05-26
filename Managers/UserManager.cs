using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;

using AppEx = TaskManager.Common.Exceptions;
using TaskManager.Data.Contexts;
using TaskManager.Data.Interfaces;
using TaskManager.Models.Requests;
using Responses = TaskManager.Models.Responses;
using System.Text.RegularExpressions;

namespace TaskManager.Managers
{
	public class UserManager : Interfaces.IUserManager
	{
		private readonly IUserContext userContext;
		private IConfiguration configuration { get; }

		private string tokenKey
		{
			get { return configuration.GetValue<string>("TokenKey"); }
		}	

		private int tokenExpirationHours
		{
			get { return configuration.GetValue<Int32>("TokenExpirationHours"); }
		}

		private string passwordRegEx
		{
			get { return configuration.GetValue<string>("PasswordRegEx"); }
		}

		private int userNameLength
		{
			get { return configuration.GetValue<int>("UsernameLength"); }
		}

		public string Authenticate(UserLoginRequest request)
		{
			string tokenResponse = null;
			
			try
			{
				userContext.LoginUser(request);

				var tokenHandler = new JwtSecurityTokenHandler();
				var key = Encoding.ASCII.GetBytes(tokenKey);
				var tokenDescriptor = new SecurityTokenDescriptor
				{
					Subject = new ClaimsIdentity(new Claim[]
					{
					new Claim(ClaimTypes.Name, request.Username)
					}),
					Expires = DateTime.UtcNow.AddHours(tokenExpirationHours),
					SigningCredentials = new SigningCredentials(
						new SymmetricSecurityKey(key),
						SecurityAlgorithms.HmacSha256Signature)
				};
				var token = tokenHandler.CreateToken(tokenDescriptor);
				tokenResponse = tokenHandler.WriteToken(token);
			}
			catch (Exception e)
			{
				if (e.Message.Contains("Incorrect credentials"))
					throw new AppEx.ApplicationException(AppEx.ApplicationExceptions.IncorrectCredentials, $"The credentials provided are incorrect.");
				else
					throw new AppEx.ApplicationException(AppEx.ApplicationExceptions.ServiceFailure, "The service failed to perform the transaction.");

			}

			return tokenResponse;
		}

		public Responses.User CreateUser(UserAddRequest request)
		{
			Responses.User user = null;
			if (request.Username.Length < userNameLength)
				throw new AppEx.ApplicationException(AppEx.ApplicationExceptions.UsernameDoesNotMeetRequirements, $"The user name entered does not meet the requirements. The user name must contain at least 8 characters long.");

			var match = Regex.Match(request.Password, passwordRegEx);

			if (!match.Success)
				throw new AppEx.ApplicationException(AppEx.ApplicationExceptions.PasswordDoesNotMeetRequirements, $"The password entered does not meet the requirements. The password must contain at least 8 characters long, a number, an upper case letter, and a special character.");

			try
			{
				user = userContext.CreateUser(request);
			}
			catch (Exception e)
			{
				if (e.Message.Contains("Cannot insert duplicate key row in object 'dbo.User' with unique index 'User_Username_IDX'."))
					throw new AppEx.ApplicationException(AppEx.ApplicationExceptions.UsernameAlreadyTakenException, $"The user name [{request.Username}] is already taken.");
				else
					throw new AppEx.ApplicationException(AppEx.ApplicationExceptions.ServiceFailure, "The service failed to perform the transaction.");
			}

			return user;
		}

		public Responses.User GetUser(string userName, long userId = 0)
		{
			Responses.User response = null;
			try
			{
				response = userContext.GetUser(userName, userId);
			}
			catch (Exception e)
			{
				throw new AppEx.ApplicationException(AppEx.ApplicationExceptions.ServiceFailure, $"The service failed to perform the transaction. {e.Message}");
			}

			return response;
		}

		public UserManager(IConfiguration configuration, UserContext userContext)
		{
			this.configuration = configuration;
			this.userContext = userContext;
		}
	}
}
