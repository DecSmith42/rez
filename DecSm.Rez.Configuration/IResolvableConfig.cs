namespace DecSm.Rez.Configuration;

/// <inheritdoc />
[PublicAPI]
public interface IResolvableConfig : IConfigurationRoot
{
    public IResolver Resolver { get; }

    public string? Resolve(string? value);
}
