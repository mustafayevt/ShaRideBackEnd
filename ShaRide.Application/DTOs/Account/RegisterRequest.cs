using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using ShaRide.Application.Attributes;

namespace ShaRide.Application.DTOs.Account
{
    public class RegisterRequest
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public List<PhoneDto> Phones { get; set; }

        [Required]
        [Password]
        public string Password { get; set; }

        [Required]
        public List<byte> Img { get; set; }

        [Required]
        public string ImgExtension { get; set; }
    }
}