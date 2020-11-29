using System.Collections.Generic;
using System.Threading.Tasks;
using ShaRide.Application.DTO.Request.BanType;
using ShaRide.Application.DTO.Response.BanType;

namespace ShaRide.Application.Services.Interface
{
    public interface IBanTypeService
    {
        Task<ICollection<BanTypeResponse>> GetBanTypesAsync();
        Task<BanTypeResponse> GetBanTypeByIdAsync(int request);
        Task<BanTypeResponse> InsertBanTypeAsync(InsertBanTypeRequest request);
        Task<ICollection<BanTypeResponse>> InsertBanTypesAsync(ICollection<InsertBanTypeRequest> request);
        Task<BanTypeResponse> UpdateBanTypeAsync(UpdateBanTypeRequest request);
        Task DeleteBanTypeAsync(int request);
    }
}