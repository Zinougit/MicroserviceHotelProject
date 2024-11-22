using Room.CMD.API.Commands;

namespace Room.CMD.Domain.Commands
{
    public interface ICommandHandler
    {
        Task HandleAsync(AddRoomCommand command);
        Task HandleAsync(DeleteRoomCommand command);
        Task HandleAsync(UpdateRoomCommad command);
    }
}
