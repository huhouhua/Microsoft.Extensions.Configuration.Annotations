# 使用 `Microsoft.Extensions.Configuration.Annotations` 注册Options
![workflow ci](https://github.com/huhouhua/Microsoft.Extensions.Configuration.Annotations/actions/workflows/dotnet.yml/badge.svg)
[![NuGet](https://img.shields.io/nuget/v/Microsoft.Extensions.Configuration.Annotation.svg)](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Annotation/)
[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/huhouhua/Microsoft.Extensions.Configuration.Annotations/blob/main/LICENSE)
[![Releases](https://img.shields.io/github/downloads/huhouhua/Microsoft.Extensions.Configuration.Annotations/total.svg)](https://github.com/huhouhua/Microsoft.Extensions.Configuration.Annotations/releases)

> [English](README.md) | 中文

> Microsoft.Extensions.Configuration.Annotations 是一个用于扩展 `Microsoft.Extensions.Configuration` 配置功能库，提供注解支持，
> 使得你可以在配置绑定过程中通过 `AOP`对配置项进行更灵活和结构化的控制。

## 特性
- 注解支持: 通过注解的方式绑定与验证配置项

- 与现有配置体系兼容: 完全兼容 Microsoft.Extensions.Configuration，可以在现有的项目中无缝集成。


## 如何使用

安装 [Nuget](https://www.nuget.org/packages/Microsoft.Extensions.Configuration.Annotations) 包.

```sh
Install-Package Microsoft.Extensions.Configuration.Annotations
```

或通过 .NET CLI：

```sh
dotnet add package Microsoft.Extensions.Configuration.Annotations
```

### Configuration
首先在 Program.cs 文件中，配置如下：

```c#
var builder = WebApplication.CreateBuilder(args);

// 添加 Microsoft.Extensions.Configuration.Annotations
// 并且把`Program`程序集下的所有定义了`OptionsAttribute`的`Attribute` 添加到IServiceCollection
builder.Services.AddAttributeConfigurationOptions(builder.Configuration,true,typeof(Program).Assembly);
```

### 如何定义?

示例 标准的Options类
```c#
[Options("app")]
public class MyAppOptions
{
    public int Id { get; set; }
    
    public string Name {get; set; }
    ...
}
```

示例 绑定非public的属性
```c#
[Options(SessionKey = "app", BindNonPublicProperties = true)]
public class AppOptions
{
    private int Id { get; set; }

    public string Name { get;set; }
    ...
}
```
示例 缺少没有定义的属性，则抛出异常
```c#
// appsettings.json配置
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
### 验证器

示例 带验证器的Options类
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

示例 自定义验证器的Options类
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
### 获取Options

示例 在构造函数中获取
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
      Console.WriteLine(options.Value.Id);
      Console.WriteLine(options.Value.Name);
    }
}
```

示例 通过IServiceProvider获取
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

builder.Services.AddAttributeConfigurationOptions(builder.Configuration,true,typeof(Program).Assembly);

var provider = services.BuildServiceProvider();

// Gets Options with Options Attribute
var options =  provider.GetService<IOptions<MyAppOptions>>();

Console.WriteLine(options.Value.Id);
Console.WriteLine(options.Value.Name);

```

## 贡献

贡献的最简单的方法之一就是是参与讨论和讨论问题（issue）。你也可以通过提交的 Pull Request 代码变更作出贡献。

### License

[MIT](https://github.com/huhouhua/Microsoft.Extensions.Configuration.Annotations/blob/main/LICENSE)