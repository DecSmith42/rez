namespace DecSm.Rez.Configuration;

/// <inheritdoc />
[PublicAPI]
public interface IResolvableConfig : IConfigurationRoot
{
    IResolver Resolver { get; }

    string? Resolve(string? value);
}
