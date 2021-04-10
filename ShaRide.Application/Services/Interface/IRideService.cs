using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShaRide.Application.DTO.Request.Ride;
using ShaRide.Application.DTO.Response.Ride;
using ShaRide.Domain.Entities;

namespace ShaRide.Application.Services.Interface
{
    public interface IRideService
    {
        Task<ICollection<RideResponse>> GetAllActiveRides();
        Task<ICollection<RideResponse>> GetActiveRides(GetActiveRidesRequest request);
        Task<int> InsertRide(InsertRideRequest request);
        Task<int> UpdateRideState(UpdateRideStateRequest request);
        Task<int> AddPassengerToRide(AddPassengerToRideRequest request);
        Task<int> RespondUserRideRequest(List<DriverRespondRequest> requests);
        Task<IEnumerable<PassengerToRideResponse>> GetPassengerToRideRequests();
        Task<int> CancelRide(CancelRideRequest request);
        Task<ICollection<RideResponse>> GetCurrentUsersRidesAsDriver();
        Task<ICollection<RideResponse>> GetCurrentUserRidesAsPassenger();
        Task<ICollection<GetUserRideRequestResponse>> GetCurrentUsersRideRequests();
        Task<int> CancelPassengerRideRequest(int rideId);
        Task<ICollection<RideResponse>> SuggestRidesToUser();
        Task<ICollection<RideResponse>> SuggestRidesToUserBasedOnUserFavoriteRoute();
        Task<List<Ride>> GetRidesForNotificationByDateTime(DateTime rideStarTime);
        Task SendNotificationsToUsersInRide(Ride ride, string notificationBody);
        Task<int> DeactivateUserRequest(int requestId);
    }
}