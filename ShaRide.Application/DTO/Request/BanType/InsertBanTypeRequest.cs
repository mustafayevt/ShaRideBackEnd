using System.ComponentModel.DataAnnotations;
using ShaRide.Application.Helpers;

namespace ShaRide.Application.DTO.Request.BanType
{
    public class InsertBanTypeRequest
    {
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public string Title { get; set; }
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public string AssetPath { get; set; }
    }
}