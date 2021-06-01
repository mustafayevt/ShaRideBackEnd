using AutoMapper;
using AutoWrapper.Wrappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Localization;
using ShaRide.Application.Contexts;
using ShaRide.Application.DTO.Request.CarModel;
using ShaRide.Application.DTO.Response.CarModel;
using ShaRide.Application.Helpers;
using ShaRide.Application.Localize;
using ShaRide.Application.Services.Interface;
using ShaRide.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShaRide.Application.Services.Concrete
{
    public class CarModelService : ICarModelService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer _localizer;

        public CarModelService(ApplicationDbContext dbContext, IMapper mapper, IStringLocalizer<Resource> localizer)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<ICollection<CarModelResponse>> GetCarModelsAsync()
        {
            var carModels = await _dbContext.CarModels
                .Where(x => x.IsRowActive)
                .Include(x => x.CarBrand)
                .ToListAsync();

            carModels.ForEach(x =>
            {
                x.CarBrand = x.CarBrand.IsRowActive ? x.CarBrand : null;
            });

            return _mapper.Map<ICollection<CarModelResponse>>(carModels);
        }

        public async Task<CarModelResponse> GetCarModelByIdAsync(int request)
        {
            var carModel = await _dbContext.CarModels
                .Where(x => x.IsRowActive)
                .Include(x => x.CarBrand)
                .FirstOrDefaultAsync(x => x.Id == request);

            carModel.CarBrand = carModel.CarBrand.IsRowActive ? carModel.CarBrand : null;

            if (carModel == null)
                throw new ApiException(_localizer[LocalizationKeys.NOT_FOUND, request]);

            return _mapper.Map<CarModelResponse>(carModel);
        }

        public async Task<CarModelResponse> InsertCarModelAsync(InsertCarModelRequest request)
        {
            var carModel = _mapper.Map<CarModel>(request);

            //validation
            if (_dbContext.CarModels.Where(x => x.IsRowActive).Any(x => x.Title == request.Title))
                throw new ApiException(_localizer[LocalizationKeys.ALREADY_EXISTS, request.Title]);

            if (!_dbContext.CarBrands.Where(x => x.IsRowActive).Any(x => x.Id == request.CarBrandId))
                throw new ApiException(_localizer[LocalizationKeys.NOT_FOUND, $"CarBrand - {request.CarBrandId}"]);

            if (!_dbContext.BanTypes.Where(x => x.IsRowActive).Any(x => x.Id == request.BanTypeId))
                throw new ApiException(_localizer[LocalizationKeys.NOT_FOUND, $"BanType - {request.BanTypeId}"]);

            var insertedCarModel = await _dbContext.CarModels.AddAsync(carModel);

            await _dbContext.SaveChangesAsync();

            await insertedCarModel.Reference(x => x.CarBrand).LoadAsync();

            return _mapper.Map<CarModelResponse>(insertedCarModel.Entity);
        }

        public async Task<ICollection<CarModelResponse>> InsertCarModelsAsync(ICollection<InsertCarModelRequest> request)
        {
            ICollection<CarModel> insertedCarModels = new List<CarModel>();

            foreach (var insertCarModelRequest in request)
            {
                // passes through from existing carModel.
                if (_dbContext.CarModels.Where(x => x.IsRowActive).Any(x => x.Title == insertCarModelRequest.Title))
                    continue;

                if (!_dbContext.CarBrands.Where(x => x.IsRowActive).Any(x => x.Id == insertCarModelRequest.CarBrandId))
                    continue;

                if (!_dbContext.BanTypes.Where(x => x.IsRowActive).Any(x => x.Id == insertCarModelRequest.BanTypeId))
                    continue;

                var carModel = _mapper.Map<CarModel>(insertCarModelRequest);

                var insertedCarModel = await _dbContext.CarModels.AddAsync(carModel);

                insertedCarModels.Add(insertedCarModel.Entity);
            }

            await _dbContext.SaveChangesAsync();

            foreach (var insertedCarModel in insertedCarModels)
            {
                await _dbContext
                    .Entry(insertedCarModel)
                    .Reference(x => x.CarBrand)
                    .LoadAsync();
            }

            return _mapper.Map<ICollection<CarModelResponse>>(insertedCarModels);
        }

        public async Task<CarModelResponse> UpdateCarModelAsync(UpdateCarModelRequest request)
        {
            var updatedCarModel = await _dbContext.CarModels.Where(x => x.IsRowActive).AsTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (updatedCarModel == null)
                throw new ApiException(_localizer[LocalizationKeys.NOT_FOUND, request.Id]);

            if (!_dbContext.CarBrands.Where(x => x.IsRowActive).Any(x => x.Id == request.CarBrandId))
                throw new ApiException(_localizer[LocalizationKeys.NOT_FOUND, $"CarBrand - {request.CarBrandId}"]);

            if (!_dbContext.BanTypes.Where(x => x.IsRowActive).Any(x => x.Id == request.BanTypeId))
                throw new ApiException(_localizer[LocalizationKeys.NOT_FOUND, $"BanType - {request.BanTypeId}"]);

            updatedCarModel.Title = request.Title;

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<CarModelResponse>(updatedCarModel);
        }

        public async Task DeleteCarModelAsync(int request)
        {
            var deleteCarModel = await _dbContext.CarModels.Where(x => x.IsRowActive).AsTracking()
                .FirstOrDefaultAsync(x => x.Id == request);

            if (deleteCarModel == null)
                throw new ApiException(_localizer[LocalizationKeys.NOT_FOUND, request]);

            deleteCarModel.IsRowActive = false;

            await _dbContext.SaveChangesAsync();
        }
    }
}