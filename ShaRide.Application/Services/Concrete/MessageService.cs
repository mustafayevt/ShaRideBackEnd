﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoWrapper.Wrappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ShaRide.Application.Contexts;
using ShaRide.Application.DTO.Request.Message;
using ShaRide.Application.DTO.Request.UserFcmToken;
using ShaRide.Application.DTO.Response;
using ShaRide.Application.DTO.Response.Account;
using ShaRide.Application.DTO.Response.Message;
using ShaRide.Application.Extensions;
using ShaRide.Application.Helpers;
using ShaRide.Application.Localize;
using ShaRide.Application.Services.Interface;
using ShaRide.Domain.Entities;
using ShaRide.Domain.Enums;

namespace ShaRide.Application.Services.Concrete
{
    public class MessageService : IMessageService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IUserFcmTokenService _fcmTokenService;
        private readonly IStringLocalizer _localizer;
        private readonly IMapper _mapper;
        private readonly IOptions<FcmNotificationContract> _fcmNotificationContract;
        private readonly IUserRatingService _userRatingService;
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly IDateTimeService _dateTimeService;

        public MessageService(ApplicationDbContext dbContext, IUserFcmTokenService fcmTokenService, IStringLocalizer<Resource> localizer, IMapper mapper, IOptions<FcmNotificationContract> fcmNotificationContract, IUserRatingService userRatingService, IAuthenticatedUserService authenticatedUserService, IDateTimeService dateTimeService)
        {
            _dbContext = dbContext;
            _fcmTokenService = fcmTokenService;
            _localizer = localizer;
            _mapper = mapper;
            _fcmNotificationContract = fcmNotificationContract;
            _userRatingService = userRatingService;
            _authenticatedUserService = authenticatedUserService;
            _dateTimeService = dateTimeService;
        }

        public async Task<int> InsertMessage(InsertMessageRequest request)
        {
            var ride = await _dbContext.Rides.Include(x=>x.RideCarSeatComposition).FirstOrDefaultAsync(x=>x.IsRowActive && x.Id.Equals(request.RideId));

            if (ride is null)
                throw new ApiException(_localizer.GetString(LocalizationKeys.RIDE_NOT_FOUND, request.RideId));

            var messageEntity = _mapper.Map<Message>(request);

            await _dbContext.Messages.AddAsync(messageEntity);

            await _dbContext.SaveChangesAsync();

            //After inserting message to our db, need to send message to other passenger/driver as well.
            await Task.Run(async () =>
            {
                var notificationsToUser = _dbContext
                    .RideCarSeatCompositions
                    .Include(x=>x.Passenger)
                    .Where(x=>x.IsRowActive && x.RideId == ride.Id && x.PassengerId.HasValue)
                    .Select(x=>x.Passenger);

                var userFcmTokens =
                    _dbContext.UserFcmTokens.Where(x => x.IsRowActive && notificationsToUser.Select(y=>y.Id).Contains(x.UserId));

                if (!userFcmTokens.Any()) return;

                //Loading sender user from db.
                await _dbContext.Attach(messageEntity).Reference(x => x.CreatedByUser).LoadAsync();
                var senderFullname = messageEntity.CreatedByUser.Name + " " + messageEntity.CreatedByUser.Surname;
                var senderRating = await _userRatingService.GetUserRating(messageEntity.CreatedByUser.Id);

                MessageToUsersVm messageToUsersVm = new MessageToUsersVm(messageEntity.Id, senderFullname, senderRating,
                    messageEntity.Content, messageEntity.MessageType, messageEntity.SenderType,
                    messageEntity.CreatedTimestamp.AddHours(-4));

                var notificationBody = JsonConvert.SerializeObject(messageToUsersVm);

                var fcmContract = _fcmNotificationContract.Value;
                fcmContract.data.click_action = "MESSAGE_NOTIFICATION_CLICK";
                fcmContract.data.id = "3";
                fcmContract.notification = new FcmNotificationContract.Notification {body = notificationBody};
                fcmContract.registration_ids = userFcmTokens.Select(x => x.Token).ToList();

                await _fcmTokenService.SendNotificationToUser(fcmContract);
            });
            
            return 0;
        }

