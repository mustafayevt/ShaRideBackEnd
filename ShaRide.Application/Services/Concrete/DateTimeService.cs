using System;
using ShaRide.Application.Services.Interface;

namespace ShaRide.Application.Services.Concrete
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime NowUtc => DateTime.UtcNow;
    }
}
