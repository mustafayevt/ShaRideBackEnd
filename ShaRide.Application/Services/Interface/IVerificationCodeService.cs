using System.Threading.Tasks;

namespace ShaRide.Application.Services.Interface
{
    public interface IVerificationCodeService
    {
        Task<int> SendVerificationCode(string to, string content, string senderId = "ShaRide");
    }
}