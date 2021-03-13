using System.Collections.Generic;

namespace ShaRide.Application.DTO.Request.Ride
{
    public class AddPassengerToRideRequest
    {
        public int DriverId { get; set; }
        public int RideId { get; set; }
        public ICollection<int> RideCarSeatCompositionIds { get; set; }
    }
}