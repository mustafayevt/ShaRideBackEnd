using System.ComponentModel.DataAnnotations;

namespace ShaRide.Application.DTO.Request.Location
{
    public class InsertLocationRequest
    {
        [Required]
        public string Name { get; set; }
    }
}