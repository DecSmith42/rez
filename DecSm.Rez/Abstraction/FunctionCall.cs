namespace DecSm.Rez.Abstraction;

/// <summary>
///     Contains arguments for a Rez function call.
///     <br /><br />
///     This is typically passed from an <see cref="IResolverSource" /> to an <see cref="IResolver" />
///     when a function is resolved in a Rez template.
/// </summary>
/// <param name="Args">The arguments for the function call, as a single string</param>
[PublicAPI]
public readonly record struct FunctionCall(string? Args);
