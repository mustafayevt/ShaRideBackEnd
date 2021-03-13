using System.ComponentModel.DataAnnotations.Schema;
using ShaRide.Domain.Common;
using ShaRide.Domain.Enums;

namespace ShaRide.Domain.Entities
{
    [Table("RideCarSeatComposition",Schema = "Ride")]
    public class RideCarSeatComposition : BaseEntity
    {
        public CarSeatComposition CarSeatComposition { get; set; }
        public int CarSeatCompositionId { get; set; }

        public SeatStatus SeatStatus { get; set; }

        [ForeignKey(nameof(Passenger))]
        public int? PassengerId { get; set; }
        public User Passenger { get; set; }
    }
}