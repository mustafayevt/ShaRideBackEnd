using System.ComponentModel.DataAnnotations;
using ShaRide.Application.Helpers;

namespace ShaRide.Application.DTO.Request.CarBrand
{
    public class InsertCarBrandRequest
    {
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public string Title { get; set; }
    }
}