        public async Task<ICollection<GetMessageGroupVm>> GetCurrentUserMessageGroups()
        {
            var rides =  await _dbContext
                .Rides
                .Include(x => x.RideCarSeatComposition)
                .ThenInclude(x => x.Passenger)
                .Include(x => x.Driver)
                .Include(x=>x.RideLocationPointComposition)
                .ThenInclude(x=>x.LocationPoint)
                .ThenInclude(x=>x.Location)
                .Where(x => x.IsRowActive && (x.DriverId.Equals(_authenticatedUserService.UserId.Value) ||
                                                x.RideCarSeatComposition.Any(y => y.PassengerId.Equals(_authenticatedUserService.UserId.Value)
                                                && (x.RideState.Equals(RideState.Finished) && x.RideStateChangeDatetime != null && (x.RideStateChangeDatetime.Value < _dateTimeService.AzerbaijanDateTime.AddDays(-2)))
                                                || (x.RideState != RideState.Canceled)
                                              )))
                .ToListAsync();
            
            foreach (var ride in rides)
            {
                ride.RideCarSeatComposition = ride.RideCarSeatComposition.Where(x => x.PassengerId.HasValue).ToList();
            }

            var rideUserIds = rides.SelectMany(x => x.RideCarSeatComposition.Select(y => y.PassengerId)).Where(y=>y.HasValue).Select(x=>(int)x).ToList();
            rideUserIds.AddRange(rides.Select(ride => ride.DriverId));

            // key - userId : value - user rating.
            var userRatingsPair = _userRatingService.GetUserRating(rideUserIds.ToArray()).ToList();

            var response = rides.Select(x => new GetMessageGroupVm
            {
                RideId = x.Id,
                RideDateTime = x.StartDate.ToCustomFormat(),
                RideStartPoint = x.RideLocationPointComposition
                    .First(x => x.LocationPointType.Equals(LocationPointType.StartPoint)).LocationPoint.Location.Name,
                RideEndPoint = x.RideLocationPointComposition
                    .First(x => x.LocationPointType.Equals(LocationPointType.FinishPoint)).LocationPoint.Location.Name,
                Participants = x.RideCarSeatComposition.Select(y => new RideParticipantVm
                {
                    UserId = y.Passenger.Id,
                    UserFullname = y.Passenger.Name + " " + y.Passenger.Surname,
                    UserRating = userRatingsPair.First(j=>j.Key.Equals(y.PassengerId.Value)).Value,
                }).Distinct(h=>h.UserId).ToList(),
                Messages = _dbContext.Messages.Include(x=>x.CreatedByUser).Where(h=>h.IsRowActive && h.RideId.Equals(x.Id)).ToList().Select(j=>new MessageResponse
                {
                    Content = j.Content,
                    MessageDatetime = j.CreatedTimestamp.ToCustomFormat(),
                    MessageType = j.MessageType,
                    SenderType = j.SenderType,
                    SenderId = j.CreatedByUserId,
                    SenderRating = userRatingsPair.First(h=>h.Key.Equals(j.CreatedByUserId)).Value,
                    SenderFullname = j.CreatedByUser.Name + " " + j.CreatedByUser.Surname
                }).ToList()
            }).ToList();

            //Adding driver as participant
            for (int i = 0; i < response.Count(); i++)
            {
                response[i].Participants.Add(new RideParticipantVm
                {
                    UserId = rides[i].Driver.Id,
                    UserFullname = rides[i].Driver.Name + " " + rides[i].Driver.Surname,
                    UserRating = userRatingsPair.First(h=>h.Key.Equals(rides[i].DriverId)).Value,
                });
                response[i].Participants = response[i].Participants.Reverse().ToList();
            }

            return response;
        }

        public Task<GetMessageGroupVm> GetCurrentUserMessageGroup(int rideId)
        {
            var messageGroup = this.GetCurrentUserMessageGroups().Result.FirstOrDefault(x => x.RideId.Equals(rideId));

            return Task.FromResult(messageGroup);
        }
    }
}