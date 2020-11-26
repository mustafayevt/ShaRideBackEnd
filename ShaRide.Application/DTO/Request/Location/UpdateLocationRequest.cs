using System.ComponentModel.DataAnnotations;

namespace ShaRide.Application.DTO.Request.Location
{
    public class UpdateLocationRequest
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}