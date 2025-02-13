using CQRS.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Room.CMD.API.Commands;
using Room.CMD.API.Dtos;
using Room.Common.DTOs;
using Room.Common.Exceptions;

namespace Room.CMD.API.Controllers;
[ApiController]
[Route("api/v1/room")]
public class CreatRoomController : ControllerBase
{
    private readonly ILogger<CreatRoomController> _logger;
    private readonly ICommandDispatcher _commandDispatcher;

    public CreatRoomController(ILogger<CreatRoomController> logger, ICommandDispatcher commandDispatcher)
    {
        _logger = logger;
        _commandDispatcher = commandDispatcher;
    }

    [HttpPost]
    public async Task<IActionResult> CreatRoomAsync(AddRoomCommand command)
    {
        command.Id = Guid.NewGuid();
        try
        {
            await _commandDispatcher.Send(command);
            return StatusCode(StatusCodes.Status201Created, new NewRoomResponse { Id = command.Id, Message = "new Room Request complet successfuly" });
        }
        catch (InvalidDataException ex)
        {
            _logger.Log(LogLevel.Warning, ex, "Client has mad a bad request");
            return StatusCode(StatusCodes.Status400BadRequest, new NewRoomResponse { Id = command.Id, Message = ex.Message });
        }
        catch (RoomInformationMissedException ex)
        {
            _logger.Log(LogLevel.Warning, ex, "Client has made a bad request");
            return StatusCode(StatusCodes.Status400BadRequest, new NewRoomResponse { Id = command.Id, Message = ex.Message });
        }
        catch (Exception ex) {
            const string error = "Error While processing request";
            _logger.Log(LogLevel.Error ,ex, error);
            return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { Message = error });
        }
    }
}
