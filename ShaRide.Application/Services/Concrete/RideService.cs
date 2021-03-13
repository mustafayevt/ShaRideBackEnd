using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoWrapper.Wrappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using ShaRide.Application.Contexts;
using ShaRide.Application.DTO.Request.Ride;
using ShaRide.Application.DTO.Request.UserFcmToken;
using ShaRide.Application.DTO.Response.Ride;
using ShaRide.Application.Helpers;
using ShaRide.Application.Localize;
using ShaRide.Application.Managers;
using ShaRide.Application.ManualMapping;
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
        private readonly IUserRatingService _userRatingService;
        private readonly UserManager _userManager;
        private IOptions<FcmNotificationContract> _fcmNotificationContract;
        private IUserFcmTokenService _userFcmTokenService;

        public RideService(ApplicationDbContext dbContext, IStringLocalizer<Resource> localizer, IMapper mapper,
            ICarService carService, IAuthenticatedUserService authenticatedUserService, UserManager userManager,
            IUserRatingService userRatingService, IOptions<FcmNotificationContract> fcmNotificationContract, IUserFcmTokenService userFcmTokenService)
        {
            _dbContext = dbContext;
            _localizer = localizer;
            _mapper = mapper;
            _carService = carService;
            _authenticatedUserService = authenticatedUserService;
            _userManager = userManager;
            _userRatingService = userRatingService;
            _fcmNotificationContract = fcmNotificationContract;
            _userFcmTokenService = userFcmTokenService;
        }

        public ICollection<RideResponse> GetAllActiveRides()
        {
            var results = _dbContext.Rides
                .Include(x => x.Driver)
                .ThenInclude(x => x.Phones)
                .Include(x => x.RideCarSeatComposition)
                .ThenInclude(x => x.CarSeatComposition)
                .ThenInclude(x => x.Car)
                .Include(x => x.RideCarSeatComposition)
                .ThenInclude(x => x.CarSeatComposition)
                .ThenInclude(x => x.Seat)
                .Include(x => x.RideCarSeatComposition)
                .ThenInclude(x => x.Passenger)
                .Include(x => x.RideLocationPointComposition)
                .ThenInclude(x => x.LocationPoint)
                .ThenInclude(x => x.Location)
                .Include(x => x.RestrictionRideComposition)
                .ThenInclude(x => x.Restriction)
                .Where(x => x.RideState != RideState.Finished);

            var rides = results.RidesToRideResponses(_mapper);

            foreach (var rideResponse in rides)
            {
                rideResponse.Driver.Rating = _userRatingService.GetUserRating(rideResponse.Driver.Id);
                if (rideResponse.Car?.CarSeats.Any() ?? false)
                    foreach (var carSeatCompositionResponse in rideResponse.Car.CarSeats)
                    {
                        if (carSeatCompositionResponse.Passenger != null)
                            carSeatCompositionResponse.Passenger.Rating =
                                _userRatingService.GetUserRating(carSeatCompositionResponse.Passenger.Id);
                    }
            }

            return rides;
        }

        public ICollection<RideResponse> GetActiveRides(GetActiveRidesRequest request)
        {
            var results = _dbContext.Rides
                .Include(x => x.Driver)
                .ThenInclude(x => x.Phones)
                .Include(x => x.RideCarSeatComposition)
                .ThenInclude(x => x.CarSeatComposition)
                .ThenInclude(x => x.Seat)
                .Include(x => x.RideCarSeatComposition)
                .ThenInclude(x => x.Passenger)
                .Include(x => x.RideCarSeatComposition)
                .ThenInclude(x => x.CarSeatComposition)
                .ThenInclude(x => x.Car)
                .ThenInclude(x => x.CarModel)
                .ThenInclude(x => x.BanType)
                .Include(x => x.RideCarSeatComposition)
                .ThenInclude(x => x.CarSeatComposition)
                .ThenInclude(x => x.Car)
                .ThenInclude(x => x.CarModel)
                .ThenInclude(x => x.CarBrand)
                .Include(x => x.RideCarSeatComposition)
                .ThenInclude(x => x.CarSeatComposition)
                .ThenInclude(x => x.Car)
                .ThenInclude(x => x.CarImages)
                .Include(x => x.RideLocationPointComposition)
                .ThenInclude(x => x.LocationPoint)
                .ThenInclude(x => x.Location)
                .Include(x => x.RestrictionRideComposition)
                .ThenInclude(x => x.Restriction)
                .Where(x => x.RideLocationPointComposition
                                .Any(y => y.LocationPoint.LocationId == request.FromLocationId &&
                                          y.LocationPointType == LocationPointType.StartPoint)
                            && x.RideLocationPointComposition.Any(y =>
                                y.LocationPoint.LocationId == request.ToLocationId &&
                                y.LocationPointType == LocationPointType.FinishPoint) &&
                            x.RideState != RideState.Finished &&
                            x.StartDate.Date == request.Date.Date &&
                            (!request.BanTypeId.HasValue || x.RideCarSeatComposition.Any(y =>
                                y.CarSeatComposition.Car.CarModel.BanTypeId == request.BanTypeId)));

            var rides = results.RidesToRideResponses(_mapper);

            foreach (var rideResponse in rides)
            {
                rideResponse.Driver.Rating = _userRatingService.GetUserRating(rideResponse.Driver.Id);
                if (rideResponse.Car?.CarSeats.Any() ?? false)
                    foreach (var carSeatCompositionResponse in rideResponse.Car.CarSeats)
                    {
                        if (carSeatCompositionResponse.Passenger != null)
                            carSeatCompositionResponse.Passenger.Rating =
                                _userRatingService.GetUserRating(carSeatCompositionResponse.Passenger.Id);
                    }
            }

            return rides;
        }

        public async Task<int> InsertRide(InsertRideRequest request)
        {
            var ride = _mapper.Map<Ride>(request);

            ride.DriverId = DriverId(request);

            if (!request.CarId.HasValue || request.CarId.Equals(0))
            {
                var carInsertResult = await _carService.InsertCarAsync(request.Car);
                request.CarId = carInsertResult.Id;
                ride.RideCarSeatComposition = new List<RideCarSeatComposition>();
                var carSeats = carInsertResult.CarSeats.ToArray();
                for (int i = 0; i < request.Car.CarSeats.Count; i++)
                {
                    ride.RideCarSeatComposition.Add(new RideCarSeatComposition
                    {
                        CarSeatCompositionId = carSeats[i].Id,
                        SeatStatus = carSeats[i].SeatType
                    });
                }
            }

            await _dbContext.Rides.AddAsync(ride);

            await _dbContext.SaveChangesAsync();

            return 0;
        }

        public async Task<int> UpdateRideState(UpdateRideStateRequest request)
        {
            var ride = await _dbContext.Rides.AsTracking()
                .FirstOrDefaultAsync(x => x.IsRowActive && x.Id == request.RideId);

            if (ride is null)
                throw new ApiException(_localizer.GetString(LocalizationKeys.RIDE_NOT_FOUND, request.RideId));

            if (ride.DriverId != _authenticatedUserService.UserId)
                throw new ApiException(_localizer.GetString(LocalizationKeys.USER_HAS_NOT_ACCESS_OPERATION));

            ride.RideState = request.RideState;

            await _dbContext.SaveChangesAsync();

            return 0;
        }

        public async Task<int> AddPassengerToRide(AddPassengerToRideRequest request)
        {
            User user;
            
            if (!_userManager.TryGetUserById(_authenticatedUserService.UserId.Value, out user))
                throw new ApiException(_localizer.GetString(LocalizationKeys.NOT_FOUND,
                    _authenticatedUserService.UserId.Value));

            var passengerToRideRequests = request.RideCarSeatCompositionIds.Select(x =>
            {
                return new PassengerToRideRequest
                {
                    UserId = _authenticatedUserService.UserId.Value,
                    RideCarSeatCompositionId = x
                };
            });

            await _dbContext.PassengerToRideRequests.AddRangeAsync(passengerToRideRequests);
            
            await _dbContext.SaveChangesAsync();

            await Task.Run(async () =>
            {
                var userFcmTokens = _dbContext.UserFcmTokens.Where(x => x.IsRowActive);

                var rideLocationComposition = _dbContext.RideLocationPointComposition.Include(x => x.Ride)
                    .Include(x => x.LocationPoint).ThenInclude(x => x.Location).Where(x => x.RideId == request.RideId);

                var startLocationName = rideLocationComposition
                    .First(x => x.LocationPointType == LocationPointType.StartPoint).LocationPoint.Location.Name;

                var finishLocationName = rideLocationComposition
                    .First(x => x.LocationPointType == LocationPointType.FinishPoint).LocationPoint.Location.Name;

                var notificationBody =
                    $"{user.Name} {user.Surname} {startLocationName} - {finishLocationName} səyahətində {request.RideCarSeatCompositionIds.Count} oturacaq sifariş etmək istəyir.";

                var fcmContract = _fcmNotificationContract.Value;
                fcmContract.data = new FcmNotificationContract.Data("testClickAction", "1", "pending");
                fcmContract.notification = new FcmNotificationContract.Notification{body = notificationBody};
                fcmContract.registration_ids = userFcmTokens.Select(x => x.Token).ToList();

                await _userFcmTokenService.SendNotificationToUser(fcmContract);
            });
            
            return 0;
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