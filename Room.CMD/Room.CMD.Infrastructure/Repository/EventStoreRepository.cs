using CQRS.Core.Domain;
using CQRS.Core.Event;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Room.CMD.Infrastructure.Config;
using Room.Common.Events;

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

    public async Task SaveAsync(EventModel eventModel)
    {
        if(eventModel.EventType == nameof(RoomCreatedEvent)) {
            var @event = (RoomCreatedEvent)eventModel.EventData;
            await VerfyAsync(@event.RoomNumber);
        }
        await _mongoCollection.InsertOneAsync(eventModel).ConfigureAwait(false);
    }

    public async Task VerfyAsync(string RoomNumber)
    {
        var Events = await _mongoCollection.Find(u => u.EventType == nameof(RoomCreatedEvent)).ToListAsync().ConfigureAwait(false);
        if (Events != null && !Events.Any() ) {
            if (Events.Select(u => u.EventData).Select(e => (RoomCreatedEvent)e).Any(e => e.RoomNumber == RoomNumber)) {
                throw new InvalidOperationException($"This Room with the number {RoomNumber} has Alredy exist");   
            }
        }
    }

}