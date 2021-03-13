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
    [Authorize]
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
    }
}