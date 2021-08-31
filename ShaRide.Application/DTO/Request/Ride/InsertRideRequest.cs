using Newtonsoft.Json;
using ShaRide.Application.DTO.Request.Car;
using ShaRide.Application.Helpers;
using ShaRide.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShaRide.Application.DTO.Request.Ride
{
    public class InsertRideRequest
    {
        public int? DriverId { get; set; }

        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public decimal PricePerSeat { get; set; }

        public string Note { get; set; }

        [JsonIgnore]
        public RideState RideState { get; set; } = RideState.New;

        public InsertCarRequest Car { get; set; }

        public int? CarId { get; set; }

        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public ICollection<RideSeatRequest> RideSeatRequests { get; set; }

        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public ICollection<RideLocationPointRequest> RideLocationPoints { get; set; }

        [Required(ErrorMessage = LocalizationKeys.REQUIRED)]
        public ICollection<RideRestrictionRequest> RideRestrictions { get; set; }
    }
}