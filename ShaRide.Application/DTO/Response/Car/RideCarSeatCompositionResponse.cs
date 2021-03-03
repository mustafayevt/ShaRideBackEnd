using ShaRide.Application.DTO.Response.Account;
using ShaRide.Domain.Enums;

namespace ShaRide.Application.DTO.Response.Car
{
    public class RideCarSeatCompositionResponse
    {
        public CarSeatCompositionResponse CarSeatComposition { get; set; }
        public SeatStatus SeatStatus { get; set; }
        public UserResponse Passenger { get; set; }
    }
}