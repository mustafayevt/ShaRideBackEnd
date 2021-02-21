using System.ComponentModel.DataAnnotations;
using ShaRide.Application.Helpers;

namespace ShaRide.Application.DTO.Request.Sms
{
    public class SendSmsRequest
    {
        [Phone(ErrorMessage = LocalizationKeys.PHONE)]
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public string PhoneNumber { get; set; }
        
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public string MessageBody { get; set; }
    }
}