// Copyright (c) Kevin Berger Authors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit.Sdk;

namespace Microsoft.Extensions.Configuration.Annotations.Test;

public class ConfigBinderTest
{
    [Theory]
    [MemberData(nameof(Setup))]
    public void EqualBindTest(IServiceProvider provider, IConfigurationRoot configurationRoot)
    {
        var options = provider.GetService<IOptions<MyAppOptions>>();
        Assert.NotNull(options);
        Assert.Equal(options.Value.Id, Convert.ToInt32(configurationRoot["app:id"]));
        Assert.Equal(options.Value.Name, configurationRoot["app:name"]);
        Assert.Equal(options.Value.Version, configurationRoot["app:version"]);
        Assert.Equal(options.Value.Description, configurationRoot["app:description"]);
    }
    
    [Theory]
    [MemberData(nameof(Setup))]
    public void ValidateTest(IServiceProvider provider, IConfigurationRoot configurationRoot)
    {
        var options = provider.GetService<IOptions<DevOptions>>();
        Assert.NotNull(options);
        Assert.Throws<OptionsValidationException>(() =>
        {
             Assert.Empty(options.Value.Name);
        });
        Assert.Throws<OptionsValidationException>(() =>
        {
            Assert.NotEmpty(options.Value.Environment);
        });
    }

    
    public static IEnumerable<object[]> Setup()
    {
        IServiceCollection services = new ServiceCollection();
        IConfigurationBuilder builder = new ConfigurationBuilder();
        builder.AddInMemoryCollection(InitialData());
        IConfigurationRoot configurationRoot = builder.Build();
        services.AddAttributeConfigurationOptions(configurationRoot,false , typeof(ConfigBinderTest).Assembly);
        yield return new Object[]
        {
            services.BuildServiceProvider(),
            configurationRoot
        };
    }

    private static Dictionary<string, string?> InitialData()
    {
         return new Dictionary<string, string?>()
         {
             { "app:id", "1" },
             { "app:name", "test app" },
             { "app:version", "1.0.0" },
             { "app:description", "test options bind" },
             
             
             {"dev:name",""},
             {"dev:environment","test"},
             {"dev:variableName","port"}
         };
    }

}