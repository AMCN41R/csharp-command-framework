namespace Example.AspCore.Commands
{
    /// <summary>
    /// My custom command context.
    /// </summary>
    internal class CustomContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomContext"/> class.
        /// </summary>
        /// <param name="username">The name of the requesting user.</param>
        public CustomContext(string username)
        {
            this.Username = username;
        }

        /// <summary>
        ///  Gets the name of the requesting user.
        /// </summary>
        public string Username { get; }
    }
}
