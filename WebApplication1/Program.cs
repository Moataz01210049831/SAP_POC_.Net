using Microsoft.Extensions.Options;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using System.Net;
using WebApplication1;
using WebApplication1.SapGenerated;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.Configure<SapSettings>(builder.Configuration.GetSection(SapSettings.SectionName));

builder.Services.AddHttpClient("SapKiotaClient")
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
    {
        AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.Brotli
    });

builder.Services.AddSingleton(sp =>
{
    var settings = sp.GetRequiredService<IOptions<SapSettings>>().Value;
    var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient("SapKiotaClient");

    if (!string.IsNullOrWhiteSpace(settings.ApiKey))
    {
        httpClient.DefaultRequestHeaders.Add("APIKey", settings.ApiKey);
    }

    var adapter = new HttpClientRequestAdapter(new AnonymousAuthenticationProvider(), httpClient: httpClient)
    {
        BaseUrl = settings.BaseUrl.TrimEnd('/') + settings.ServicePath.TrimEnd('/')
    };

    return new SapBusinessPartnerClient(adapter);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();