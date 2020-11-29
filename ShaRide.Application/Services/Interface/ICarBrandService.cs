using System.Collections.Generic;
using System.Threading.Tasks;
using ShaRide.Application.DTO.Request.CarBrand;
using ShaRide.Application.DTO.Response.CarBrand;

namespace ShaRide.Application.Services.Interface
{
    public interface ICarBrandService
    {
        Task<ICollection<CarBrandResponse>> GetCarBrandsAsync();
        Task<CarBrandResponse> GetCarBrandByIdAsync(int request);
        Task<CarBrandResponse> InsertCarBrandAsync(InsertCarBrandRequest request);
        Task<ICollection<CarBrandResponse>> InsertCarBrandsAsync(ICollection<InsertCarBrandRequest> request);
        Task<CarBrandResponse> UpdateCarBrandAsync(UpdateCarBrandRequest request);
        Task DeleteCarBrandAsync(int request);
    }
}