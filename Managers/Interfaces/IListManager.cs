using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TaskManager.Models.Requests;
using Responses = TaskManager.Models.Responses;

namespace TaskManager.Managers.Interfaces
{
	public interface IListManager
	{
		public Responses.ToDoList CreateList(string userName, ToDoListAddRequest request);

		public List<Responses.ToDoList> GetList(string userName, ToDoListGet request);

		public Responses.ToDoList GetList(string userName, long listId);

		public void UpdateList(string userName, long listId, ToDoListUpdateRequest request);

		public void DeleteList(string userName, long listId);

		public Responses.Task CreateTask(string userName, long listId, TaskAddRequest request);

		public List<Responses.Task> GetTask(string userName, long listId, TaskGet request);

		public Responses.Task GetTask(string userName, long listId, long taskId);

		public void UpdateTask(string userName, long listId, long taskId, TaskUpdateRequest request);

		public void DeleteTask(string userName, long listId, long taskId);
	}
}
