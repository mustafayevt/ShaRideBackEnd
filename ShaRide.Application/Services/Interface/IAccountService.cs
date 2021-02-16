using System.Threading.Tasks;
using ShaRide.Application.DTO.Request;
using ShaRide.Application.DTO.Request.Account;
using ShaRide.Application.DTO.Response;
using ShaRide.Application.DTO.Response.Account;
using ShaRide.Domain.Entities;

namespace ShaRide.Application.Services.Interface
{
    public interface IAccountService
    {
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request, string ipAddress);
        Task<AuthenticationResponse> RegisterAsync(RegisterRequest request, string origin);
        Task<string> GetVerificationCode(string phoneNumber);
        Task<UserImage> GetUserThumbnailPhoto(int userId);

        // Task<string> ConfirmPhoneAsync(int userId, string code);
        // Task ForgotPassword(ForgotPasswordRequest model, string origin);
        // Task<string> ResetPassword(ResetPasswordRequest model);
    }
}
