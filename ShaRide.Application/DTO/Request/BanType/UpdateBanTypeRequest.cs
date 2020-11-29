using System.ComponentModel.DataAnnotations;
using ShaRide.Application.Helpers;

namespace ShaRide.Application.DTO.Request.BanType
{
    public class UpdateBanTypeRequest
    {
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public int Id { get; set; }
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public string Title { get; set; }
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public string AssetPath { get; set; }
    }
}