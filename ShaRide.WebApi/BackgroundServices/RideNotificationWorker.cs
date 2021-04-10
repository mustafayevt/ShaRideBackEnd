using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ShaRide.Application.Services.Interface;
using ShaRide.Domain.Entities;
using ShaRide.Domain.Enums;

namespace ShaRide.WebApi.BackgroundServices
{
    /// <summary>
    /// Worker that takes care of sending notification to user about ride starting.
    /// </summary>
    public class RideNotificationWorker : BackgroundService
    {
        private readonly IRideService _rideService;
        // 4 hour for our system datetime synchronization.
        private readonly DateTime _serverDate;

        public RideNotificationWorker(IServiceScopeFactory serviceScopeFactory)
        {
            //Creating service scope. Don't use 'CreateScope()' method in 'using' statement. we need this object as singleton.
            IServiceScope serviceScope = serviceScopeFactory.CreateScope();
            _rideService = serviceScope.ServiceProvider.GetRequiredService<IRideService>();
            _serverDate = serviceScope.ServiceProvider.GetRequiredService<IDateTimeService>().AzerbaijanDateTime;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
#if !DEBUG
            while (!stoppingToken.IsCancellationRequested)
            {
                // 1 hour before ride starts.
                var ridesBeforeTime = _serverDate.AddHours(1);

                var rides = await _rideService.GetRidesForNotificationByDateTime(ridesBeforeTime);

                await SendNotification(rides.Select(x => new RideNotificationModel
                {
                    Ride = x,
                    NotificationBody = GenerateNotificationBody(x)
                }));

                int delayValue = (int)TimeSpan.FromMinutes(30).TotalMilliseconds; // 30 min.
                
                await Task.Delay(delayValue, stoppingToken);
            }
#endif
        }
        
        private async Task SendNotification(IEnumerable<RideNotificationModel> notificationModels)
        {
            foreach (var notificationModel in notificationModels)
            {
                await _rideService.SendNotificationsToUsersInRide(notificationModel.Ride, notificationModel.NotificationBody);
            }
        }

        private string GenerateNotificationBody(Ride ride)
        {
            var startLocation =
                ride.RideLocationPointComposition.SingleOrDefault(x =>
                    x.LocationPointType == LocationPointType.StartPoint);
            
            var finishLocation =
                ride.RideLocationPointComposition.SingleOrDefault(x =>
                    x.LocationPointType == LocationPointType.FinishPoint);

            var remainingInMinute = (ride.StartDate - _serverDate).Minutes;

            if (remainingInMinute == 0)
                return $"{startLocation.LocationPoint.Location.Name} - {finishLocation.LocationPoint.Location.Name} səyahətin vaxtıdır!";
            
            return $"{startLocation.LocationPoint.Location.Name} - {finishLocation.LocationPoint.Location.Name} səyahətinə son {remainingInMinute} dəqiqə.";
        }
    }

    internal class RideNotificationModel
    {
        public Ride Ride { get; set; }
        public string NotificationBody { get; set; }
    }
}