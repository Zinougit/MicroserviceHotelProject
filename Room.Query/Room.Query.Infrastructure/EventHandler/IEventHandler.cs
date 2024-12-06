using Room.Common.Events;

namespace Room.Query.Infrastructure.EventHandler
{
    public interface IEventHandler
    {
        Task On(RoomCreatedEvent @event);
        Task On(RoomUpdatedEvent @event);
        Task On(RoomDeletedEvent @event);
    }
}
