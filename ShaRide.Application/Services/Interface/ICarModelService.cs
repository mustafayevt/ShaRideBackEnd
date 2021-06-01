using ShaRide.Application.DTO.Request.CarModel;
using ShaRide.Application.DTO.Response.CarModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShaRide.Application.Services.Interface
{
    public interface ICarModelService
    {
        Task<ICollection<CarModelResponse>> GetCarModelsAsync();
        Task<CarModelResponse> GetCarModelByIdAsync(int request);
        Task<CarModelResponse> InsertCarModelAsync(InsertCarModelRequest request);
        Task<ICollection<CarModelResponse>> InsertCarModelsAsync(ICollection<InsertCarModelRequest> request);
        Task<CarModelResponse> UpdateCarModelAsync(UpdateCarModelRequest request);
        Task DeleteCarModelAsync(int request);
    }
}