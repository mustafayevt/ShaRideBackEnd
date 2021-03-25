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
        Task<ICollection<LocationResponse>> GetLocationsAsync();
        Task<ICollection<LocationPointResponse>> GetLocationPointsAsync();
        Task<ICollection<LocationPointResponse>> GetLocationPointsByLocationIdAsync(int request);
        Task<LocationResponse> GetLocationByIdAsync(int request);
        Task<LocationPointResponse> GetLocationPointByIdAsync(int request);
        Task<LocationPointResponse> GetLocationPointByName(string request);
        Task<LocationResponse> InsertLocationAsync(InsertLocationRequest request);
        Task<LocationPointResponse> InsertLocationPointAsync(InsertLocationPointRequest request);
        Task<LocationResponse> UpdateLocationAsync(UpdateLocationRequest request);
        Task<LocationPointResponse> UpdateLocationPointAsync(UpdateLocationPointRequest request);
        Task DeleteLocationAsync(int request);
        Task DeleteLocationPointAsync(int request);
    }

}