using System.Collections.Generic;
using System.Threading.Tasks;
using ShaRide.Application.DTO.Request.Car;
using ShaRide.Application.DTO.Response;
using ShaRide.Application.DTO.Response.Car;

namespace ShaRide.Application.Services.Interface
{
    public interface ICarService
    {
        Task<ICollection<CarResponse>> GetCarsAsync();
        Task<CarResponse> GetCarByIdAsync(int request);
        Task<ICollection<CarResponse>> GetCarsByUserIdAsync(int request);
        Task<CarResponse> InsertCarAsync(InsertCarRequest request);
        Task<ICollection<CarResponse>> InsertCarsAsync(ICollection<InsertCarRequest> request);
        Task DeleteCarAsync(int request);
    }
}