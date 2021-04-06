using System.ComponentModel.DataAnnotations;
using ShaRide.Application.Helpers;

namespace ShaRide.Application.DTO.Request.CarModel
{
    public class UpdateCarModelBanIdRequest
    {
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public int ModelId { get; set; }
        
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public int BanId { get; set; }
    }
}