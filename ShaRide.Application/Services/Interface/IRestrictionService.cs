using System.Collections.Generic;
using System.Threading.Tasks;
using ShaRide.Application.DTO.Request.Restriction;
using ShaRide.Application.DTO.Response.Restriction;

namespace ShaRide.Application.Services.Interface
{
    public interface IRestrictionService
    {
        Task<ICollection<RestrictionResponse>> GetRestrictions();
        Task<RestrictionResponse> GetRestrictionById(int request);
        Task<RestrictionResponse> InsertRestriction(InsertRestrictionRequest request);
        Task<ICollection<RestrictionResponse>> InsertRestrictions(ICollection<InsertRestrictionRequest> request);
        Task<RestrictionResponse> UpdateRestriction(UpdateRestrictionRequest request);
        Task DeleteRestriction(int request);
    }

}