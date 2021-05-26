using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.Common.Exceptions
{
	public class ApplicationExceptions : ExceptionType
	{

		public static readonly ApplicationExceptions GeneralValidationException = new ApplicationExceptions(1001, "GeneralValidationException");
		public static readonly ApplicationExceptions UsernameAlreadyTakenException = new ApplicationExceptions(1002, "UsernameAlreadyTakenException");
		public static readonly ApplicationExceptions IncorrectCredentials = new ApplicationExceptions(1003, "IncorrectCredentials");
		public static readonly ApplicationExceptions UserDoesNotExist = new ApplicationExceptions(1004, "UserDoesNotExist");
		public static readonly ApplicationExceptions UnathorizedAccess = new ApplicationExceptions(1005, "UnathorizedAccess");
		public static readonly ApplicationExceptions ListDoesNotExist = new ApplicationExceptions(1006, "ListDoesNotExist");
		public static readonly ApplicationExceptions TaskDoesNotExist = new ApplicationExceptions(1007, "TaskDoesNotExist");
		public static readonly ApplicationExceptions TaskDoesNotBelongToList = new ApplicationExceptions(1008, "TaskDoesNotBelongToList");
		public static readonly ApplicationExceptions RankExceedsMaximumValue = new ApplicationExceptions(1009, "RankExceedsMaximumValue");
		public static readonly ApplicationExceptions UsernameDoesNotMeetRequirements = new ApplicationExceptions(1010, "UsernameDoesNotMeetRequirements");
		public static readonly ApplicationExceptions PasswordDoesNotMeetRequirements = new ApplicationExceptions(1011, "PasswordDoesNotMeetRequirements");
		public static readonly ApplicationExceptions ServiceFailure = new ApplicationExceptions(5000, "ServiceFailure");

		protected ApplicationExceptions(int code, string description)
			: base(code, description)
		{
		}
	}
}
