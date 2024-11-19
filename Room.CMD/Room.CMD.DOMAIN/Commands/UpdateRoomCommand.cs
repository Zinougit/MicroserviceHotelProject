using CQRS.Core.Command;
using Room.Common.Enums;
using Room.Common.Shared;

namespace Room.CMD.API.Commands;
public class UpdateRoomCommad : BaseCommand {
    public string? RoomNumber {get;set;}
    public string? PricePerNight {get;set;}
    public string? RoomArea {get;set;}
    public decimal? SizeInSquareMeters {get;set;}
    public bool? IsSmokedAllowed {get;set;}
    public int? Floor {get;set;}
    public Amenities? amenities {get;set;}
    public ViewEnums? view {get;set;}
    public DateTime? LastRenovationDate{get;set;}
}