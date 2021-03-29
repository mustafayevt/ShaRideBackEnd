using System.ComponentModel.DataAnnotations;
using ShaRide.Application.Helpers;
using ShaRide.Domain.Enums;

namespace ShaRide.Application.DTO.Request.Message
{
    public class InsertMessageRequest
    {
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public string Content { get; set; }
        
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public MessageType MessageType { get; set; }
        
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public MessageSenderType SenderType { get; set; }
        
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public int RideId { get; set; }
    }
}