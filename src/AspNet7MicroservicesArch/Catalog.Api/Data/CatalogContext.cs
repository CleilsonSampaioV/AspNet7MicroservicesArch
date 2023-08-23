using Catalog.Api.Entities;
using MongoDB.Driver;

namespace Catalog.Api.Data;

public class CatalogContext:ICatalogContext
{
    private readonly MongoClient _mongoClient;

    public CatalogContext(IConfiguration configuration)
    {
        _mongoClient = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        var dataBase = _mongoClient.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));

        Products = dataBase.GetCollection<Product>(configuration.GetValue<string>("DatabaseSettings:CollectionName"));
        CatalogContextSeed.SeedData(Products);
    }

    public IMongoCollection<Product> Products { get; }
}
