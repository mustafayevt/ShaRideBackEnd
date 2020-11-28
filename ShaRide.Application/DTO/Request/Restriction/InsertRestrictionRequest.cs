using System.ComponentModel.DataAnnotations;

namespace ShaRide.Application.DTO.Request.Restriction
{
    public class InsertRestrictionRequest
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string AssertPath { get; set; }
    }
}