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

    public string? ResolveVariable(string name) =>
        _variables.GetValueOrDefault(name);

    public Func<FunctionCall, string>? ResolveFunction(string name) =>
        _functions.GetValueOrDefault(name);
}
