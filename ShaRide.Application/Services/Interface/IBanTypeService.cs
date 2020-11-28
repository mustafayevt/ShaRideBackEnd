using System.Collections.Generic;
using System.Threading.Tasks;
using ShaRide.Application.DTO.Response.BanType;

namespace ShaRide.Application.Services.Interface
{
    public interface IBanTypeService
    {
        Task<ICollection<BanTypeResponse>> GetBanTypes();
    }
}