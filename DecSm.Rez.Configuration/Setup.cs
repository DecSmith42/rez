namespace DecSm.Rez.Configuration;

[PublicAPI]
public static class Setup
{
    public static IServiceCollection AddResolvableConfiguration(this IServiceCollection services) =>
        services.AddSingleton<IResolvableConfig>(x =>
            new ResolvableConfigurationRoot(x.GetRequiredService<IConfigurationRoot>(),
                x.GetRequiredService<IResolver>()));
}
