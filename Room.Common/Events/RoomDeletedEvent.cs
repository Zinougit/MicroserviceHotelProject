using CQRS.Core.Event;

namespace Room.Common.Events ;
public class RoomDeletedEvent : BaseEvent{
    public RoomDeletedEvent() : base(nameof(RoomDeletedEvent)){}
    public DateTime Deleted {get;set;}
}