using ShaRide.Application.Helpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShaRide.Application.DTO.Request.Ride
{
    public class UpdateSeatsStatusRequest
    {
        public int RideId { get; set; }
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public ICollection<RideSeatRequest> RideSeatRequests { get; set; }
    }
}