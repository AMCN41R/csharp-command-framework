namespace CommandApi
{
    /// <summary>
    /// Dependency Registration Extensions.
    /// </summary>
    public static class CommandMetadataExtensions
    {
        /// <summary>
        /// Gets the dynamic context from the command metadata as the given type.
        /// If either the metadata or context is null, or is of the wrong type,
        /// the method will return <c>null</c>.
        /// </summary>
        /// <typeparam name="T">The context type.</typeparam>
        /// <param name="metadata">The command metadata.</param>
        /// <returns>The custom context as the given type.</returns>
        public static T? GetContext<T>(this CommandMetadata metadata)
            where T : class
        {
            if (metadata?.Context == null)
            {
                return null;
            }

            if (metadata.Context is T context)
            {
                return context;
            }

            return null;
        }
    }
}
