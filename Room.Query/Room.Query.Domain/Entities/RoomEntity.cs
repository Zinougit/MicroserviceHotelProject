using Room.Common.Enums;
using Room.Common.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Room.Query.Domain.Entities
{
    [Table("Room")]
    public class RoomEntity
    {
        [Key]
        public Guid Id { get; set; }
        public required string RoomNumber { get; set; }
        public required string PricePerNight { get; set; }
        public required string RoomArea { get; set; }
        public decimal SizeInSquareMeters { get; set; }
        public bool IsSmokedAllowed { get; set; }
        public int Floor { get; set; }
        public ViewEnums view { get; set; }
        public required Amenities Amenities { get; set; }
        public DateTime? LastRenovationDate { get; set; }

    }
}
