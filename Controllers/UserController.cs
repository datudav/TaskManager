using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using AppEx = TaskManager.Common.Exceptions;
using TaskManager.Managers.Interfaces;
using TaskManager.Models.Requests;

namespace TaskManager.Controllers
{
	[Authorize]
	[ApiController]
	[Route("users")]
	public class UserController : ControllerBase
	{

		private readonly IUserManager userManager;

		public UserController(IUserManager userManager)
		{
			this.userManager = userManager;
		}

		[AllowAnonymous]
		[HttpPost]
		[Route("authenticationToken")]
		public IActionResult AuthenticateUser([FromBody] UserLoginRequest request)
		{
			try
			{
				var token = userManager.Authenticate(request);
				return Ok(token);
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

		[AllowAnonymous]
		[HttpPost]
		public IActionResult CreateUser([FromBody] UserAddRequest userAddRequest)
		{
			try
			{
				var response = userManager.CreateUser(userAddRequest);
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
	}
}
