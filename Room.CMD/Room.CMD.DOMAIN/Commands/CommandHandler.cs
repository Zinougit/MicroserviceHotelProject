using CQRS.Core.Handlers;
using Room.CMD.API.Commands;
using Room.CMD.Domain.Aggregate;

namespace Room.CMD.Domain.Commands
{
    public class CommandHandler : ICommandHandler
    {
        private readonly IEventSourcingHandler<RoomAggregate> _eventSourcingHandler;
        public CommandHandler(IEventSourcingHandler<RoomAggregate> eventSourcingHandler)
        {
            _eventSourcingHandler = eventSourcingHandler;
        }
    
        public async Task HandleAsync(AddRoomCommand command)
        {
            var aggregate = new RoomAggregate(command);
            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(DeleteRoomCommand command)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            aggregate.DeleteRoom(command);
            await _eventSourcingHandler.SaveAsync(aggregate);
        }

        public async Task HandleAsync(UpdateRoomCommad command)
        {
            var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
            aggregate.UpdateRoom(command);
            await _eventSourcingHandler.SaveAsync(aggregate);
        }
    }
}
