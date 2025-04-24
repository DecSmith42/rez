namespace DecSm.Rez.Abstraction;

/// <summary>
///     Provides a map of variables and functions to be used by an <see cref="IResolver" />,
///     as well as the ability to add and remove variables and functions at any time.
/// </summary>
[PublicAPI]
public interface IResolverStore : IResolverSource
{
    /// <summary>
    ///     Adds a variable to the store.<br />
    ///     If the variable is already added, this method does nothing.
    /// </summary>
    /// <param name="name">The name of the variable to add.</param>
    /// <param name="value">
    ///     The value of the variable to add.<br />
    ///     Note: the value should never be null; instead, use an empty string for a blank result.
    /// </param>
    /// <remarks>
    ///     The braces that denote a function should not be included in the name.
    /// </remarks>
    void AddVariable(string name, string value);

    /// <summary>
    ///     Adds a function to the store.<br />
    ///     If the function is already added, this method does nothing.
    /// </summary>
    /// <param name="name">The name of the function to add.</param>
    /// <param name="function">
    ///     The function to add.<br />
    ///     Note: the value should never be null; instead, use an empty string for a blank result.
    /// </param>
    /// <remarks>
    ///     The braces that denote a function should not be included in the name, nor should the parentheses or any parameters.
    /// </remarks>
    void AddFunction(string name, Func<FunctionCall, string> function);

    /// <summary>
    ///     Removes a variable from the store.<br />
    ///     If the variable was not added, this method does nothing.
    /// </summary>
    /// <param name="name">The name of the variable to remove.</param>
    /// <remarks>
    ///     The braces that denote a function should not be included in the name.
    /// </remarks>
    void RemoveVariable(string name);

    /// <summary>
    ///     Removes a function from the store.<br />
    ///     If the function was not added, this method does nothing.
    /// </summary>
    /// <param name="name">The name of the function to remove.</param>
    /// <remarks>
    ///     The braces that denote a function should not be included in the name, nor should the parentheses or any parameters.
    /// </remarks>
    void RemoveFunction(string name);
}
