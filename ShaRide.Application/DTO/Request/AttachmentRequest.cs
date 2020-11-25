using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShaRide.Application.DTO.Request
{
    public class AttachmentRequest
    {
        [Required]
        public List<byte> Content { get; set; }
        
        [Required]
        public string Extension { get; set; }
    }

}