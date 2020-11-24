using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ShaRide.Application.Helpers;

namespace ShaRide.Application.DTO.Request
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = LocalizationKeys.PHONE_ALREADY_TAKEN)]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<PhoneRequest> Phones { get; set; }

        public string Password { get; set; }

        public AttachmentRequest Attachment { get; set; }
    }
}