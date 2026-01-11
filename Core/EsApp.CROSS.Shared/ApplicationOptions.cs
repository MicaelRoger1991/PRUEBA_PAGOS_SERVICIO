using System;

namespace EsApp.CROSS.Shared;

public record ApplicationOptions
{
    public required string AllowedHosts { get; set; }
    public required string Resources { get; set; }
    public required string From { get; set; }
}
