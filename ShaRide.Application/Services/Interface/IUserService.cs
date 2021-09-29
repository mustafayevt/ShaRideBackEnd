using ShaRide.Application.DTO.Request.Management;
using ShaRide.Application.DTO.Response.Management;
using ShaRide.Application.Pagination;
using System.Threading.Tasks;

namespace ShaRide.Application.Services.Interface
{
    public interface IUserService
    {
        Task<PaginatedList<UserFilterResponse>> AllUsers(UserFilterRequest request);
    }
}
