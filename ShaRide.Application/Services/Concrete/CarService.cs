using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using ShaRide.Application.Contexts;
using ShaRide.Application.DTO.Request.Car;
using ShaRide.Application.DTO.Response;
using ShaRide.Application.DTO.Response.Car;
using ShaRide.Application.Localize;
using ShaRide.Application.Services.Interface;
using ShaRide.Domain.Entities;

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
            var car = await _dbContext.Cars.Where(x => x.IsRowActive).FirstOrDefaultAsync(x => x.Id == request);

            return _mapper.Map<CarResponse>(car);
        }

        public async Task<ICollection<CarResponse>> GetCarsByUserIdAsync(int request)
        {
            var rides =  _dbContext.Rides
                .Include(x=>x.RideCarSeatComposition)
                .ThenInclude(x=>x.CarSeatComposition)
                .ThenInclude(x=>x.Car)
                .Where(x => x.IsRowActive)
                .Where(x => x.DriverId == request);

            var car = rides.SelectMany(x => x.RideCarSeatComposition)
                .Select(x => x.CarSeatComposition)
                .Select(x => x.Car)
                .Distinct();

            return _mapper.Map<ICollection<CarResponse>>(car);
        }

        public async Task<CarResponse> InsertCarAsync(InsertCarRequest request)
        {

            var car = _mapper.Map<Car>(request);
            
            await _dbContext.Cars.AddAsync(car);

            await _dbContext.SaveChangesAsync();
            
            return new CarResponse
            {
                
            };
        }

        public Task<ICollection<CarResponse>> InsertCarsAsync(ICollection<InsertCarRequest> request)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteCarAsync(int request)
        {
            throw new System.NotImplementedException();
        }
    }
}