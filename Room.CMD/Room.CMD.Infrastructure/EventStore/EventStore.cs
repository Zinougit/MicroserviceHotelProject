using CQRS.Core.Domain;
using CQRS.Core.Event;
using CQRS.Core.Infrastructure;
using CQRS.Core.Exceptions;
using Microsoft.Extensions.Options;
using CQRS.Core.Topics;
using CQRS.Core.Producer;

namespace Room.CMD.Infrastructure.EventStore
{
    public class EventStore : IEventStore
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly string _topic;
        private readonly IEventProducer _producer;
        public EventStore(IEventStoreRepository eventStoreRepository, IOptions<KafkaTopic> options, IEventProducer producer)
        {
            _eventStoreRepository = eventStoreRepository;
            _topic = options.Value.Topic;
            _producer = producer;
        }

        public async Task<List<BaseEvent>> GetEventsAsynsc(Guid AggregateId)
        {
            List<EventModel> eventStream = await _eventStoreRepository.GetByAggregatId(AggregateId);
            if (eventStream == null || eventStream.Any()) {
                throw new AggregateNotFoundException("This Aggregate Is Not Found");
            }
            var events = eventStream.OrderBy(u=>u.Version).Select(e=>e.EventData).ToList();
            return events;
        }

        public async Task SaveEventAsync(IEnumerable<BaseEvent> events, int version, Guid id)
        {
            List<EventModel> eventStream = await _eventStoreRepository.GetByAggregatId(id);

            if ((version != -1) && (eventStream[-1].Version != version))
            {
                throw new CocurencyException();
            }
            var Version = version;
            
            foreach(var @event in events)
            {
                var eventModel = new EventModel
                {
                    AggregateId = id,
                    Version = Version++,
                    AggregateType = "RoomAggregate",
                    EventType = @event.Type,
                    TimeStamp = DateTime.UtcNow,
                    EventData = @event
                };
                await _eventStoreRepository.SaveAsync(eventModel);
                await _producer.ProduceAsync(@event, _topic);                
            }

        }
    }
}
