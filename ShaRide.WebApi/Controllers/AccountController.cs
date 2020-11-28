using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using ShaRide.Application.Attributes;
using ShaRide.Application.DTO.Request;
using ShaRide.Application.DTO.Request.Account;
using ShaRide.Application.DTO.Response.Account;
using ShaRide.Application.Localize;
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
        [HttpPost("get-verification-code")]
        [Produces(typeof(string))]
        public async Task<IActionResult> GetVerificationCodeAsync(string phoneNumber)
        {
            return Ok(await _accountService.GetVerificationCode(phoneNumber));
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