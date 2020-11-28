using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ShaRide.Application.Attributes;
using ShaRide.Application.Helpers;

namespace ShaRide.Application.DTO.Request.Account
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public string LastName { get; set; }

        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public List<PhoneRequest> Phones { get; set; }

        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        [Password(ErrorMessage = LocalizationKeys.PASSWORD)]
        public string Password { get; set; }

        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public AttachmentRequest Attachment { get; set; }
    }
}