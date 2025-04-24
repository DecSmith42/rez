namespace DecSm.Rez.Configuration;

/// <summary>
///     Provides a map of variables populated by an <see cref="IConfigurationRoot" />.
/// </summary>
[PublicAPI]
public sealed class ConfigResolverSource : IResolverSource
{
    private readonly IConfigurationRoot _configuration;

    /// <summary>
    ///     Creates a new <see cref="ConfigResolverSource" /> with the given <see cref="IConfigurationRoot" />.
    /// </summary>
    /// <param name="configuration">
    ///     The <see cref="IConfigurationRoot" /> to use.<br />
    ///     Configuration names that are nested (e.g., "Section": { "Key" } will be flattened (e.g., "Section:Key").
    /// </param>
    /// <exception cref="ArgumentNullException">Thrown if the configuration parameter is null.</exception>
    public ConfigResolverSource(IConfigurationRoot configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public string? ResolveVariable(string name) =>
        _configuration[name];

    public Func<FunctionCall, string>? ResolveFunction(string name) =>
        null;
}
