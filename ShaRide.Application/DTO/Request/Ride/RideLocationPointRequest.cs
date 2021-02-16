using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using ShaRide.Application.Helpers;
using ShaRide.Domain.Enums;

namespace ShaRide.Application.DTO.Request.Ride
{
    public class RideLocationPointRequest
    {
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        [Range(1,int.MaxValue,ErrorMessage = LocalizationKeys.RANGE_VALIDATION)]
        public int LocationPointId { get; set; }

        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public LocationPointType LocationPointType { get; set; }
    }
}