using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using ShaRide.Application.DTO.Request.Location;
using ShaRide.Application.Helpers;
using ShaRide.Domain.Enums;

namespace ShaRide.Application.DTO.Request.Ride
{
    public class RideLocationPointRequest
    {
        public int? LocationPointId { get; set; }

        public InsertLocationPointRequest LocationPoint { get; set; }

        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public LocationPointType LocationPointType { get; set; }
    }
}