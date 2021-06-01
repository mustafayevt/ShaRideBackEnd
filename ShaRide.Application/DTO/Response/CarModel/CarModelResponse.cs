using ShaRide.Application.DTO.Response.CarBrand;

namespace ShaRide.Application.DTO.Response.CarModel
{
    public class CarModelResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public CarBrandResponse CarBrand { get; set; }
    }
}