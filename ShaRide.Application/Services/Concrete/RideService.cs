using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoWrapper.Wrappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using ShaRide.Application.Contexts;
using ShaRide.Application.DTO.Request.Ride;
using ShaRide.Application.DTO.Response;
using ShaRide.Application.DTO.Response.Ride;
using ShaRide.Application.Helpers;
using ShaRide.Application.Localize;
using ShaRide.Application.Managers;
using ShaRide.Application.Services.Interface;
using ShaRide.Domain.Entities;
using ShaRide.Domain.Enums;

namespace ShaRide.Application.Services.Concrete
{
    public class RideService : IRideService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IStringLocalizer _localizer;
        private readonly IMapper _mapper;
        private readonly ICarService _carService;
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly UserManager _userManager;

        public RideService(ApplicationDbContext dbContext, IStringLocalizer<Resource> localizer, IMapper mapper,
            ICarService carService, IAuthenticatedUserService authenticatedUserService, UserManager userManager)
        {
            _dbContext = dbContext;
            _localizer = localizer;
            _mapper = mapper;
            _carService = carService;
            _authenticatedUserService = authenticatedUserService;
            _userManager = userManager;
        }

        public ICollection<RideResponse> GetAllActiveRides()
        {
            var results = _dbContext.Rides
                .Include(x=>x.Driver)
                .ThenInclude(x=>x.Phones)
                .Include(x=>x.RideCarSeatComposition)
                .ThenInclude(x=>x.CarSeatComposition)
                .ThenInclude(x=>x.Car)
                .Include(x=>x.RideCarSeatComposition)
                .ThenInclude(x=>x.CarSeatComposition)
                .ThenInclude(x=>x.Seat)
                .Include(x=>x.RideCarSeatComposition)
                .ThenInclude(x=>x.Passenger)
                .Include(x=>x.RideLocationPointComposition)
                .ThenInclude(x=>x.LocationPoint)
                .ThenInclude(x=>x.Location)
                .Include(x=>x.RestrictionRideComposition)
                .ThenInclude(x=>x.Restriction)
                .Where(x => x.RideState != RideState.Finished);

            return _mapper.Map<ICollection<RideResponse>>(results);
        }

        public ICollection<RideResponse> GetActiveRides(GetActiveRidesRequest request)
        {
            var results = _dbContext.Rides
                .Include(x=>x.Driver)
                .ThenInclude(x=>x.Phones)
                .Include(x=>x.RideCarSeatComposition)
                .ThenInclude(x=>x.CarSeatComposition)
                .ThenInclude(x=>x.Car)
                .Include(x=>x.RideCarSeatComposition)
                .ThenInclude(x=>x.CarSeatComposition)
                .ThenInclude(x=>x.Seat)
                .Include(x=>x.RideCarSeatComposition)
                .ThenInclude(x=>x.Passenger)
                .Include(x=>x.RideLocationPointComposition)
                .ThenInclude(x=>x.LocationPoint)
                .ThenInclude(x=>x.Location)
                .Include(x=>x.RestrictionRideComposition)
                .ThenInclude(x=>x.Restriction)
                .Where(x => x.RideLocationPointComposition
                                .Any(y => y.LocationPoint.LocationId == request.FromLocationId &&
                                          y.LocationPointType == LocationPointType.StartPoint ||
                                          y.LocationPoint.LocationId == request.ToLocationId &&
                                          y.LocationPointType == LocationPointType.FinishPoint) &&
                            x.RideState != RideState.Finished);

            return _mapper.Map<ICollection<RideResponse>>(results);
        }

        public async Task<RideResponse> InsertRide(InsertRideRequest request)
        {
            var ride = _mapper.Map<Ride>(request);

            ride.DriverId = DriverId(request);

            if (!request.CarId.HasValue)
            {
                var carInsertResult = await _carService.InsertCarAsync(request.Car);
                request.CarId = carInsertResult.Id;
            }

            await _dbContext.Rides.AddAsync(ride);

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<RideResponse>(ride);
        }
        protected int DriverId(InsertRideRequest rideRequest)
        {
            int driverId;

            if (rideRequest.DriverId.HasValue && rideRequest.DriverId != _authenticatedUserService.UserId)
            {
                User user;
                if (!_userManager.TryGetUserById(rideRequest.DriverId.Value, out user))
                {
                    throw new ApiException(_localizer[LocalizationKeys.NOT_FOUND, $"Driver - {rideRequest.DriverId}"]);
                }

                driverId = rideRequest.DriverId.Value;
            }
            else
            {
                driverId = _authenticatedUserService.UserId.Value;
            }

            return driverId;
        }
    }
}