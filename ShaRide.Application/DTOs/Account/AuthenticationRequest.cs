using System.ComponentModel.DataAnnotations;

namespace ShaRide.Application.DTOs.Account
{
    /// <summary>
    /// Authentication request model.
    /// </summary>
    public class AuthenticationRequest
    {
        /// <summary>
        /// Phone number.
        /// </summary>
        public string Phone { get; set; }
        
        /// <summary>
        /// Password.
        /// </summary>
        public string Password { get; set; }
    }
}
