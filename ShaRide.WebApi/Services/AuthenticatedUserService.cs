using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using ShaRide.Application.Contexts;
using ShaRide.Application.Services.Interface;
using ShaRide.Domain.Entities;

namespace ShaRide.WebApi.Services
{
    public class AuthenticatedUserService : IAuthenticatedUserService
    {
        public AuthenticatedUserService(IHttpContextAccessor httpContextAccessor)
        {
            if (int.TryParse(httpContextAccessor.HttpContext?.User?.FindFirstValue("uid"), out var userId))
            {
                UserId = userId;
            };
            
        }

        public int? UserId { get;}
        public bool IsUserAuthenticate => UserId.HasValue;
    }
}
