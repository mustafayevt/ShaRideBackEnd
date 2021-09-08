using ShaRide.Application.DTO.Request.Car;
using ShaRide.Application.DTO.Response.Car;
using ShaRide.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShaRide.Application.Services.Interface
{
    public interface ICarService
    {
        Task<ICollection<CarResponse>> GetCarsAsync();
        Task<CarResponse> GetCarByIdAsync(int request);
        Task<ICollection<CarResponse>> GetCarsByUserIdAsync(int request);
        Task<ICollection<CarResponse>> GetUserCarsAsync(int request); 
        Task<ICollection<CarResponse>> MakeCarDefault(int carId); 
        Task<CarResponse> InsertCarAsync(InsertCarRequest request);
        Task<ICollection<CarResponse>> InsertCarsAsync(ICollection<InsertCarRequest> request);
        Task<CarImage> GetCarImageByCarImageId(int request);
        Task<int> UpdateCarBanIdAsync(UpdateCarBanIdRequest request);
        Task DeleteCarAsync(int carId);
        //Task MigrateCars();
    }
}