using MongoDB.Driver;
using MongoDB.Entities;
using SearchServices.Models;
using SearchServices.Services;

namespace SearchServices.Data;

public class DbInitializer
{
    public static async Task InitDb(WebApplication webApplication)
    {
        await DB.InitAsync("SearchDb", MongoClientSettings
            .FromConnectionString(webApplication.Configuration.GetConnectionString("MongoDbConnection")));

        await DB.Index<Item>()
            .Key(x => x.Make, KeyType.Text)
            .Key(x => x.Model, KeyType.Text)
            .Key(x => x.Color, KeyType.Text)
            .CreateAsync();

        using var scope = webApplication.Services.CreateScope();

        var httpClient = scope.ServiceProvider.GetRequiredService<AuctionSvcHttpClient>();

        var items = await httpClient.GetItemsForSearchDb();

        Console.WriteLine(items.Count + "returned from the auction service");
        if (items.Count > 0) await DB.SaveAsync(items);
    }
}
