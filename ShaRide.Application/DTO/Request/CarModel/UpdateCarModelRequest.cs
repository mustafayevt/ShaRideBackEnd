using System.ComponentModel.DataAnnotations;
using ShaRide.Application.Helpers;

namespace ShaRide.Application.DTO.Request.CarModel
{
    public class UpdateCarModelRequest
    {
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public int Id { get; set; }
        
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public string Title { get; set; }

        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public int CarBrandId { get; set; }

        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public int BanTypeId { get; set; }
    }
}