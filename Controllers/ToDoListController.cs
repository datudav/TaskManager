using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;

using AppEx = TaskManager.Common.Exceptions;
using TaskManager.Models.Requests;
using TaskManager.Managers.Interfaces;

namespace TaskManager.Controllers
{
	[Authorize]
	[ApiController]
	[Route("toDoLists")] 
	public class ToDoListController : ControllerBase
	{

		private readonly IListManager listManager;

		#region To Do List Endpoints

		// POST: toDoLists
		[HttpPost]
		public IActionResult CreateToDoList([FromBody] ToDoListAddRequest request)
		{
			try
			{
				var user = this.User.Identity;
				var response = listManager.CreateList(user.Name, request);
				return Ok(response);
			}
			catch (AppEx.ApplicationException ae)
			{
				return errorHandler(AppEx.ExceptionHelper.HandleApplicationExceptions(ae));
			}
			catch (Exception e)
			{
				return errorHandler(AppEx.ExceptionHelper.HandleApplicationExceptions(e));
			}
		}

		// GET: toDoLists
		[HttpGet]
		[Route("{listId}")]
		public IActionResult GetToDoList([FromRoute] long listId)
		{
			try
			{
				var user = this.User.Identity;
				var response = listManager.GetList(user.Name, listId);
				if (response is null)
					return NotFound($"The list [{listId}] does not exist.");
				return Ok(response);
			}
			catch (AppEx.ApplicationException ae)
			{
				return errorHandler(AppEx.ExceptionHelper.HandleApplicationExceptions(ae));
			}
			catch (Exception e)
			{
				return errorHandler(AppEx.ExceptionHelper.HandleApplicationExceptions(e));
			}
		}

		// GET: toDoLists
		[HttpGet]
		public IActionResult GetListOfToDoLists([FromQuery] ToDoListGet request)
		{
			try
			{
				var user = this.User.Identity;
				var response = listManager.GetList(user.Name, request);
				return Ok(response);
			}
			catch (AppEx.ApplicationException ae)
			{
				return errorHandler(AppEx.ExceptionHelper.HandleApplicationExceptions(ae));
			}
			catch (Exception e)
			{
				return errorHandler(AppEx.ExceptionHelper.HandleApplicationExceptions(e));
			}
		}

		[HttpPatch]
		[Route("{listId}")]
		public IActionResult UpdateToDoList([FromRoute] long listId, [FromBody] ToDoListUpdateRequest request)
		{
			try
			{
				var user = this.User.Identity;
				listManager.UpdateList(user.Name, listId, request);
				return NoContent();
			}
			catch (AppEx.ApplicationException ae)
			{
				return errorHandler(AppEx.ExceptionHelper.HandleApplicationExceptions(ae));
			}
			catch (Exception e)
			{
				return errorHandler(AppEx.ExceptionHelper.HandleApplicationExceptions(e));
			}
		}

		[HttpDelete]
		[Route("{listId}")]
		public IActionResult DeleteToDoList([FromRoute] long listId)
		{
			try
			{
				var user = this.User.Identity;
				listManager.DeleteList(user.Name, listId);
				return NoContent();
			}
			catch (AppEx.ApplicationException ae)
			{
				return errorHandler(AppEx.ExceptionHelper.HandleApplicationExceptions(ae));
			}
			catch (Exception e)
			{
				return errorHandler(AppEx.ExceptionHelper.HandleApplicationExceptions(e));
			}
		}

		#endregion To Do List Endpoints

		#region Task Endpoints

		[HttpPost]
		[Route("{listId}/tasks")]
		public IActionResult CreateTask([FromRoute] long listId, [FromBody] TaskAddRequest request)
		{
			try
			{
				var user = this.User.Identity;
				var response = listManager.CreateTask(user.Name, listId, request);
				return Ok(response);
			}
			catch (AppEx.ApplicationException ae)
			{
				return errorHandler(AppEx.ExceptionHelper.HandleApplicationExceptions(ae));
			}
			catch (Exception e)
			{
				return errorHandler(AppEx.ExceptionHelper.HandleApplicationExceptions(e));
			}
		}

		[HttpGet]
		[Route("{listId}/tasks")]
		public IActionResult GetListOfTasks([FromRoute] long listId, [FromQuery] TaskGet request)
		{
			try
			{
				var user = this.User.Identity;
				var response = listManager.GetTask(user.Name, listId, request);
				return Ok(response);
			}
			catch (AppEx.ApplicationException ae)
			{
				return errorHandler(AppEx.ExceptionHelper.HandleApplicationExceptions(ae));
			}
			catch (Exception e)
			{
				return errorHandler(AppEx.ExceptionHelper.HandleApplicationExceptions(e));
			}
		}

		[HttpGet]
		[Route("{listId}/tasks/{taskId}")]
		public IActionResult GetTask([FromRoute] long listId, [FromRoute] long taskId)
		{
			try
			{
				var user = this.User.Identity;
				var response = listManager.GetTask(user.Name, listId, taskId);
				return Ok(response);
			}
			catch (AppEx.ApplicationException ae)
			{
				return errorHandler(AppEx.ExceptionHelper.HandleApplicationExceptions(ae));
			}
			catch (Exception e)
			{
				return errorHandler(AppEx.ExceptionHelper.HandleApplicationExceptions(e));
			}
		}

		[HttpPatch]
		[Route("{listId}/tasks/{taskId}")]
		public IActionResult UpdateTask([FromRoute] long listId, [FromRoute] long taskId, [FromBody] TaskUpdateRequest request)
		{
			try
			{
				var user = this.User.Identity;
				listManager.UpdateTask(user.Name, listId, taskId, request);
				return NoContent();
			}
			catch (AppEx.ApplicationException ae)
			{
				return errorHandler(AppEx.ExceptionHelper.HandleApplicationExceptions(ae));
			}
			catch (Exception e)
			{
				return errorHandler(AppEx.ExceptionHelper.HandleApplicationExceptions(e));
			}
		}

		[HttpDelete]
		[Route("{listId}/tasks/{taskId}")]
		public IActionResult DeleteTask([FromRoute] long listId, [FromRoute] long taskId)
		{
			try
			{
				var user = this.User.Identity;
				listManager.DeleteTask(user.Name, listId, taskId);
				return NoContent();
			}
			catch (AppEx.ApplicationException ae)
			{
				return errorHandler(AppEx.ExceptionHelper.HandleApplicationExceptions(ae));
			}
			catch (Exception e)
			{
				return errorHandler(AppEx.ExceptionHelper.HandleApplicationExceptions(e));
			}
		}

		#endregion Task Endpoints

		private IActionResult errorHandler(ObjectResult result)
		{
			if (result is BadRequestObjectResult)
				return BadRequest(((AppEx.ApplicationException)result.Value).Message);
			else if (result is NotFoundObjectResult)
				return NotFound(((AppEx.ApplicationException)result.Value).Message);
			else if (result is UnauthorizedObjectResult)
				return Unauthorized(((AppEx.ApplicationException)result.Value).Message);
			else
				return StatusCode(StatusCodes.Status500InternalServerError, "The service failed to perform the transaction.");
		}

		public ToDoListController(IListManager listManager)
		{
			this.listManager = listManager;
		}
	}
}
