namespace DecSm.Rez.Abstraction;

/// <summary>
///     Provides a map of variables and functions to be used by an <see cref="IResolver" />.
/// </summary>
[PublicAPI]
public interface IResolverSource
{
    /// <summary>
    ///     Resolves a variable with the given name.
    /// </summary>
    /// <param name="name">The name of the variable to resolve.</param>
    /// <returns>The value of the variable, or null if the variable cannot be resolved.</returns>
    /// <remarks>
    ///     The braces that denote a variable should not be included in the name.
    /// </remarks>
    string? ResolveVariable(string name);

    /// <summary>
    ///     Resolves a function with the given name.
    /// </summary>
    /// <param name="name">The name of the function to resolve.</param>
    /// <returns>The function, or null if the function cannot be resolved.</returns>
    /// <remarks>
    ///     The braces that denote a function should not be included in the name.
    /// </remarks>
    Func<FunctionCall, string>? ResolveFunction(string name);
}
