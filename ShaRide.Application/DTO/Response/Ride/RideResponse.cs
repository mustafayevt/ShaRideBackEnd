using System;
using System.Collections.Generic;
using ShaRide.Application.DTO.Response.Account;
using ShaRide.Application.DTO.Response.Car;
using ShaRide.Application.DTO.Response.Location;
using ShaRide.Application.DTO.Response.Restriction;
using ShaRide.Domain.Enums;

namespace ShaRide.Application.DTO.Response.Ride
{
    public class RideResponse
    {
        public int Id { get; set; }

        public UserResponse Driver { get; set; }

        public DateTime StartDate { get; set; }
        
        public decimal PricePerSeat { get; set; }

        public string Note { get; set; }
        
        public RideState RideState { get; set; }

        public ICollection<RideCarSeatCompositionResponse> RideCarSeatComposition { get; set; }
        public ICollection<RideLocationPointCompositionResponse> LocationPoints { get; set; }
        
        public ICollection<RestrictionResponse> Restrictions { get; set; }
    }
}