using System;

namespace ShaRide.Application.Services.Interface
{
    public interface IDateTimeService
    {
        DateTime NowUtc { get; }
    }
}
