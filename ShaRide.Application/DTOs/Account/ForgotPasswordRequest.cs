﻿using System.ComponentModel.DataAnnotations;

namespace ShaRide.Application.DTOs.Account
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
