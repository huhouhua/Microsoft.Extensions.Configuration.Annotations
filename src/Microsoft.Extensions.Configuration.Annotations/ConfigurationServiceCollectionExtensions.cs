// Copyright (c) Kevin Berger Authors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Annotations;
using Microsoft.Extensions.Configuration.Annotations.Binders;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for registering configuration options with attributes.
/// </summary>
public static class ConfigurationServiceCollectionExtensions
{
    /// <summary>
    /// Adds services for all classes in the specified assemblies that are marked with `OptionsAttribute`,
    /// binding them to configuration sections using the provided binder.
    /// </summary>
    /// <param name="services">The DI container.</param>
    /// <param name="configuration">The configuration source containing configuration data.</param>
    /// <param name="binder">The binder instance used to bind configuration data to options types.</param>
    /// <param name="assemblies">The assemblies to scan for option classes marked with `OptionsAttribute`.</param>
    /// <param name="enableGlobalAnnotation">Global annotation enables automatic validation and binding behavior based on the ValidateDataAnnotations.</param>
    /// <returns>The <see cref="IServiceCollection"/> with registered options services.</returns>
    /// <exception cref="ArgumentNullException">Thrown if any of the parameters are null.</exception>
    /// <exception cref="TypeInitializationException">Thrown if the provided binder is not of type `ConfigurationOptionsBinder`.</exception>
    public static IServiceCollection AddAttributeConfigurationOptions(
        this IServiceCollection services,
        IConfiguration configuration,
        bool enableGlobalAnnotation,
        ConfigurationOptionsBinder binder,
        params Assembly[] assemblies)
    {
        if (services is null) throw new ArgumentNullException(nameof(services));
        if (configuration is null) throw new ArgumentNullException(nameof(configuration));
        if (binder is null) throw new ArgumentNullException(nameof(binder));

        // Ensure that the binder is of type ConfigurationOptionsBinder
        var binderType = binder.GetType();
        MethodInfo? binderMethod = binderType.GetMethod("Bind");

        // Iterate through each assembly to find types with the `OptionsAttribute`
        foreach (var assembly in assemblies)
        {
            foreach (var optionsType in assembly.GetTypes().Where(q => q.IsDefined(typeof(OptionsAttribute), true)))
            {
                var optionsAttribute = optionsType.GetCustomAttribute<OptionsAttribute>();
                if (optionsAttribute is null) continue;

                // Get the optional `ValidateAttribute` for validation
                var validateAttribute = optionsType.GetCustomAttribute<ValidateAttribute>();

                // Get the section key from the attribute or use the class name as default
                var key = optionsAttribute.SessionKey ?? optionsType.Name;
                var section = configuration.GetSection(key);

                var context = new BinderContext(optionsAttribute, validateAttribute, enableGlobalAnnotation);
                var options = section.Get(optionsType, context.ConfigureBinder);

                // Invoke the binder's `Bind` method via reflection
                binderMethod?.MakeGenericMethod(optionsType)
                    .Invoke(binder, new object?[] { context, section, options });
            }
        }

        return services;
    }

    /// <summary>
    /// Adds services for all classes in the specified assemblies that are marked with `OptionsAttribute`,
    /// using the default implementation of `ConfigurationOptionsBinderImpl` for binding.
    /// </summary>
    /// <param name="services">The DI container.</param>
    /// <param name="configuration">The configuration source containing configuration data.</param>
    /// <param name="assemblies">The assemblies to scan for option classes marked with `OptionsAttribute`.</param>
    /// <param name="enableGlobalAnnotation">Global annotation enables automatic validation and binding behavior based on the ValidateDataAnnotations.</param>
    /// <returns>The <see cref="IServiceCollection"/> with registered options services.</returns>
    public static IServiceCollection AddAttributeConfigurationOptions(
        this IServiceCollection services,
        IConfiguration configuration,
        bool enableGlobalAnnotation = true,
        params Assembly[] assemblies)
    {
        // Use a default instance of ConfigurationOptionsBinderImpl when no binder is provided
        return services.AddAttributeConfigurationOptions(configuration, enableGlobalAnnotation, 
            new ConfigurationOptionsBinderImpl(services), assemblies);
    }
}