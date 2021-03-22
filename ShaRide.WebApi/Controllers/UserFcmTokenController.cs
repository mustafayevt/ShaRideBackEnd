using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoWrapper.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShaRide.Application.Attributes;
using ShaRide.Application.DTO.Request.Account;
using ShaRide.Application.DTO.Request.Feedback;
using ShaRide.Application.DTO.Request.UserFcmToken;
using ShaRide.Application.DTO.Response.Account;
using ShaRide.Application.DTO.Response.UserFcmToken;
using ShaRide.Application.Services.Interface;

namespace ShaRide.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ApiKey]
    public class UserFcmTokenController : ControllerBase
    {
        private readonly IUserFcmTokenService _userFcmTokenService;

        public UserFcmTokenController(IUserFcmTokenService userFcmTokenService)
        {
            _userFcmTokenService = userFcmTokenService;
        }

        /// <summary>
        /// Inserts new FCM token of user.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces(typeof(UserFcmTokenResponse))]
        public async Task<IActionResult> InsertToken(UserFcmTokenInsertRequest request)
        {
            return Ok(await _userFcmTokenService.InsertToken(request));
        }

        /// <summary>
        /// Updates existing FCM token of user.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [Produces(typeof(UserFcmTokenResponse))]
        public async Task<IActionResult> UpdateToken(UserFcmTokenUpdateRequest request)
        {
            return Ok(await _userFcmTokenService.UpdateToken(request));
        }

        /// <summary>
        /// Deletes existing FCM token of user.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteToken(UserFcmTokenDeleteRequest request)
        {
            return Ok(await _userFcmTokenService.DeleteToken(request));
        }
    }
}