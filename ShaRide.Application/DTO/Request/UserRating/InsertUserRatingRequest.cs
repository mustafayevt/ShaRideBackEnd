using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ShaRide.Application.Helpers;

namespace ShaRide.Application.DTO.Request.UserRating
{
    public class InsertUserRatingRequest
    {
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public List<RatingRequest> Ratings{ get; set; }
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public int RideId { get; set; }
    }
}