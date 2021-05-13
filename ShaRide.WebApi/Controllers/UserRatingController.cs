using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoWrapper.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShaRide.Application.Attributes;
using ShaRide.Application.DTO.Request.Account;
using ShaRide.Application.DTO.Request.Feedback;
using ShaRide.Application.DTO.Request.UserRating;
using ShaRide.Application.DTO.Response.Account;
using ShaRide.Application.DTO.Response.UserRating;
using ShaRide.Application.Services.Interface;

namespace ShaRide.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKey]
    [Authorize(Roles = "Admin,Basic")]
    public class UserRatingController : ControllerBase
    {
        private readonly IUserRatingService _userRatingService;

        public UserRatingController(IUserRatingService userRatingService)
        {
            _userRatingService = userRatingService;
        }

        [HttpPost("rate-user")]
        [Produces(typeof(UserRatingResponse))]
        public async Task<IActionResult> RateUser(InsertUserRatingRequest request)
        {
            return Ok(await _userRatingService.InsertRating(request));
        }

        /// <summary>
        /// Returns user rating based on user id that sent in parameter.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("GetUserRating/{userId}")]
        public async Task<IActionResult> GetUserRating(int userId)
        {
            return Ok(await _userRatingService.GetUserRating(userId));
        }

        /// <summary>
        /// Returns current user rating based on JWT authentication.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCurrentUserRating")]
        public async Task<IActionResult> GetCurrentUserRating()
        {
            return Ok(await _userRatingService.GetCurrentUserRating());
        }
    }
}