using CQRS.Core.Domain;
using CQRS.Core.Handlers;
using CQRS.Core.Infrastructure;
using Room.CMD.Domain.Aggregate;

namespace Room.CMD.Infrastructure.Handlers
{
    public class EventSourcingHandler : IEventSourcingHandler<RoomAggregate>
    {
        private readonly IEventStore _eventStore;
        public EventSourcingHandler(IEventStore eventStore) { 
            _eventStore = eventStore;
        }
        public async Task<RoomAggregate> GetByIdAsync(Guid aggregateId)
        {
            var aggregate = new RoomAggregate();
            var events = await _eventStore.GetEventsAsynsc(aggregateId);
            if (events == null || !events.Any()) { 
                return aggregate;
            }
            aggregate.ReplayEvent(events);
            var Version = events.Select(a=>a.Version).Max();
            aggregate.Version = Version;    
            return aggregate;
        }

        public async Task SaveAsync(AggregateRoot aggregate)
        {
            await _eventStore.SaveEventAsync(aggregate.GetUncommitedChages(), aggregate.Version,aggregate.Id);
        }
    }
}
