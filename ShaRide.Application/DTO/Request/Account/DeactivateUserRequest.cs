using ShaRide.Application.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace ShaRide.Application.DTO.Request.Account
{
    public class DeactivateUserRequest
    {
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public int UserId { get; set; }
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public string Reason { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
