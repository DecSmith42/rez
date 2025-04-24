namespace DecSm.Rez.Abstraction;

/// <summary>
///     Provides functionality for resolving variables and functions in a Rez template into an output string.
/// </summary>
[PublicAPI]
public interface IResolver
{
    /// <summary>
    ///     Adds a new source to the resolver.<br />
    ///     If the source is already added, this method does nothing.
    /// </summary>
    /// <param name="source">The source to add.</param>
    /// <returns>The resolver instance.</returns>
    /// <remarks>
    ///     The order in which sources are added is important - sources are resolved in the order they are added.
    /// </remarks>
    IResolver AddSource(IResolverSource source);

    /// <summary>
    ///     Removes a source from the resolver.<br />
    ///     If the source was not added, this method does nothing.
    /// </summary>
    /// <param name="source">The source to remove.</param>
    /// <returns>The resolver instance.</returns>
    IResolver RemoveSource(IResolverSource source);

    /// <summary>
    ///     Resolves variables and functions within the given template text.
    /// </summary>
    /// <param name="input">The template text containing variables and/or functions to be resolved.</param>
    /// <returns>The resolved output text.</returns>
    /// <example>
    ///     The following example demonstrates how to use the Resolve method to resolve variables and functions:
    ///     <br />
    ///     <code lang="csharp">
    ///     resolverInstance.Resolve("The {color} {animal} jumps over the \{escapedVariable\} and calls {function()}.");
    ///     </code>
    ///     <br />
    ///     The output could look like: "The brown fox jumps over the {escapedVariable} and calls the-function-result."
    /// </example>
    string? Resolve(string? input);
}
