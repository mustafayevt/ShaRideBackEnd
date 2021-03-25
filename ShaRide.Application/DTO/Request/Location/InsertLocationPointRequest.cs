using System.ComponentModel.DataAnnotations;
using ShaRide.Application.Helpers;
using ShaRide.Domain.Enums;

namespace ShaRide.Application.DTO.Request.Location
{
    public class InsertLocationPointRequest
    {
        public int LocationId { get; set; }
        public string Name { get; set; }
        
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public double Latitude { get; set; }
        
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public double Longitude { get; set; }
    }
}