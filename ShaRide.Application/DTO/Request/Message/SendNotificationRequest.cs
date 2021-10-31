using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ShaRide.Application.Helpers;

namespace ShaRide.Application.DTO.Request.Message
{
    public class SendNotificationRequest
    {
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public string Content { get; set; }

        public ICollection<int> UserIds { get; set; }
    }
}