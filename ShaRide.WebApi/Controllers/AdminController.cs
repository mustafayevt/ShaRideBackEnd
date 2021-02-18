using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ShaRide.Application.ViewModels;

namespace ShaRide.WebApi.Controllers
{
    [Route("Admin")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AdminController : Controller
    {
        private readonly IOptions<AdminAuthorizationRequest> _authorizationRequest;

        public AdminController(IOptions<AdminAuthorizationRequest> authorizationRequest)
        {
            _authorizationRequest = authorizationRequest;
        }

        [HttpGet("Login")]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            await HttpContext.SignOutAsync();
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(AdminAuthorizationRequest authorizationRequest)
        {
            if (!ModelState.IsValid)
                return View(authorizationRequest);

            if (!_authorizationRequest.Value.Username.Equals(authorizationRequest.Username) ||
                !_authorizationRequest.Value.Password.Equals(authorizationRequest.Password))
            {
                ModelState.AddModelError("WrongCredentials","Username or password is wrong");
                return View(authorizationRequest);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, Guid.NewGuid().ToString())
            };
 
            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true, 
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10) 
            };
 
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            var returnUrl = (string)ViewData["ReturnUrl"];
            
            if (!String.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            
            return Redirect("/swagger");
        }
    }
}