using CQRS.Core.Event;
using Room.Common.Enums;
using Room.Common.Shared;
namespace Room.Common.Events;
public class RoomCreatedEvent : BaseEvent{
    public RoomCreatedEvent() : base(nameof(RoomCreatedEvent)){}
     public required string RoomNumber {get;set;}
    public required string PricePerNight {get;set;}
    public required string RoomArea {get;set;}
    public decimal SizeInSquareMeters {get;set;}
    public bool IsSmokedAllowed {get;set;}
    public int Floor {get;set;}
    public ViewEnums View {get;set;}
    public required Amenities Amenities {get;set;}
    public DateTime? LastRenovationDate{get;set;}
    public DateTime Posted {get;set;}
}