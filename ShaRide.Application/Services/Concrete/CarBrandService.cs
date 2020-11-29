using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoWrapper.Wrappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using ShaRide.Application.Contexts;
using ShaRide.Application.DTO.Request.CarBrand;
using ShaRide.Application.DTO.Response.CarBrand;
using ShaRide.Application.Helpers;
using ShaRide.Application.Localize;
using ShaRide.Application.Services.Interface;
using ShaRide.Domain.Entities;

namespace ShaRide.Application.Services.Concrete
{
    public class CarBrandService : ICarBrandService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer _localizer;

        public CarBrandService(ApplicationDbContext dbContext, IMapper mapper, IStringLocalizer<Resource> localizer)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<ICollection<CarBrandResponse>> GetCarBrands()
        {
            var carBrands = await _dbContext.CarBrands.ToListAsync();

            return _mapper.Map<ICollection<CarBrandResponse>>(carBrands);
        }

        public async Task<CarBrandResponse> GetCarBrandById(int request)
        {
            var carBrand = await _dbContext.CarBrands.Where(x=>x.IsRowActive).FirstOrDefaultAsync(x => x.Id == request);

            if (carBrand == null)
                throw new ApiException(_localizer[LocalizationKeys.NOT_FOUND,request]);

            return _mapper.Map<CarBrandResponse>(carBrand);
        }

        public async Task<CarBrandResponse> InsertCarBrand(InsertCarBrandRequest request)
        {
            var carBrand = _mapper.Map<CarBrand>(request);

            //validation
            if(_dbContext.CarBrands.Where(x=>x.IsRowActive).Any(x=>x.Title == request.Title))
                throw new ApiException(_localizer[LocalizationKeys.ALREADY_EXISTS,request.Title]);
            
            var insertedCarBrand = await _dbContext.CarBrands.AddAsync(carBrand);

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<CarBrandResponse>(insertedCarBrand.Entity);
        }

        public async Task<ICollection<CarBrandResponse>> InsertCarBrands(ICollection<InsertCarBrandRequest> request)
        {
            ICollection<CarBrand> insertedCarBrands = new List<CarBrand>();

            foreach (var insertCarBrandRequest in request)
            {
                // passes through from existing carBrand.
                if(_dbContext.CarBrands.Where(x=>x.IsRowActive).Any(x=>x.Title == insertCarBrandRequest.Title))
                    continue;
                
                var carBrand = _mapper.Map<CarBrand>(insertCarBrandRequest);

                var insertedCarBrand = await _dbContext.CarBrands.AddAsync(carBrand);
                
                insertedCarBrands.Add(insertedCarBrand.Entity);
            }

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<ICollection<CarBrandResponse>>(insertedCarBrands);
        }

        public async Task<CarBrandResponse> UpdateCarBrand(UpdateCarBrandRequest request)
        {
            var updatedCarBrand = await _dbContext.CarBrands.Where(x=>x.IsRowActive).AsTracking().FirstOrDefaultAsync(x => x.Id == request.Id);

            if (updatedCarBrand == null)
                throw new ApiException(_localizer[LocalizationKeys.NOT_FOUND,request.Id]);

            updatedCarBrand.Title = request.Title;

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<CarBrandResponse>(updatedCarBrand);
        }

        public async Task DeleteCarBrand(int request)
        {
            var deleteCarBrand = await _dbContext.CarBrands.Where(x=>x.IsRowActive).AsTracking().FirstOrDefaultAsync(x => x.Id == request);

            if (deleteCarBrand == null)
                throw new ApiException(_localizer[LocalizationKeys.NOT_FOUND,request]);

            deleteCarBrand.IsRowActive = false;

            await _dbContext.SaveChangesAsync();
        }
    }
}