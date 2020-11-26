using System.ComponentModel.DataAnnotations;

namespace ShaRide.Application.DTO.Request.Account
{
    public class PhoneRequest
    {
        [Phone]
        [Required]
        public string Number { get; set; }
        public bool IsMain { get; set; }
    }

}