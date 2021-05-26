using System;
using Microsoft.AspNetCore.Mvc;

namespace TaskManager.Common.Exceptions
{
	public static class ExceptionHelper
	{
		public static ObjectResult HandleApplicationExceptions(Exception ex)
		{
			if (ex is ApplicationException)
			{
				var appEx = (ApplicationException)ex;
				switch (appEx.Type.Code)
				{
					case 1001:
					case 1002:
					case 1003:
					case 1008:
					case 1009:
					case 1010:
					case 1011:
						return new BadRequestObjectResult(ex);
					case 1004:
					case 1006:
					case 1007:
						return new NotFoundObjectResult(ex);
					case 1005:
						return new UnauthorizedObjectResult(ex);
					default:
						return new ObjectResult(ex);
				}
			}
			else
				return new ObjectResult(ex);
		}

	}
}
