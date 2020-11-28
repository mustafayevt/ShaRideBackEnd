using System.ComponentModel.DataAnnotations;
using ShaRide.Application.Helpers;

namespace ShaRide.Application.DTO.Request.Account
{
    public class EmailRequest
    {
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public string To { get; set; }

        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public string Subject { get; set; }

        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public string Body { get; set; }
        
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public string From { get; set; }
    }
}