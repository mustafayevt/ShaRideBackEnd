using AutoMapper;
using AutoWrapper.Wrappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Localization;
using ShaRide.Application.Contexts;
using ShaRide.Application.DTO.Request.Car;
using ShaRide.Application.DTO.Response.Car;
using ShaRide.Application.Helpers;
using ShaRide.Application.Localize;
using ShaRide.Application.Managers;
using ShaRide.Application.Services.Interface;
using ShaRide.Domain.Entities;
using ShaRide.Domain.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace ShaRide.Application.Services.Concrete
{
    public class CarService : ICarService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IStringLocalizer _localizer;
        private readonly IMapper _mapper;
        private readonly UserManager _userManager;

        public CarService(ApplicationDbContext dbContext, IStringLocalizer<Resource> localizer, IMapper mapper, UserManager userManager)
        {
            _dbContext = dbContext;
            _localizer = localizer;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<ICollection<CarResponse>> GetCarsAsync()
        {
            var cars = await _dbContext.Cars
                .Include(x => x.CarImages)
                .Include(x => x.CarModel)
                .ThenInclude(x => x.CarBrand)
                .Include(x => x.BanType)
                .Where(x => x.IsRowActive)
                .ToListAsync();

            return _mapper.Map<ICollection<CarResponse>>(cars);
        }

        public async Task<CarResponse> GetCarByIdAsync(int request)
        {
            var car = await _dbContext.Cars.Include(x => x.CarImages).Where(x => x.IsRowActive).FirstOrDefaultAsync(x => x.Id == request);

            return _mapper.Map<CarResponse>(car);
        }

        public async Task<ICollection<CarResponse>> GetCarsByUserIdAsync(int request)
        {
            var cars = await _dbContext
                .Cars
                .Include(x => x.BanType)
                .Include(x => x.CarModel)
                .ThenInclude(x => x.CarBrand)
                .Include(x => x.CarSeatComposition)
                .ThenInclude(x => x.Seat)
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

        public async Task<ICollection<CarResponse>> GetUserCarsAsync(int userId)
        {
            var cars = await _dbContext.Users
                .Include(x => x.UserCars).ThenInclude(x => x.BanType)
                .Include(x => x.UserCars).ThenInclude(x => x.CarModel).ThenInclude(x => x.CarBrand)
                .Include(x => x.UserCars).ThenInclude(x => x.CarSeatComposition).ThenInclude(x => x.Seat)
                .Where(x => x.Id == userId)
                .SelectMany(c => c.UserCars).ToListAsync();

            foreach (var car in cars)
            {
                foreach (var carSeatComposition in car.CarSeatComposition)
                {
                    if (carSeatComposition.SeatType == SeatStatus.Sold)
                        carSeatComposition.SeatType = SeatStatus.Suitable;
                }
            }

            var responseCars = _mapper.Map<ICollection<CarResponse>>(cars);
            if (!responseCars.Any(c => c.IsDefault))
            {
                responseCars.OrderByDescending(c => c.Id).FirstOrDefault().IsDefault = true;
            }

            return responseCars;
        }

        public async Task<ICollection<CarResponse>> MakeCarDefault(int carId)
        {
            var user = await _userManager.GetCurrentUser();

            if (user == null)
                throw new ApiException(_localizer.GetString(LocalizationKeys.INVALID_CREDENTIALS));

            var cars = await _dbContext.Users
                .Include(x => x.UserCars)
                .Where(x => x.Id == user.Id)
                .SelectMany(c => c.UserCars).ToListAsync();

            foreach (var car in cars)
            {
                if (car.Id != carId)
                {
                    car.IsDefault = false;
                }
                else
                {
                    car.IsDefault = true;
                }
            }
            _dbContext.Cars.UpdateRange(cars);
            await _dbContext.SaveChangesAsync();

            var resultCars = await _dbContext.Cars
            .Include(x => x.BanType)
            .Include(x => x.CarModel).ThenInclude(x => x.CarBrand)
            .Include(x => x.CarSeatComposition).ThenInclude(x => x.Seat)
            .Where(c => cars.Contains(c))
            .ToListAsync();

            foreach (var car in resultCars)
            {
                foreach (var carSeatComposition in car.CarSeatComposition)
                {
                    if (carSeatComposition.SeatType == SeatStatus.Sold)
                        carSeatComposition.SeatType = SeatStatus.Suitable;
                }
            }

            var responseCars = _mapper.Map<ICollection<CarResponse>>(resultCars);
            if (!responseCars.Any(c => c.IsDefault))
            {
                responseCars.OrderByDescending(c => c.Id).FirstOrDefault().IsDefault = true;
            }

            return responseCars;
        }

        //public async Task MigrateCars()
        //{
        //    try
        //    {
        //        var rides = await _dbContext.Rides
        //       .Include(x1 => x1.RideCarSeatComposition)
        //       .ThenInclude(x2 => x2.CarSeatComposition)
        //       .ThenInclude(x2 => x2.Car)
        //       .ToListAsync();

        //        List<Car> cars = new List<Car>();
        //        foreach (var ride in rides)
        //        {
        //            foreach (var car in ride.RideCarSeatComposition.Where(r => r.CarSeatComposition.Car != null).Select(r=>r.CarSeatComposition.Car))
        //            {
        //                if (!cars.Select(c=>c.Id).Contains(car.Id))
        //                {
        //                    cars.Add(car);
        //                    car.UserId = ride.DriverId;
        //                    _dbContext.Cars.Update(car);
        //                }
        //            }
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {

        //        throw;
        //    }


        //    await _dbContext.SaveChangesAsync();
        //}

        public async Task<CarResponse> InsertCarAsync(InsertCarRequest request)
        {
            var user = await _userManager.GetCurrentUser();

            if (user == null)
                throw new ApiException(_localizer.GetString(LocalizationKeys.INVALID_CREDENTIALS));

            var car = _mapper.Map<Car>(request);

            car.UserId = user.Id;

            var userInDb = await _dbContext.Users.Include(u => u.UserCars).FirstOrDefaultAsync(u => u.Id == user.Id);
            if (!user.UserCars.Any())
            {
                car.IsDefault = true;
            }

            var insertedCar = await _dbContext.Cars.AddAsync(car);

            await _dbContext.SaveChangesAsync();

            await insertedCar.Reference(c => c.BanType).LoadAsync();

            await insertedCar.Reference(x => x.CarModel).LoadAsync();

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

        public async Task<int> UpdateCarBanIdAsync(UpdateCarBanIdRequest request)
        {
            var car = await _dbContext.Cars.AsTracking().FirstOrDefaultAsync(x => request.CarId == x.Id);
            car.BanTypeId = request.BanId;

            await _dbContext.SaveChangesAsync();
            return 0;
        }

        public Task DeleteCarAsync(int request)
        {
            throw new System.NotImplementedException();
        }
    }
}