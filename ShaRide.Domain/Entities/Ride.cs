using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ShaRide.Domain.Common;
using ShaRide.Domain.Enums;

namespace ShaRide.Domain.Entities
{
    [Table("Rides",Schema = "Ride")]
    public class Ride : BaseEntity
    {
        [ForeignKey(nameof(Driver))]
        public int DriverId { get; set; }
        public virtual User Driver { get; set; }

        public DateTime StartDate { get; set; }

        public decimal PricePerSeat { get; set; }

        public string Note { get; set; }

        public RideState RideState { get; set; }

        public DateTime? RideStateChangeDatetime { get; set; }

        public ICollection<RideCarSeatComposition> RideCarSeatComposition { get; set; }

        public ICollection<RideLocationPointComposition> RideLocationPointComposition { get; set; }

        public ICollection<RestrictionRideComposition> RestrictionRideComposition { get; set; }
    }
}