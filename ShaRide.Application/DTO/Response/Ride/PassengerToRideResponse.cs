using System.Collections.Generic;
using ShaRide.Application.DTO.Response.Car;
using ShaRide.Domain.Entities;

namespace ShaRide.Application.DTO.Response.Ride
{
    public class PassengerToRideResponse
    {
        public RideResponse Ride { get; set; }
        public CarSeatCompositionResponse PassengerRequest { get; set; }
        public int RequestId { get; set; }
    }
}