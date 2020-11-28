using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ShaRide.Application.Helpers;

namespace ShaRide.Application.DTO.Request.Account
{
    public class AttachmentRequest
    {
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public List<byte> Content { get; set; }
        
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public string Extension { get; set; }
    }

}