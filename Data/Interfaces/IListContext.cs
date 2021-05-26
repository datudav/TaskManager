using System.Collections.Generic;

using TaskManager.Models.Requests;
using Responses = TaskManager.Models.Responses;

namespace TaskManager.Data.Interfaces
{
	public interface IListContext
	{
		public Responses.ToDoList CreateToDoList(long userId, ToDoListAddRequest request);

		public List<Responses.ToDoList> GetToDoList(long userId, ToDoListGet request);

		public Responses.ToDoList GetToDoList(long listId);

		public void UpdateList(long listId, ToDoListUpdateRequest request);

		public void DeleteList(long listId);

		public Responses.Task CreateTask(long userId, long listId, TaskAddRequest request);

		public List<Responses.Task> GetTask(long userId, long listId, TaskGet request);

		public Responses.Task GetTask(long taskId);

		public void UpdateTask(long listID, long taskId, TaskUpdateRequest request);

		public void DeleteTask(long listId, long taskId);
	}
}
