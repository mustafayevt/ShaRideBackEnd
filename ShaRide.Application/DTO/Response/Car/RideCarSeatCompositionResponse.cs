using ShaRide.Application.DTO.Response.Account;
using ShaRide.Domain.Enums;

namespace ShaRide.Application.DTO.Response.Car
{
    public class RideCarSeatCompositionResponse
    {
        public CarResponse Car { get; set; }
        public SeatStatus SeatStatus { get; set; }
        public UserResponse Passenger { get; set; }
    }
}