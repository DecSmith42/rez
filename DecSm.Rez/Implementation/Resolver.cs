namespace DecSm.Rez.Implementation;

/// <inheritdoc />
[PublicAPI]
public sealed
    #if NET8_0_OR_GREATER
    partial
    #endif
    class Resolver : IResolver
{
    #if NET9_0_OR_GREATER
    [GeneratedRegex(@"^(.+?)\((.*)\)$", RegexOptions.Compiled)]
    private static partial Regex FunctionRegex { get; }

    #elif NET8_0_OR_GREATER
    [GeneratedRegex(@"^(.+?)\((.*)\)$", RegexOptions.Compiled)]
    private static partial Regex FunctionRegexGenerated();

    private static Regex FunctionRegex => FunctionRegexGenerated();

    #else
    private static Regex FunctionRegex { get; } = new(@"^(.+?)\((.*)\)$", RegexOptions.Compiled);

    #endif

    private readonly List<IResolverSource> _sources = [];

    public IResolver AddSource(IResolverSource source)
    {
        if (!_sources.Contains(source))
            _sources.Add(source);

        return this;
    }

    public IResolver RemoveSource(IResolverSource source)
    {
        _sources.Remove(source);

        return this;
    }

    public string? Resolve(string? input) =>
        input is null
            ? input
            : Executor.Execute(input, ResolveParsedMember);

    private string? ResolveParsedMember(string input)
    {
        var functionMatch = FunctionRegex.Match(input);

        string? result = null;

        if (functionMatch.Success)
            result = ResolveFunction(functionMatch.Groups[1].Value, new(functionMatch.Groups[2].Value));

        result ??= ResolveVariable(input);

        return result;
    }

    private string? ResolveFunction(string name, FunctionCall functionCall) =>
        _sources
            .Select(source => source
                .ResolveFunction(name)
                ?.Invoke(functionCall))
            .FirstOrDefault(result => result is not null);

    private string? ResolveVariable(string input) =>
        _sources
            .Select(source => source.ResolveVariable(input))
            .FirstOrDefault(result => result is not null);
}
