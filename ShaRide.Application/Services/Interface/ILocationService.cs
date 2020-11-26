using System.Collections.Generic;
using System.Threading.Tasks;
using ShaRide.Application.DTO.Request;
using ShaRide.Application.DTO.Request.Location;
using ShaRide.Application.DTO.Response;
using ShaRide.Application.DTO.Response.Location;
using ShaRide.Application.Helpers;

namespace ShaRide.Application.Services.Interface
{
    public interface ILocationService
    {
        Task<ICollection<LocationResponse>> GetLocations();
        Task<ICollection<LocationPointResponse>> GetLocationPoints();
        Task<ICollection<LocationPointResponse>> GetLocationPointsByLocationId(int request);
        Task<LocationResponse> GetLocationById(int request);
        Task<LocationResponse> InsertLocation(InsertLocationRequest request);
        Task<LocationPointResponse> InsertLocationPoint(InsertLocationPointRequest request);
        Task<LocationResponse> UpdateLocation(UpdateLocationRequest request);
        Task<LocationPointResponse> UpdateLocationPoint(UpdateLocationPointRequest request);
        Task DeleteLocation(int request);
        Task DeleteLocationPoint(int request);
    }

}