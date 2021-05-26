using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

using TaskManager.Models.Requests;
using Responses = TaskManager.Models.Responses;
using TaskManager.Data.Interfaces;

namespace TaskManager.Data.Contexts
{
	public class ListContext : DbContext, IListContext
	{
		public ListContext(DbContextOptions<ListContext> options) : base(options)
		{
		}

		public DbSet<Responses.ToDoList> Lists { get; set; }
		public DbSet<Responses.Task> Tasks { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Responses.ToDoList>().HasNoKey();
			modelBuilder.Entity<Responses.Task>().HasNoKey();
		}

		#region List Methods

		public Responses.ToDoList CreateToDoList(long userId, ToDoListAddRequest request)
		{
			Responses.ToDoList toDoList = null;

			DateTime utcNow = DateTime.UtcNow;
			string sql = "EXEC [dbo].[InsertToDoList] @userId, @name, @description, @createdDate, @modifiedDate";

			List<SqlParameter> parameters = new List<SqlParameter>
			{ 
				// Create parameters    
				new SqlParameter { ParameterName = "@userId", Value = userId },
				new SqlParameter { ParameterName = "@name", Value = request.Name },
				new SqlParameter { ParameterName = "@description", Value = request.Description },
				new SqlParameter { ParameterName = "@createdDate", Value = utcNow },
				new SqlParameter { ParameterName = "@modifiedDate", Value = utcNow }
			};

			toDoList = Lists.FromSqlRaw<Responses.ToDoList>(sql, parameters.ToArray()).ToList().FirstOrDefault();

			return toDoList;
		}

		public List<Responses.ToDoList> GetToDoList(long userId, ToDoListGet request)
		{
			List<Responses.ToDoList> toDoLists = null;

			string sql = "EXEC [dbo].[GetToDoList] @userId, @createdDateStart, @createdDateEnd, @keyword";

			List<SqlParameter> parameters = new List<SqlParameter>
			{ 
				// Create parameters    
				new SqlParameter { ParameterName = "@userId", Value = userId },
				new SqlParameter { ParameterName = "@createdDateStart", Value = request.CreatedDateStart },
				new SqlParameter { ParameterName = "@createdDateEnd", Value = request.CreatedDateEnd },
				new SqlParameter { ParameterName = "@keyword", Value = !string.IsNullOrWhiteSpace(request.Keyword) ? request.Keyword : string.Empty }
			};

			toDoLists = Lists.FromSqlRaw<Responses.ToDoList>(sql, parameters.ToArray()).ToList();

			return toDoLists;
		}

		public Responses.ToDoList GetToDoList(long listId)
		{
			Responses.ToDoList toDoList = null;

			string sql = "EXEC [dbo].[GetToDoListById] @listId";

			List<SqlParameter> parameters = new List<SqlParameter>
			{ 
				// Create parameters    
				new SqlParameter { ParameterName = "@listId", Value = listId }
			};

			toDoList = Lists.FromSqlRaw<Responses.ToDoList>(sql, parameters.ToArray()).ToList().FirstOrDefault();

			return toDoList;
		}

		public void UpdateList(long listId, ToDoListUpdateRequest request)
		{
			DateTime utcNow = DateTime.UtcNow;
			string sql = "EXEC [dbo].[UpdateToDoList] @listId, @name, @description, @modifiedDate";

			List<SqlParameter> parameters = new List<SqlParameter>
			{ 
				// Create parameters    
				new SqlParameter { ParameterName = "@listId", Value = listId },
				new SqlParameter { ParameterName = "@name", Value = !string.IsNullOrWhiteSpace(request.Name) ? request.Name : string.Empty },
				new SqlParameter { ParameterName = "@description", Value = !string.IsNullOrWhiteSpace(request.Description) ? request.Description : string.Empty },
				new SqlParameter { ParameterName = "@modifiedDate", Value = utcNow }
			};

			Database.ExecuteSqlRaw(sql, parameters.ToArray());
		}

		public void DeleteList(long listId)
		{
			DateTime utcNow = DateTime.UtcNow;
			string sql = "EXEC [dbo].[DeleteToDoList] @listId";

			List<SqlParameter> parameters = new List<SqlParameter>
			{ 
				// Create parameters    
				new SqlParameter { ParameterName = "@listId", Value = listId }
			};

			Database.ExecuteSqlRaw(sql, parameters.ToArray());
		}

		#endregion List Methods

		#region Task Methods

