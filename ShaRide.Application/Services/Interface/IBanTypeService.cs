using System.Collections.Generic;
using System.Threading.Tasks;
using ShaRide.Application.DTO.Request.BanType;
using ShaRide.Application.DTO.Response.BanType;

namespace ShaRide.Application.Services.Interface
{
    public interface IBanTypeService
    {
        Task<ICollection<BanTypeResponse>> GetBanTypes();
        Task<BanTypeResponse> GetBanTypeById(int request);
        Task<BanTypeResponse> InsertBanType(InsertBanTypeRequest request);
        Task<ICollection<BanTypeResponse>> InsertBanTypes(ICollection<InsertBanTypeRequest> request);
        Task<BanTypeResponse> UpdateBanType(UpdateBanTypeRequest request);
        Task DeleteBanType(int request);
    }
}