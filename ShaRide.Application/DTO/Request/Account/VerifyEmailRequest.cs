using System.ComponentModel.DataAnnotations;
using ShaRide.Application.Helpers;

namespace ShaRide.Application.DTO.Request.Account
{
    public class ResetPasswordRequest
    {
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        [EmailAddress(ErrorMessage = LocalizationKeys.EMAIL)]
        public string Email { get; set; }
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public string Token { get; set; }
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        [MinLength(6,ErrorMessage = LocalizationKeys.INVALID_CREDENTIALS)]
        public string Password { get; set; }
    }
}
