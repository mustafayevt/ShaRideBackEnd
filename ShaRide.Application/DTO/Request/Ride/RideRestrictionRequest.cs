using System.ComponentModel.DataAnnotations;
using ShaRide.Application.Helpers;

namespace ShaRide.Application.DTO.Request.Ride
{
    public class RideRestrictionRequest
    {
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public int RestrictionId { get; set; }

        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public bool IsPossible { get; set; }
    }
}