using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using AutoMapper;
using AutoWrapper.Wrappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Localization;
using ShaRide.Application.Contexts;
using ShaRide.Application.DTO.Request.Car;
using ShaRide.Application.DTO.Response;
using ShaRide.Application.DTO.Response.Car;
using ShaRide.Application.Helpers;
using ShaRide.Application.Localize;
using ShaRide.Application.Services.Interface;
using ShaRide.Domain.Entities;
using ShaRide.Domain.Enums;

namespace ShaRide.Application.Services.Concrete
{
    public class CarService : ICarService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IStringLocalizer _localizer;
        private readonly IMapper _mapper;

        public CarService(ApplicationDbContext dbContext, IStringLocalizer<Resource> localizer, IMapper mapper)
        {
            _dbContext = dbContext;
            _localizer = localizer;
            _mapper = mapper;
        }

        public async Task<ICollection<CarResponse>> GetCarsAsync()
        {
            var cars = await _dbContext.Cars
                .Include(x=>x.CarImages)
                .Include(x=>x.CarModel)
                .ThenInclude(x=> x.CarBrand)
                .Include(x=>x.CarModel)
                .ThenInclude(x=>x.BanType)
                .Where(x => x.IsRowActive)
                .ToListAsync();

            return _mapper.Map<ICollection<CarResponse>>(cars);
        }

        public async Task<CarResponse> GetCarByIdAsync(int request)
        {
            var car = await _dbContext.Cars.Include(x=>x.CarImages).Where(x => x.IsRowActive).FirstOrDefaultAsync(x => x.Id == request);

            return _mapper.Map<CarResponse>(car);
        }

        public async Task<ICollection<CarResponse>> GetCarsByUserIdAsync(int request)
        {
            var cars = await _dbContext
                .Cars
                .Include(x => x.CarModel)
                .ThenInclude(x => x.BanType)
                .Include(x => x.CarModel)
                .ThenInclude(x => x.CarBrand)
                .Include(x=>x.CarSeatComposition)
                .ThenInclude(x=>x.Seat)
                .Where(x =>
                    _dbContext
                        .Rides
                        .Include(x1 => x1.RideCarSeatComposition)
                        .ThenInclude(x2 => x2.CarSeatComposition)
                        .Any(y => y.RideCarSeatComposition.Any(h => h.CarSeatComposition.CarId == x.Id) &&
                                  y.DriverId == request)).ToListAsync();
            
                foreach (var car in cars)
                {
                    foreach (var carSeatComposition in car.CarSeatComposition)
                    {
                        if (carSeatComposition.SeatType == SeatStatus.Sold)
                            carSeatComposition.SeatType = SeatStatus.Suitable;
                    }
                }

            return _mapper.Map<ICollection<CarResponse>>(cars);
        }

        public async Task<CarResponse> InsertCarAsync(InsertCarRequest request)
        {

            var car = _mapper.Map<Car>(request);
            
            await _dbContext.Cars.AddAsync(car);

            await _dbContext.SaveChangesAsync();
            
            return _mapper.Map<CarResponse>(car);
        }

        public Task<ICollection<CarResponse>> InsertCarsAsync(ICollection<InsertCarRequest> request)
        {
            throw new System.NotImplementedException();
        }

        public async Task<CarImage> GetCarImageByCarImageId(int request)
        {
            var carImage = await _dbContext.CarImages.FirstOrDefaultAsync(x => x.IsRowActive && x.Id == request);

            if (carImage is null)
                throw new ApiException(_localizer[LocalizationKeys.NOT_FOUND]);

            return carImage;
        }

        public Task DeleteCarAsync(int request)
        {
            throw new System.NotImplementedException();
        }
    }
}