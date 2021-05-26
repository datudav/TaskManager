namespace TaskManager.Common.Exceptions
{
	public class BaseException
	{
        #region Declarations

        /// <summary>
        /// The description of the current exception.
        /// </summary>
        protected string _description;

        /// <summary>
        /// The code of the current exception.
        /// </summary>
        protected int _code;

        #endregion Declarations

        #region Properties

        /// <summary>
        /// The code for the enumeration.
        /// </summary>
        public int Code
        {
            get { return _code; }
        }

        /// <summary>
        /// The description for the enumeration.
        /// </summary>
        public string Description
        {
            get { return _description; }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Parameterless constructor. Required for serialization.
        /// </summary>
        protected BaseException() { }

        /// <summary>
        /// Overloaded Constructor.
        /// </summary>
        /// <param name="code">The enumeration code.</param>
        /// <param name="description">The enumeration description.</param>
        protected BaseException(int code, string description)
        {
            _code = code;
            _description = description;
        }

        #endregion Constructors
    }
}
