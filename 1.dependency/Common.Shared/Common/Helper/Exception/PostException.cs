namespace Common.Shared
{


    /// <summary>
    ///     Exception raised when a postcondition fails.
    /// </summary>
    public class PostException : DesignByContractException
    {
        /// <summary>
        ///     Postcondition Exception.
        /// </summary>
        public PostException()
        {
        }

        /// <summary>
        ///     Postcondition Exception.
        /// </summary>
        public PostException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Postcondition Exception.
        /// </summary>
        public PostException(string message, System.Exception inner)
            : base(message, inner)
        {
        }
    }
}