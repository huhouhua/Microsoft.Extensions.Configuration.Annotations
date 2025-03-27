// Copyright (c) Kevin Berger Authors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Annotations.Binders;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for registering configuration options with attributes.
/// </summary>
public static class ConfigurationServiceCollectionExtensions
{
    /// <summary>
    /// Adds services for all classes in the specified assemblies that are marked with `OptionsAttribute`.
    /// This method scans the given assemblies for types that are decorated with the `OptionsAttribute` 
    /// and binds them to the configuration sections using the provided binder.
    /// </summary>
    /// <param name="services">The DI container.</param>
    /// <param name="configuration">The configuration source containing configuration data.</param>
    /// <param name="binder">The binder instance used to bind configuration data to options types.</param>
    /// <param name="assemblies">The assemblies to scan for option classes marked with `OptionsAttribute`.</param>
    /// <returns>The <see cref="IServiceCollection"/> with registered options services.</returns>
    /// <exception cref="ArgumentNullException">Thrown if any of the parameters are null.</exception>
    /// <exception cref="TypeInitializationException">Thrown if the provided binder is not of type `ConfigurationOptionsBinder`.</exception>
    public static IServiceCollection AddAttributeConfigurationOptions(
        this IServiceCollection services,
        IConfiguration configuration,
        object binder,
        params Assembly[] assemblies)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }
        if (configuration is null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }
        if (binder is null)
        {
            throw new ArgumentNullException(nameof(binder));
        }

        // Ensure that the binder is of type ConfigurationOptionsBinder
        var binderType = binder.GetType();
        if (binder is not ConfigurationOptionsBinder)
        {
            throw new TypeInitializationException(binderType.FullName, null);
        }

        // Get the 'Bind' method from the binder
        MethodInfo? binderMethod = binderType.GetMethod("Bind");

        // Iterate through each assembly to find types with the `OptionsAttribute`
        foreach (var assembly in assemblies)
        {
            // Find all types in the assembly marked with `OptionsAttribute`
            foreach (var optionsType in assembly.GetTypes().Where(q => q.IsDefined(typeof(OptionsAttribute), true)))
            {
                var optionsAttribute = optionsType.GetCustomAttribute<OptionsAttribute>();
                if (optionsAttribute is not null)
                {
                    // Get the section key from the attribute or use the class name as default
                    var key = optionsAttribute.SessionKey ?? optionsType.Name;
                    var section = configuration.GetSection(key);

                    // Configure the BinderOptions using the attributes
                    Action<BinderOptions>? configureBinder = binderOptions =>
                    {
                        binderOptions.BindNonPublicProperties = optionsAttribute.BindNonPublicProperties;
                        binderOptions.ErrorOnUnknownConfiguration = optionsAttribute.ErrorOnUnknownConfiguration;
                    };

                    // Bind the options from the configuration section
                    var options = section.Get(optionsType, configureBinder);

                    // Create a binder for the options type and invoke the 'Bind' method via reflection
                    binderMethod?.MakeGenericMethod(optionsType)
                        .Invoke(binder, new object?[] { section, options, configureBinder });
                }
            }
        }

        // Return the service collection with the added configuration options
        return services;
    }

    /// <summary>
    /// Adds services for all classes in the specified assemblies that are marked with `OptionsAttribute`.
    /// This overload uses the default implementation of `ConfigurationOptionsBinderImpl` for binding.
    /// </summary>
    /// <param name="services">The DI container.</param>
    /// <param name="configuration">The configuration source containing configuration data.</param>
    /// <param name="assemblies">The assemblies to scan for option classes marked with `OptionsAttribute`.</param>
    /// <returns>The <see cref="IServiceCollection"/> with registered options services.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the `services` or `configuration` is null.</exception>
    public static IServiceCollection AddAttributeConfigurationOptions(
        this IServiceCollection services,
        IConfiguration configuration,
        params Assembly[] assemblies)
    {
        // Use a default instance of ConfigurationOptionsBinderImpl when no binder is provided
        return services.AddAttributeConfigurationOptions(configuration, new ConfigurationOptionsBinderImpl(services), assemblies);
    }
}
