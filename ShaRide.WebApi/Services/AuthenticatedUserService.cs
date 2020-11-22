using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using ShaRide.Application.Services.Interface;

namespace ShaRide.WebApi.Services
{
    public class AuthenticatedUserService : IAuthenticatedUserService
    {
        public AuthenticatedUserService(IHttpContextAccessor httpContextAccessor)
        {
            var userId = 0;
            int.TryParse(httpContextAccessor.HttpContext?.User?.FindFirstValue("uid"),out userId);
            UserId = userId;
        }

        public int UserId { get; }
    }
}
