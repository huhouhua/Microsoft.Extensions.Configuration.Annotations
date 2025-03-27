// Copyright (c) Kevin Berger Authors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Configuration.Annotations.Binders;

/// <summary>
/// A concrete implementation of `ConfigurationOptionsBinder` for binding `IConfiguration`
/// </summary>
internal class ConfigurationOptionsBinderImpl : ConfigurationOptionsBinder
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
    /// <param name="options">The options object that will be populated with the configuration values. 
    /// This object must be of the specified <typeparamref name="TOptions"/> type.</param>
    /// <typeparam name="TOptions">The options class type to be bound.</typeparam>
    public override void Bind<TOptions>(IConfiguration configuration,TOptions options, Action<BinderOptions>? configureBinder)
    {
        // Registers TOptions in the DI container and binds it to the provided configuration section.
        this.services.AddOptions<TOptions>().Bind(configuration, configureBinder);
    }
}