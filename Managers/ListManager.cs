using System;
using System.Collections.Generic;

using AppEx = TaskManager.Common.Exceptions;
using TaskManager.Data.Contexts;
using TaskManager.Data.Interfaces;
using TaskManager.Models.Requests;
using Responses = TaskManager.Models.Responses; 

namespace TaskManager.Managers
{
	public class ListManager : Interfaces.IListManager
	{
		private readonly IListContext listContext;
		private readonly Interfaces.IUserManager userManager;


		#region List Methods

		public Responses.ToDoList CreateList(string userName, ToDoListAddRequest request)
		{
			Responses.ToDoList response = null;

			var user = userManager.GetUser(userName);

			if (user is null)
				throw new AppEx.ApplicationException(AppEx.ApplicationExceptions.UserDoesNotExist, $"The user [{userName}] does not exist.");
				
			response = listContext.CreateToDoList(user.UserId, request);

			return response;
		}

		public List<Responses.ToDoList> GetList(string userName, ToDoListGet request)
		{
			List<Responses.ToDoList> response = null;

			var user = userManager.GetUser(userName);

			if (user is null)
				throw new AppEx.ApplicationException(AppEx.ApplicationExceptions.UserDoesNotExist, $"The user [{userName}] does not exist.");

			response = listContext.GetToDoList(user.UserId, request);

			return response;
		}

		public Responses.ToDoList GetList(string userName, long listId)
		{
			Responses.ToDoList response = null;

			var user = userManager.GetUser(userName);

			if (user is null)
				throw new AppEx.ApplicationException(AppEx.ApplicationExceptions.UserDoesNotExist, $"The user [{userName}] does not exist.");

			response = listContext.GetToDoList(listId);

			if (response is not null && response.UserId != user.UserId)
				throw new AppEx.ApplicationException(AppEx.ApplicationExceptions.UnathorizedAccess, $"The user [{userName}] is not allowed to access the resource: list [{listId}].");

			return response;
		}

		public void UpdateList(string userName, long listId, ToDoListUpdateRequest request)
		{
			var user = userManager.GetUser(userName);

			if (user is null)
				throw new AppEx.ApplicationException(AppEx.ApplicationExceptions.UserDoesNotExist, $"The user [{userName}] does not exist.");

			var list = listContext.GetToDoList(listId);

			if (list is null)
				throw new AppEx.ApplicationException(AppEx.ApplicationExceptions.ListDoesNotExist, $"The list [{listId}] does not exist.");

			if (list is not null && list.UserId != user.UserId)
				throw new AppEx.ApplicationException(AppEx.ApplicationExceptions.UnathorizedAccess, $"The user [{userName}] is not allowed to access the resource: list [{listId}].");

			listContext.UpdateList(listId, request);
		}

		public void DeleteList(string userName, long listId)
		{
			var user = userManager.GetUser(userName);

			if (user is null)
				throw new AppEx.ApplicationException(AppEx.ApplicationExceptions.UserDoesNotExist, $"The user [{userName}] does not exist.");

			var list = listContext.GetToDoList(listId);

			if (list is null)
				throw new AppEx.ApplicationException(AppEx.ApplicationExceptions.ListDoesNotExist, $"The list [{listId}] does not exist.");

			if (list is not null && list.UserId != user.UserId)
				throw new AppEx.ApplicationException(AppEx.ApplicationExceptions.UnathorizedAccess, $"The user [{userName}] is not allowed to access the resource: list [{listId}].");

			listContext.DeleteList(listId);
		}

		#endregion List Methods

		#region Task Methods

		public Responses.Task CreateTask(string userName, long listId, TaskAddRequest request)
		{
			validate(userName, listId, null);
			var user = userManager.GetUser(userName);
			return listContext.CreateTask(user.UserId, listId, request);
		}

		public List<Responses.Task> GetTask(string userName, long listId, TaskGet request)
		{
			validate(userName, listId, null);
			var user = userManager.GetUser(userName);
			return listContext.GetTask(user.UserId, listId, request);
		}

		public Responses.Task GetTask(string userName, long listId, long taskId)
		{
			validate(userName, listId, taskId);	
			return listContext.GetTask(taskId);
		}

		public void UpdateTask(string userName, long listId, long taskId, TaskUpdateRequest request)
		{
			validate(userName, listId, taskId);
			try
			{
				listContext.UpdateTask(listId, taskId, request);
			}
			catch (Exception e)
			{
				if (e.Message.Contains("The rank value exceeds the maximum possible value."))
					throw new AppEx.ApplicationException(AppEx.ApplicationExceptions.RankExceedsMaximumValue, $"The rank [{request.Rank}] exceeds the maximum possible value.");
				else
					throw new AppEx.ApplicationException(AppEx.ApplicationExceptions.ServiceFailure, "The service failed to perform the transaction.");
			}
		}

		public void DeleteTask(string userName, long listId, long taskId)
		{
			validate(userName, listId, taskId);
			listContext.DeleteTask(listId, taskId);
		}

		private void validate(string userName, long listId, long? taskId)
		{
			if (!string.IsNullOrWhiteSpace(userName))
			{
				var user = userManager.GetUser(userName);

				if (user is null)
					throw new AppEx.ApplicationException(AppEx.ApplicationExceptions.UserDoesNotExist, $"The user [{userName}] does not exist.");

				var list = GetList(userName, listId);

				if (list is null)
					throw new AppEx.ApplicationException(AppEx.ApplicationExceptions.ListDoesNotExist, $"The list [{listId}] does not exist.");

				if (user.UserId != list.UserId)
					throw new AppEx.ApplicationException(AppEx.ApplicationExceptions.UnathorizedAccess, $"The user [{userName}] is not allowed to access the resource: list [{listId}].");
				
				if (taskId.HasValue)
				{
					var task = listContext.GetTask(taskId.GetValueOrDefault());
					if (task is null)
						throw new AppEx.ApplicationException(AppEx.ApplicationExceptions.TaskDoesNotExist, $"The task [{taskId.GetValueOrDefault()}] does not exist.");

					if (task.ListId != list.ListId)
						throw new AppEx.ApplicationException(AppEx.ApplicationExceptions.TaskDoesNotBelongToList, $"The task [{taskId.GetValueOrDefault()}] does not belong to list [{listId}].");
				}
			}
		}

		#endregion Task Methods

		public ListManager(ListContext listContext, Interfaces.IUserManager userManager)
		{
			this.listContext = listContext;
			this.userManager = userManager;
		}


	}
}
