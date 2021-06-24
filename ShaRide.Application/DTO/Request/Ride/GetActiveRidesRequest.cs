using ShaRide.Application.Helpers;
using System;
using System.ComponentModel.DataAnnotations;

namespace ShaRide.Application.DTO.Request.Ride
{
    public class GetActiveRidesRequest
    {
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public int FromLocationId { get; set; }

        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public int ToLocationId { get; set; }

        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public DateTime Date { get; set; }

        public int? BanTypeId { get; set; }
    }
}