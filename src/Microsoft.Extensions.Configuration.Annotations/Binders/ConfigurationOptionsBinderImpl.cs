using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Configuration.Annotations.Binders;

/// <summary>
/// A concrete implementation of `ConfigurationOptionsBinder` for binding `IConfiguration`
/// to an options class of type `TOptions`.
/// </summary>
/// <typeparam name="TOptions">The options class type to be bound.</typeparam>
internal class ConfigurationOptionsBinderImpl<TOptions> : ConfigurationOptionsBinder
    where TOptions : class
{
    private readonly IServiceCollection services;

    /// <summary TOptions="`.">
    /// Initializes an instance of `ConfigurationOptionsBinderImpl
    /// </summary>
    /// <param name="services">Dependency injection container.</param>
    public ConfigurationOptionsBinderImpl(IServiceCollection services)
    {
        this.services = services;
    }

    /// <summary>
    /// Binds the configuration section to the specified `TOptions` type.
    /// </summary>
    /// <param name="configuration">Configuration source to bind.</param>
    /// <param name="configureBinder">Optional action to configure binding behavior.</param>
    public override void Bind(IConfiguration configuration, Action<BinderOptions> configureBinder)
    {
        // Registers TOptions in the DI container and binds it to the provided configuration section.
        this.services.AddOptions<TOptions>().Bind(configuration, configureBinder);
    }
}