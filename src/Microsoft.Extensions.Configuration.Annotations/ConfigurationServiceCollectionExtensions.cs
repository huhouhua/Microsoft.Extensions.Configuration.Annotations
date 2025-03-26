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
    /// </summary>
    /// <param name="services">The DI container.</param>
    /// <param name="configuration">The configuration source.</param>
    /// <param name="assemblies">Assemblies to scan for option classes.</param>
    public static IServiceCollection AddAttributeConfigurationOptions(
        this IServiceCollection services,
        IConfiguration configuration,
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

        foreach (var assembly in assemblies)
        {
            // Find all types marked with `OptionsAttribute`
            foreach (var optionsType in assembly.GetTypes().Where(q => q.IsDefined(typeof(OptionsAttribute), true)))
            {
                var optionsAttribute = optionsType.GetCustomAttribute<OptionsAttribute>();
                if (optionsAttribute != null)
                {
                    // Get the section key from the attribute or use the class name as default
                    var key = optionsAttribute.SessionKey ?? optionsType.Name;
                    var section = configuration.GetSection(key);

                    // Create a binder for the options type and bind it
                    ConfigurationOptionsBinder.Create(services, optionsType).Bind(section, binderOptions =>
                    {
                        binderOptions.BindNonPublicProperties = optionsAttribute.BindNonPublicProperties;
                        binderOptions.ErrorOnUnknownConfiguration = optionsAttribute.ErrorOnUnknownConfiguration;
                    });
                }
            }
        }
        return services;
    }
}