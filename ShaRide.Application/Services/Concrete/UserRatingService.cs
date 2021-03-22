using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AutoMapper;
using AutoWrapper.Wrappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using ShaRide.Application.Contexts;
using ShaRide.Application.DTO.Request.UserRating;
using ShaRide.Application.DTO.Response.UserRating;
using ShaRide.Application.Helpers;
using ShaRide.Application.Localize;
using ShaRide.Application.Managers;
using ShaRide.Application.Services.Interface;
using ShaRide.Domain.Entities;

namespace ShaRide.Application.Services.Concrete
{
    public class UserRatingService : IUserRatingService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer _localizer;
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly UserManager _userManager;

        public UserRatingService(ApplicationDbContext dbContext, IMapper mapper, IStringLocalizer<Resource> localizer, IAuthenticatedUserService authenticatedUserService, UserManager userManager)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _localizer = localizer;
            _authenticatedUserService = authenticatedUserService;
            _userManager = userManager;
        }
        
        public async Task<UserRatingResponse> InsertRating(InsertUserRatingRequest request)
        {
            if (!_authenticatedUserService.IsUserAuthenticate)
                throw new ApiException("User is not authenticated");
            
            var userRatings = new List<UserRating>();
            var destinationUsers = _dbContext.Users.Where(x=>request.Ratings.Select(y=>y.DestinationUserId).Contains(x.Id));
            request.Ratings.ForEach(ratingRequest =>
            {
                var userRating = new UserRating
                {
                    SourceUserId = _authenticatedUserService.UserId.Value,
                    DestinationUserId = ratingRequest.DestinationUserId,
                    Value = ratingRequest.Value,
                    RideId = request.RideId
                };
                userRatings.Add(userRating);
            });

            await _dbContext.UserRatings.AddRangeAsync(userRatings);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<UserRatingResponse>(request);
        }

        public Task<short> GetUserRating(int userId)
        {
            if (!_userManager.TryGetUserById(userId, out _))
                throw new ApiException(_localizer[LocalizationKeys.NOT_FOUND, userId]);

            var ratings = _dbContext.UserRatings.Where(x => x.IsRowActive && x.DestinationUserId.Equals(userId));
            var sumOfRating = ratings.Any() ? ratings.Average(x => x.Value) : 5;
            var fraction = sumOfRating - (int) sumOfRating;

            return Task.FromResult((short) (fraction >= 0.5 ? sumOfRating + 1 : sumOfRating));
        }

        public async Task<short> GetCurrentUserRating()
        {
            if (!_authenticatedUserService.IsUserAuthenticate)
                throw new ApiException(_localizer.GetString(LocalizationKeys.USER_HAS_NOT_ACCESS_OPERATION));
            
            return await GetUserRating(_authenticatedUserService.UserId.Value);
        }
    }
}