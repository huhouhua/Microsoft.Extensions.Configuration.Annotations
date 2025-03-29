using Microsoft.Extensions.Configuration.Annotations;
using Microsoft.Extensions.Options;

namespace Annotations.ConsoleApp.Examples;

[Validate]
[Options("app")]
public class AppOptions
{
    public int Id { get; set; }
    
    public string Name {get; set; }
    
    public string Version { get; set; }
    
    public string Description { get; set; }
}


public class AppValidateOptions : IValidateOptions<AppOptions>
{
    public ValidateOptionsResult Validate(string name, AppOptions options)
    {
        // To do validate for AppOptions
        
        return ValidateOptionsResult.Success;
    }
}