// Copyright (c) Kevin Berger Authors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Extensions.Configuration.Annotations;

/// <summary>
/// Represents an options attribute for marking configuration classes.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class OptionsAttribute : Attribute
{
    /// <summary>
    /// Gets the configuration section name.
    /// </summary>
    public string? SessionKey { get; set; }

    /// <summary>
    /// Determines whether non-public properties should be bound.
    /// Default is false
    /// </summary>
    public bool BindNonPublic { get; set; }
    
    /// <summary>
    /// Specifies whether an error should be thrown for unknown configuration properties.
    /// Default is false
    /// </summary>
    public bool ThrowOnUnknownConfig { get; set; }
    
    
    /// <summary>
    /// Initializes an instance of `OptionsAttribute` with an empty section key.
    /// </summary>
    public OptionsAttribute() : this(string.Empty)
    {
    }
    
    /// <summary>
    /// Initializes an instance of `OptionsAttribute` with a specified section key.
    /// </summary>
    /// <param name="sessionKey">Configuration section name.</param>
    public OptionsAttribute(string sessionKey) : this(sessionKey, false, false)
    {
        
    }
    
    /// <summary>
    /// Initializes an instance of `OptionsAttribute` with additional binding configurations.
    /// </summary>
    /// <param name="sessionKey">Configuration section name.</param>
    /// <param name="bindNonPublic">Specifies whether non-public properties should be bound.</param>
    /// <param name="throwOnUnknownConfig">Specifies whether an error should be thrown for unknown configuration properties.</param>
    public OptionsAttribute(string sessionKey, bool bindNonPublic , bool throwOnUnknownConfig)
    {
        this.SessionKey = sessionKey;
        this.BindNonPublic = bindNonPublic;
        this.ThrowOnUnknownConfig = throwOnUnknownConfig;
    }
}