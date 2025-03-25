using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Configuration.Annotations.Binders;

/// <summary>
/// Abstract base class for a configuration options binder, providing a `Bind` method to load settings from `IConfiguration`.
/// </summary>
internal abstract class ConfigurationOptionsBinder
{
    /// <summary>
    /// Binds `IConfiguration` to the specified options type.
    /// </summary>
    /// <param name="configuration">Configuration source</param>
    /// <param name="configureBinder">Optional action to configure the binding behavior</param>
    public abstract void Bind(IConfiguration configuration, Action<BinderOptions>? configureBinder = null);


    /// <summary>
    /// Creates an instance of `ConfigurationOptionsBinder` for a specific options type.
    /// </summary>
    /// <param name="services">The IServiceCollection to add the services to.</param>
    /// <param name="optionsType">The options class type to bind, e.g., `MyOptions`</param>
    /// <returns>Returns an instance of `ConfigurationOptionsBinder`</returns>
    /// <exception cref="TypeInitializationException">Throws if instance creation fails</exception>
    public static ConfigurationOptionsBinder Create(IServiceCollection services, 
        Type optionsType)
    {
        var binderType = typeof(ConfigurationOptionsBinderImpl<>).MakeGenericType(optionsType);
        var binder = Activator.CreateInstance(binderType, new object[] { services });

        return binder is ConfigurationOptionsBinder optionsBinder
            ? optionsBinder
            : throw new TypeInitializationException(binderType.FullName, null);
    }
}