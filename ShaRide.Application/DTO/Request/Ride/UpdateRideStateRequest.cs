using ShaRide.Domain.Enums;

namespace ShaRide.Application.DTO.Request.Ride
{
    public class UpdateRideStateRequest
    {
        public int RideId { get; set; }
        public RideState RideState { get; set; }
    }
}