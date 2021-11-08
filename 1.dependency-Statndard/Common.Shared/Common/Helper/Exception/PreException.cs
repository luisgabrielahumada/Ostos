namespace Common.Shared
{


    /// <summary>
    ///     Exception raised when a precondition fails.
    /// </summary>
    public class PreException : DesignByContractException
    {
        /// <summary>
        ///     Precondition Exception.
        /// </summary>
        public PreException()
        {
        }

        /// <summary>
        ///     Precondition Exception.
        /// </summary>
        public PreException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Precondition Exception.
        /// </summary>
        public PreException(string message, System.Exception inner)
            : base(message, inner)
        {
        }
    }
}