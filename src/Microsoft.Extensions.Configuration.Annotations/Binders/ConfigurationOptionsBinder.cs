// Copyright (c) Kevin Berger Authors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Configuration.Annotations.Binders;

/// <summary>
/// Abstract base class for a configuration options binder, providing a `Bind` method to load settings from `IConfiguration`.
/// </summary>
public abstract class ConfigurationOptionsBinder
{
    /// <summary>
    /// Binds the specified configuration section to the given options type.
    /// This method is responsible for mapping the configuration values from the provided 
    /// <paramref name="configuration"/> to an instance of <paramref name="options"/> of type <typeparamref name="TOptions"/>.
    /// It can optionally configure the binding behavior using the <paramref name="configureBinder"/> action.
    /// </summary>
    /// <param name="configuration">The configuration source to bind from. This is typically a configuration section, 
    /// such as from an `IConfiguration` instance (e.g., from appsettings.json or environment variables).</param>
    /// <param name="options">The options object that will be populated with the configuration values. 
    /// This object must be of the specified <typeparamref name="TOptions"/> type.</param>
    /// <param name="configureBinder">An optional action to configure the binding behavior, such as setting whether non-public 
    /// properties should be bound or whether unknown configuration properties should result in an error. 
    /// If not provided, the default behavior will be used.</param>
    /// <typeparam name="TOptions">The type of the options class to bind the configuration to. 
    /// This class must be a reference type (e.g., a class) and should have properties that match the configuration keys.</typeparam>
    /// <remarks>
    /// This method is designed to bind configuration data to a strongly-typed options object. 
    /// The <paramref name="configureBinder"/> allows for fine-tuning how the binding should behave, such as:
    /// - Binding non-public properties
    /// - Handling unknown configuration keys (e.g., whether to throw an error or silently ignore them)
    /// 
    /// Typically, this method is used in scenarios where you have a set of strongly-typed options classes and want to bind 
    /// configuration sections to those classes, enabling easy and type-safe access to configuration values.
    /// </remarks>
    public abstract void Bind<TOptions>(IConfiguration configuration, TOptions options,
        Action<BinderOptions>? configureBinder) where TOptions : class;

}
