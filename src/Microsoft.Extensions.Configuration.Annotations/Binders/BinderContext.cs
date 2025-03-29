// Copyright (c) Kevin Berger Authors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Extensions.Configuration.Annotations.Binders
{
    /// <summary>
    /// Binder context containing information needed for configuration binding.
    /// </summary>
    public class BinderContext
    {
        /// <summary>
        /// Gets the `OptionsAttribute` that marks the configuration class.
        /// </summary>
        public OptionsAttribute OptionsAttribute { get; }
        
        /// <summary>
        /// Gets the optional `ValidateAttribute` that provides a custom validation type.
        /// </summary>
        public ValidateAttribute? ValidateAttribute { get; }
        
        /// <summary>
        /// Gets whether global annotation is enabled.
        /// Global annotation enables automatic validation and binding behavior based on the global configuration.
        /// </summary>
        public bool EnableGlobalAnnotation { get; }
        
        /// <summary>
        /// Gets the optional action to configure the binding behavior.
        /// </summary>
        public Action<BinderOptions>? ConfigureBinder { get; }
        
        /// <summary>
        /// Initializes a new instance of `BinderContext` with the provided options and validation attributes.
        /// </summary>
        /// <param name="optionsAttribute">The `OptionsAttribute` marking the configuration class.</param>
        /// <param name="validateAttribute">An optional `ValidateAttribute` for validation information.</param>
        /// <param name="enableGlobalAnnotation">
        /// A flag indicating whether global annotations should be enabled. When enabled, 
        /// automatic validation and binding behavior is applied globally, making it unnecessary to 
        /// specify these behaviors explicitly in each configuration class.
        /// Default is `true`.
        /// </param>
        public BinderContext(OptionsAttribute optionsAttribute,
            ValidateAttribute? validateAttribute,
            bool enableGlobalAnnotation = true)
        {
            this.OptionsAttribute = optionsAttribute ?? throw new ArgumentNullException(nameof(optionsAttribute));
            this.ValidateAttribute = validateAttribute;
            this.EnableGlobalAnnotation = enableGlobalAnnotation;
            this.ConfigureBinder = binderOptions =>
            {
                // Configure binding options: whether to bind non-public properties and whether to throw errors for unknown configuration properties
                binderOptions.BindNonPublicProperties = OptionsAttribute.BindNonPublicProperties;
                binderOptions.ErrorOnUnknownConfiguration = OptionsAttribute.ErrorOnUnknownConfiguration;
            };
        }
    }
}
