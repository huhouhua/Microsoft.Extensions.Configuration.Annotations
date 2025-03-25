using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit.Sdk;

namespace Microsoft.Extensions.Configuration.Annotations.Test;

public class ConfigBinderTest
{
    [Theory]
    [MemberData(nameof(Setup))]
    public void ReadEqualTest(IServiceProvider provider, IConfigurationRoot configurationRoot)
    {
        var options = provider.GetService<IOptions<MyAppOptions>>();
        Assert.NotNull(options);
        Assert.Equal(options.Value.Id, Convert.ToInt32(configurationRoot["app:id"]));
        Assert.Equal(options.Value.Name, configurationRoot["app:name"]);
        Assert.Equal(options.Value.Version, configurationRoot["app:version"]);
        Assert.Equal(options.Value.Description, configurationRoot["app:description"]);
    }
    
    public static IEnumerable<object[]> Setup()
    {
        IServiceCollection services = new ServiceCollection();
        IConfigurationBuilder builder = new ConfigurationBuilder();
        builder.AddInMemoryCollection(InitialData());
        IConfigurationRoot configurationRoot = builder.Build();
        services.AddAttributeConfigurationOptions(configurationRoot, typeof(ConfigBinderTest).Assembly);
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
             
         };
    }

}