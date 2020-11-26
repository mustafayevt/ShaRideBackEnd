using ShaRide.Domain.Enums;

namespace ShaRide.Application.DTO.Response.Location
{
    public class LocationPointResponse
    {
        public int Id { get; set; }
        public int LocationId { get; set; }
        public string Name { get; set; }
        public LocationPointType LocationPointType { get; set; }
    }
}