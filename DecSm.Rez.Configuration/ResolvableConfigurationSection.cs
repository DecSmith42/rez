namespace DecSm.Rez.Configuration;

/// <inheritdoc />
[UsedImplicitly]
internal sealed class ResolvableConfigurationSection(IConfigurationSection target, IResolver resolver) : IConfigurationSection
{
    public IConfigurationSection GetSection(string key) =>
        new ResolvableConfigurationSection(target.GetSection(key), resolver);

    public IEnumerable<IConfigurationSection> GetChildren() =>
        target
            .GetChildren()
            .Select(x => new ResolvableConfigurationSection(x, resolver));

    public IChangeToken GetReloadToken() =>
        target.GetReloadToken();

    public string? this[string key]
    {
        get => resolver.Resolve(target[key]);
        set => target[key] = value;
    }

    public string Key => target.Key;

    public string Path => target.Path;

    public string? Value
    {
        get => resolver.Resolve(target.Value);
        set => target.Value = value;
    }
}
