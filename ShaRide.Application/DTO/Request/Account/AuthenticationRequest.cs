using System.ComponentModel.DataAnnotations;
using ShaRide.Application.Helpers;

namespace ShaRide.Application.DTO.Request.Account
{
    /// <summary>
    /// Authentication request model.
    /// </summary>
    public class AuthenticationRequest
    {
        /// <summary>
        /// Phone number.
        /// </summary>
        [Phone(ErrorMessage = LocalizationKeys.PHONE)]
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public string Phone { get; set; }
        
        /// <summary>
        /// Password.
        /// </summary>
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public string Password { get; set; }
    }
}
