using System.Threading.Tasks;
using ShaRide.Application.DTOs.Email;

namespace ShaRide.Application.Services.Interface
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequest request);
    }
}
