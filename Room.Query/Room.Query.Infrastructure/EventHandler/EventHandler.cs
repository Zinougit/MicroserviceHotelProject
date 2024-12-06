using Room.Common.Events;
using Room.Query.Domain.Entities;
using Room.Query.Domain.Repository;

namespace Room.Query.Infrastructure.EventHandler
{
    public class EventHandler : IEventHandler
    {
        private readonly IRoomRepository _roomRepository;

        public EventHandler(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task On(RoomCreatedEvent @event)
        {
            var Room = new RoomEntity
            {
                Id = @event.Id,
                RoomArea = @event.RoomArea,
                RoomNumber = @event.RoomNumber,
                PricePerNight = @event.PricePerNight,
                Amenities = @event.Amenities,
                Floor = @event.Floor,
                IsSmokedAllowed = @event.IsSmokedAllowed,
                LastRenovationDate = @event.LastRenovationDate,
                view = @event.View,
                SizeInSquareMeters = @event.SizeInSquareMeters,
            };
            await _roomRepository.CreatAsync(Room);
        }

        public async Task On(RoomUpdatedEvent @event)
        {
            var Room = await _roomRepository.GetByIdAsync(@event.Id);
            if(Room == null) { return; }
            await _roomRepository.UpdateAsync(Room);
        }

        public async Task On(RoomDeletedEvent @event)
        {
             await _roomRepository.DeleteAsync(@event.Id);
        }
    }
}
