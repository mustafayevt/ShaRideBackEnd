using System.ComponentModel.DataAnnotations;
using ShaRide.Application.Helpers;

namespace ShaRide.Application.DTO.Request.Restriction
{
    public class UpdateRestrictionRequest
    {
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string AssetPath { get; set; }
    }
}