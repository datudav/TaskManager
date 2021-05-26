using System;

namespace TaskManager.Common.Exceptions
{
	public class ApplicationException : Exception
	{
		private string message;
		private ExceptionType exceptionType;

		public ExceptionType Type
		{
			get { return exceptionType; }
		}

		public override string Message
		{
			get { return message; }
		}

		public ApplicationException(ExceptionType type, string message) : base(message)
		{
			this.message = message;
			this.exceptionType = type;
		}
	}
}
