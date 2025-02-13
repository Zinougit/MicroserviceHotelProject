using CQRS.Core.Command;
using Room.Common.Enums;
using Room.Common.Shared;

namespace Room.CMD.API.Commands;

public class AddRoomCommand : BaseCommand {
    public required string RoomNumber {get;set;}
    public required string PricePerNight {get;set;}
    public required string RoomArea {get;set;}
    public decimal SizeInSquareMeters {get;set;}
    public bool IsSmokedAllowed {get;set;}
    public int Floor {get;set;}
    public ViewEnums View {get;set;}
    public required Amenities amenities {get;set;}
    public DateTime? LastRenovationDate{get;set;}
}
