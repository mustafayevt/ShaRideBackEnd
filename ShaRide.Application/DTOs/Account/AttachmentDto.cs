using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShaRide.Application.DTOs.Account
{
    public class AttachmentDto
    {
        [Required]
        public List<byte> Content { get; set; }
        [Required]
        public string Extension { get; set; }
    }
}