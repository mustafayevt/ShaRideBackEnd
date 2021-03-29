using System;
using System.Globalization;
using ShaRide.Application.Extensions;
using ShaRide.Application.Services.Interface;

namespace ShaRide.Application.Services.Concrete
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime NowUtc => DateTime.UtcNow;

        public DateTime AzerbaijanDateTime
        {
            get
            {
                var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Azerbaijan Standard Time");

                var dateTimeNow = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
                
                return dateTimeNow;
            }
        }
        public string FormattedDateTimeNow => FormattedDateTime(AzerbaijanDateTime);

        /// <summary>
        /// <example>
        /// 28 Mart
        /// 18:00
        /// </example>
        /// </summary>
        public string FormattedDateTime(DateTime dateTime) => dateTime.ToCustomFormat();
    }
}
