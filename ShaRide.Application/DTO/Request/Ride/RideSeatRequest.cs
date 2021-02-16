using System.ComponentModel.DataAnnotations;
using ShaRide.Application.Helpers;
using ShaRide.Domain.Enums;

namespace ShaRide.Application.DTO.Request.Ride
{
    public class RideSeatRequest
    {
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public int Id { get; set; }

        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public SeatStatus SeatStatus { get; set; }
    }
}