		public Responses.Task CreateTask(long userId, long listId, TaskAddRequest request)
		{
			Responses.Task task = null;

			DateTime utcNow = DateTime.UtcNow;
			string sql = "EXEC [dbo].[InsertTask] @userId, @listId, @name, @description, @createdDate, @modifiedDate";

			List<SqlParameter> parameters = new List<SqlParameter>
			{ 
				// Create parameters    
				new SqlParameter { ParameterName = "@userId", Value = userId },
				new SqlParameter { ParameterName = "@listId", Value = listId },
				new SqlParameter { ParameterName = "@name", Value = request.Name },
				new SqlParameter { ParameterName = "@description", Value = !string.IsNullOrWhiteSpace(request.Description) ? request.Description : string.Empty },
				new SqlParameter { ParameterName = "@createdDate", Value = utcNow },
				new SqlParameter { ParameterName = "@modifiedDate", Value = utcNow }
			};

			task = Tasks.FromSqlRaw<Responses.Task>(sql, parameters.ToArray()).ToList().FirstOrDefault();

			return task;
		}

		public List<Responses.Task> GetTask(long userId, long listId, TaskGet request)
		{
			List<Responses.Task> tasks = null;

			string sql = "EXEC [dbo].[GetTask] @userId, @listId, @createdDateStart, @createdDateEnd, @keyword, @descendingOrder";

			List<SqlParameter> parameters = new List<SqlParameter>
			{ 
				// Create parameters    
				new SqlParameter { ParameterName = "@userId", Value = userId },
				new SqlParameter { ParameterName = "@listId", Value = listId },
				new SqlParameter { ParameterName = "@createdDateStart", Value = request.CreatedDateStart },
				new SqlParameter { ParameterName = "@createdDateEnd", Value = request.CreatedDateEnd },
				new SqlParameter { ParameterName = "@keyword", Value = !string.IsNullOrWhiteSpace(request.Keyword) ? request.Keyword : string.Empty },
				new SqlParameter { ParameterName = "@descendingOrder", Value = false }
			};

			tasks = Tasks.FromSqlRaw<Responses.Task>(sql, parameters.ToArray()).ToList();

			return tasks;
		}

		public Responses.Task GetTask(long taskId)
		{
			Responses.Task task = null;

			string sql = "EXEC [dbo].[GetTaskById] @taskId";

			List<SqlParameter> parameters = new List<SqlParameter>
			{ 
				// Create parameters    
				new SqlParameter { ParameterName = "@taskId", Value = taskId }
			};

			task = Tasks.FromSqlRaw<Responses.Task>(sql, parameters.ToArray()).ToList().FirstOrDefault();

			return task;
		}

		public void UpdateTask(long listID, long taskId, TaskUpdateRequest request)
		{
			DateTime utcNow = DateTime.UtcNow;
			string sql = "EXEC [dbo].[UpdateTask] @listId, @taskId, @rank, @name, @description, @modifiedDate";

			List<SqlParameter> parameters = new List<SqlParameter>
			{ 
				// Create parameters    
				new SqlParameter { ParameterName = "@listId", Value = listID },
				new SqlParameter { ParameterName = "@taskId", Value = taskId },
				new SqlParameter { ParameterName = "@rank", Value = request.Rank.HasValue ? request.Rank : 0 },
				new SqlParameter { ParameterName = "@name", Value = !string.IsNullOrWhiteSpace(request.Name) ? request.Name : string.Empty },
				new SqlParameter { ParameterName = "@description", Value = !string.IsNullOrWhiteSpace(request.Description) ? request.Description : string.Empty },
				new SqlParameter { ParameterName = "@modifiedDate", Value = utcNow }
			};

			Database.ExecuteSqlRaw(sql, parameters.ToArray());
		}

		public void DeleteTask(long listId, long taskId)
		{
			DateTime utcNow = DateTime.UtcNow;
			string sql = "EXEC [dbo].[DeleteTask] @listId, @taskId, @modifiedDate";

			List<SqlParameter> parameters = new List<SqlParameter>
			{ 
				// Create parameters    
				new SqlParameter { ParameterName = "@listId", Value = listId },
				new SqlParameter { ParameterName = "@taskId", Value = taskId },
				new SqlParameter { ParameterName = "@modifiedDate", Value = utcNow }
			};

			Database.ExecuteSqlRaw(sql, parameters.ToArray());
		}

		#endregion
	}
}
