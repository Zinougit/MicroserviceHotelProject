using CQRS.Core.Event;
using Room.Common.Enums;
using Room.Common.Shared;
namespace Room.Common.Events;
public class RoomUpdatedEvent : BaseEvent {
    public RoomUpdatedEvent() : base(nameof(RoomUpdatedEvent)){}
    public string? RoomNumber {get;set;}
    public string? PricePerNight {get;set;}
    public string? RoomArea {get;set;}
    public decimal? SizeInSquareMeters {get;set;}
    public bool? IsSmokedAllowed {get;set;}
    public int? Floor {get;set;}
    public Amenities? Amenities {get;set;}
    public ViewEnums? View {get;set;}
    public DateTime? LastRenovationDate{get;set;}
    public DateTime Updated {get;set;}
}