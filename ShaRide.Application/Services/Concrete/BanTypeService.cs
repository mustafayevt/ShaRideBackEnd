using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoWrapper.Wrappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using ShaRide.Application.Contexts;
using ShaRide.Application.DTO.Request.BanType;
using ShaRide.Application.DTO.Response.BanType;
using ShaRide.Application.Helpers;
using ShaRide.Application.Localize;
using ShaRide.Application.Services.Interface;
using ShaRide.Domain.Entities;

namespace ShaRide.Application.Services.Concrete
{
    public class BanTypeService : IBanTypeService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer _localizer;

        public BanTypeService(ApplicationDbContext dbContext, IMapper mapper, IStringLocalizer<Resource> localizer)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<ICollection<BanTypeResponse>> GetBanTypesAsync()
        {
            var banTypes = await _dbContext.BanTypes.Where(x => x.IsRowActive).ToListAsync();

            return _mapper.Map<ICollection<BanTypeResponse>>(banTypes);
        }

        public async Task<BanTypeResponse> GetBanTypeByIdAsync(int request)
        {
            var banType = await _dbContext.BanTypes.Where(x => x.IsRowActive).FirstOrDefaultAsync(x => x.Id == request);

            if (banType == null)
                throw new ApiException(_localizer[LocalizationKeys.NOT_FOUND, request]);

            return _mapper.Map<BanTypeResponse>(banType);
        }

        public async Task<BanTypeResponse> InsertBanTypeAsync(InsertBanTypeRequest request)
        {
            var banType = _mapper.Map<BanType>(request);

            //validation
            if (_dbContext.BanTypes.Where(x => x.IsRowActive).Any(x => x.Title == request.Title))
                throw new ApiException(_localizer[LocalizationKeys.ALREADY_EXISTS, request.Title]);

            var insertedBanType = await _dbContext.BanTypes.AddAsync(banType);

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<BanTypeResponse>(insertedBanType.Entity);
        }

        public async Task<ICollection<BanTypeResponse>> InsertBanTypesAsync(ICollection<InsertBanTypeRequest> request)
        {
            ICollection<BanType> insertedBanTypes = new List<BanType>();

            foreach (var insertBanTypeRequest in request)
            {
                // passes through from existing banType.
                if (_dbContext.BanTypes.Where(x => x.IsRowActive).Any(x => x.Title == insertBanTypeRequest.Title))
                    continue;

                var banType = _mapper.Map<BanType>(insertBanTypeRequest);

                var insertedBanType = await _dbContext.BanTypes.AddAsync(banType);

                insertedBanTypes.Add(insertedBanType.Entity);
            }

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<ICollection<BanTypeResponse>>(insertedBanTypes);
        }

        public async Task<BanTypeResponse> UpdateBanTypeAsync(UpdateBanTypeRequest request)
        {
            var updatedBanType = await _dbContext.BanTypes.Where(x => x.IsRowActive).AsTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (updatedBanType == null)
                throw new ApiException(_localizer[LocalizationKeys.NOT_FOUND, request.Id]);

            updatedBanType.Title = request.Title;
            updatedBanType.AssetPath = request.AssetPath;

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<BanTypeResponse>(updatedBanType);
        }

        public async Task DeleteBanTypeAsync(int request)
        {
            var deleteBanType = await _dbContext.BanTypes.Where(x=>x.IsRowActive).AsTracking().FirstOrDefaultAsync(x => x.Id == request);

            if (deleteBanType == null)
                throw new ApiException(_localizer[LocalizationKeys.NOT_FOUND, request]);

            deleteBanType.IsRowActive = false;

            await _dbContext.SaveChangesAsync();
        }
    }
}