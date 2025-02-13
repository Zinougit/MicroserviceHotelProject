using CQRS.Core.Domain;
using Room.CMD.API.Commands;
using Room.Common.Enums;
using Room.Common.Events;
using Room.Common.Exceptions;
using Room.Common.Shared;

namespace Room.CMD.Domain.Aggregate;

public class RoomAggregate : AggregateRoot {
    private string _roomNumber;
    private string _pricePerNight ;
    private string _roomArea;
    private decimal _sizeInSquareMeters;
    private bool _isSmokedAllowed;
    private int _floor;
    private ViewEnums _view;
    private Amenities _amenities;
    private DateTime? _lastRenovationDate;
    private DateTime _created;
    private DateTime? _updated;

    public RoomAggregate(){}

    public RoomAggregate(AddRoomCommand command){
        if(string.IsNullOrWhiteSpace(command.RoomNumber)){
            throw new RoomInformationMissedException(nameof(command.RoomNumber));
        }
        if(string.IsNullOrWhiteSpace(command.RoomArea)){
            throw new RoomInformationMissedException(nameof(command.RoomArea));
        }
        if(string.IsNullOrWhiteSpace(command.PricePerNight)){
            throw new RoomInformationMissedException(nameof(command.PricePerNight));
        }
        if(command.Floor<0){
            throw new InvalidDataException("the Floor cannot be negative");
        }
        if(command.SizeInSquareMeters<0){
            throw new InvalidDataException("The Size of the room cannot be negative");
        }
        if(int.TryParse(command.PricePerNight, out _))
        {
            throw new RoomInformationMissedException(nameof(command.PricePerNight));
        }
        RaiseEvent(new RoomCreatedEvent {
            Id = command.Id,
            RoomNumber = command.RoomNumber,
            RoomArea = command.RoomArea,
            PricePerNight = command.PricePerNight,
            Floor = command.Floor,
            Amenities = command.amenities,
            LastRenovationDate = command.LastRenovationDate,
            View = command.View,
            Posted = DateTime.UtcNow,
            IsSmokedAllowed = command.IsSmokedAllowed,
            SizeInSquareMeters = command.SizeInSquareMeters
        });
    }
    public void Apply(RoomCreatedEvent @event){
         IsActive = true;
        _id = @event.Id;
        _roomNumber = @event.RoomNumber;
        _roomArea = @event.RoomArea;
        _floor = @event.Floor;
        _amenities = @event.Amenities;
        _isSmokedAllowed = @event.IsSmokedAllowed;
        _view = @event.View;
        _lastRenovationDate = @event.LastRenovationDate;
        _sizeInSquareMeters = @event.SizeInSquareMeters;
        _pricePerNight = @event.PricePerNight;
        _created = DateTime.UtcNow;   
        _updated = null;
    }

    public void UpdateRoom(UpdateRoomCommad command){
        if(!IsActive){
            throw new InvalidOperationException("you cannot Update this Room");
        }
        if(command.RoomNumber is not null){
            if(string.IsNullOrWhiteSpace(command.RoomNumber)){
                throw new RoomInformationMissedException(nameof(command.RoomNumber));
            }

        }
        if(command.RoomArea is not null){
            if(string.IsNullOrWhiteSpace(command.RoomArea)){
                throw new RoomInformationMissedException(nameof(command.RoomArea));
            }
        }
        if(command.PricePerNight is not null){
            if(string.IsNullOrWhiteSpace(command.PricePerNight)){
                throw new RoomInformationMissedException(nameof(command.PricePerNight));
            }
        }
        if(command.SizeInSquareMeters is not null){
            if(command.SizeInSquareMeters<0){
                throw new InvalidDataException("The Size of the room cannot be negative");
            }
        }
        if(command.Floor is not null){
            if(command.Floor<0){
                throw new InvalidDataException("the Floor cannot be negative");
            }
        }
        RaiseEvent(new RoomUpdatedEvent {
            Id = _id,
            RoomNumber = command.RoomNumber,
            RoomArea = command.RoomArea,
            PricePerNight = command.PricePerNight,
            Floor = command.Floor,
            Amenities = command.amenities,
            LastRenovationDate = command.LastRenovationDate,
            View = command.view,
            Updated = DateTime.UtcNow,
            IsSmokedAllowed = command.IsSmokedAllowed,
            SizeInSquareMeters = command.SizeInSquareMeters
        });
    }
    public void Apply (RoomUpdatedEvent @event){
        _roomNumber = @event.RoomNumber ?? _roomNumber;
        _roomArea = @event.RoomArea ?? _roomArea;
        _pricePerNight = @event.PricePerNight ?? _pricePerNight;
        _floor = @event.Floor ?? _floor;
        _amenities = @event.Amenities ?? _amenities;
        _isSmokedAllowed = @event.IsSmokedAllowed ?? _isSmokedAllowed;
        _lastRenovationDate = @event.LastRenovationDate ?? @event.LastRenovationDate;
        _sizeInSquareMeters = @event.SizeInSquareMeters ?? _sizeInSquareMeters;
        _view = @event.View ?? _view;
        _updated = DateTime.UtcNow;
    }
    public void DeleteRoom(DeleteRoomCommand command){
        if(!IsActive){
            throw new InvalidOperationException("you cannot Delete this Room");
        }
        RaiseEvent(new RoomDeletedEvent {
            Id = _id,
            Deleted = DateTime.UtcNow,
        });
    }
    public void Apply(RoomDeletedEvent @event){
        IsActive = false;
    }
}
