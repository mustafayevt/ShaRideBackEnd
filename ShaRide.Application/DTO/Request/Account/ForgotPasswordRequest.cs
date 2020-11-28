using System.ComponentModel.DataAnnotations;
using ShaRide.Application.Helpers;

namespace ShaRide.Application.DTO.Request.Account
{
    public class ForgotPasswordRequest
    {
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        [EmailAddress(ErrorMessage = LocalizationKeys.EMAIL)]
        public string Email { get; set; }
    }
}
