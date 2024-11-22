using CQRS.Core.Domain;
using CQRS.Core.Event;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Room.CMD.Infrastructure.Config;

namespace Room.CMD.Infrastracture.Repository;

public class EventStoreRepository : IEventStoreRepository
{
    private readonly IMongoCollection<EventModel> _mongoCollection;

    public EventStoreRepository(IOptions<MongoConfig> options) { 
        var mongoClient = new MongoClient(options.Value.ConnectionString);
        var DataBase = mongoClient.GetDatabase(options.Value.DatabaseName);
        _mongoCollection = DataBase.GetCollection<EventModel>(options.Value.Collection);
        
    }
    public async Task<List<EventModel>> GetByAggregatId(Guid id)
    {
        var Events =  await _mongoCollection.Find(u=>u.AggregateId == id).ToListAsync().ConfigureAwait(false);
        return Events;
    }

    public async Task SaveAsync(EventModel @event)
    {
        await _mongoCollection.InsertOneAsync(@event).ConfigureAwait(false);
    }
}