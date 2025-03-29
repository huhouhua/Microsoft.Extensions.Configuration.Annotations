// Copyright (c) Kevin Berger Authors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.Configuration.Annotations.Test;

[Validate(typeof(DevValidateOptions))]
[Options("dev")]
public class DevOptions
{
    public string Name { get; set; }

    public string Environment { get; set; }

    public string VariableName {get; set; }
}

public class DevValidateOptions : IValidateOptions<DevOptions>
{
    public ValidateOptionsResult Validate(string name, DevOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.Name))
        {
            return ValidateOptionsResult.Fail("name is null or empty");     
        }
        if (options.Name.StartsWith("dev"))
        {
            return ValidateOptionsResult.Fail("The name is incorrect. The beginning character of the name is missing dev");     
        }
        if (options.VariableName.Equals("port"))
        {
            return ValidateOptionsResult.Success;
        }

        return ValidateOptionsResult.Success;
    }
}