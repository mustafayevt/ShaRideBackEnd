using System.Threading.Tasks;
using ShaRide.Application.DTO.Request.Sms;

namespace ShaRide.Application.Services.Interface
{
    public interface ISmsService
    {
        Task<int> SendSms(SendSmsRequest request);
    }
}