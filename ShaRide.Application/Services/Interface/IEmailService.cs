using System.Threading.Tasks;
using ShaRide.Application.DTO.Request;

namespace ShaRide.Application.Services.Interface
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequest request);
    }
}
