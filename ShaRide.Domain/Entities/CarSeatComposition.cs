using System.ComponentModel.DataAnnotations.Schema;
using ShaRide.Domain.Common;
using ShaRide.Domain.Enums;

namespace ShaRide.Domain.Entities
{
    [Table("CarSeatComposition",Schema = "Ride")]
    public class CarSeatComposition : BaseEntity
    {
        public int CarId { get; set; }
        public Car Car { get; set; }

        public int SeatId { get; set; }
        public Seat Seat { get; set; }

        public SeatRotate SeatRotate { get; set; }

        public SeatType SeatType { get; set; }
    }
}