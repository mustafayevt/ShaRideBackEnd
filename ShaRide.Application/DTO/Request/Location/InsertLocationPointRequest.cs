using ShaRide.Domain.Enums;

namespace ShaRide.Application.DTO.Request.Location
{
    public class InsertLocationPointRequest
    {
        public int LocationId { get; set; }
        public string Name { get; set; }
        public LocationPointType LocationPointType { get; set; }
    }
}