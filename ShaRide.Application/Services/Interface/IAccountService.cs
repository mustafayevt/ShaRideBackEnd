using System.Threading.Tasks;
using ShaRide.Application.DTOs.Account;
using ShaRide.Application.Wrappers;

namespace ShaRide.Application.Services.Interface
{
    public interface IAccountService
    {
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request, string ipAddress);
        Task<AuthenticationResponse> RegisterAsync(RegisterRequest request, string origin);
        Task<string> GetVerificationCode(string phoneNumber);
        // Task<string> ConfirmPhoneAsync(int userId, string code);
        // Task ForgotPassword(ForgotPasswordRequest model, string origin);
        // Task<string> ResetPassword(ResetPasswordRequest model);
    }
}
