using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ShaRide.Application.Attributes;
using ShaRide.Application.Helpers;

namespace ShaRide.Application.DTO.Request
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