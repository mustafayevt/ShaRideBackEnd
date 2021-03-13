using System;
using ShaRide.Domain.Common;
using ShaRide.Domain.Enums;

namespace ShaRide.Domain.Entities
{
    public class PassengerToRideRequest : BaseEntity
    {
        public RideCarSeatComposition RideCarSeatComposition { get; set; }
        public int RideCarSeatCompositionId { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;

        public PassengerToRideRequestStatus RequestStatus { get; set; } =
            PassengerToRideRequestStatus.WaitingForResponse;
    }
}