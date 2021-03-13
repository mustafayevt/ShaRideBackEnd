using System.Collections.Generic;

namespace ShaRide.Application.DTO.Response.UserRating
{
    public class UserRatingResponse
    {
        public ICollection<RatingResponse> Ratings { get; set; }
        public int RideId { get; set; }
    }
}