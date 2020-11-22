using System.ComponentModel.DataAnnotations;

namespace ShaRide.Application.DTOs.Account
{
    public class AuthenticationRequest
    {
        public string Phone { get; set; }
        public string Password { get; set; }
    }
}
