using ShaRide.Application.DTO.Request.Common;
using ShaRide.Application.Helpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShaRide.Application.DTO.Request.Account
{
    public class UpdateUserInfoRequest
    {
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public int UserId { get; set; }

        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public string LastName { get; set; }

        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public List<PhoneRequest> Phones { get; set; }

        public AttachmentUpdateRequest Attachment { get; set; }
    }
}