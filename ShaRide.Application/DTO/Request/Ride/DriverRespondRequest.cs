using ShaRide.Domain.Enums;

namespace ShaRide.Application.DTO.Request.Ride
{
    public class DriverRespondRequest
    {
        public int RequestId { get; set; }
        public PassengerToRideRequestStatus RespondStatus { get; set; }
    }
}