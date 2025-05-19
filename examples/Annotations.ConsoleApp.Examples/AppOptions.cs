using Microsoft.Extensions.Configuration.Annotations;
using Microsoft.Extensions.Options;

namespace Annotations.ConsoleApp.Examples;

[Validate(typeof(AppValidateOptions))]
[Options(SessionKey = "app",BindNonPublic = true,ThrowOnUnknownConfig = true)]
public class AppOptions
{
    private int Id { get; set; }
    
    public string Name {get; set; }
    
    public string Version { get; set; }
    
    internal string Description { get; set; }
}


public class AppValidateOptions : IValidateOptions<AppOptions>
{
    public ValidateOptionsResult Validate(string name, AppOptions options)
    {
        // To do validate for AppOptions
        
        return ValidateOptionsResult.Success;
    }
}