# Binder Options with Microsoft.Extensions.Configuration.Annotations
![workflow ci](https://github.com/huhouhua/Microsoft.Extensions.Configuration.Annotations/actions/workflows/dotnet.yml/badge.svg)
[![NuGet](https://img.shields.io/nuget/v/Ares.Extensions.Configuration.Annotations.svg?style=flat-square)](https://www.nuget.org/Ares.Extensions.Configuration.Annotations)
[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/huhouhua/Microsoft.Extensions.Configuration.Annotations/blob/main/LICENSE)
[![Releases](https://img.shields.io/github/downloads/huhouhua/Microsoft.Extensions.Configuration.Annotations/total.svg)](https://github.com/huhouhua/Microsoft.Extensions.Configuration.Annotations/releases)

> English | [中文](README_zh.md)

> Microsoft.Extensions.Configuration.Annotations is a library that extends the Microsoft.Extensions.Configuration
> system by providing attribute-based support, allowing more flexible and structured control over configuration
> items during the binding process via AOP.

## Features
- Attribute Support: Automatically bind and verify configuration items through attributes
- Automatic registration options
- Compatibility with Existing Configuration System: Fully compatible with Microsoft.Extensions.Configuration 
and can be seamlessly integrated into existing projects

## Requirements

This library requires .NET 8.0+.

## How to Use

Install the [Nuget](https://www.nuget.org/packages/Ares.Extensions.Configuration.Annotations) package:

```sh
Install-Package Ares.Extensions.Configuration.Annotations
```

Or via .NET CLI:

```sh
dotnet add package Ares.Extensions.Configuration.Annotations
```

### Configuration
First, configure it in the Program.cs file as follows:

```c#
var builder = WebApplication.CreateBuilder(args);

// Add Ares.Extensions.Configuration.Annotations
// Add all `OptionsAttribute` defined in the `Program` assembly to the IServiceCollection
builder.Services.AddAttributeConfigurationOptions(builder.Configuration,true,typeof(Program).Assembly);
```

### How to Define?

The [examples](examples/)  directory contains a couple of clear examples

Example: Standard Options Class
```c#
[Options("app")]
public class MyAppOptions
{
    public int Id { get; set; }
    
    public string Name {get; set; }
    ...
}
```

Example: Bind Non-Public Properties
```c#
[Options(SessionKey = "app", BindNonPublicProperties = true)]
public class AppOptions
{
    private int Id { get; set; }

    public string Name { get;set; }
    ...
}
```
Example: Throw Exception if Missing Properties
```c#
// appsettings.json Configuration
//
//  "App":{
//     "Id":1,
//     "Name":"my app",
//     "Version": "1.0.0"
// }
[Options(SessionKey = "app", BindNonPublicProperties = true,ErrorOnUnknownConfiguration = true)]
public class AppOptions
{
    private int Id { get; set; }

    public string Name { get;set; }
    
    ...
}
```
### Validators

Example: Options Class with Validator
```c#
[Validate]
[Options("app")]
public class MyAppOptions
{
    [Range(1,20)]
    public int Id { get; set; }
    
    [Required]
    public string Name {get; set; }
    ...
}
```

Example: Options Class with Custom Validator
```c#
[Validate(typeof(MyAppValidateOptions))]
[Options("app")]
public class MyAppOptions
{
    public int Id { get; set; }
    
    public string Name {get; set; }
    ...
}

public class MyAppValidateOptions : IValidateOptions<MyAppOptions>
{
    public ValidateOptionsResult Validate(string name, MyAppOptions options)
    {
        // To do validate for MyAppOptions
        
        return ValidateOptionsResult.Success;
    }
}
```
### Accessing Options

Example: Accessing in Constructor
```c#
public class MyService
{
    private readonly MyAppOptions _myAppOptions;

    public MyService(IOptions<MyAppOptions> myAppOptions)
    {
        _myAppOptions = myAppOptions.Value;
    }

    public void PrintSettings()
    {
      Console.WriteLine(_myAppOptions.Id);
      Console.WriteLine(_myAppOptions.Name);
    }
}
```

Example: Accessing via IServiceProvider
```c#
IServiceCollection services = new ServiceCollection();

IConfigurationBuilder builder = new ConfigurationBuilder();

// Add option data
builder.AddInMemoryCollection(new Dictionary<string, string?>()
    {
        { "app:id", "1" },
        { "app:name", "test app" },
        { "app:version", "1.0.0" },
        { "app:description", "test options bind" },
    });

IConfigurationRoot configurationRoot = builder.Build();

services.AddAttributeConfigurationOptions(configurationRoot,true,typeof(Program).Assembly);

var provider = services.BuildServiceProvider();

// Gets Options with Options Attribute
var options =  provider.GetService<IOptions<MyAppOptions>>();

Console.WriteLine(options.Value.Id);
Console.WriteLine(options.Value.Name);

```

## Contribute

One of the easiest ways to contribute is to participate in discussions and discuss issues. You can also contribute by submitting pull requests with code changes.

### License

[MIT](https://github.com/huhouhua/Microsoft.Extensions.Configuration.Annotations/blob/main/LICENSE)
