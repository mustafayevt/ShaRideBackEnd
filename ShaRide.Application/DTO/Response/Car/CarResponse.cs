using System.Collections.Generic;
using ShaRide.Application.DTO.Request.Car;
using ShaRide.Application.DTO.Response.BanType;
using ShaRide.Application.DTO.Response.CarModel;

namespace ShaRide.Application.DTO.Response.Car
{
    public class CarResponse
    {
        public int Id { get; set; }
        public CarModelResponse Model { get; set; }

        public string RegisterNumber { get; set; }

        public ICollection<int> CarImageIds { get; set; }

        public ICollection<CarSeatCompositionResponse> CarSeats { get; set; }
    }
}