using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

using TaskManager.Common.Enumerations;
using TaskManager.Models.Requests;
using Responses = TaskManager.Models.Responses;
using TaskManager.Data.Interfaces;

namespace TaskManager.Data.Contexts
{
	public class UserContext : DbContext, IUserContext
	{
		public UserContext(DbContextOptions<UserContext> options) : base(options)
		{
		}

		public DbSet<Responses.User> Users { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Responses.User>().HasNoKey();
		}

		public Responses.User CreateUser(UserAddRequest request)
		{
			Responses.User user = null;

			DateTime utcNow = DateTime.UtcNow;
			string sql = "EXEC [dbo].[InsertUser] @username, @password, @accountStatus, @lastLoginDate, @createdDate, @modifiedDate";

			List<SqlParameter> parameters = new List<SqlParameter>
			{ 
				// Create parameters    
				new SqlParameter { ParameterName = "@username", Value = request.Username },
				new SqlParameter { ParameterName = "@password", Value = request.Password },
				new SqlParameter { ParameterName = "@accountStatus", Value = AccountStatus.Active },
				new SqlParameter { ParameterName = "@lastLoginDate", Value = utcNow },
				new SqlParameter { ParameterName = "@createdDate", Value = utcNow },
				new SqlParameter { ParameterName = "@modifiedDate", Value = utcNow }
			};

			if (!string.IsNullOrWhiteSpace(sql))
				user = Users.FromSqlRaw<Responses.User>(sql, parameters.ToArray()).ToList().First();

			return user;
		}

		public void LoginUser(UserLoginRequest request)
		{
			DateTime utcNow = DateTime.UtcNow;
			string sql = "EXEC [dbo].[LoginUser] @username, @password";

			List<SqlParameter> parameters = new List<SqlParameter>
			{ 
				// Create parameters    
				new SqlParameter { ParameterName = "@username", Value = request.Username },
				new SqlParameter { ParameterName = "@password", Value = request.Password }
			};

			Database.ExecuteSqlRaw(sql, parameters.ToArray());
		}

		public Responses.User GetUser(string userName, long userId = 0)
		{

			Responses.User user = null;

			DateTime utcNow = DateTime.UtcNow;
			string sql = string.Empty;

			List<SqlParameter> parameters = new List<SqlParameter>();	
			
			if (!string.IsNullOrWhiteSpace(userName))
			{
				sql = "EXEC [dbo].[GetUser] @username, null";
				parameters.Add(new SqlParameter { ParameterName = "@username", Value = userName });
			}

			if (userId > 0)
			{
				sql = "EXEC [dbo].[GetUser] null, @userId";
				parameters.Add(new SqlParameter { ParameterName = "@userId", Value = userId });
			}


			if (!string.IsNullOrWhiteSpace(sql))
				user = Users.FromSqlRaw<Responses.User>(sql, parameters.ToArray()).ToList().First();

			return user;
		}
	}
}
