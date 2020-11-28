using System.ComponentModel.DataAnnotations;
using ShaRide.Application.Helpers;

namespace ShaRide.Application.DTO.Request.Location
{
    public class InsertLocationRequest
    {
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public string Name { get; set; }
    }
}