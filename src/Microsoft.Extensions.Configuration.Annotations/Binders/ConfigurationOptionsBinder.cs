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
    /// Abstract method to bind configuration to the specified `TOptions` type.
    /// </summary>
    /// <param name="context">The binding context that contains configuration and validation information.</param>
    /// <param name="configuration">The configuration source to bind from.</param>
    /// <param name="options">The options object to be populated with the configuration values.</param>
    /// <typeparam name="TOptions">The type of the options class to bind.</typeparam>
    public abstract void Bind<TOptions>(BinderContext context, IConfiguration configuration, TOptions options) 
        where TOptions : class;

}
