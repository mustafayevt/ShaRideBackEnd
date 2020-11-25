using System.ComponentModel.DataAnnotations;
using ShaRide.Application.Helpers;

namespace ShaRide.Application.DTO.Request
{
    public class PhoneRequest
    {
        [Phone()]
        public string Number { get; set; }
        public bool IsMain { get; set; }
    }

}