using ShaRide.Application.Attributes;
using ShaRide.Application.DTO.Request.Common;
using ShaRide.Application.Helpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        public AttachmentRequest Attachment { get; set; }
    }
}