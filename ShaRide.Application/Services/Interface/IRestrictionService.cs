using System.Collections.Generic;
using System.Threading.Tasks;
using ShaRide.Application.DTO.Request.Restriction;
using ShaRide.Application.DTO.Response.Restriction;

namespace ShaRide.Application.Services.Interface
{
    public interface IRestrictionService
    {
        Task<ICollection<RestrictionResponse>> GetRestrictionsAsync();
        Task<RestrictionResponse> GetRestrictionByIdAsync(int request);
        Task<RestrictionResponse> InsertRestrictionAsync(InsertRestrictionRequest request);
        Task<ICollection<RestrictionResponse>> InsertRestrictionsAsync(ICollection<InsertRestrictionRequest> request);
        Task<RestrictionResponse> UpdateRestrictionAsync(UpdateRestrictionRequest request);
        Task DeleteRestrictionAsync(int request);
    }

}