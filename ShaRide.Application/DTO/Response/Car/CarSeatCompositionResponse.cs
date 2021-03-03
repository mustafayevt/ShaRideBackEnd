using ShaRide.Domain.Enums;

namespace ShaRide.Application.DTO.Response.Car
{
    public class CarSeatCompositionResponse
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public short xCordinant { get; set; }
        public short yCordinant { get; set; }
        public SeatRotate SeatRotate { get; set; }
        public SeatType SeatType { get; set; }
    }
}