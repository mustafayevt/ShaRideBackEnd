using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ShaRide.Application.Helpers;

namespace ShaRide.Application.DTO.Request.Invoice
{
    public class RegisterInvoiceRequest
    {
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public string InvoiceNumber { get; set; }

        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        [JsonIgnore]
        public DateTime InvoiceRegisterDate { get; set; } = DateTime.UtcNow;
        
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        [Range(1,int.MaxValue,ErrorMessage = LocalizationKeys.RANGE_VALIDATION)]
        public decimal Amount { get; set; }
        
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public string UserId { get; set; }
    }
}