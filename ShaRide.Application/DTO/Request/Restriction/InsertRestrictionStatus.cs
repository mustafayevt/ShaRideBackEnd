using System.ComponentModel.DataAnnotations;

namespace ShaRide.Application.DTO.Request.Restriction
{
    public class InsertRestrictionStatus
    {
        [Required]
        public bool IsPossible { get; set; }
        [Required]
        public int RestrictionId { get; set; }
    }
}