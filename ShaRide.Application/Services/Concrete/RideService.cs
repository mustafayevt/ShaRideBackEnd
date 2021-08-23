using AutoMapper;
using AutoWrapper.Wrappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using ShaRide.Application.Contexts;
using ShaRide.Application.DTO.Request.Account;
using ShaRide.Application.DTO.Request.Ride;
using ShaRide.Application.DTO.Request.UserFcmToken;
using ShaRide.Application.DTO.Response.Message;
using ShaRide.Application.DTO.Response.Ride;
using ShaRide.Application.Extensions;
using ShaRide.Application.Helpers;
using ShaRide.Application.Localize;
using ShaRide.Application.Managers;
using ShaRide.Application.ManualMapping;
using ShaRide.Application.Pagination;
using ShaRide.Application.Services.Interface;
using ShaRide.Domain.Entities;
using ShaRide.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

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
        private readonly IOptions<FcmNotificationContract> _fcmNotificationContract;
        private readonly IUserFcmTokenService _userFcmTokenService;
        private readonly IDateTimeService _dateTimeService;
        private readonly PaymentManager _paymentManager;

        public RideService(ApplicationDbContext dbContext, IStringLocalizer<Resource> localizer, IMapper mapper,
            ICarService carService, IAuthenticatedUserService authenticatedUserService, UserManager userManager,
            IUserRatingService userRatingService, IOptions<FcmNotificationContract> fcmNotificationContract,
            IUserFcmTokenService userFcmTokenService, IDateTimeService dateTimeService, PaymentManager paymentManager)
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
            _dateTimeService = dateTimeService;
            _paymentManager = paymentManager;
        }

        public async Task<ICollection<RideResponse>> GetAllActiveRides()
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
                .Where(x => x.RideState != RideState.Finished && x.RideState != RideState.Canceled);

            var rides = results.RidesToRideResponses(_mapper);

            foreach (var rideResponse in rides)
            {
                rideResponse.Driver.Rating = await _userRatingService.GetUserRating(rideResponse.Driver.Id);
                if (rideResponse.Car?.CarSeats.Any() ?? false)
                    foreach (var carSeatCompositionResponse in rideResponse.Car.CarSeats)
                    {
                        if (carSeatCompositionResponse.Passenger != null)
                            carSeatCompositionResponse.Passenger.Rating =
                                await _userRatingService.GetUserRating(carSeatCompositionResponse.Passenger.Id);
                    }
            }

            return rides;
        }

        /// <summary>
        /// Returns filtered Rides.
        /// </summary>
        /// <param name="ridesFilterRequest"></param>
        /// <returns></returns>
        /// <exception cref="ApiException"></exception>
        public async Task<PaginatedList<RideResponse>> GetRides(RidesFilterRequest ridesFilterRequest)
        {
            var ridesQuery = _dbContext.Rides
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
                .ThenInclude(x => x.Restriction).AsNoTracking().AsQueryable();

            if (ridesFilterRequest.FromLocationId.HasValue)
            {
                ridesQuery = ridesQuery.Where(x => x.RideLocationPointComposition
                    .Any(y => y.LocationPoint.LocationId == ridesFilterRequest.FromLocationId &&
                              y.LocationPointType == LocationPointType.StartPoint));
            }

            if (ridesFilterRequest.ToLocationId.HasValue)
            {
                ridesQuery = ridesQuery.Where(x => x.RideLocationPointComposition
                    .Any(y => y.LocationPoint.LocationId == ridesFilterRequest.ToLocationId &&
                              y.LocationPointType == LocationPointType.FinishPoint));
            }

            if (ridesFilterRequest.PricePerSeatFrom.HasValue)
            {
                ridesQuery = ridesQuery.Where(x => x.PricePerSeat >= ridesFilterRequest.PricePerSeatFrom);
            }

            if (ridesFilterRequest.PricePerSeatTo.HasValue)
            {
                ridesQuery = ridesQuery.Where(x => x.PricePerSeat <= ridesFilterRequest.PricePerSeatTo);
            }

            if (ridesFilterRequest.StartDateFrom.HasValue)
            {
                ridesQuery = ridesQuery.Where(x => x.StartDate >= ridesFilterRequest.StartDateFrom);
            }

            if (ridesFilterRequest.StartDateTo.HasValue)
            {
                ridesQuery = ridesQuery.Where(x => x.StartDate <= ridesFilterRequest.StartDateTo);
            }

            if (ridesFilterRequest.RideStates != null && ridesFilterRequest.RideStates.Count() > 0)
            {
                ridesQuery = ridesQuery.Where(x => ridesFilterRequest.RideStates.Contains(x.RideState));
            }

            if (ridesFilterRequest.BanTypeIds != null && ridesFilterRequest.BanTypeIds.Count() > 0)
            {
                ridesQuery = ridesQuery.Where(x => x.RideCarSeatComposition.Any(y =>
                                ridesFilterRequest.BanTypeIds.Contains(y.CarSeatComposition.Car.BanTypeId)));
            }

            if (ridesFilterRequest.DriverIds != null && ridesFilterRequest.DriverIds.Count() > 0)
            {
                ridesQuery = ridesQuery.Where(x => x.RideCarSeatComposition.Any(y =>
                                ridesFilterRequest.BanTypeIds.Contains(y.CarSeatComposition.Car.BanTypeId)));
            }

            var ridesFiltered = await ridesQuery.Skip((ridesFilterRequest.PageNumber - 1) * ridesFilterRequest.PageSize).Take(ridesFilterRequest.PageSize).ToListAsync();

            var ridesResponses = ridesFiltered.RidesToRideResponses(_mapper);

            foreach (var rideResponse in ridesResponses)
            {
                rideResponse.Driver.Rating = await _userRatingService.GetUserRating(rideResponse.Driver.Id);
                if (rideResponse.Car?.CarSeats.Any() ?? false)
                    foreach (var carSeatCompositionResponse in rideResponse.Car.CarSeats)
                    {
                        if (carSeatCompositionResponse.Passenger != null)
                            carSeatCompositionResponse.Passenger.Rating =
                                await _userRatingService.GetUserRating(carSeatCompositionResponse.Passenger.Id);
                    }
            }

            var ridesPaginated = new PaginatedList<RideResponse>
                (ridesResponses.ToList(), await ridesQuery.CountAsync(), ridesFilterRequest.PageNumber, ridesFilterRequest.PageSize);

            return ridesPaginated;
        }

        public async Task<ICollection<RideResponse>> GetActiveRides(GetActiveRidesRequest request)
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
                            x.RideState != RideState.Canceled &&
                            x.StartDate.Date == request.Date.Date &&
                            (!request.BanTypeId.HasValue || x.RideCarSeatComposition.Any(y =>
                                y.CarSeatComposition.Car.BanTypeId == request.BanTypeId))
                            && x.RideCarSeatComposition.Any(y => y.SeatStatus == SeatStatus.Suitable)
                            && x.DriverId != _authenticatedUserService.UserId);

            var rides = results.RidesToRideResponses(_mapper);

            foreach (var rideResponse in rides)
            {
                rideResponse.Driver.Rating = await _userRatingService.GetUserRating(rideResponse.Driver.Id);
                if (rideResponse.Car?.CarSeats.Any() ?? false)
                    foreach (var carSeatCompositionResponse in rideResponse.Car.CarSeats)
                    {
                        if (carSeatCompositionResponse.Passenger != null)
                            carSeatCompositionResponse.Passenger.Rating =
                                await _userRatingService.GetUserRating(carSeatCompositionResponse.Passenger.Id);
                    }
            }

            return rides;
        }

        public async Task<int> InsertRide(InsertRideRequest request)
        {
            var ride = _mapper.Map<Ride>(request);
            ride.StartDate = ride.StartDate;

            var driver = await GetUser(request);

            if (driver.Balance < -10m)
                throw new ApiException(_localizer.GetString(LocalizationKeys.USER_DOES_NOT_HAVE_ENOUGH_BALANCE));

            ride.DriverId = driver.Id;

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

            #region Ride location

            //Avoiding null reference.
            ride.RideLocationPointComposition ??= new List<RideLocationPointComposition>();

            foreach (var rideLocationPointComposition in request.RideLocationPoints)
            {
                if (rideLocationPointComposition.LocationPointId.HasValue)
                {
                    ride.RideLocationPointComposition.Add(new RideLocationPointComposition
                    {
                        LocationPointId = rideLocationPointComposition.LocationPointId.Value,
                        LocationPointType = rideLocationPointComposition.LocationPointType
                    });
                }
                else
                {
                    //Custom selection
                    if (rideLocationPointComposition.LocationPoint.Name.Equals("Xəritədən seçildi"))
                    {
                        LocationPoint locationPoint;
                        locationPoint = await _dbContext.LocationPoints.Include(x => x.Location)
                            .FirstOrDefaultAsync(x =>
                                x.IsRowActive && x.Latitude.Equals(rideLocationPointComposition.LocationPoint.Latitude) &&
                                x.Longitude.Equals(rideLocationPointComposition.LocationPoint.Longitude));

                        if (locationPoint == null)
                        {
                            ride.RideLocationPointComposition.Add(new RideLocationPointComposition
                            {
                                LocationPoint = _mapper.Map<LocationPoint>(rideLocationPointComposition.LocationPoint),
                                LocationPointType = rideLocationPointComposition.LocationPointType
                            });
                        }
                        else
                        {
                            ride.RideLocationPointComposition.Add(new RideLocationPointComposition
                            {
                                LocationPointId = locationPoint.Id,
                                LocationPointType = rideLocationPointComposition.LocationPointType
                            });
                        }
                    }
                    else
                    {
                        LocationPoint locationPoint;
                        locationPoint = await _dbContext.LocationPoints.Include(x => x.Location)
                            .FirstOrDefaultAsync(x =>
                                x.IsRowActive && x.Name.Equals(rideLocationPointComposition.LocationPoint.Name));

                        if (locationPoint == null)
                        {
                            ride.RideLocationPointComposition.Add(new RideLocationPointComposition
                            {
                                LocationPoint = _mapper.Map<LocationPoint>(rideLocationPointComposition.LocationPoint),
                                LocationPointType = rideLocationPointComposition.LocationPointType
                            });
                        }
                        else
                        {
                            ride.RideLocationPointComposition.Add(new RideLocationPointComposition
                            {
                                LocationPointId = locationPoint.Id,
                                LocationPointType = rideLocationPointComposition.LocationPointType
                            });
                        }
                    }
                }
            }

            #endregion

            await _dbContext.Rides.AddAsync(ride);

            await _dbContext.SaveChangesAsync();

            return 0;
        }

        public async Task<int> UpdateRideState(UpdateRideStateRequest request)
        {
            var ride = await _dbContext
                .Rides
                .Include(x => x.Driver)
                .Include(x => x.RideCarSeatComposition)
                .ThenInclude(x => x.Passenger)
                .AsTracking()
                .FirstOrDefaultAsync(x => x.IsRowActive && x.Id == request.RideId);

            if (ride is null)
                throw new ApiException(_localizer.GetString(LocalizationKeys.RIDE_NOT_FOUND, request.RideId));

            if (ride.DriverId != _authenticatedUserService.UserId)
                throw new ApiException(_localizer.GetString(LocalizationKeys.USER_HAS_NOT_ACCESS_OPERATION));

            ride.RideState = request.RideState;
            ride.RideStateChangeDatetime = _dateTimeService.AzerbaijanDateTime;

            //Payment stuff
            if (request.RideState.Equals(RideState.Finished))
                _paymentManager.ProcessPayment(ride);

            //Sending greeting messages to passengers.
            await Task.Run(async () =>
            {
                if (!ride.RideState.In(RideState.Finished, RideState.OnRoad))
                    return;

                var usersInRide = _dbContext
                    .RideCarSeatCompositions
                    .Include(x => x.Passenger)
                    .Where(x => x.IsRowActive && x.PassengerId.HasValue && x.RideId == ride.Id)
                    .ToList();

                var notificationsToUser = usersInRide
                    .Where(x => x.RideId == ride.Id)
                    .Select(x => x.Passenger)
                    .ToList();

                notificationsToUser.Add(ride.Driver);

                notificationsToUser = notificationsToUser.Distinct(x => x.Id).ToList();

                var userFcmTokens =
                    _dbContext.UserFcmTokens.Where(x => x.IsRowActive && notificationsToUser.Select(y => y.Id).Contains(x.UserId));

                if (!userFcmTokens.Any())
                    return;

                var notificationBody = string.Empty;
                var fcmContract = _fcmNotificationContract.Value;

                if (ride.RideState.Equals(RideState.OnRoad))
                {
                    fcmContract.data.ActionInApp = "RIDE_STARTED";
                    notificationBody = "Yaxşı yol!";
                }
                else if (ride.RideState.Equals(RideState.Finished))
                {
                    fcmContract.data.ActionInApp = "RIDE_HAS_FINISHED";
                    notificationBody = "Bizimlə səyahət etdiyiniz üçün təşəkkür edirik, xahiş olunur ki, səyahət iştirakçılarını dəyərləndirəsiniz.";
                }

                if (string.IsNullOrEmpty(notificationBody))
                    throw new NotImplementedException();


                fcmContract.notification = new FcmNotificationContract.Notification(notificationBody, fcmContract.data.Title);
                foreach (var userFcmToken in userFcmTokens)
                {
                    if (userFcmToken.UserId == ride.DriverId)
                        continue;

                    fcmContract.registration_ids = new List<string>() { userFcmToken.Token };
                    fcmContract.data.Model = new RateUserNotificationVm
                    {
                        RideId = ride.Id,
                        Participants = notificationsToUser.Where(x => x.Id != userFcmToken.UserId)
                            .Select(x => new RateUserNotificationVm.RateUserNotificationDetailVm
                            {
                                Type = ride.DriverId == x.Id ? MessageSenderType.Driver : MessageSenderType.Passenger,
                                UserFullname = $"{x.Name} {x.Surname}",
                                UserId = x.Id
                            }).ToList()
                    };
                    await _userFcmTokenService.SendNotificationToUser(fcmContract);
                }

            });

            await _dbContext.SaveChangesAsync();

            return 0;
        }

        public async Task<int> GiveFeedbackForRide(RideFeedbackRequest request)
        {
            var ride = await _dbContext
                .Rides
                .Include(x => x.Driver)
                .Include(x => x.RideCarSeatComposition)
                .ThenInclude(x => x.Passenger)
                .AsTracking()
                .FirstOrDefaultAsync(x => x.IsRowActive && x.Id == request.RideId);

            if (ride is null)
                throw new ApiException(_localizer.GetString(LocalizationKeys.RIDE_NOT_FOUND, request.RideId));

            if (ride.RideCarSeatComposition.Select(c => c.Passenger).All(p => p.Id != request.SenderId))
                throw new ApiException(_localizer.GetString(LocalizationKeys.USER_HAS_NOT_ACCESS_OPERATION));

            //Sending messages to passengers about feedback.
            await Task.Run(async () =>
            {
                var notificationsToUser = await _dbContext.RideCarSeatCompositions
                    .Where(x => x.RideId == ride.Id &&
                                x.IsRowActive &&
                                x.PassengerId.HasValue)
                    .Select(x => x.Passenger)
                    .ToListAsync();

                notificationsToUser.Add(ride.Driver);

                var sender = notificationsToUser.FirstOrDefault(u => u.Id == request.SenderId);

                notificationsToUser = notificationsToUser.Distinct(x => x.Id).Where(u => u.Id != request.SenderId).ToList();

                var userFcmTokens =
                    _dbContext.UserFcmTokens.Where(x => x.IsRowActive && notificationsToUser.Select(y => y.Id).Contains(x.UserId));

                if (!userFcmTokens.Any())
                    return;

                var fcmContract = _fcmNotificationContract.Value;

                fcmContract.data.ActionInApp = $"{sender.Name} {sender.Surname} sent feedback to your ride";
                var notificationBody = "Yaxşı yol!";

                if (string.IsNullOrEmpty(notificationBody))
                    throw new NotImplementedException();


                fcmContract.notification = new FcmNotificationContract.Notification(notificationBody, fcmContract.data.Title);
                foreach (var userFcmToken in userFcmTokens)
                {
                    if (userFcmToken.UserId == ride.DriverId)
                        continue;

                    fcmContract.registration_ids = new List<string>() { userFcmToken.Token };
                    fcmContract.data.Model = new RateUserNotificationVm
                    {
                        RideId = ride.Id,
                        Participants = notificationsToUser.Where(x => x.Id != userFcmToken.UserId)
                            .Select(x => new RateUserNotificationVm.RateUserNotificationDetailVm
                            {
                                Type = ride.DriverId == x.Id ? MessageSenderType.Driver : MessageSenderType.Passenger,
                                UserFullname = $"{x.Name} {x.Surname}",
                                UserId = x.Id
                            }).ToList()
                    };
                    await _userFcmTokenService.SendNotificationToUser(fcmContract);
                }

            });

            await _dbContext.SaveChangesAsync();

            return 0;
        }

        public async Task<int> AddPassengerToRide(AddPassengerToRideRequest request)
        {

            if (!_userManager.TryGetUserById(_authenticatedUserService.UserId.Value, out User user))
                throw new ApiException(_localizer.GetString(LocalizationKeys.NOT_FOUND,
                    _authenticatedUserService.UserId.Value));

            #region User balance rule checking

            var ridePricePerSeat = _dbContext.RideCarSeatCompositions
                .Include(x => x.Ride)
                .FirstOrDefault(x => x.IsRowActive && request.RideCarSeatCompositionIds.Contains(x.Id))
                ?.Ride.PricePerSeat;

            if (request.PaymentType.Equals(PaymentType.Balance))
            {
                var amountOfRequestedSeats = request.RideCarSeatCompositionIds.Count * ridePricePerSeat.Value;

                if (user.Balance < amountOfRequestedSeats)
                    throw new ApiException(_localizer.GetString(LocalizationKeys.USER_DOES_NOT_HAVE_ENOUGH_BALANCE));
            }

            #endregion

            #region Remove user all old requests

            var userOldRequests = _dbContext.PassengerToRideRequests.AsTracking().Include(x => x.RideCarSeatComposition).Where(x =>
                x.IsRowActive &&
                request.RideId == x.RideCarSeatComposition.RideId && x.UserId == _authenticatedUserService.UserId);

            foreach (var userOldRequest in userOldRequests)
            {
                if (userOldRequest.RequestStatus != PassengerToRideRequestStatus.WaitingForResponse)
                    continue;

                userOldRequest.IsRowActive = false;
            }

            #endregion

            #region Add new requests

            var passengerToRideRequests = request.RideCarSeatCompositionIds.Select(x =>
            {
                return new PassengerToRideRequest
                {
                    UserId = _authenticatedUserService.UserId.Value,
                    RideCarSeatCompositionId = x,
                    PaymentType = request.PaymentType
                };
            });

            await _dbContext.PassengerToRideRequests.AddRangeAsync(passengerToRideRequests);

            #endregion

            await _dbContext.SaveChangesAsync();

            //Sending notification to driver through FCM.
            await Task.Run(async () =>
            {
                var userFcmTokens = _dbContext.UserFcmTokens.Where(x => x.IsRowActive && x.UserId == request.DriverId);

                var rideLocationComposition = _dbContext.RideLocationPointComposition.Include(x => x.Ride)
                    .Include(x => x.LocationPoint).ThenInclude(x => x.Location).Where(x => x.RideId == request.RideId);

                var startLocationName = rideLocationComposition
                    .First(x => x.LocationPointType == LocationPointType.StartPoint).LocationPoint.Location.Name;

                var finishLocationName = rideLocationComposition
                    .First(x => x.LocationPointType == LocationPointType.FinishPoint).LocationPoint.Location.Name;

                var notificationBody =
                    $"{user.Name} {user.Surname} {startLocationName} - {finishLocationName} səyahətində {request.RideCarSeatCompositionIds.Count} oturacaq sifariş etmək istəyir.";

                var fcmContract = _fcmNotificationContract.Value;
                fcmContract.data.Body = notificationBody;
                fcmContract.data.Title = "ShaRide";
                fcmContract.data.ActionInApp = "ADD_PASSENGER_TO_RIDE";
                fcmContract.notification = new FcmNotificationContract.Notification(notificationBody, "ShaRide");
                fcmContract.registration_ids = userFcmTokens.Select(x => x.Token).ToList();

                await _userFcmTokenService.SendNotificationToUser(fcmContract);
            });

            return 0;
        }

        public async Task<int> RespondUserRideRequest(List<DriverRespondRequest> requests)
        {
            List<PassengerToRideRequest> updatedRequests = _dbContext.PassengerToRideRequests.AsTracking().Include(x => x.RideCarSeatComposition)
                .ThenInclude(x => x.Ride).Where(x => x.IsRowActive && requests.Select(driverRespondRequest => driverRespondRequest.RequestId).Contains(x.Id)).ToList();

            //Rule check for if there is any suitable seat for specific ride. 
            foreach (var passengerToRideRequest in updatedRequests)
            {
                if (_dbContext.RideCarSeatCompositions.Count(composition =>
                    composition.IsRowActive &&
                    composition.SeatStatus == SeatStatus.Suitable) < updatedRequests.Count(x =>
                    x.RideCarSeatComposition.RideId == passengerToRideRequest.RideCarSeatComposition.RideId))
                {
                    throw new ApiException("There is no suitable seats in this ride.");
                }
            }

            //this is core part. Changes status in PassengerToRideRequest table.
            foreach (var driverRespondRequest in requests)
            {
                var requestEntity =
                    updatedRequests.FirstOrDefault(x => x.Id == driverRespondRequest.RequestId);

                if (requestEntity is null)
                    throw new ApiException(new InvalidOperationException(), 400);

                requestEntity.RequestStatus = driverRespondRequest.RespondStatus;
            }

            if (requests.FirstOrDefault()?.RespondStatus == PassengerToRideRequestStatus.Confrimed)
                await AcceptRideRequest(updatedRequests);

            else if (requests.FirstOrDefault()?.RespondStatus == PassengerToRideRequestStatus.Rejected)
                await RejectRideRequest(updatedRequests);

            await _dbContext.SaveChangesAsync();

            return 0;
        }

        public async Task<IEnumerable<PassengerToRideResponse>> GetPassengerToRideRequests()
        {
            var requests = await _dbContext
                .PassengerToRideRequests
                .AsTracking()
                .Include(x => x.User)
                .Include(x => x.RideCarSeatComposition)
                .ThenInclude(x => x.CarSeatComposition)
                .ThenInclude(x => x.Seat)
                .Include(x => x.RideCarSeatComposition)
                .ThenInclude(x => x.Ride)
                .ThenInclude(x => x.RideLocationPointComposition)
                .ThenInclude(x => x.LocationPoint)
                .ThenInclude(x => x.Location)
                .Where(x => x.IsRowActive &&
                            x.RequestStatus == PassengerToRideRequestStatus.WaitingForResponse &&
                            x.RideCarSeatComposition.Ride.DriverId == _authenticatedUserService.UserId)
                .ToListAsync();

            //Assigning ride car compositions 
            requests.ForEach(x =>
            {

                var rideCarSeatComposition = _dbContext.RideCarSeatCompositions
                    .Include(y => y.CarSeatComposition)
                    .ThenInclude(y => y.Seat)
                    .Include(y => y.Ride)
                    .ThenInclude(y => y.RideLocationPointComposition)
                    .ThenInclude(y => y.LocationPoint)
                    .ThenInclude(y => y.Location)
                    .FirstOrDefault(y => y.RideId.Equals(x.RideCarSeatComposition.RideId));

                if (rideCarSeatComposition is null)
                    return;

                var car = _dbContext.Cars
                    .Include(y => y.CarSeatComposition)
                    .ThenInclude(y => y.Seat)
                    .FirstOrDefault(y => y.Id.Equals(x.RideCarSeatComposition.CarSeatComposition.CarId));

                x.RideCarSeatComposition.CarSeatComposition.Car = car;

            });

            var response = (requests).ToList().ToPassengerToRideResponse(_mapper).ToList();

            foreach (var passengerToRideResponse in response)
            {
                passengerToRideResponse.PassengerRequest.Passenger.Rating =
                    await _userRatingService.GetUserRating(passengerToRideResponse.PassengerRequest.Passenger.Id);
            }

            return response;
        }

        public async Task<int> CancelRide(CancelRideRequest request)
        {
            var ride = await _dbContext.Rides.AsTracking().FirstOrDefaultAsync(x => x.IsRowActive && x.Id.Equals(request.RideId));

            if (ride == null)
                throw new ApiException(_localizer[LocalizationKeys.RIDE_NOT_FOUND, request.RideId]);

            //ride date validation check.
            TimeSpan difference = ride.StartDate.Subtract(request.CancelDate);
            if (difference.Hours < 1)
                throw new ApiException(_localizer.GetString(LocalizationKeys.RIDE_CANNOT_CANCELLED));

            ride.RideState = RideState.Canceled;

            await _dbContext.SaveChangesAsync();

            await Task.Run(async () =>
            {
                var notificationsToUser = _dbContext
                    .RideCarSeatCompositions
                    .Include(x => x.Passenger)
                    .Where(x => x.IsRowActive && x.RideId == ride.Id && x.PassengerId.HasValue)
                    .Select(x => x.Passenger);

                var userFcmTokens =
                    _dbContext.UserFcmTokens.Where(x => x.IsRowActive && notificationsToUser.Select(y => y.Id).Contains(x.UserId));

                if (!userFcmTokens.Any()) return;

                var rideLocationComposition = _dbContext.RideLocationPointComposition.Include(x => x.Ride)
                    .Include(x => x.LocationPoint).ThenInclude(x => x.Location).Where(x =>
                        x.RideId == ride.Id);

                var startLocationName = rideLocationComposition
                    .First(x => x.LocationPointType == LocationPointType.StartPoint).LocationPoint.Location.Name;

                var finishLocationName = rideLocationComposition
                    .First(x => x.LocationPointType == LocationPointType.FinishPoint).LocationPoint.Location.Name;

                var notificationBody =
                    $"{ride.StartDate.ToString("F", new CultureInfo("az-AZ"))} tarixli {startLocationName} - {finishLocationName} səyahəti sürücü tərəfindən ləğv edildi.";

                var fcmContract = _fcmNotificationContract.Value;
                fcmContract.notification = new FcmNotificationContract.Notification(notificationBody, "ShaRide");
                fcmContract.data.Body = notificationBody;
                fcmContract.data.Title = "ShaRide";
                fcmContract.data.ActionInApp = "ADD_PASSENGER_TO_RIDE";
                fcmContract.registration_ids = userFcmTokens.Select(x => x.Token).ToList();

                await _userFcmTokenService.SendNotificationToUser(fcmContract);
            });

            return 0;
        }

        public async Task<ICollection<RideResponse>> GetCurrentUsersRidesAsDriver()
        {
            var dbResult = _dbContext
                .Rides
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
                .Where(x => x.IsRowActive &&
                            x.DriverId == _authenticatedUserService.UserId &&
                            x.RideState != RideState.Canceled)
                .OrderByDescending(x => x.StartDate)
                .Take(50);

            var rides = dbResult.RidesToRideResponses(_mapper);

            foreach (var rideResponse in rides)
            {
                rideResponse.Driver.Rating = await _userRatingService.GetUserRating(rideResponse.Driver.Id);
                if (rideResponse.Car?.CarSeats != null)
                    foreach (var carSeatCompositionResponse in rideResponse.Car.CarSeats)
                    {
                        if (carSeatCompositionResponse.Passenger is null)
                            continue;

                        carSeatCompositionResponse.Passenger.Rating =
                            await _userRatingService.GetUserRating(carSeatCompositionResponse.Passenger.Id);
                    }
            }

            return rides;
        }

        public async Task<ICollection<RideResponse>> GetCurrentUserRidesAsPassenger()
        {
            var dbResult = _dbContext
                .Rides
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
                .Where(x => x.IsRowActive &&
                            x.RideCarSeatComposition.Any(y => y.PassengerId.Equals(_authenticatedUserService.UserId)) &&
                            x.RideState != RideState.Canceled)
                .OrderByDescending(x => x.StartDate)
                .Take(50);

            var rides = dbResult.RidesToRideResponses(_mapper);

            foreach (var rideResponse in rides)
            {
                rideResponse.Driver.Rating = await _userRatingService.GetUserRating(rideResponse.Driver.Id);
                foreach (var carSeatCompositionResponse in rideResponse.Car.CarSeats)
                {
                    if (carSeatCompositionResponse.Passenger is null)
                        continue;

                    carSeatCompositionResponse.Passenger.Rating = await _userRatingService.GetUserRating(carSeatCompositionResponse.Passenger.Id);
                }
            }

            return rides;
        }

        public async Task<ICollection<GetUserRideRequestResponse>> GetCurrentUsersRideRequests()
        {
            var dbResult = await _dbContext
                .PassengerToRideRequests
                .Include(x => x.RideCarSeatComposition)
                .ThenInclude(x => x.Ride)
                .ThenInclude(x => x.RideLocationPointComposition)
                .ThenInclude(x => x.LocationPoint)
                .ThenInclude(x => x.Location)
                .Where(x => x.UserId.Equals(_authenticatedUserService.UserId)
                            && x.IsRowActive
                            && x.RideCarSeatComposition.Ride.RideState != RideState.Canceled)
                .ToListAsync();

            var requests = dbResult.GroupBy(y => new { y.RideCarSeatComposition.Ride.Id, y.RequestStatus }).Select(x =>
              {
                  var passengerToRideRequest = x.Select(y => y).First();
                  return new GetUserRideRequestResponse
                  {
                      Ride = _mapper.Map<RideResponse>(passengerToRideRequest.RideCarSeatComposition.Ride),
                      OrderCount = x.Count(),
                      OrderTime = passengerToRideRequest.Date.AddHours(4),
                      RequestStatus = passengerToRideRequest.RequestStatus,
                      SumAmountOfOrders = passengerToRideRequest.RideCarSeatComposition.Ride.PricePerSeat * x.Count()
                  };
              });

            return requests.ToList();
        }

        public async Task<int> CancelPassengerRideRequest(int rideId)
        {
            var ride = await _dbContext
                .Rides
                .AsTracking()
                .Include(x => x.Driver)
                .Include(x => x.RideCarSeatComposition)
                .ThenInclude(x => x.CarSeatComposition)
                .FirstOrDefaultAsync(x => x.IsRowActive && x.Id == rideId);

            if (ride == null)
                throw new ApiException(_localizer.GetString(LocalizationKeys.RIDE_NOT_FOUND, rideId));

            var soldSeats =
                ride.RideCarSeatComposition.Where(x => x.PassengerId.Equals(_authenticatedUserService.UserId));

            var rideCarSeatCompositions = ride.RideCarSeatComposition.ToList();

            var passengerRideRequests = _dbContext.PassengerToRideRequests.AsTracking().Where(x =>
                x.IsRowActive && rideCarSeatCompositions.Select(y => y.CarSeatCompositionId).Contains(x.RideCarSeatComposition.CarSeatCompositionId) &&
                x.RideCarSeatComposition.RideId.Equals(ride.Id));

            foreach (var passengerToRideRequest in passengerRideRequests)
            {
                passengerToRideRequest.IsRowActive = false;
            }

            foreach (var rideCarSeatComposition in soldSeats)
            {
                rideCarSeatComposition.PassengerId = null;
                rideCarSeatComposition.SeatStatus = SeatStatus.Suitable;
            }

            await _dbContext.SaveChangesAsync();

            //Sending notification to driver through FCM.
            await Task.Run(async () =>
            {
                var notificationToUser = ride.Driver;

                var currentUser = await _userManager.GetCurrentUser();

                var userFcmTokens =
                    _dbContext.UserFcmTokens.Where(x => x.IsRowActive && notificationToUser.Id == x.UserId);

                var rideLocationComposition = _dbContext.RideLocationPointComposition.Include(x => x.Ride)
                    .Include(x => x.LocationPoint).ThenInclude(x => x.Location).Where(x =>
                        x.RideId == ride.Id);

                var startLocationName = rideLocationComposition
                    .First(x => x.LocationPointType == LocationPointType.StartPoint).LocationPoint.Location.Name;

                var finishLocationName = rideLocationComposition
                    .First(x => x.LocationPointType == LocationPointType.FinishPoint).LocationPoint.Location.Name;

                var notificationBody =
                    $"Sərnişin {currentUser.Name} {currentUser.Surname}, {startLocationName} - {finishLocationName} səyahəti üçün verdiyi sifarişdən imtina etdi.";

                var fcmContract = _fcmNotificationContract.Value;
                fcmContract.notification = new FcmNotificationContract.Notification(notificationBody, "ShaRide");
                fcmContract.data.Body = notificationBody;
                fcmContract.data.Title = "ShaRide";
                fcmContract.data.ActionInApp = "CANCEL_ORDER";
                fcmContract.registration_ids = userFcmTokens.Select(x => x.Token).ToList();

                await _userFcmTokenService.SendNotificationToUser(fcmContract);
            });

            return 0;
        }

        public async Task<ICollection<RideResponse>> SuggestRidesToUser()
        {
            var currentUser = await _userManager.GetCurrentUser();

            var userLastRide = _dbContext.Rides
                .Include(x => x.RideCarSeatComposition)
                .Include(x => x.RideLocationPointComposition)
                .ThenInclude(x => x.LocationPoint)
                .ThenInclude(x => x.Location)
                .OrderByDescending(y => y.StartDate)
                .FirstOrDefault(x => x.RideCarSeatComposition.Any(y => y.PassengerId.Equals(currentUser.Id)));

            var userStartLocation = userLastRide?.RideLocationPointComposition?.SingleOrDefault(x => x.LocationPointType == LocationPointType.StartPoint);
            var userFinishLocation = userLastRide?.RideLocationPointComposition?.SingleOrDefault(x => x.LocationPointType == LocationPointType.FinishPoint);

            if (userLastRide is null)
                throw new ApiException(_localizer.GetString(LocalizationKeys.NOT_FOUND, currentUser.UserUniqueKey));

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
                                .Any(y => y.LocationPoint.LocationId == userFinishLocation.LocationPoint.LocationId &&
                                          y.LocationPointType == LocationPointType.StartPoint)
                            && x.RideLocationPointComposition.Any(y =>
                                y.LocationPoint.LocationId == userStartLocation.LocationPoint.LocationId &&
                                y.LocationPointType == LocationPointType.FinishPoint) &&
                            x.RideState != RideState.Finished &&
                            x.RideState != RideState.Canceled &&
                            x.StartDate.Date > userLastRide.StartDate.Date &&
                            (x.RideCarSeatComposition.Any(y =>
                                y.SeatStatus == SeatStatus.Suitable)));

            var rides = results.RidesToRideResponses(_mapper);

            foreach (var rideResponse in rides)
            {
                rideResponse.Driver.Rating = await _userRatingService.GetUserRating(rideResponse.Driver.Id);
                if (rideResponse.Car?.CarSeats.Any() ?? false)
                    foreach (var carSeatCompositionResponse in rideResponse.Car.CarSeats)
                    {
                        if (carSeatCompositionResponse.Passenger != null)
                            carSeatCompositionResponse.Passenger.Rating =
                                await _userRatingService.GetUserRating(carSeatCompositionResponse.Passenger.Id);
                    }
            }

            return rides;
        }

        public async Task<ICollection<RideResponse>> SuggestRidesToUserBasedOnUserFavoriteRoute()
        {
            var currentUser = await _userManager.GetCurrentUser();

            var userLastRide = _dbContext.Rides
                .Include(x => x.RideCarSeatComposition)
                .Include(x => x.RideLocationPointComposition)
                .ThenInclude(x => x.LocationPoint)
                .ThenInclude(x => x.Location)
                .Where(x => x.RideCarSeatComposition.Any(y => y.PassengerId.Equals(currentUser.Id)))
                .GroupBy(x => new
                {
                    StartPointId = x.RideLocationPointComposition
                        .First(y => y.LocationPointType == LocationPointType.StartPoint).LocationPoint.Location.Id,
                    FinishPointId = x.RideLocationPointComposition
                        .First(y => y.LocationPointType == LocationPointType.FinishPoint).LocationPoint.Location.Id,
                })
                .OrderByDescending(x => x.Count())
                .Select(x => x.Key)
                .FirstOrDefault();

            if (userLastRide is null)
                throw new ApiException(_localizer.GetString(LocalizationKeys.NOT_FOUND));

            var userMostVisitedStartLocationId = userLastRide.StartPointId;
            var userMostVisitedFinishLocationId = userLastRide.FinishPointId;

            if (userLastRide is null)
                throw new ApiException(_localizer.GetString(LocalizationKeys.NOT_FOUND, currentUser.UserUniqueKey));

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
                .Where(x =>
                    x.DriverId != currentUser.Id &&
                    x.RideLocationPointComposition
                                .Any(y => y.LocationPoint.LocationId == userMostVisitedStartLocationId &&
                                          y.LocationPointType == LocationPointType.StartPoint)
                            && x.RideLocationPointComposition.Any(y =>
                                y.LocationPoint.LocationId == userMostVisitedFinishLocationId &&
                                y.LocationPointType == LocationPointType.FinishPoint) &&
                            x.RideState != RideState.Finished &&
                            x.RideState != RideState.Canceled &&
                            (x.RideCarSeatComposition.Any(y =>
                                y.SeatStatus == SeatStatus.Suitable)));

            var rides = results.RidesToRideResponses(_mapper);

            foreach (var rideResponse in rides)
            {
                rideResponse.Driver.Rating = await _userRatingService.GetUserRating(rideResponse.Driver.Id);
                if (rideResponse.Car?.CarSeats.Any() ?? false)
                    foreach (var carSeatCompositionResponse in rideResponse.Car.CarSeats)
                    {
                        if (carSeatCompositionResponse.Passenger != null)
                            carSeatCompositionResponse.Passenger.Rating =
                                await _userRatingService.GetUserRating(carSeatCompositionResponse.Passenger.Id);
                    }
            }

            return rides;
        }

        public Task<List<Ride>> GetRidesForNotificationByDateTime(DateTime rideStarTime)
        {
            var dateTimeNow = DateTime.Now.ToAzerbaijanDateTime();

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
                .Where(x =>
                            x.RideState != RideState.Finished &&
                            x.RideState != RideState.Canceled &&
                            x.StartDate >= dateTimeNow &&
                            x.StartDate <= rideStarTime)
                .ToList();

            return Task.FromResult(results);
        }

        /// <summary>
        /// Sends notification to all users in the passed ride.
        /// </summary>
        /// <param name="ride"></param>
        /// <param name="notificationBody"></param>
        /// <returns></returns>
        public async Task SendNotificationsToUsersInRide(Ride ride, string notificationBody)
        {
            var notificationsToUser = ride.RideCarSeatComposition.Select(x => x.PassengerId).ToList();
            var driverId = ride.DriverId;

            if (!notificationsToUser.Any())
                return;

            var userFcmTokens =
                _dbContext.UserFcmTokens.Where(x =>
                        x.IsRowActive && notificationsToUser.Contains(x.UserId) || x.UserId.Equals(driverId)).ToList()
                    .Distinct(y => y.Token);

            var fcmContract = _fcmNotificationContract.Value;
            fcmContract.notification = new FcmNotificationContract.Notification(notificationBody, "ShaRide");
            fcmContract.data.Body = notificationBody;
            fcmContract.data.Title = "ShaRide";
            fcmContract.data.ActionInApp = "GET_READY";
            fcmContract.registration_ids = userFcmTokens.Select(x => x.Token).ToList();

            await _userFcmTokenService.SendNotificationToUser(fcmContract);
        }

        public async Task<int> DeactivateUserRequest(int requestId)
        {
            var request = _dbContext.PassengerToRideRequests
                .AsTracking()
                .FirstOrDefault(x => x.IsRowActive && x.Id.Equals(requestId));

            if (request is null)
                throw new ApiException(_localizer.GetString(LocalizationKeys.NOT_FOUND, requestId));

            if (request.UserId != _authenticatedUserService.UserId)
                throw new ApiException(_localizer.GetString(LocalizationKeys.USER_HAS_NOT_ACCESS_OPERATION));

            if (request.RequestStatus == PassengerToRideRequestStatus.WaitingForResponse)
                throw new ApiException(_localizer.GetString(LocalizationKeys.USER_HAS_NOT_ACCESS_OPERATION));

            request.IsRowActive = false;
            await _dbContext.SaveChangesAsync();

            return 0;
        }

        /// <summary>
        /// Returns user by ride request for driver registration.
        /// </summary>
        /// <param name="rideRequest"></param>
        /// <returns></returns>
        /// <exception cref="ApiException"></exception>
        protected async Task<User> GetUser(InsertRideRequest rideRequest)
        {
            User user;

            if (rideRequest.DriverId.HasValue && rideRequest.DriverId != _authenticatedUserService.UserId)
            {
                if (!_userManager.TryGetUserById(rideRequest.DriverId.Value, out user))
                {
                    throw new ApiException(_localizer[LocalizationKeys.NOT_FOUND, $"Driver - {rideRequest.DriverId}"]);
                }
            }
            else
            {
                user = await _userManager.GetCurrentUser();
            }

            return user;
        }

        /// <summary>
        /// Accepts users ride request after pass rule checking.
        /// </summary>
        /// <param name="updatedRequests"></param>
        /// <returns></returns>
        private async Task AcceptRideRequest(List<PassengerToRideRequest> updatedRequests)
        {
            updatedRequests.ForEach(x =>
            {
                x.RideCarSeatComposition.SeatStatus = SeatStatus.Sold;
                x.RideCarSeatComposition.PassengerId = x.UserId;
                x.RideCarSeatComposition.PaymentType = x.PaymentType;
            });

            //Remove rejected requests.
            await RejectOtherRideRequests(updatedRequests);

            //Sending notification to passenger through FCM.
            await Task.Run(async () =>
            {
                var notificationsToUser = updatedRequests.Select(x => x.RideCarSeatComposition.PassengerId).ToList();

                if (!notificationsToUser.Any())
                    return;

                var userFcmTokens =
                    _dbContext.UserFcmTokens.Where(x => x.IsRowActive && notificationsToUser.First() == x.UserId);

                var rideLocationComposition = _dbContext.RideLocationPointComposition.Include(x => x.Ride)
                    .Include(x => x.LocationPoint).ThenInclude(x => x.Location).Where(x =>
                        x.RideId == updatedRequests.FirstOrDefault().RideCarSeatComposition.RideId);

                var startLocationName = rideLocationComposition
                    .First(x => x.LocationPointType == LocationPointType.StartPoint).LocationPoint.Location.Name;

                var finishLocationName = rideLocationComposition
                    .First(x => x.LocationPointType == LocationPointType.FinishPoint).LocationPoint.Location.Name;

                var notificationBody =
                    $"Sürücü {startLocationName} - {finishLocationName} səyahəti üçün verdiyiniz sifarişi qəbul etdi.";

                var fcmContract = _fcmNotificationContract.Value;
                fcmContract.notification = new FcmNotificationContract.Notification(notificationBody, "ShaRide");
                fcmContract.data.Body = notificationBody;
                fcmContract.data.Title = "ShaRide";
                fcmContract.data.ActionInApp = "ADD_PASSENGER_TO_RIDE";
                fcmContract.registration_ids = userFcmTokens.Select(x => x.Token).ToList();

                await _userFcmTokenService.SendNotificationToUser(fcmContract);
            });
        }

        /// <summary>
        /// Rejects other users request in same seat in ride.
        /// </summary>
        /// <param name="updatedRequests"></param>
        /// <returns></returns>
        private async Task RejectOtherRideRequests(List<PassengerToRideRequest> updatedRequests)
        {
            //This implementation is considered for single request. for multiple requests need to change this method as well.
            if (updatedRequests.FirstOrDefault()?.RequestStatus != PassengerToRideRequestStatus.Confrimed)
                return;

            var rejectedRequests = _dbContext.PassengerToRideRequests.AsTracking().Include(x => x.RideCarSeatComposition)
                .ToList().AsEnumerable().Where(x =>
                    x.IsRowActive && updatedRequests.Count(y =>
                        y.RideCarSeatComposition.RideId == x.RideCarSeatComposition.RideId &&
                        y.RideCarSeatComposition.CarSeatCompositionId ==
                        x.RideCarSeatComposition.CarSeatCompositionId) > 0 &&
                    x.RequestStatus == PassengerToRideRequestStatus.WaitingForResponse).ToList();

            rejectedRequests.ForEach(x => x.IsRowActive = false);

            //Sending notification to driver through FCM.
            await Task.Run(async () =>
            {
                var notificationsToUser = rejectedRequests.Select(x => x.UserId).ToList();

                if (!notificationsToUser.Any())
                    return;

                var userFcmTokens =
                    _dbContext.UserFcmTokens.Where(x => x.IsRowActive && notificationsToUser.Contains(x.UserId));

                var rideLocationComposition = _dbContext.RideLocationPointComposition.Include(x => x.Ride)
                    .Include(x => x.LocationPoint).ThenInclude(x => x.Location).Where(x =>
                        x.RideId == updatedRequests.FirstOrDefault().RideCarSeatComposition.RideId);

                var startLocationName = rideLocationComposition
                    .First(x => x.LocationPointType == LocationPointType.StartPoint).LocationPoint.Location.Name;

                var finishLocationName = rideLocationComposition
                    .First(x => x.LocationPointType == LocationPointType.FinishPoint).LocationPoint.Location.Name;

                var notificationBody =
                    $"{startLocationName} - {finishLocationName} səyahəti üçün verdiyiniz sifariş sistem tərəfindən ləğv olundu.";

                var fcmContract = _fcmNotificationContract.Value;
                fcmContract.data.Body = notificationBody;
                fcmContract.data.Title = "ShaRide";
                fcmContract.data.ActionInApp = "ADD_PASSENGER_TO_RIDE";
                fcmContract.notification = new FcmNotificationContract.Notification(notificationBody, "ShaRide");
                fcmContract.registration_ids = userFcmTokens.Select(x => x.Token).ToList();

                await _userFcmTokenService.SendNotificationToUser(fcmContract);
            });
        }

        /// <summary>
        /// Rejects users ride request after pass rule checking
        /// </summary>
        /// <param name="updatedRequests"></param>
        /// <returns></returns>
        private async Task RejectRideRequest(List<PassengerToRideRequest> updatedRequests)
        {
            updatedRequests.ForEach(x =>
            {
                x.RequestStatus = PassengerToRideRequestStatus.Rejected;
            });

            //Sending notification to driver through FCM.
            await Task.Run(async () =>
            {
                var notificationsToUser = updatedRequests.Select(x => x.UserId);

                var userFcmTokens =
                    _dbContext.UserFcmTokens.Where(x => x.IsRowActive && notificationsToUser.First() == x.UserId);

                var rideLocationComposition = _dbContext.RideLocationPointComposition.Include(x => x.Ride)
                    .Include(x => x.LocationPoint).ThenInclude(x => x.Location).Where(x =>
                        x.RideId == updatedRequests.FirstOrDefault().RideCarSeatComposition.RideId);

                var startLocationName = rideLocationComposition
                    .First(x => x.LocationPointType == LocationPointType.StartPoint).LocationPoint.Location.Name;

                var finishLocationName = rideLocationComposition
                    .First(x => x.LocationPointType == LocationPointType.FinishPoint).LocationPoint.Location.Name;

                var notificationBody =
                    $"Sürücü {startLocationName} - {finishLocationName} səyahəti üçün verdiyiniz sifarişdən imtina etdi.";

                var fcmContract = _fcmNotificationContract.Value;
                fcmContract.data.Body = notificationBody;
                fcmContract.data.Title = "ShaRide";
                fcmContract.data.ActionInApp = "ADD_PASSENGER_TO_RIDE";
                fcmContract.notification = new FcmNotificationContract.Notification(notificationBody, "ShaRide");
                fcmContract.registration_ids = userFcmTokens.Select(x => x.Token).ToList();

                await _userFcmTokenService.SendNotificationToUser(fcmContract);
            });
        }
    }
}