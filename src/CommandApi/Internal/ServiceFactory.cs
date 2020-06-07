namespace CommandApi.Internal
{
    using System;

    /// <summary>
    /// A factory function that can dynamically resolve an object instance
    /// from its type.
    /// </summary>
    /// <param name="type">The type to resolve.</param>
    /// <returns>The resolved object instance.</returns>
    internal delegate object ServiceFactory(Type type);
}
