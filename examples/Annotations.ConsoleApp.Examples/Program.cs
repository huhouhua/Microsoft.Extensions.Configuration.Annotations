// See https://aka.ms/new-console-template for more information

using Annotations.ConsoleApp.Examples;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

IServiceCollection services = new ServiceCollection();
IConfigurationBuilder builder = new ConfigurationBuilder();

// Add option data
builder.AddInMemoryCollection(InitialData());

IConfigurationRoot configurationRoot = builder.Build();

// Injection of Microsoft.Extensions.Configuration.Annotations components
services.AddAttributeConfigurationOptions(configurationRoot,true , typeof(Program).Assembly);


var provider = services.BuildServiceProvider();

// Gets Options with Options Attribute
var options =  provider.GetService<IOptions<AppOptions>>();

Console.WriteLine(options.Value.Id);
Console.WriteLine(options.Value.Name);
Console.WriteLine(options.Value.Version);
Console.WriteLine(options.Value.Description);

static Dictionary<string, string?> InitialData()
{
    return new Dictionary<string, string?>()
    {
        { "app:id", "1" },
        { "app:name", "test app" },
        { "app:version", "1.0.0" },
        { "app:description", "test options bind" },
    };
}