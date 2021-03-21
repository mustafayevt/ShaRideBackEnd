using System;
using System.ComponentModel.DataAnnotations;
using ShaRide.Application.Helpers;

namespace ShaRide.Application.DTO.Request.Ride
{
    public class CancelRideRequest
    {
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public DateTime CancelDate { get; set; }
        
        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public int RideId { get; set; }
    }
}