﻿namespace ShaRide.Application.Services.Interface
{
    public interface IAuthenticatedUserService
    {
        int? UserId { get; }
        bool IsUserAuthenticate { get; }
    }
}
