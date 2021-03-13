using System.ComponentModel.DataAnnotations;
using ShaRide.Application.Helpers;

namespace ShaRide.Application.DTO.Request.UserRating
{
    public class RatingRequest
    {
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public int DestinationUserId { get; set; }
        [Range(1,5,ErrorMessage = LocalizationKeys.RANGE_VALIDATION)]
        public short Value { get; set; }
    }
}