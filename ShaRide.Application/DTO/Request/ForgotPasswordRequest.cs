using System.ComponentModel.DataAnnotations;

namespace ShaRide.Application.DTO.Request
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
