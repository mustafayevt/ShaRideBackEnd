using System.ComponentModel.DataAnnotations;
using ShaRide.Application.Helpers;

namespace ShaRide.Application.DTO.Request.UserFcmToken
{
    public class UserFcmTokenInsertRequest
    {
        public int UserId { get; set; }
        
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public string Token { get; set; }

        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public string DeviceId { get; set; }
    }
}