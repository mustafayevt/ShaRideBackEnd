using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Mvc;
using ShaRide.Application.DTOs.Account;
using ShaRide.Application.Services.Interface;

namespace ShaRide.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpPost("authenticate")]
        public async Task<ApiResponse> AuthenticateAsync(AuthenticationRequest request)
        {
            return new ApiResponse(await _accountService.AuthenticateAsync(request, GenerateIpAddress()));
        }

        [HttpPost("register")]
        public async Task<ApiResponse> RegisterAsync(RegisterRequest request)
        {
            var origin = Request.Headers["origin"];
            return new ApiResponse(await _accountService.RegisterAsync(request, origin));
        }

        [HttpPost("get-verification-code")]
        public async Task<ApiResponse> GetVerificationCodeAsync(string phoneNumber)
        {
            return new ApiResponse(await _accountService.GetVerificationCode(phoneNumber));
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