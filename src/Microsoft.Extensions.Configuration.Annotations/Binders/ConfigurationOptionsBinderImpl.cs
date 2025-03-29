// Copyright (c) Kevin Berger Authors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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
    /// Binds the configuration to the specified options and registers services.
    /// </summary>
    /// <param name="context">The binder context containing configuration and validation information.</param>
    /// <param name="configuration">The configuration source.</param>
    /// <param name="options">The options object to bind.</param>
    /// <typeparam name="TOptions">The type of the options class to bind.</typeparam>
    public override void Bind<TOptions>(BinderContext context, IConfiguration configuration,TOptions options)
    {
        // Register TOptions and bind it to the provided configuration section.
        var builder = this.services.AddOptions<TOptions>().Bind(configuration, context.ConfigureBinder);
        
        // If a custom validation type is provided, add it to the DI container
        if (context.ValidateAttribute?.Type is not null)
        {
            this.services.AddSingleton(typeof(IValidateOptions<TOptions>), context.ValidateAttribute.Type);
            return;
        }
        // Validate based on enable global configuration
        if (context.EnableGlobalAnnotation || context.ValidateAttribute is not null)
        {
            builder.ValidateDataAnnotations();
        }
    }
}

