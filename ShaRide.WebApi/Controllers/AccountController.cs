using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShaRide.Application.Attributes;
using ShaRide.Application.DTO.Request.Account;
using ShaRide.Application.DTO.Response.Account;
using ShaRide.Application.Services.Interface;

namespace ShaRide.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKey]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        
        /// <summary>
        /// Authenticate user.
        /// </summary>
        /// <returns></returns>
        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync(AuthenticationRequest request)
        {
            return Ok(await _accountService.AuthenticateAsync(request, GenerateIpAddress()));
        }

        /// <summary>
        /// Register user.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("register")]
        [Produces(typeof(AuthenticationResponse))]
        public async Task<IActionResult> RegisterAsync(RegisterRequest request)
        {
            var origin = Request.Headers["origin"];
            return Ok(await _accountService.RegisterAsync(request, origin));
        }

        /// <summary>
        /// Gets verification code by given phone number.
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        [HttpGet("send-phone-verification-sms")]
        [Produces(typeof(string))]
        public async Task<IActionResult> SendPhoneVerificationSmsAsync([Required]string phoneNumber)
        {
            return Ok(await _accountService.SendPhoneVerificationSms(phoneNumber));
        }

        /// <summary>
        /// Gets verification code by given phone number.
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        [HttpGet("send-password-reset-sms")]
        [Produces(typeof(string))]
        public async Task<IActionResult> SendPasswordResetSmsAsync([Required]string phoneNumber)
        {
            return Ok(await _accountService.SendPasswordResetSms(phoneNumber));
        }

        /// <summary>
        /// Resets user password by given user phone.
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="newPassword">new password of user</param>
        /// <returns></returns>
        [HttpGet("reset-user-password")]
        [Produces(typeof(int))]
        public async Task<IActionResult> ResetUserPasswordAsync([Required]string phoneNumber, [Required]string newPassword)
        {
            return Ok(await _accountService.ResetUserPassword(phoneNumber,newPassword));
        }

        [HttpGet("get-user-thumbnail-photo")]
        [Produces(typeof(byte[]))]
        public async Task<IActionResult> GetUserThumbnailPhoto(int userId)
        {
            var image = await _accountService.GetUserThumbnailPhoto(userId);
            var data = image.Image;
            var filename = image.Id + image.Extension;
            return File(data, "application/force-download", filename);
        }

        private string GenerateIpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}