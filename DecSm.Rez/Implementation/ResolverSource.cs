namespace DecSm.Rez.Implementation;

/// <inheritdoc />
[PublicAPI]
public sealed class ResolverSource : IResolverSource
{
    private readonly Dictionary<string, Func<FunctionCall, string>> _functions;
    private readonly Dictionary<string, string> _variables;

    public ResolverSource(Dictionary<string, string> variables, Dictionary<string, Func<FunctionCall, string>> functions)
    {
        _variables = variables ?? throw new ArgumentNullException(nameof(variables));
        _functions = functions ?? throw new ArgumentNullException(nameof(functions));
    }

    public ResolverSource(
        IEnumerable<KeyValuePair<string, string>> variables,
        IEnumerable<KeyValuePair<string, Func<FunctionCall, string>>> functions)
    {
        _variables = variables.ToDictionary(x => x.Key, x => x.Value);
        _functions = functions.ToDictionary(x => x.Key, x => x.Value);
    }

    public ResolverSource(Dictionary<string, string> variables)
    {
        _variables = variables ?? throw new ArgumentNullException(nameof(variables));
        _functions = new();
    }

    public ResolverSource(IEnumerable<KeyValuePair<string, string>> variables)
    {
        _variables = variables.ToDictionary(x => x.Key, x => x.Value);
        _functions = new();
    }

    public ResolverSource(Dictionary<string, Func<FunctionCall, string>> functions)
    {
        _variables = new();
        _functions = functions ?? throw new ArgumentNullException(nameof(functions));
    }

    public ResolverSource(IEnumerable<KeyValuePair<string, Func<FunctionCall, string>>> functions)
    {
        _variables = new();
        _functions = functions.ToDictionary(x => x.Key, x => x.Value);
    }

    public string? ResolveVariable(string name) =>
        _variables.GetValueOrDefault(name);

    public Func<FunctionCall, string>? ResolveFunction(string name) =>
        _functions.GetValueOrDefault(name);
}
