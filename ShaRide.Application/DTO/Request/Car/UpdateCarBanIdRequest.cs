using System.ComponentModel.DataAnnotations;
using ShaRide.Application.Helpers;

namespace ShaRide.Application.DTO.Request.Car
{
    public class UpdateCarBanIdRequest
    {
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public int CarId { get; set; }

        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public int BanId { get; set; }
    }
}