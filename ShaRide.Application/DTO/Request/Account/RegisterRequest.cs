using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ShaRide.Application.Attributes;

namespace ShaRide.Application.DTO.Request.Account
{
    public class RegisterRequest
    {
        [Required()]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public List<PhoneRequest> Phones { get; set; }

        [Required]
        [Password]
        public string Password { get; set; }

        [Required]
        public AttachmentRequest Attachment { get; set; }
    }
}