using AutoMapper;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using ShaRide.Application.Contexts;
using ShaRide.Application.DTO.Request.Message;
using ShaRide.Application.DTO.Request.UserFcmToken;
using ShaRide.Application.Localize;
using ShaRide.Application.Services.Interface;
using System.Threading.Tasks;

namespace ShaRide.Application.Services.Concrete
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IUserFcmTokenService _fcmTokenService;
        private readonly IStringLocalizer _localizer;
        private readonly IMapper _mapper;
        private readonly IOptions<FcmNotificationContract> _fcmNotificationContract;
        private readonly IUserRatingService _userRatingService;
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly IDateTimeService _dateTimeService;

        public NotificationService(ApplicationDbContext dbContext,
            IUserFcmTokenService fcmTokenService,
            IStringLocalizer<Resource> localizer,
            IMapper mapper,
            IOptions<FcmNotificationContract> fcmNotificationContract,
            IUserRatingService userRatingService,
            IAuthenticatedUserService authenticatedUserService,
            IDateTimeService dateTimeService)
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

        public async Task<bool> SendNotification(SendNotificationRequest request)
        {
            return true;
            //var ride = await _dbContext
            //    .Rides
            //    .Include(x => x.RideCarSeatComposition)
            //    .Include(x => x.RideLocationPointComposition)
            //    .ThenInclude(x => x.LocationPoint)
            //    .ThenInclude(x => x.Location)
            //    .FirstOrDefaultAsync(x => x.IsRowActive && x.Id.Equals(request.RideId));

            ////After inserting message to our db, need to send message to other passenger/driver as well.
            //await Task.Run(async () =>
            //{
            //    var notificationsToPassenger = _dbContext
            //        .RideCarSeatCompositions
            //        .Include(x => x.Passenger)
            //        .Where(x => x.IsRowActive && x.RideId == ride.Id && x.PassengerId.HasValue && x.PassengerId != _authenticatedUserService.UserId)
            //        .Select(x => x.Passenger)
            //        .Where(x => x.Id != _authenticatedUserService.UserId); // excluding sender.

            //    var driverId = _dbContext.Rides.FindAsync(ride.Id).Result.DriverId;

            //    var userFcmTokens =
            //        _dbContext.UserFcmTokens.Where(x => x.IsRowActive && notificationsToPassenger.Select(y => y.Id).Contains(x.UserId) || (driverId != _authenticatedUserService.UserId && x.UserId.Equals(driverId)));

            //    if (!userFcmTokens.Any()) return;

            //    //Loading sender user from db.
            //    await _dbContext.Attach(messageEntity).Reference(x => x.CreatedByUser).LoadAsync();
            //    var senderId = messageEntity.CreatedByUserId;
            //    var senderFullname = messageEntity.CreatedByUser.Name + " " + messageEntity.CreatedByUser.Surname;
            //    var senderRating = await _userRatingService.GetUserRating(messageEntity.CreatedByUser.Id);

            //    var startLocationName = ride.RideLocationPointComposition
            //        .First(x => x.LocationPointType == LocationPointType.StartPoint).LocationPoint.Location.Name;

            //    var finishLocationName = ride.RideLocationPointComposition
            //        .First(x => x.LocationPointType == LocationPointType.FinishPoint).LocationPoint.Location.Name;

            //    MessageToUsersVm messageToUsersVm = new MessageToUsersVm(messageEntity.Id, senderFullname, senderRating,
            //        messageEntity.Content, messageEntity.MessageType, messageEntity.SenderType,
            //        messageEntity.CreatedTimestamp, startLocationName, finishLocationName, ride.StartDate, ride.Id, senderId);

            //    var notificationBody = JsonConvert.SerializeObject(messageToUsersVm);

            //    var fcmContract = _fcmNotificationContract.Value;
            //    fcmContract.data.ActionInApp = "MESSAGE_NOTIFICATION_CLICK";
            //    fcmContract.notification = new FcmNotificationContract.Notification($"{senderFullname} : {messageEntity.Content}", $"{startLocationName} - {finishLocationName} səyahətindən");
            //    fcmContract.data.Message = notificationBody;
            //    fcmContract.data.Body = $"{senderFullname} : {messageEntity.Content}";
            //    fcmContract.data.Title = $"{startLocationName} - {finishLocationName} səyahətindən";
            //    fcmContract.registration_ids = userFcmTokens.Select(x => x.Token).ToList();

            //    await _fcmTokenService.SendNotificationToUser(fcmContract);
            //});

            //return 0;
        }
    }
}