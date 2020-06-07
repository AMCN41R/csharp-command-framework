namespace CommandApi.Exceptions
{
    using System;

    /// <summary>
    /// An exception that is thrown when trying to register commands with names
    /// that are not unique.
    /// </summary>
    [Serializable]
    public class DuplicateCommandNameException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateCommandNameException"/> class.
        /// </summary>
        /// <param name="message">The exception message.</param>
        internal DuplicateCommandNameException(string message)
            : base(message)
        {
        }
    }
}
