using System.ComponentModel.DataAnnotations;
using ShaRide.Application.Helpers;

namespace ShaRide.Application.DTO.Request.Account
{
    public class PhoneRequest
    {
        [Phone(ErrorMessage = LocalizationKeys.PHONE)]
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public string Number { get; set; }

        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public bool IsMain { get; set; }
    }
}