using System.Net;
using Polly;
using Polly.Extensions.Http;
using SearchServices.Data;
using SearchServices.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpClient<AuctionSvcHttpClient>().AddPolicyHandler(GetPolicy());

var app = builder.Build();

app.UseAuthorization();
app.MapControllers();

app.Lifetime.ApplicationStarted.Register(async () =>
{
    // Seeding Db
    try
    {
        await DbInitializer.InitDb(app);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        throw;
    }
});

app.Run();

static IAsyncPolicy<HttpResponseMessage> GetPolicy()
    => HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(m => m.StatusCode == HttpStatusCode.NotFound)
        .WaitAndRetryForeverAsync(_ => TimeSpan.FromSeconds(3));