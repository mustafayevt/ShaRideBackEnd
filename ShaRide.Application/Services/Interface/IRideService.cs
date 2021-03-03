using System.Collections.Generic;
using System.Threading.Tasks;
using ShaRide.Application.DTO.Request.Ride;
using ShaRide.Application.DTO.Response;
using ShaRide.Application.DTO.Response.Ride;

namespace ShaRide.Application.Services.Interface
{
    public interface IRideService
    {
        ICollection<RideResponse> GetAllActiveRides();
        ICollection<RideResponse> GetActiveRides(GetActiveRidesRequest request);
        Task<RideResponse> InsertRide(InsertRideRequest request);
    }
}