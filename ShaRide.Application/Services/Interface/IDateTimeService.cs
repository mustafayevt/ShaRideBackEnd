using System;

namespace ShaRide.Application.Services.Interface
{
    public interface IDateTimeService
    {
        DateTime NowUtc { get; }

        /// <summary>
        /// Converted datetime.
        /// </summary>
        DateTime AzerbaijanDateTime { get; }
        
        string FormattedDateTimeNow { get; }

        string FormattedDateTime (DateTime dateTime);
    }
}
