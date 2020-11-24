using System.ComponentModel.DataAnnotations;
using ShaRide.Application.Helpers;

namespace ShaRide.Application.DTO.Request
{
    public class PhoneRequest
    {
        [Phone(ErrorMessage = LocalizationKeys.INVALID_CREDENTIALS)]
        public string Number { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsMain { get; set; }
    }

}