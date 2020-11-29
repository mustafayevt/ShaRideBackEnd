using System.Collections.Generic;
using System.Threading.Tasks;
using ShaRide.Application.DTO.Request.CarModel;
using ShaRide.Application.DTO.Response.CarModel;

namespace ShaRide.Application.Services.Interface
{
    public interface ICarModelService
    {
        Task<ICollection<CarModelResponse>> GetCarModels();
        Task<CarModelResponse> GetCarModelById(int request);
        Task<CarModelResponse> InsertCarModel(InsertCarModelRequest request);
        Task<ICollection<CarModelResponse>> InsertCarModels(ICollection<InsertCarModelRequest> request);
        Task<CarModelResponse> UpdateCarModel(UpdateCarModelRequest request);
        Task DeleteCarModel(int request);
    }
}