using System.Collections.Generic;
using System.Threading.Tasks;
using ShaRide.Application.DTO.Request.CarBrand;
using ShaRide.Application.DTO.Response.CarBrand;

namespace ShaRide.Application.Services.Interface
{
    public interface ICarBrandService
    {
        Task<ICollection<CarBrandResponse>> GetCarBrands();
        Task<CarBrandResponse> GetCarBrandById(int request);
        Task<CarBrandResponse> InsertCarBrand(InsertCarBrandRequest request);
        Task<ICollection<CarBrandResponse>> InsertCarBrands(ICollection<InsertCarBrandRequest> request);
        Task<CarBrandResponse> UpdateCarBrand(UpdateCarBrandRequest request);
        Task DeleteCarBrand(int request);
    }
}