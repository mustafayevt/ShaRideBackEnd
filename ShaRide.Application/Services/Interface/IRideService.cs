using System.Threading.Tasks;
using ShaRide.Application.DTO.Request.Ride;
using ShaRide.Application.DTO.Response;
using ShaRide.Application.DTO.Response.Ride;

namespace ShaRide.Application.Services.Interface
{
    public interface IRideService
    {
        Task<RideResponse> GetActiveRides();
        Task<RideResponse> InsertRide(InsertRideRequest request);
    }
}