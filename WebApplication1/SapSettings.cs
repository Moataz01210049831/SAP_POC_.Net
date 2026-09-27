namespace WebApplication1;

public sealed class SapSettings
{
    public const string SectionName = "Sap";

    public string BaseUrl { get; set; } = string.Empty;
    public string ServicePath { get; set; } = string.Empty;
    public string? ApiKey { get; set; }
}