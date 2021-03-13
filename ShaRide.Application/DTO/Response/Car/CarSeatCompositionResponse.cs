using ShaRide.Application.DTO.Response.Account;
using ShaRide.Domain.Enums;

namespace ShaRide.Application.DTO.Response.Car
{
    public class CarSeatCompositionResponse
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public int SeatId { get; set; }
        public short xCordinant { get; set; }
        public short yCordinant { get; set; }
        public SeatRotate SeatRotate { get; set; }
        public SeatStatus SeatType { get; set; }
        public UserResponse Passenger { get; set; }
    }
}