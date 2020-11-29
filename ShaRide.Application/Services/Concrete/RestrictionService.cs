using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoWrapper.Wrappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using ShaRide.Application.Contexts;
using ShaRide.Application.DTO.Request.Restriction;
using ShaRide.Application.DTO.Response.Restriction;
using ShaRide.Application.Helpers;
using ShaRide.Application.Localize;
using ShaRide.Application.Services.Interface;
using ShaRide.Domain.Entities;

namespace ShaRide.Application.Services.Concrete
{
    public class RestrictionService : IRestrictionService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer _localizer;

        public RestrictionService(ApplicationDbContext dbContext, IMapper mapper, IStringLocalizer<Resource> localizer)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<ICollection<RestrictionResponse>> GetRestrictionsAsync()
        {
            var restrictions = await _dbContext.Restrictions.Where(x=>x.IsRowActive).ToListAsync();

            return _mapper.Map<ICollection<RestrictionResponse>>(restrictions);
        }

        public async Task<RestrictionResponse> GetRestrictionByIdAsync(int request)
        {
            var restriction = await _dbContext.Restrictions.Where(x=>x.IsRowActive).FirstOrDefaultAsync(x => x.Id == request);

            if (restriction == null)
                throw new ApiException(_localizer[LocalizationKeys.RESTRICTOIN_NOT_FOUND,request]);

            return _mapper.Map<RestrictionResponse>(restriction);
        }

        public async Task<RestrictionResponse> InsertRestrictionAsync(InsertRestrictionRequest request)
        {
            //validation
            if(_dbContext.Restrictions.Where(x=>x.IsRowActive).Any(x=>x.Title == request.Title))
                throw new ApiException(_localizer[LocalizationKeys.RESTRICTION_ALREADY_EXISTS]);
            
            var restriction = _mapper.Map<Restriction>(request);
            
            var insertedRestriction = await _dbContext.Restrictions.AddAsync(restriction);

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<RestrictionResponse>(insertedRestriction.Entity);
        }

        public async Task<ICollection<RestrictionResponse>> InsertRestrictionsAsync(ICollection<InsertRestrictionRequest> request)
        {
            ICollection<Restriction> insertedRestrictions = new List<Restriction>();

            foreach (var insertRestrictionRequest in request)
            {
                // passes through from existing restriction.
                if(_dbContext.Restrictions.Where(x=>x.IsRowActive).Any(x=>x.Title == insertRestrictionRequest.Title))
                    continue;
                
                var restriction = _mapper.Map<Restriction>(insertRestrictionRequest);

                var insertedRestriction = await _dbContext.Restrictions.AddAsync(restriction);
                
                insertedRestrictions.Add(insertedRestriction.Entity);
            }

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<ICollection<RestrictionResponse>>(insertedRestrictions);
        }

        public async Task<RestrictionResponse> UpdateRestrictionAsync(UpdateRestrictionRequest request)
        {
            var updatedRestriction = await _dbContext.Restrictions.Where(x=>x.IsRowActive).AsTracking().FirstOrDefaultAsync(x => x.Id == request.Id);

            if (updatedRestriction == null)
                throw new ApiException(_localizer[LocalizationKeys.RESTRICTOIN_NOT_FOUND,request.Id]);

            updatedRestriction.Title = request.Title;
            updatedRestriction.AssetPath = request.AssetPath;

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<RestrictionResponse>(updatedRestriction);
        }

        public async Task DeleteRestrictionAsync(int request)
        {
            var deleteRestriction = await _dbContext.Restrictions.Where(x=>x.IsRowActive).AsTracking().FirstOrDefaultAsync(x => x.Id == request);

            if (deleteRestriction == null)
                throw new ApiException(_localizer[LocalizationKeys.RESTRICTOIN_NOT_FOUND,request]);

            deleteRestriction.IsRowActive = false;

            await _dbContext.SaveChangesAsync();
        }
    }
}