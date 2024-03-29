﻿using System.Collections.Generic;
using System.Threading.Tasks;
using ShaRide.Application.DTO.Request.UserRating;
using ShaRide.Application.DTO.Response.UserRating;

namespace ShaRide.Application.Services.Interface
{
    public interface IUserRatingService
    {
        Task<UserRatingResponse> InsertRating(InsertUserRatingRequest request);
        Task<short> GetUserRating(int userId);
        
        /// <summary>
        /// <returns>
        /// Key = userId - value = rating.
        /// </returns>
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        IEnumerable<KeyValuePair<int, short>> GetUserRating(params int[] userIds);
        Task<short> GetCurrentUserRating();
    }
}