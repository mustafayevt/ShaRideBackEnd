using System;
using ShaRide.Domain.Enums;

namespace ShaRide.Application.DTO.Response.Ride
{
    public class GetUserRideRequestResponse
    {
        public RideResponse Ride { get; set; }

        public PassengerToRideRequestStatus RequestStatus { get; set; }

        public DateTime OrderTime { get; set; }

        public int OrderCount { get; set; }
        
        public decimal SumAmountOfOrders { get; set; }
    }
}