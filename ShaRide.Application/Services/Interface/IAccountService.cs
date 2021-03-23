using System.Collections.Generic;
using System.Threading.Tasks;
using ShaRide.Application.DTO.Request;
using ShaRide.Application.DTO.Request.Account;
using ShaRide.Application.DTO.Request.Feedback;
using ShaRide.Application.DTO.Response;
using ShaRide.Application.DTO.Response.Account;
using ShaRide.Application.DTO.Response.Feedback;
using ShaRide.Domain.Entities;

namespace ShaRide.Application.Services.Interface
{
    public interface IAccountService
    {
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request, string ipAddress);
        Task<AuthenticationResponse> RegisterAsync(RegisterRequest request, string origin);
        Task<string> SendPhoneVerificationSms(string phoneNumber);
        Task<UserImage> GetUserThumbnailPhoto(int userId);
        Task<string> SendPasswordResetSms(string phoneNumber);

        Task<AuthenticationResponse> ResetUserPassword(string phone, string newPassword);
        Task<FeedbackResponse> Feedback(InsertFeedbackRequest request);
        Task<ICollection<FeedbackResponse>> GetAllFeedbacks();
        Task<decimal> GetCurrentUserBalance();
        Task<decimal> GetUserBalance(int userId);
        Task<int> InsertPotentialClientPhone(InsertPotentialClientPhoneRequest request);
    }
}
