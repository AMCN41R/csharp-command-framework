namespace CommandApi.Internal.Registry
{
    using System;

    /// <summary>
    /// A registry of <see cref="CommandHandlerDetail"/>.
    /// </summary>
    internal interface ICommandHandlerRegistry
    {
        /// <summary>
        /// Searches the registry for details for the given handler item name.
        /// </summary>
        /// <param name="name">The name to search the registry with.</param>
        /// <returns>The <see cref="CommandHandlerDetail"/> with the given handler item name.</returns>
        CommandHandlerDetail FindByName(string name);

        /// <summary>
        /// Searches the registry for details for the given handler item type.
        /// </summary>
        /// <param name="typeName">The handler item type name.</param>
        /// <returns>The <see cref="CommandHandlerDetail"/> with the given handler item type.</returns>
        CommandHandlerDetail FindByTypeName(string typeName);

        /// <summary>
        /// Searches the registry for details for the given handler item type.
        /// </summary>
        /// <param name="type">The handler item type.</param>
        /// <returns>The <see cref="CommandHandlerDetail"/> with the given handler item type.</returns>
        CommandHandlerDetail FindByType(Type type);
    }
}
