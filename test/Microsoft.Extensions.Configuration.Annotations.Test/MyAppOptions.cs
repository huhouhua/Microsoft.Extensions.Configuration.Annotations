using Microsoft.Extensions.Configuration.Annotations.Binders;

namespace Microsoft.Extensions.Configuration.Annotations.Test;

[Options("app")]
public class MyAppOptions
{
    public int Id { get; set; }
    
    public string Name {get; set; }
    
    public string Version { get; set; }
    
    public string Description { get; set; }
}