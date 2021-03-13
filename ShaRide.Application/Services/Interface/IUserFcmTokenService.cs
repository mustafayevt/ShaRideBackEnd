using System.Threading.Tasks;
using ShaRide.Application.DTO.Request.UserFcmToken;
using ShaRide.Application.DTO.Response.UserFcmToken;

namespace ShaRide.Application.Services.Interface
{
    public interface IUserFcmTokenService
    {
        Task<UserFcmTokenResponse> InsertToken(UserFcmTokenInsertRequest request);
        Task<UserFcmTokenResponse> UpdateToken(UserFcmTokenUpdateRequest request);
        Task<int> SendNotificationToUser(FcmNotificationContract contract);
    }
}