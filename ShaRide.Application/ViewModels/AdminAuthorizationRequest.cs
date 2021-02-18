using System.ComponentModel.DataAnnotations;

namespace ShaRide.Application.ViewModels
{
    public class AdminAuthorizationRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}