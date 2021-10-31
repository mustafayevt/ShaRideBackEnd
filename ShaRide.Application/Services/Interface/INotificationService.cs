using ShaRide.Application.DTO.Request.Message;
using System.Threading.Tasks;

namespace ShaRide.Application.Services.Interface
{
    public interface INotificationService
    {
        Task<bool> SendNotification(SendNotificationRequest request);
    }
}