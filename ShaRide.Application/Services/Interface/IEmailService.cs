using System.Threading.Tasks;
using ShaRide.Application.DTO.Request;
using ShaRide.Application.DTO.Request.Account;

namespace ShaRide.Application.Services.Interface
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequest request);
    }
}
