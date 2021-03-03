using ShaRide.Domain.Enums;

namespace ShaRide.Application.DTO.Response.Location
{
    public class RideLocationPointCompositionResponse
    {
        public LocationPointResponse LocationPoint { get; set; }
        public LocationPointType LocationPointType { get; set; }
    }
}