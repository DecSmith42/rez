namespace DecSm.Rez.Implementation;

/// <inheritdoc />
[PublicAPI]
public sealed class ResolverStore : IResolverStore
{
    private readonly Dictionary<string, Func<FunctionCall, string>> _functions = new();
    private readonly Dictionary<string, string> _variables = new();

    public void AddVariable(string name, string value) =>
        _variables[name] = value;

    public void AddFunction(string name, Func<FunctionCall, string> function) =>
        _functions[name] = function;

    public void RemoveVariable(string name) =>
        _variables.Remove(name);

    public void RemoveFunction(string name) =>
        _functions.Remove(name);

    public string? ResolveVariable(string name)
    {
        #if NET8_0_OR_GREATER
        return _variables.GetValueOrDefault(name);
        #else
        return _variables!.GetValueOrDefault(name);
        #endif
    }

    public Func<FunctionCall, string>? ResolveFunction(string name)
    {
        #if NET8_0_OR_GREATER
        return _functions.GetValueOrDefault(name);
        #else
        return _functions!.GetValueOrDefault(name);
        #endif
    }
}
