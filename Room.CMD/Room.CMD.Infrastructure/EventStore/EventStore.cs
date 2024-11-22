using CQRS.Core.Domain;
using CQRS.Core.Event;
using CQRS.Core.Infrastructure;
using CQRS.Core.Exceptions;

namespace Room.CMD.Infrastructure.EventStore
{
    public class EventStore : IEventStore
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        public EventStore(IEventStoreRepository eventStoreRepository)
        {
            _eventStoreRepository = eventStoreRepository;
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
            }
            
        }
    }
}
