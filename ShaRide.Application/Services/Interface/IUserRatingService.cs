using System.Threading.Tasks;
using ShaRide.Application.DTO.Request.UserRating;
using ShaRide.Application.DTO.Response.UserRating;

namespace ShaRide.Application.Services.Interface
{
    public interface IUserRatingService
    {
        Task<UserRatingResponse> InsertRating(InsertUserRatingRequest request);
        short GetUserRating(int userId);
    }
}