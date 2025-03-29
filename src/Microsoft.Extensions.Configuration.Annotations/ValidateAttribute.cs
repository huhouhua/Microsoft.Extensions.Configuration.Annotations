// Copyright (c) Kevin Berger Authors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Extensions.Configuration.Annotations;

/// <summary>
/// Attribute used to mark a configuration class that requires validation.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class ValidateAttribute : Attribute
{
    /// <summary>
    /// Gets or sets the custom validation type.
    /// </summary>
    public Type? Type { get; }

    /// <summary>
    /// Default constructor, creating a `ValidateAttribute` without a custom validation type.
    /// </summary>
    public ValidateAttribute() { }

    /// <summary>
    /// Initializes a `ValidateAttribute` with a specified validation type.
    /// </summary>
    /// <param name="type">The validation type.</param>
    public ValidateAttribute(Type type)
    {
        this.Type = type;
    }
}