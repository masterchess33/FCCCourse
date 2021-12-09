using Catalogo.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalogo.Repositories;

public class MongoDbItemsRepository: IItemsRepository
{
    private const string DatabaseName = "Catalog";
    private const string CollectionName = "items";
    private readonly IMongoCollection<Item> _itemsCollection;
    private readonly FilterDefinitionBuilder<Item> _filterbuilder = Builders<Item>.Filter;

    public MongoDbItemsRepository(IMongoClient mongoClient)
    {
        // Comando para crear contenedor con base de datos
        // docker run -d --name mongo -p 27017:27017 -v mongodbdata:/data/db -e MONGO_INITDB_ROOT_USERNAME=mongoadmin -e MONGO_INITDB_ROOT_PASSWORD=Pass#word1 mongo
        IMongoDatabase database = mongoClient.GetDatabase(DatabaseName);
        _itemsCollection = database.GetCollection<Item>(CollectionName);
    }

    public async Task<Item> GetItemAsync(Guid id)
    {
        var filter = _filterbuilder.Eq(item => item.Id, id);
        return await _itemsCollection.Find(filter).SingleOrDefaultAsync()!;
    }

    public async Task<IEnumerable<Item>> GetItemsAsync()
    {
        return await _itemsCollection.Find(new BsonDocument()).ToListAsync();
    }

    public async Task CreateItemAsync(Item item)
    {
        await _itemsCollection.InsertOneAsync(item);
    }

    public async Task UpdateItemAsync(Item item)
    {
        var filter = _filterbuilder.Eq(existingItem => existingItem.Id, item.Id);
        await _itemsCollection.ReplaceOneAsync(filter, item);
    }

    public async Task DeleteItemAsync(Guid id)
    {
        var filter = _filterbuilder.Eq(item => item.Id, id);
        await _itemsCollection.DeleteOneAsync(filter);
    }
}