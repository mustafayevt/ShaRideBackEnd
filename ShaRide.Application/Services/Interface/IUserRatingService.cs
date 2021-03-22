using System.Threading.Tasks;
using ShaRide.Application.DTO.Request.UserRating;
using ShaRide.Application.DTO.Response.UserRating;

namespace ShaRide.Application.Services.Interface
{
    public interface IUserRatingService
    {
        Task<UserRatingResponse> InsertRating(InsertUserRatingRequest request);
        Task<short> GetUserRating(int userId);
        Task<short> GetCurrentUserRating();
    }
}