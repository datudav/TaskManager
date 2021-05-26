namespace TaskManager.Common.Exceptions
{
	public class ExceptionType : BaseException
	{

        public static readonly ExceptionType Warning = new ExceptionType(2000, "Warning");

        /// <summary>
        /// Overloaded Constructor.
        /// </summary>
        /// <param name="code">The code for the enumeration.</param>
        /// <param name="description">The description for the enumeration.</param>
        public ExceptionType(int code, string description) : base(code, description)
        {
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        protected ExceptionType() { }
    }
}
