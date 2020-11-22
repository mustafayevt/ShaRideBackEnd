using System.ComponentModel.DataAnnotations;

namespace ShaRide.Application.DTOs.Account
{
    public class PhoneDto
    {
        public int Id { get; set; }
        [Required]
        [Phone]
        public string Number { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsMain { get; set; }
    }
}